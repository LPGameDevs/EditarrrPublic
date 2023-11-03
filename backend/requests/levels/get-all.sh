#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

curl ${URL}?'limit=20' | python3 -m json.tool

# To get the second page:
# curl ${URL}?'limit=5&cursor=fadefc47-f92d-449a-a01f-38e8dca88365' | python3 -m json.tool