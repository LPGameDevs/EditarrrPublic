#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

curl ${URL} | python3 -m json.tool