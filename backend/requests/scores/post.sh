#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

LEVEL_ID="<LEVEL_ID>"
if [ $# -eq 1 ]; then
  LEVEL_ID=$1
fi

curl -X "POST" -H "Content-Type: application/json" -d \
    '{
        "score": "1.0", 
        "code": "Murphys Dads Level",
        "creator": "murphys-dad-id",
        "creatorName": "Murphys Dad"
    }' \
    ${URL}/${LEVEL_ID}/scores | python3 -m json.tool
