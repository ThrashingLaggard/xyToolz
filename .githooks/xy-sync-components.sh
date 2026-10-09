#!/usr/bin/env bash
#
# xy-sync-components.sh
#
# SYNOPSIS
#   Flattens each module of the xyToolz monorepo into its own standalone repo and
#   pushes it, which triggers that repo's post-receive hook (build -> pack -> publish).
#
# DESCRIPTION
#   This is the script that xyExtensions.csproj refers to in its header comment but
#   that did not exist anywhere in the tree. Without it, nine of the ten component
#   repos under VersionControl/ stay empty forever, which is exactly the state they
#   were found in.
#
#   Why a rewrite is needed at all: inside the monorepo the component csproj files
#   reference things that only make sense there --
#
#       <Compile Include="..\..\xyToolz\Security\**\*.cs" />     source lives elsewhere
#       <ProjectReference Include="..\xyQOL\xyQOL.csproj" />     sibling folder
#       <PackageOutputPath>..\..\NuGets</PackageOutputPath>      escapes the repo root
#       <EnableDefaultCompileItems>false</...>                   suppresses local .cs
#
#   None of those survive being pushed into a standalone repo. This script copies the
#   source in flat and rewrites the csproj accordingly.
#
# OPTIONS
#   -o, --only <id>         Sync a single component, e.g. --only xySecurity.
#                           Default: all, in dependency order.
#   -n, --dry-run           Do everything except commit and push. Prints the
#                           resulting csproj.
#   -r, --repo-root <path>  Monorepo root. Default: git toplevel, else the parent
#                           of this script's directory.
#   -h, --help              Show this help.
#
#   The PowerShell spellings -Only, -DryRun and -RepoRoot are accepted as well, so
#   existing call sites keep working after swapping the file extension.
#
# EXAMPLES
#   .githooks/xy-sync-components.sh
#   .githooks/xy-sync-components.sh --only xyQOL --dry-run
#
# REQUIREMENTS
#   bash >= 4.4, git, jq, xmlstarlet

set -euo pipefail

# --------------------------------------------------------------------------
# Output helpers
# --------------------------------------------------------------------------
if [[ -t 1 && -z ${NO_COLOR:-} ]]; then
    C_CYAN=$'\e[36m'; C_YELLOW=$'\e[33m'; C_GRAY=$'\e[90m'; C_RESET=$'\e[0m'
else
    C_CYAN=''; C_YELLOW=''; C_GRAY=''; C_RESET=''
fi

info() { printf '  %s\n' "$*"; }
step() { printf '\n%s=== %s ===%s\n' "$C_CYAN" "$*" "$C_RESET"; }
warn() { printf '%s  ! %s%s\n' "$C_YELLOW" "$*" "$C_RESET"; }
die()  { printf 'FEHLER: %s\n' "$*" >&2; exit 1; }

usage() {
    # Print the header comment block (everything up to the first non-comment line).
    sed -n '2,/^[^#]/{/^#/!d;s/^# \{0,1\}//;p;}' "${BASH_SOURCE[0]}"
}

# --------------------------------------------------------------------------
# Arguments
# --------------------------------------------------------------------------
only=''
dry_run=0
repo_root=''

while (($#)); do
    case $1 in
        -o|--only|-Only)
            (($# >= 2)) || die "Option $1 braucht einen Wert."
            only=$2; shift 2 ;;
        --only=*)
            only=${1#*=}; shift ;;
        -n|--dry-run|-DryRun)
            dry_run=1; shift ;;
        -r|--repo-root|-RepoRoot)
            (($# >= 2)) || die "Option $1 braucht einen Wert."
            repo_root=$2; shift 2 ;;
        --repo-root=*)
            repo_root=${1#*=}; shift ;;
        -h|--help)
            usage; exit 0 ;;
        *)
            die "Unbekanntes Argument: $1 (siehe --help)" ;;
    esac
done

# --------------------------------------------------------------------------
# Preconditions
# --------------------------------------------------------------------------
if ((BASH_VERSINFO[0] < 4 || (BASH_VERSINFO[0] == 4 && BASH_VERSINFO[1] < 4))); then
    die "bash >= 4.4 noetig (gefunden: $BASH_VERSION)."
fi

# Some distributions ship the xmlstarlet binary as plain "xml".
XML=''
for candidate in xmlstarlet xml; do
    if command -v "$candidate" >/dev/null 2>&1; then XML=$candidate; break; fi
done
[[ -n $XML ]] || die "xmlstarlet fehlt (z.B. 'sudo dnf install xmlstarlet')."
for tool in git jq; do
    command -v "$tool" >/dev/null 2>&1 || die "$tool fehlt."
done

# --------------------------------------------------------------------------
# 0. Locate the monorepo root and load the manifest
# --------------------------------------------------------------------------
script_dir=$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)

