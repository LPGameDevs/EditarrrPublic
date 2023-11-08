#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

LEVEL_ID="<LEVEL_ID>"
if [ $# -eq 1 ]; then
  LEVEL_ID=$1
fi

echo "Getting the first 10 top (lowest) scores"
curl ${URL}/${LEVEL_ID}/scores | python3 -m json.tool
echo

echo "Getting the first 5 top (lowest) scores"
curl ${URL}/${LEVEL_ID}/scores?'limit=5' | python3 -m json.tool
echo

echo "Getting the lowest (highest value) 10 scores"
curl ${URL}/${LEVEL_ID}/scores?'sort-asc=false' | python3 -m json.tool
echo

echo "De-dupe scores for users (if there are any)"
curl ${URL}/${LEVEL_ID}/scores?'de-dupe-by-user=true' | python3 -m json.tool
echo