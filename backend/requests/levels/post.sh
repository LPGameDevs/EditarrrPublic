#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

curl -X "POST" -H "Content-Type: application/json" -d \
    '{
        "name": "Murphys Dads Level", 
        "creator": {
            "id": "murphys-dad-id",
            "name": "Murphys Dad"
        }, 
        "status": "PUBLISHED", 
        "data": {"foo": "bar"} 
    }' \
    ${URL} | python3 -m json.tool