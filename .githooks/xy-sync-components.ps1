<#
.SYNOPSIS
    Flattens each module of the xyToolz monorepo into its own standalone repo and
    pushes it, which triggers that repo's post-receive hook (build -> pack -> publish).

.DESCRIPTION
    This is the script that xyExtensions.csproj refers to in its header comment but
    that did not exist anywhere in the tree. Without it, nine of the ten component
    repos under VersionControl/ stay empty forever, which is exactly the state they
    were found in.

    Why a rewrite is needed at all: inside the monorepo the component csproj files
    reference things that only make sense there --

        <Compile Include="..\..\xyToolz\Security\**\*.cs" />     source lives elsewhere
        <ProjectReference Include="..\xyQOL\xyQOL.csproj" />     sibling folder
        <PackageOutputPath>..\..\NuGets</PackageOutputPath>      escapes the repo root
        <EnableDefaultCompileItems>false</...>                   suppresses local .cs

    None of those survive being pushed into a standalone repo. This script copies the
    source in flat and rewrites the csproj accordingly.

.PARAMETER Only
    Sync a single component, e.g. -Only xySecurity. Default: all, in dependency order.

.PARAMETER DryRun
    Do everything except commit and push. Prints the resulting csproj diff.

.EXAMPLE
    pwsh .githooks/xy-sync-components.ps1
    pwsh .githooks/xy-sync-components.ps1 -Only xyQOL -DryRun
