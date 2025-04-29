#!/bin/bash

# Usage:
# ./generate-workflow.sh <ProjectName> <NuGetId> <GitHubSecretName>

# Read parameters
PROJECT_NAME=$1
NUGET_ID=$2
SECRET_NAME=$3

# Validate parameters
if [ $# -ne 3 ]; then
    echo "Usage: $0 <ProjectName> <NuGetId> <GitHubSecretName>"
    echo "Example: ./generate-workflow.sh xyPortHelper xyporthelper XYPORTHELPER_API_KEY"
    exit 1
fi

# Template and output
TEMPLATE="ci-template-xyProjects.yml"
OUTPUT=".github/workflows/ci-${PROJECT_NAME}.yml"

# Check if template exists
if [ ! -f "$TEMPLATE" ]; then
    echo "Error: Template file '$TEMPLATE' not found."
    exit 1
fi

# Create workflows folder if not exists
mkdir -p .github/workflows

# Create new workflow file by replacing placeholders
sed \
  -e "s|<ProjectName>|$PROJECT_NAME|g" \
  -e "s|<nuget-id>|$NUGET_ID|g" \
  -e "s|<YOUR_SECRET_KEY>|$SECRET_NAME|g" \
  "$TEMPLATE" > "$OUTPUT"

# Success message
echo "✅ Workflow created successfully: $OUTPUT"
