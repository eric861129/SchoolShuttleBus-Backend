# CI/CD 規劃

## Pipeline 分段

### CI

- `dotnet tool restore`
- `dotnet restore`
- `dotnet build`
- `dotnet test`

### CD

- `dotnet publish src/SchoolShuttleBus.Api`
- `dotnet dotnet-ef database update`
- deploy to Azure App Service

## GitHub Secrets

- `AZURE_WEBAPP_NAME`
- `AZURE_WEBAPP_PUBLISH_PROFILE`
- `AZURE_SQL_CONNECTION_STRING`

## 設計原則

- migration 與 deploy 分開，避免未更新 schema 就切流量
- 沒有 deploy secret 時仍可跑純 CI
- 使用 repo-local `dotnet-ef` 工具，避免 runner 版本漂移
