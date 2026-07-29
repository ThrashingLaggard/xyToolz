#!/bin/sh
exec powershell.exe -NoProfile -ExecutionPolicy Bypass -File "$(dirname "$0")/xy-sync-components.ps1"