# feed-app-api

### Lage ny migration av database

```shell
dotnet ef migrations add <MigrationName>
```

### Oppdatere lokal database i henhold til siste migration

```shell
dotnet ef database update
```