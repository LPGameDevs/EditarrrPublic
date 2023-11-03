#!/bin/bash
set -eo pipefail

(cd lambda && npm ci && npm test)
