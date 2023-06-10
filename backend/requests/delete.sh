#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

LEVEL_ID="<LEVEL_ID>"
if [ $# -eq 1 ]; then
  LEVEL_ID=$1
fi

curl -X "DELETE" ${URL}/${LEVEL_ID} | python3 -m json.tool