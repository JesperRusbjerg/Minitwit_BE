docker-compose -f dev-docker-compose.yml rm -f &&
docker build -t miniapi -f Minitwit_BE.Api/Dockerfile . && 
docker-compose -f dev-docker-compose.yml up
