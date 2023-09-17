#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

curl -X "POST" -H "Content-Type: application/json" -d \
    '{"name": "First Level", "username": "MurphysDad", "status": "published", "levelData": {"foo": "bar"} }' \
    ${URL} | python3 -m json.tool