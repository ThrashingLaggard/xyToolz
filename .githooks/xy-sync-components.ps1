param(
    [ValidateSet("commit", "push")]
    [string]$Mode = "commit"
)

$ErrorActionPreference = "Continue"

# Unconditional trace so we can tell whether the hook was even invoked at all,
# independent of anything else the script does.
$LogPath = Join-Path (git rev-parse --show-toplevel) ".githooks\hook.log"
"$(Get-Date -Format o) MODE=$Mode PID=$PID" | Add-Content -Path $LogPath

# Module -> package name. Code stays under $BasePrefix/<Module>; the standalone
# csproj lives under xyComponents/<PackageName>. Both are combined into a flat
# snapshot in the staging worktree and pushed to the module's bare repo.
$Modules = @{
    "Extensions"    = "xyExtensions"
    "QOL"           = "xyQOL"
    "Chrono"        = "xyChrono"
    "Fonts"         = "xyFonts"
    "Enumerables"   = "xyEnumerables"
    "Maths"         = "xyMaths"
    "PDF"           = "xyPdf"
    "Filesystem"    = "xyFilesystem"
    "Serialization" = "xySerialization"
    "Security"      = "xySecurity"
}

$RepoRoot      = git rev-parse --show-toplevel
$BasePrefix    = Join-Path $RepoRoot "xyToolz"
$ComponentsDir = Join-Path $RepoRoot "xyComponents"
$BareDir       = Join-Path $RepoRoot "VersionControl"
$StagingDir    = Join-Path $BareDir ".staging"

# Distinguish "the ref range doesn't resolve" (no @{u}, no HEAD~1 yet - fall back to
# treating everything as changed) from "the range resolved fine but is empty" (a genuine
# no-op push/commit - must NOT be treated as "everything changed", or every module gets
# needlessly re-synced and, worse, re-evaluated for a version bump against a stale commit
# message).
$RangeResolved = $true
if ($Mode -eq "push") {
    # pre-push: cover every commit that's about to go out, not just the last one -
    # otherwise commits made before the most recent one get silently skipped.
    $Changed = git diff --name-only '@{u}..HEAD' 2>$null
    if ($LASTEXITCODE -ne 0) {
        $Changed = git diff --name-only HEAD~1 HEAD 2>$null
        if ($LASTEXITCODE -ne 0) { $RangeResolved = $false }
    }
    $CommitBodies = git log '@{u}..HEAD' --pretty=%B 2>$null
    if ($LASTEXITCODE -ne 0) { $CommitBodies = git log -1 --pretty=%B }
} else {
    $Changed = git diff --name-only HEAD~1 HEAD 2>$null
    if ($LASTEXITCODE -ne 0) { $RangeResolved = $false }
    $CommitBodies = git log -1 --pretty=%B
}
if (-not $RangeResolved) { $Changed = git ls-files }
$CommitBodies = $CommitBodies -join "`n"

