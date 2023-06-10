#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

LEVEL_ID="<LEVEL_ID>"
if [ $# -eq 1 ]; then
  LEVEL_ID=$1
fi

curl -X "PUT" -H "Content-Type: application/json" -d '{"name": "First Level v2", "status": "published", "levelData": {"foo": "bar2"}}' ${URL}/levels/${LEVEL_ID} | python3 -m json.tool