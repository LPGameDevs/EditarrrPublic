#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

curl -X "POST" -H "Content-Type: application/json" -d \
    '{
        "name": "Second Level", 
        "creator": {
            "id": "user1",
            "name": "MurphysDad"
        }, 
        "status": "PUBLISHED", 
        "data": {"foo": "bar"} 
    }' \
    ${URL} | python3 -m json.tool