if [[ -z $repo_root ]]; then
    repo_root=$(git rev-parse --show-toplevel 2>/dev/null) || repo_root=''
    [[ -n $repo_root ]] || repo_root=$(dirname -- "$script_dir")
fi
[[ -d $repo_root ]] || die "Repo-Root nicht gefunden: $repo_root"
repo_root=$(cd -- "$repo_root" && pwd)

manifest="$repo_root/build/components.json"
[[ -f $manifest ]] || die "Manifest fehlt: $manifest"

# The manifest was written for Windows tooling, so tolerate backslash separators.
to_posix() { printf '%s' "${1//\\//}"; }

# Read a mandatory top-level string from the manifest.
mf_get() {
    local value
    value=$(jq -er --arg k "$1" '.[$k] | strings' "$manifest") \
        || die "Manifest-Eintrag fehlt: $1 ($manifest)"
    to_posix "$value"
}

mf_source_root=$(mf_get monorepoSourceRoot)
src_root="$repo_root/$mf_source_root"
comp_root="$repo_root/$(mf_get componentRoot)"
bare_root="$repo_root/$(mf_get bareRepoRoot)"
stage_root="$repo_root/$(mf_get stagingRoot)"

mkdir -p -- "$stage_root"

# --------------------------------------------------------------------------
# 1. Helper: what version of component X is currently published?
#    Read from that component's bare repo tags. Deterministic, no floating
#    ranges, and it fails loudly if a dependency was never synced.
#    Prints the version on stdout; call it via $(...) || exit 1.
# --------------------------------------------------------------------------
get_published_version() {
    local id=$1
    local bare="$bare_root/$id.git"
    local tags tag

    [[ -d $bare ]] || die "Bare-Repo fehlt: $bare"

    # Take the first line in the shell instead of piping into head: with pipefail
    # a SIGPIPE on git would otherwise turn a successful lookup into a failure.
    tags=$(git --git-dir="$bare" tag -l 'v*' --sort=-v:refname)
    tag=${tags%%$'\n'*}
    if [[ -z $tag ]]; then
        die "Komponente '$id' hat noch kein Release. Erst '$id' synchronisieren, dann das hier nochmal. (Reihenfolge steht in build/components.json)"
    fi

    # Strip every leading "v", like TrimStart('v') did.
    while [[ $tag == v* ]]; do tag=${tag#v}; done
    printf '%s\n' "$tag"
}

# --------------------------------------------------------------------------
# 2. Helper: rewrite a monorepo csproj into a standalone one
# --------------------------------------------------------------------------

# Succeeds if the file starts with an XML declaration.
has_xml_decl() {
    local first_line=''
    IFS= read -r first_line < "$1" || true
    [[ $first_line == *'<?xml'* ]]
}

# xml_edit <file> <xmlstarlet ed arguments...>
# Prints the edited document: re-indented with 2 spaces, UTF-8 without BOM, LF
# endings, and with an XML declaration only if the input had one.
#
# libxml2 writes non-ASCII text as numeric character references (f&#xFC;r) unless
# the document declares an encoding. So a file without a declaration gets a UTF-8
# one for the duration of the edit, which is stripped from the result again.
# (xmlstarlet's own "fo" and "ed -O" are no help here: in 1.6.1 "fo" exits with a
# non-zero status even on success, and "-O" drops the encoding together with the
# declaration.)
xml_edit() {
    local file=$1
    shift
    if has_xml_decl "$file"; then
        "$XML" ed "$@" "$file"
    else
        { printf '<?xml version="1.0" encoding="utf-8"?>\n'; sed '1s/^\xEF\xBB\xBF//' "$file"; } \
            | "$XML" ed "$@" \
            | sed '1{/^<?xml/d;}'
    fi
}

# Populated by convert_csproj with {depId -> version} for every rewritten
# ProjectReference, so the caller can add matching <PackageVersion/> entries to the
# component's copy of Directory.Packages.props. An inline Version="..." on the
# PackageReference itself would conflict with CPM (NU1008: "cannot define a value for
# Version" once ManagePackageVersionsCentrally is on) - confirmed by an actual restore
# failure. DEP_IDS keeps the document order so the output is deterministic.
declare -A DEP_VERSIONS=()
declare -a DEP_IDS=()

# convert_csproj <source.csproj> <destination.csproj>
# Must be called directly (not in a subshell), otherwise DEP_VERSIONS is lost.
convert_csproj() {
    local src=$1 dst=$2
    local -a includes=() ref_edits=()
    local i inc dep ver

    DEP_VERSIONS=()
    DEP_IDS=()

    # ProjectReference -> PackageReference, pinned to the dependency's latest tag.
    # No inline Version here (see DEP_VERSIONS above) - the caller adds a matching
    # <PackageVersion/> to the component's copy of Directory.Packages.props.
    mapfile -t includes < <("$XML" sel -t -m '/*/ItemGroup/ProjectReference' -v '@Include' -n "$src" 2>/dev/null)
    for i in "${!includes[@]}"; do
        inc=${includes[i]}
        # File name without directory and extension; accept both path separators.
        dep=${inc##*[/\\]}
        dep=${dep%.*}
        ver=$(get_published_version "$dep") || exit 1
        [[ -n ${DEP_VERSIONS[$dep]+set} ]] || DEP_IDS+=("$dep")
        DEP_VERSIONS[$dep]=$ver
        # Address by position: the count and order of the nodes do not change
        # while their attributes are being rewritten.
        ref_edits+=(-u "(/*/ItemGroup/ProjectReference)[$((i + 1))]/@Include" -v "$dep")
        info "ProjectReference $dep -> PackageReference $dep $ver"
    done

    # EnableDefaultCompileItems=false suppresses the flat .cs we just copied in.
    # PackageOutputPath ..\..\NuGets points outside a standalone repo.
    # GeneratePackageOnBuild double-packs; the hook packs explicitly.
    # PackageVersion is injected by the hook via -p:, so it cannot drift.
    #
    # Then: drop every Compile Include that reaches outside the component folder,
    # reduce each ProjectReference to its Include attribute, rename it, and finally
    # strip ItemGroups that ended up empty.
    xml_edit "$src" \
        -d '/*/PropertyGroup/EnableDefaultCompileItems' \
        -d '/*/PropertyGroup/PackageOutputPath' \
        -d '/*/PropertyGroup/GeneratePackageOnBuild' \
        -d '/*/PropertyGroup/PackageVersion' \
        -d '/*/ItemGroup/Compile[starts-with(@Include,"..") or contains(@Include,"\..\")]' \
        -d '/*/ItemGroup/ProjectReference/@*[name()!="Include"]' \
        -d '/*/ItemGroup/ProjectReference/node()' \
        "${ref_edits[@]}" \
        -r '/*/ItemGroup/ProjectReference' -v 'PackageReference' \
        -d '/*/ItemGroup[not(*) and not(comment()) and not(processing-instruction()) and normalize-space()=""]' \
        > "$dst"
}

# add_package_versions <Directory.Packages.props>
# Appends one <PackageVersion/> per entry in DEP_VERSIONS to the first ItemGroup.
add_package_versions() {
    local dpp=$1
    local tmp dep
    local -a edits=()

    [[ $("$XML" sel -t -v 'count(/Project/ItemGroup)' "$dpp") -gt 0 ]] \
        || die "Keine ItemGroup in $dpp - <PackageVersion/> kann nicht ergaenzt werden."

    for dep in "${DEP_IDS[@]}"; do
        edits+=(
            -s '(/Project/ItemGroup)[1]' -t elem -n 'PackageVersion'
            -i '(/Project/ItemGroup)[1]/PackageVersion[last()]' -t attr -n 'Include' -v "$dep"
            -i '(/Project/ItemGroup)[1]/PackageVersion[last()]' -t attr -n 'Version' -v "${DEP_VERSIONS[$dep]}"
        )
    done

    tmp=$(mktemp "$dpp.XXXXXX")
    xml_edit "$dpp" "${edits[@]}" > "$tmp"
    mv -f -- "$tmp" "$dpp"
}

# --------------------------------------------------------------------------
# 3. Main loop, in the topological order defined by the manifest
# --------------------------------------------------------------------------
mapfile -t comp_ids     < <(jq -r '.components[] | .id // ""' "$manifest")
mapfile -t comp_srcdirs < <(jq -r '.components[] | .sourceDir // ""' "$manifest")

declare -a targets=()   # indices into comp_ids / comp_srcdirs
for i in "${!comp_ids[@]}"; do
    if [[ -z $only || ${comp_ids[i]} == "$only" ]]; then targets+=("$i"); fi
done
if [[ -n $only && ${#targets[@]} -eq 0 ]]; then die "Unbekannte Komponente: $only"; fi

src_sha=$(git -C "$repo_root" rev-parse --short HEAD)
src_subject=$(git -C "$repo_root" log -1 --pretty=%s)

# --------------------------------------------------------------------------
# 2b. What actually changed since the last push? Without this, every run -
# every single push, once wired into pre-push - did the full staging-clone
# fetch/reset/copy/rewrite dance for all ten components before checking (via
# git status) whether anything even needed pushing. Cheap compared to a real
# build, but it adds up over ten components on every push. changed_resolved=0
# means "could not resolve a range" (first-ever run, no upstream, detached HEAD,
# etc.) - treated as "process everything", same conservative fallback the old
# auto-fire hook used. A resolved but empty list means a genuine no-op.
# --only bypasses this entirely - an explicit ask for one component always runs.
changed_resolved=0
declare -a changed_files=()
if [[ -z $only ]]; then
    diff_out=''
    if diff_out=$(git -C "$repo_root" -c core.quotepath=off diff --name-only '@{u}..HEAD' 2>/dev/null); then
        changed_resolved=1
    elif diff_out=$(git -C "$repo_root" -c core.quotepath=off diff --name-only 'HEAD~1..HEAD' 2>/dev/null); then
        changed_resolved=1
    fi
    if ((changed_resolved)) && [[ -n $diff_out ]]; then
        mapfile -t changed_files <<< "$diff_out"
    fi
fi

declare -a summary_ids=() summary_actions=()
add_summary() { summary_ids+=("$1"); summary_actions+=("$2"); }

# Copy a single file from the monorepo root into the staging repo, if it exists.
copy_root_file() {
    local name=$1 stage=$2
    if [[ -e "$repo_root/$name" ]]; then cp -f -- "$repo_root/$name" "$stage/$name"; fi
}

for idx in "${targets[@]}"; do
    id=${comp_ids[idx]}
    source_dir=$(to_posix "${comp_srcdirs[idx]}")

    step "$id"

    module_dir="$src_root/$source_dir"
    comp_dir="$comp_root/$id"
    bare="$bare_root/$id.git"
    stage="$stage_root/$id"

    if [[ ! -d $module_dir ]]; then warn "Quellordner fehlt: $module_dir - uebersprungen"; continue; fi
    if [[ ! -d $bare ]];       then warn "Bare-Repo fehlt: $bare - uebersprungen";       continue; fi

    # extraFiles may be missing, a single string or an array.
    mapfile -t extras < <(jq -r --argjson i "$idx" \
        '.components[$i].extraFiles // [] | if type == "array" then .[] else . end | strings | select(. != "")' \
        "$manifest")
    for i in "${!extras[@]}"; do extras[i]=$(to_posix "${extras[i]}"); done

    if ((changed_resolved)); then
        rel_prefixes=("$mf_source_root/$source_dir/" "$id/")
        for extra in "${extras[@]}"; do rel_prefixes+=("$mf_source_root/$extra"); done

        touched=0
        for f in "${changed_files[@]}"; do
            for prefix in "${rel_prefixes[@]}"; do
                if [[ $f == "$prefix"* ]]; then touched=1; break 2; fi
            done
        done
        if ((!touched)); then
            info "keine Aenderung im Quellcode seit dem letzten Push - uebersprungen (kein Staging-Clone noetig)"
            add_summary "$id" 'skipped (unchanged)'
            continue
        fi
    fi

    # -- 3a. staging clone -------------------------------------------------
    if [[ ! -e "$stage/.git" ]]; then
        info "Staging-Clone anlegen"
        if [[ -n $(git --git-dir="$bare" for-each-ref --count=1) ]]; then
            git clone --quiet "$bare" "$stage"
        else
            mkdir -p -- "$stage"
            git -C "$stage" init --quiet -b master
            git -C "$stage" remote add origin "$bare"
        fi
    else
        # Best effort, like the original: an empty bare repo has no origin/master yet.
        git -C "$stage" fetch --quiet origin 2>/dev/null || true
        git -C "$stage" reset --hard --quiet origin/master 2>/dev/null || true
    fi

    # -- 3b. wipe worktree, keep .git --------------------------------------
    find "$stage" -mindepth 1 -maxdepth 1 ! -name .git -exec rm -rf -- {} +

    # -- 3c. copy module source in FLAT ------------------------------------
    # Sorted, so that a file name collision resolves the same way on every run.
    file_count=0
    while IFS= read -r -d '' f; do
        target="$stage/$(basename -- "$f")"
        if [[ -e $target ]]; then warn "Namenskollision beim Flachkopieren: $(basename -- "$f") (ueberschrieben durch $f)"; fi
        cp -f -- "$f" "$target"
        file_count=$((file_count + 1))
    done < <(find "$module_dir" -type f -name '*.cs' -print0 | sort -z)
    info "$file_count Quelldatei(en) kopiert"

    for extra in "${extras[@]}"; do
        p="$src_root/$extra"
        if [[ -e $p ]]; then
            cp -f -- "$p" "$stage/$(basename -- "$extra")"
            info "extra: $extra"
        else
            warn "extraFile fehlt: $p"
        fi
    done

    # -- 3d. README + .gitignore -------------------------------------------
    if [[ -e "$comp_dir/README.md" ]]; then
        cp -f -- "$comp_dir/README.md" "$stage/README.md"
    else
        warn "README.md fehlt fuer $id - PackageReadmeFile wird den Pack sprengen"
    fi

    if [[ -e "$comp_dir/.gitignore" ]]; then cp -f -- "$comp_dir/.gitignore" "$stage/.gitignore"; fi

    # Shared metadata travels with the component, otherwise the standalone build
    # loses Authors/License/Copyright and packs an anaemic .nuspec.
    copy_root_file 'Directory.Build.props' "$stage"

    # Central Package Management travels with the component too: the component csproj
    # files have version-less <PackageReference/> (CPM in the monorepo), so a standalone
    # build without this file fails with NU1015 "no version specified" the moment it
    # leaves the monorepo - confirmed by an actual restore against the flattened repo.
    copy_root_file 'Directory.Packages.props' "$stage"

    # global.json travels with the component too. build/post-receive builds in a mktemp
    # directory outside the monorepo tree, so without this file the pinned net8.0 SDK band
    # never applies there and the build silently falls back to whatever SDK is ambient on
    # the machine - confirmed by an actual build that produced a genuinely different compile
    # error (CS0029 in AutoResourceFontResolver.cs) under an ambient .NET 10 SDK versus a
    # clean build under the pinned 8.0.100 band.
    copy_root_file 'global.json' "$stage"

    # -- 3e. rewrite csproj -------------------------------------------------
    src_csproj="$comp_dir/$id.csproj"
    if [[ ! -e $src_csproj ]]; then warn "csproj fehlt: $src_csproj - uebersprungen"; continue; fi

    dst_csproj="$stage/$id.csproj"
    convert_csproj "$src_csproj" "$dst_csproj"

    # Add a <PackageVersion/> for every rewritten intra-family dependency to the
    # component's copy of Directory.Packages.props - CPM requires one for every
    # PackageReference once it is enabled, and these dependencies aren't in the
    # monorepo's Directory.Packages.props (only external packages are).
    staged_dpp="$stage/Directory.Packages.props"
    if ((${#DEP_IDS[@]} > 0)) && [[ -e $staged_dpp ]]; then
        add_package_versions "$staged_dpp"
    fi

    if ((dry_run)); then
        printf '%s--- %s.csproj (DryRun) ---%s\n' "$C_GRAY" "$id" "$C_RESET"
        cat -- "$dst_csproj"
        add_summary "$id" 'dry-run'
        continue
    fi

    # -- 3f. commit + push --------------------------------------------------
    git -C "$stage" add -A
    if [[ -z $(git -C "$stage" status --porcelain) ]]; then
        info "keine Aenderung -> kein Push, kein Release"
        add_summary "$id" 'unchanged'
        continue
    fi

    # Carry the monorepo subject through verbatim, so that feat:/fix:/BREAKING
    # in the monorepo produce the matching bump in the component repo. The old
    # setup hardcoded "sync: ..." as the subject, which never matched the hook's
    # keyword rules -- so nothing ever released unless you got lucky.
    msg="$src_subject"$'\n\n'"sync: $id @ $src_sha"
    git -C "$stage" -c user.name='xy sync' -c user.email='sync@localhost' commit --quiet -m "$msg"

    info "push -> $bare"
    git -C "$stage" push --quiet origin master || die "Push fuer $id fehlgeschlagen."

    add_summary "$id" 'pushed'
done

step 'Zusammenfassung'
if ((${#summary_ids[@]} > 0)); then
    width=9   # length of the "Component" header
    for id in "${summary_ids[@]}"; do ((${#id} > width)) && width=${#id}; done
    printf '%-*s  %s\n' "$width" 'Component' 'Action'
    printf '%-*s  %s\n' "$width" '---------' '------'
    for i in "${!summary_ids[@]}"; do
        printf '%-*s  %s\n' "$width" "${summary_ids[i]}" "${summary_actions[i]}"
    done
fi
printf '\nRelease-Logs pro Komponente: %s/<id>.git/xy-release.log\n\n' "$bare_root"
