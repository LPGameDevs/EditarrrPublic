#!/usr/bin/env bash

set -e
set +x

# Check for changelog.
if test -f "$UNITY_DIR/RELEASE.md"; then
    echo 'Found release file. Adding description variable.'
    EXTRA_DESCRIPTION=$(cat $UNITY_DIR/RELEASE.md)
fi
