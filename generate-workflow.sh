#!/usr/bin/env bash
# Generates .github/workflows/ci-<ProjectName>.yml from ci-template-xyProjects.yml
#
# Usage: ./generate-workflow.sh <ProjectName> <NuGetId> <GitHubSecretName>
#    e.g. ./generate-workflow.sh xyPortHelper xyporthelper XYPORTHELPER_API_KEY
#
# Two things that made the previous version unusable:
#
#  1. The file was committed with CRLF line endings, so the shebang read
#     "#!/bin/bash\r" and the shell answered "bad interpreter: /bin/bash^M".
#     Fixed at the source in .gitattributes (*.sh text eol=lf).
#
#  2. The placeholders were <ProjectName> / <nuget-id> / <YOUR_SECRET_KEY>.
#     Angle brackets are also YAML/shell metacharacters and are a nightmare to
#     grep for. They are now __PROJECT__ / __NUGETID__ / __SECRET__.

set -euo pipefail

if [ $# -ne 3 ]; then
    echo "Usage: $0 <ProjectName> <NuGetId> <GitHubSecretName>"
    echo "Example: $0 xyPortHelper xyporthelper XYPORTHELPER_API_KEY"
    exit 1
fi

PROJECT_NAME=$1
NUGET_ID=$2
SECRET_NAME=$3

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
TEMPLATE="$SCRIPT_DIR/ci-template-xyProjects.yml"
OUTDIR=".github/workflows"
OUTPUT="$OUTDIR/ci-${PROJECT_NAME}.yml"

if [ ! -f "$TEMPLATE" ]; then
    echo "Error: Template '$TEMPLATE' not found." >&2
    exit 1
fi

# A GitHub secret name must be [A-Z0-9_] and may not start with a digit.
# Getting this wrong produces an empty api-key at runtime and a push that
# fails with a completely unrelated-looking 401.
if ! printf '%s' "$SECRET_NAME" | grep -qE '^[A-Z_][A-Z0-9_]*$'; then
    echo "Error: '$SECRET_NAME' ist kein gueltiger Secret-Name (nur A-Z, 0-9, _; nicht mit Ziffer beginnend)." >&2
    exit 1
fi

mkdir -p "$OUTDIR"

sed \
  -e "s|__PROJECT__|${PROJECT_NAME}|g" \
  -e "s|__NUGETID__|${NUGET_ID}|g" \
  -e "s|__SECRET__|${SECRET_NAME}|g" \
  "$TEMPLATE" > "$OUTPUT"

# Guard against a template that still has unreplaced placeholders.
if grep -q '__[A-Z]*__' "$OUTPUT"; then
    echo "Warnung: nicht ersetzte Platzhalter in $OUTPUT:" >&2
    grep -n '__[A-Z]*__' "$OUTPUT" >&2
fi

echo "Workflow erstellt: $OUTPUT"
echo "Nicht vergessen: Secret '$SECRET_NAME' in den Repo-Settings hinterlegen."