foreach ($folder in $Modules.Keys) {
  try {
    $pkg = $Modules[$folder]

    $codePath   = Join-Path $BasePrefix $folder
    $csprojPath = Join-Path $ComponentsDir "$pkg\$pkg.csproj"
    $barePath   = Join-Path $BareDir "$pkg.git"

    $relCodePrefix = "xyToolz/$folder/"
    $relCsprojPath = "xyComponents/$pkg/$pkg.csproj"
    if (-not (($Changed -match [regex]::Escape($relCodePrefix)) -or ($Changed -match [regex]::Escape($relCsprojPath)))) {
        continue
    }

    if (-not (Test-Path $barePath)) {
        Write-Host "==> ${folder}: kein bare Repo unter $barePath, ueberspringe"
        continue
    }

    Write-Host "==> $folder geaendert -> sync nach $pkg"

    $work = Join-Path $StagingDir $pkg
    if (-not (Test-Path $work)) {
        New-Item -ItemType Directory -Path $StagingDir -Force | Out-Null
        git clone $barePath $work
    }

    # Mirror current code into the staging worktree (flat, no subfolders), excluding .git.
    Get-ChildItem -Path $work -Exclude ".git" | Remove-Item -Recurse -Force
    Copy-Item -Path (Join-Path $codePath "*.cs") -Destination $work -Force -ErrorAction SilentlyContinue

    $gitignorePath = Join-Path $ComponentsDir "$pkg\.gitignore"
    if (Test-Path $gitignorePath) {
        Copy-Item -Path $gitignorePath -Destination $work -Force
    }

    $readmePath = Join-Path $ComponentsDir "$pkg\README.md"
    if (Test-Path $readmePath) {
        Copy-Item -Path $readmePath -Destination $work -Force
    }

    # Snapshot csproj with the local Compile-Include hack stripped — in the export
    # repo the code sits right next to the csproj, so default SDK globbing covers it.
    $csprojContent = Get-Content $csprojPath -Raw
    $csprojContent = $csprojContent -replace '(?s)\s*<EnableDefaultCompileItems>false</EnableDefaultCompileItems>', ''
    $csprojContent = $csprojContent -replace '(?s)<ItemGroup>\s*(<Compile Include="[^"]*"\s*/>\s*)+</ItemGroup>\s*', ''

    # Version bump follows the same Conventional-Commits rule as ci-template-xyProjects.yml:
    # BREAKING CHANGE -> major, feat -> minor, fix -> patch, else unchanged. Just like that
    # workflow's "sed" step, this only touches the throwaway export snapshot — never the
    # tracked source csproj under xyComponents/. The starting point is the latest "v*" tag
    # on this module's own bare repo (fallback: current PackageVersion in the source csproj,
    # i.e. first-ever release).
    #
    # Bump/tag only happens in "push" mode, using every commit since the last push
    # (@{u}..HEAD, computed above) - that's the point where work actually goes out, and it
    # naturally covers multiple fix/feat commits made since the last push in one go.
    # "commit" mode fires on every local commit, often several times before you ever push -
    # cutting a release/tag there would be premature and would double-count once pre-push
    # runs the same range again. Commit mode still mirrors content into the bare repo (local
    # backup of the split-out source) but leaves the version exactly where it is.
    $ownLastTag = git -C $barePath tag -l "v*" --sort=-v:refname | Select-Object -First 1
    if ($ownLastTag) {
        $baseVersion = $ownLastTag.TrimStart("v")
    } else {
        $baseVersionMatch = [regex]::Match($csprojContent, '<PackageVersion>([^<]*)</PackageVersion>')
        $baseVersion = if ($baseVersionMatch.Success) { $baseVersionMatch.Groups[1].Value } else { "1.0.0" }
    }

    $ownVersion = $baseVersion
    if ($Mode -eq "push") {
        $verParts = $baseVersion -split '\.'
        $major = [int]$verParts[0]; $minor = [int]$verParts[1]; $patch = [int]$verParts[2]

        if ($CommitBodies -match "(?im)BREAKING CHANGE") {
            $major++; $minor = 0; $patch = 0
        } elseif ($CommitBodies -match "(?im)^feat") {
            $minor++; $patch = 0
        } elseif ($CommitBodies -match "(?im)^fix") {
            $patch++
        } else {
            Write-Host "    kein fix/feat/BREAKING CHANGE in der Commit-Message - Version bleibt $baseVersion"
        }
        $ownVersion = "$major.$minor.$patch"
        if ($ownVersion -ne $baseVersion) {
            Write-Host "    Version bump $pkg`: $baseVersion -> $ownVersion (nur im Export, xyComponents/$pkg/$pkg.csproj bleibt unveraendert)"
        }
    }
    $csprojContent = [regex]::Replace($csprojContent, '<PackageVersion>[^<]*</PackageVersion>', "<PackageVersion>$ownVersion</PackageVersion>")

    # Cross-module <ProjectReference Include="..\xyOther\xyOther.csproj" /> can't
    # resolve in the flat single-module export, so rewrite it to a PackageReference
    # pinned to the latest "v*" tag of that module's own bare repo (fallback 1.0.0
    # if that module hasn't been published yet).
    $projRefEvaluator = {
        param($match)
        $refPkg = $match.Groups[1].Value
        $refBare = Join-Path $BareDir "$refPkg.git"
        $refVersion = "1.0.0"
        if (Test-Path $refBare) {
            $lastTag = git -C $refBare tag -l "v*" --sort=-v:refname | Select-Object -First 1
            if ($lastTag) { $refVersion = $lastTag.TrimStart("v") }
        }
        "<PackageReference Include=""$refPkg"" Version=""$refVersion"" />"
    }
    $csprojContent = [regex]::Replace(
        $csprojContent,
        '<ProjectReference Include="\.\.\\(xy[A-Za-z0-9]+)\\xy[A-Za-z0-9]+\.csproj"\s*/>',
        $projRefEvaluator
    )

    Set-Content -Path (Join-Path $work "$pkg.csproj") -Value $csprojContent -NoNewline

    Push-Location $work
    try {
        git add -A
        $sourceSha = git -C $RepoRoot rev-parse --short HEAD
        $sourceMsg = git -C $RepoRoot log -1 --pretty=%s
        $status = git status --porcelain
        if ($status) {
            git commit -m "sync: $folder @ $sourceSha - $sourceMsg" | Out-Null
            git push origin master
            if ($ownVersion -ne $baseVersion) {
                git tag "v$ownVersion"
                git push origin "v$ownVersion"
            }
        } else {
            Write-Host "    keine inhaltlichen Aenderungen, ueberspringe Commit/Push"
        }
    } finally {
        Pop-Location
    }
  } catch {
    Write-Host "==> FEHLER beim Sync von $folder ($pkg): $($_.Exception.Message) - andere Module werden trotzdem weiterverarbeitet."
  }
}
