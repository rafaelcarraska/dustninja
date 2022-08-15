# DustNinja Instruction For Docker`s

### build image docker

exec command below

```
docker build -t dustmedicalninja -f DustMedicalNinja/Dockerfile .
```

### run container docker with expose mongodb instaled on machine

exec command below

```
docker run -d -e ROOT_URL=http://localhost -e MONGO_URL=mongodb://localhost:27017 --network="host" -p 80:80 --name dustcore dustmedicalninja
```

## Read from faq:
[Dockerize a .NET Core application](https://docs.docker.com/engine/examples/dotnetcore/)
