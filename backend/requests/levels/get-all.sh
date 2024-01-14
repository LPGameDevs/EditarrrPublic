#!/bin/bash
set -eo pipefail

. requests/set-vars.sh

echo "Getting most recently updated levels"
curl ${URL}?'limit=2' | python3 -m json.tool
echo

echo "Getting the second page of most recently updated levels"
curl ${URL}?'cursor=4f8a034d-3b9c-430d-b9bf-1ca458625bab' | python3 -m json.tool
echo

LIMIT=1

echo "Getting the oldest levels"
curl ${URL}?'sort-option=created-at&sort-asc=true&limit='${LIMIT} | python3 -m json.tool
echo

echo "Getting the most recently created levels"
curl ${URL}?'sort-option=created-at&sort-asc=false&limit='${LIMIT} | python3 -m json.tool
echo

echo "Getting the levels with the highest average scores (the hardest levels)"
curl ${URL}?'sort-option=avg-score&sort-asc=false&limit='${LIMIT} | python3 -m json.tool
echo

echo "Getting the levels with the most scores (the most played)" 
curl ${URL}?'sort-option=total-scores&sort-asc=false&limit='${LIMIT} | python3 -m json.tool
echo

echo "Getting the highest rated levels (the most popular ones)"
curl ${URL}?'sort-option=avg-rating&sort-asc=false&limit='${LIMIT} | python3 -m json.tool
echo

echo "Getting the levels with the most ratings"
curl ${URL}?'sort-option=total-ratings&sort-asc=false&limit='${LIMIT} | python3 -m json.tool
echo

echo "Getting the levels by code, alpha-numerically sorted ascending"
curl ${URL}?'sort-option=level-code&sort-asc=true' | python3 -m json.tool
echo

echo "Getting the levels by creator name, alphabetically sorted ascending"
curl ${URL}?'sort-option=creator-name&sort-asc=true' | python3 -m json.tool
echo

echo "Getting levels that have the 'test' or 'GDFG' labels"
curl ${URL}?'any-of-labels=test,GDFG' | python3 -m json.tool
echo

echo "Getting levels that have '2' in their name"
curl ${URL}?'nameContains=2' | python3 -m json.tool
echo

echo "Getting levels that have the 'test' or 'GDFG' labels AND have '2' in their name"
curl ${URL}?'any-of-labels=test,GDFG&nameContains=2' | python3 -m json.tool
echo

echo "Getting levels paginates correctly even if the 'nameContains' (or other filters that us DDB FilterExpression) is provided (this query should return an level)"
curl ${URL}?'nameContains=56197' | python3 -m json.tool
echo