#>
[CmdletBinding()]
param(
    [string]  $Only,
    [switch]  $DryRun,
    [string]  $RepoRoot
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

# --------------------------------------------------------------------------
# 0. Locate the monorepo root and load the manifest
# --------------------------------------------------------------------------
if (-not $RepoRoot) {
    $RepoRoot = (& git rev-parse --show-toplevel 2>$null)
    if (-not $RepoRoot) { $RepoRoot = Split-Path -Parent $PSScriptRoot }
}
$RepoRoot = (Resolve-Path $RepoRoot).Path

$manifestPath = Join-Path $RepoRoot 'build/components.json'
if (-not (Test-Path $manifestPath)) { throw "Manifest fehlt: $manifestPath" }
$mf = Get-Content $manifestPath -Raw | ConvertFrom-Json

$srcRoot   = Join-Path $RepoRoot $mf.monorepoSourceRoot
$compRoot  = Join-Path $RepoRoot $mf.componentRoot
$bareRoot  = Join-Path $RepoRoot $mf.bareRepoRoot
$stageRoot = Join-Path $RepoRoot $mf.stagingRoot

New-Item -ItemType Directory -Force -Path $stageRoot | Out-Null

function Info  ($m) { Write-Host "  $m" }
function Step  ($m) { Write-Host "`n=== $m ===" -ForegroundColor Cyan }
function Warn  ($m) { Write-Host "  ! $m" -ForegroundColor Yellow }

# --------------------------------------------------------------------------
# 1. Helper: what version of component X is currently published?
#    Read from that component's bare repo tags. Deterministic, no floating
#    ranges, and it fails loudly if a dependency was never synced.
# --------------------------------------------------------------------------
function Get-PublishedVersion([string]$id) {
    $bare = Join-Path $bareRoot "$id.git"
    if (-not (Test-Path $bare)) { throw "Bare-Repo fehlt: $bare" }
    $tag = (& git --git-dir=$bare tag -l 'v*' --sort=-v:refname | Select-Object -First 1)
    if (-not $tag) {
        throw "Komponente '$id' hat noch kein Release. Erst '$id' synchronisieren, dann das hier nochmal. (Reihenfolge steht in build/components.json)"
    }
    return $tag.TrimStart('v')
}

# --------------------------------------------------------------------------
# 2. Helper: rewrite a monorepo csproj into a standalone one
# --------------------------------------------------------------------------
function Convert-Csproj {
    param([string]$Path, [object]$Component)

    # Populated with {depId -> version} for every rewritten ProjectReference, so the
    # caller can add matching <PackageVersion/> entries to the component's copy of
    # Directory.Packages.props. An inline Version="..." on the PackageReference itself
    # would conflict with CPM (NU1008: "cannot define a value for Version" once
    # ManagePackageVersionsCentrally is on) - confirmed by an actual restore failure.
    $script:LastDepVersions = @{}

    [xml]$x = Get-Content $Path -Raw
    $proj = $x.DocumentElement

    foreach ($pg in @($proj.SelectNodes('PropertyGroup'))) {
        foreach ($name in @('EnableDefaultCompileItems','PackageOutputPath','GeneratePackageOnBuild','PackageVersion')) {
            $node = $pg.SelectSingleNode($name)
            # EnableDefaultCompileItems=false suppresses the flat .cs we just copied in.
            # PackageOutputPath ..\..\NuGets points outside a standalone repo.
            # GeneratePackageOnBuild double-packs; the hook packs explicitly.
            # PackageVersion is injected by the hook via -p:, so it cannot drift.
            if ($node) { [void]$pg.RemoveChild($node) }
        }
    }

    foreach ($ig in @($proj.SelectNodes('ItemGroup'))) {
        # Drop every Compile Include that reaches outside the component folder.
        foreach ($c in @($ig.SelectNodes('Compile'))) {
            $inc = $c.GetAttribute('Include')
            if ($inc -like '..*' -or $inc -like '*\..\*') { [void]$ig.RemoveChild($c) }
        }
        # ProjectReference -> PackageReference, pinned to the dependency's latest tag.
        # No inline Version here (see $script:LastDepVersions above) - the caller adds a
        # matching <PackageVersion/> to the component's copy of Directory.Packages.props.
        foreach ($pr in @($ig.SelectNodes('ProjectReference'))) {
            $inc  = $pr.GetAttribute('Include')
            $depId = [System.IO.Path]::GetFileNameWithoutExtension($inc)
            $ver  = Get-PublishedVersion $depId
            $script:LastDepVersions[$depId] = $ver
            $new  = $x.CreateElement('PackageReference')
            $new.SetAttribute('Include', $depId)
            [void]$ig.ReplaceChild($new, $pr)
            Info "ProjectReference $depId -> PackageReference $depId $ver"
        }
    }

    # Strip ItemGroups that ended up empty.
    foreach ($ig in @($proj.SelectNodes('ItemGroup'))) {
        if (-not $ig.HasChildNodes) { [void]$proj.RemoveChild($ig) }
    }

    $sw = New-Object System.IO.StringWriter
    $xw = New-Object System.Xml.XmlTextWriter($sw)
    $xw.Formatting = 'Indented'; $xw.Indentation = 2
    $x.Save($xw)
    # UTF-8 without BOM, LF endings.
    return ($sw.ToString() -replace "`r`n", "`n")
}

# --------------------------------------------------------------------------
# 3. Main loop, in the topological order defined by the manifest
# --------------------------------------------------------------------------
$targets = $mf.components
if ($Only) {
    $targets = @($mf.components | Where-Object { $_.id -eq $Only })
    if (-not $targets) { throw "Unbekannte Komponente: $Only" }
}

$srcSha     = (& git -C $RepoRoot rev-parse --short HEAD)
$srcSubject = (& git -C $RepoRoot log -1 --pretty=%s)

# --------------------------------------------------------------------------
# 2b. What actually changed since the last push? Without this, every run -
# every single push, once wired into pre-push - did the full staging-clone
# fetch/reset/copy/rewrite dance for all ten components before checking (via
# git status) whether anything even needed pushing. Cheap compared to a real
# build, but it adds up over ten components on every push. $null means "could
# not resolve a range" (first-ever run, no upstream, detached HEAD, etc.) -
# treated as "process everything", same conservative fallback the old
# auto-fire hook used. An empty (but resolved) array means a genuine no-op.
# -Only bypasses this entirely - an explicit ask for one component always runs.
$changedFiles = $null
if (-not $Only) {
    $diffOut = & git -C $RepoRoot diff --name-only '@{u}..HEAD' 2>$null
    if ($LASTEXITCODE -eq 0) {
        $changedFiles = @($diffOut)
    } else {
        $diffOut = & git -C $RepoRoot diff --name-only 'HEAD~1..HEAD' 2>$null
        if ($LASTEXITCODE -eq 0) { $changedFiles = @($diffOut) }
    }
}

$summary = @()

foreach ($c in $targets) {
    Step "$($c.id)"

    $moduleDir = Join-Path $srcRoot $c.sourceDir
    $compDir   = Join-Path $compRoot $c.id
    $bare      = Join-Path $bareRoot "$($c.id).git"
    $stage     = Join-Path $stageRoot $c.id

    if (-not (Test-Path $moduleDir)) { Warn "Quellordner fehlt: $moduleDir - uebersprungen"; continue }
    if (-not (Test-Path $bare))      { Warn "Bare-Repo fehlt: $bare - uebersprungen";      continue }

    if ($null -ne $changedFiles) {
        $relPrefixes = @("$($mf.monorepoSourceRoot)/$($c.sourceDir)/", "$($c.id)/")
        foreach ($extra in @($c.extraFiles)) {
            if ($extra) { $relPrefixes += "$($mf.monorepoSourceRoot)/$extra" }
        }
        $touched = $changedFiles | Where-Object { $f = $_; $relPrefixes | Where-Object { $f -like "$_*" } }
        if (-not $touched) {
            Info "keine Aenderung im Quellcode seit dem letzten Push - uebersprungen (kein Staging-Clone noetig)"
            $summary += [pscustomobject]@{ Component = $c.id; Action = 'skipped (unchanged)' }
            continue
        }
    }

    # -- 3a. staging clone -------------------------------------------------
    if (-not (Test-Path (Join-Path $stage '.git'))) {
        Info "Staging-Clone anlegen"
        $hasRefs = (& git --git-dir=$bare for-each-ref --count=1)
        if ($hasRefs) {
            & git clone --quiet $bare $stage
        } else {
            New-Item -ItemType Directory -Force -Path $stage | Out-Null
            & git -C $stage init --quiet -b master
            & git -C $stage remote add origin $bare
        }
    } else {
        & git -C $stage fetch --quiet origin 2>$null
        & git -C $stage reset --hard --quiet origin/master 2>$null
    }

    # -- 3b. wipe worktree, keep .git --------------------------------------
    Get-ChildItem -Path $stage -Force |
        Where-Object { $_.Name -ne '.git' } |
        Remove-Item -Recurse -Force

    # -- 3c. copy module source in FLAT ------------------------------------
    # @(...) forces an array even when exactly one file matches - otherwise Get-ChildItem
    # returns a scalar FileInfo and ".Count" throws under Set-StrictMode (confirmed: broke
    # on xyEnumerables/xyChrono, which each have exactly one source file).
    $files = @(Get-ChildItem -Path $moduleDir -Filter *.cs -Recurse -File)
    foreach ($f in $files) { Copy-Item $f.FullName (Join-Path $stage $f.Name) -Force }
    Info "$($files.Count) Quelldatei(en) kopiert"

    foreach ($extra in @($c.extraFiles)) {
        if (-not $extra) { continue }
        $p = Join-Path $srcRoot $extra
        if (Test-Path $p) {
            Copy-Item $p (Join-Path $stage (Split-Path $extra -Leaf)) -Force
            Info "extra: $extra"
        } else { Warn "extraFile fehlt: $p" }
    }

    # -- 3d. README + .gitignore -------------------------------------------
    $readme = Join-Path $compDir 'README.md'
    if (Test-Path $readme) { Copy-Item $readme (Join-Path $stage 'README.md') -Force }
    else { Warn "README.md fehlt fuer $($c.id) - PackageReadmeFile wird den Pack sprengen" }

    $gi = Join-Path $compDir '.gitignore'
    if (Test-Path $gi) { Copy-Item $gi (Join-Path $stage '.gitignore') -Force }

    # Shared metadata travels with the component, otherwise the standalone build
    # loses Authors/License/Copyright and packs an anaemic .nuspec.
    $dbp = Join-Path $RepoRoot 'Directory.Build.props'
    if (Test-Path $dbp) { Copy-Item $dbp (Join-Path $stage 'Directory.Build.props') -Force }

    # Central Package Management travels with the component too: the component csproj
    # files have version-less <PackageReference/> (CPM in the monorepo), so a standalone
    # build without this file fails with NU1015 "no version specified" the moment it
    # leaves the monorepo - confirmed by an actual restore against the flattened repo.
    $dpp = Join-Path $RepoRoot 'Directory.Packages.props'
    if (Test-Path $dpp) { Copy-Item $dpp (Join-Path $stage 'Directory.Packages.props') -Force }

    # global.json travels with the component too. build/post-receive builds in a mktemp
    # directory outside the monorepo tree, so without this file the pinned net8.0 SDK band
    # never applies there and the build silently falls back to whatever SDK is ambient on
    # the machine - confirmed by an actual build that produced a genuinely different compile
    # error (CS0029 in AutoResourceFontResolver.cs) under an ambient .NET 10 SDK versus a
    # clean build under the pinned 8.0.100 band.
    $gj = Join-Path $RepoRoot 'global.json'
    if (Test-Path $gj) { Copy-Item $gj (Join-Path $stage 'global.json') -Force }

    # -- 3e. rewrite csproj -------------------------------------------------
    $srcCsproj = Join-Path $compDir "$($c.id).csproj"
    if (-not (Test-Path $srcCsproj)) { Warn "csproj fehlt: $srcCsproj - uebersprungen"; continue }

    $out = Convert-Csproj -Path $srcCsproj -Component $c
    $dst = Join-Path $stage "$($c.id).csproj"
    [System.IO.File]::WriteAllText($dst, $out, (New-Object System.Text.UTF8Encoding($false)))

    # Add a <PackageVersion/> for every rewritten intra-family dependency to the
    # component's copy of Directory.Packages.props - CPM requires one for every
    # PackageReference once it is enabled, and these dependencies aren't in the
    # monorepo's Directory.Packages.props (only external packages are).
    $stagedDpp = Join-Path $stage 'Directory.Packages.props'
    if ($script:LastDepVersions.Count -gt 0 -and (Test-Path $stagedDpp)) {
        [xml]$dppXml = Get-Content $stagedDpp -Raw
        $itemGroup = $dppXml.Project.ItemGroup
        if ($itemGroup -is [System.Array]) { $itemGroup = $itemGroup[0] }
        foreach ($depId in $script:LastDepVersions.Keys) {
            $pv = $dppXml.CreateElement('PackageVersion')
            $pv.SetAttribute('Include', $depId)
            $pv.SetAttribute('Version', $script:LastDepVersions[$depId])
            [void]$itemGroup.AppendChild($pv)
        }
        $sw2 = New-Object System.IO.StringWriter
        $xw2 = New-Object System.Xml.XmlTextWriter($sw2)
        $xw2.Formatting = 'Indented'; $xw2.Indentation = 2
        $dppXml.Save($xw2)
        $dppOut = ($sw2.ToString() -replace "`r`n", "`n")
        [System.IO.File]::WriteAllText($stagedDpp, $dppOut, (New-Object System.Text.UTF8Encoding($false)))
    }

    if ($DryRun) {
        Write-Host "--- $($c.id).csproj (DryRun) ---" -ForegroundColor DarkGray
        Write-Host $out
        $summary += [pscustomobject]@{ Component = $c.id; Action = 'dry-run' }
        continue
    }

    # -- 3f. commit + push --------------------------------------------------
    & git -C $stage add -A
    $dirty = (& git -C $stage status --porcelain)
    if (-not $dirty) {
        Info "keine Aenderung -> kein Push, kein Release"
        $summary += [pscustomobject]@{ Component = $c.id; Action = 'unchanged' }
        continue
    }

    # Carry the monorepo subject through verbatim, so that feat:/fix:/BREAKING
    # in the monorepo produce the matching bump in the component repo. The old
    # setup hardcoded "sync: ..." as the subject, which never matched the hook's
    # keyword rules -- so nothing ever released unless you got lucky.
    $msg = "$srcSubject`n`nsync: $($c.id) @ $srcSha"
    & git -C $stage -c user.name='xy sync' -c user.email='sync@localhost' commit --quiet -m $msg

    Info "push -> $bare"
    & git -C $stage push --quiet origin master
    if ($LASTEXITCODE -ne 0) { throw "Push fuer $($c.id) fehlgeschlagen." }

    $summary += [pscustomobject]@{ Component = $c.id; Action = 'pushed' }
}

Step 'Zusammenfassung'
$summary | Format-Table -AutoSize
Write-Host "Release-Logs pro Komponente: $bareRoot\<id>.git\xy-release.log`n"
