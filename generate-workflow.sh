#!/bin/bash

# Usage:
# ./generate-workflow.sh <ProjectName> <RelativePathToCsproj> <NuGetId> <GitHubSecretName>

# Read parameters
PROJECT_NAME=$1
RELATIVE_PATH=$2
NUGET_ID=$3
SECRET_NAME=$4

# Validate parameters
if [ $# -ne 4 ]; then
    echo "Usage: $0 <ProjectName> <RelativePathToCsproj> <NuGetId> <GitHubSecretName>"
    exit 1
fi

# Create destination directory if not exists
mkdir -p .github/workflows

# Replace placeholders in the template and create the new workflow
sed \
    -e "s|<relative-path-to>/<ProjectName>|$RELATIVE_PATH/$PROJECT_NAME|g" \
    -e "s|<nuget-id>|$NUGET_ID|g" \
    -e "s|<YOUR_SECRET_KEY>|$SECRET_NAME|g" \
    ci-template-xyProjects.yml > .github/workflows/ci-$PROJECT_NAME.yml

echo "Workflow created successfully: .github/workflows/ci-$PROJECT_NAME.yml"
