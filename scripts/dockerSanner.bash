#!/bin/bash

docker scan --json --group-issues jrusbjerg/twitbackend > dockerscan.json

if [ -s dockerscan.json ];
then
  echo "Critical issues found in backend"
  cat critical.json
  rm dockerscan.json
  rm critical.json
  exit 1
else
  echo "No critical issues found in backend"
  rm dockerscan.json
  rm critical.json
fi

docker scan --json --group-issues  snyklabs/tomas-goof > dockerscan.json

if [ -s dockerscan.json ];
then
  echo "Critical issues found in mariaDB"
  rm dockerscan.json
  exit 1
else
  echo "No critical issues found in backend"
  rm dockerscan.json
  exit 0
fi



