#!/bin/bash
set -eo pipefail

DDB="dynamodb"

if docker ps -f "name=$DDB" --format '{{.Names}}' | grep -q "^$DDB$"; then
    echo "Stopping running container '$DDB'..."
    docker stop $DDB
fi