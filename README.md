# feed-app-api

### Lage ny migration av database

```shell
dotnet ef migrations add <MigrationName>
```

### Oppdatere lokal database i henhold til siste migration

```shell
dotnet ef database update
```

## Kjøre RabbitMQ
### Installer Docker
### Kjør docker-script
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.11-management
```