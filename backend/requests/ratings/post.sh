#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

LEVEL_ID="<LEVEL_ID>"
if [ $# -eq 1 ]; then
  LEVEL_ID=$1
fi

curl -X "POST" -H "Content-Type: application/json" -d \
    '{
        "rating": 1, 
        "code": "29995",
        "creator": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
        "creatorName": "Murphys dad"
    }' \
    ${URL}/${LEVEL_ID}/ratings | python3 -m json.tool
