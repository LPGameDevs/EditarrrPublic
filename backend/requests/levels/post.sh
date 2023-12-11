#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

curl -X "POST" -H "Content-Type: application/json" -d \
    '{
        "name": "12345", 
        "creator": {
            "id": "2cbe4992-950e-43bc-9fec-4e6138b5ce74",
            "name": "Murphys dad"
        }, 
        "status": "PUBLISHED", 
        "data": {"foo": "bar"},
        "labels": [ "test" ]
    }' \
    ${URL} | python3 -m json.tool