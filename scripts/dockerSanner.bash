#!/bin/bash

docker scan --json --group-issues jrusbjerg/twitbackend > dockerscan.json

cat dockerscan.json | grep '"severity": "critical"' > critical.json

if [ -s critical.json ];
then
  echo "Critical issues found in backend"
  rm dockerscan.json
  rm critical.json
  exit 1
else
  echo "No critical issues found in backend"
  rm dockerscan.json
  rm critical.json
fi

docker scan --json --group-issues  snyklabs/tomas-goof > dockerscan.json

cat dockerscan.json | grep '"severity": "critical"' > critical.json

if [ -s critical.json ];
then
  echo "Critical issues found in mariaDB"
  rm dockerscan.json
  rm critical.json
  exit 1
else
  echo "No critical issues found in backend"
  rm dockerscan.json
  rm critical.json
fi


exit 0
