# School Shuttle Bus Backend Demo

康橋智慧交通車登記系統後端示範專案，使用 `.NET 8`、`ASP.NET Core Web API`、`EF Core`、`ASP.NET Core Identity`、`JWT` 與 `Azure SQL Database` 建置。專案同時兼顧面試 Demo、作品集展示與實際可維運的工程結構。

## 專案亮點

- 模組化單體後端：`Api / Application / Domain / Infrastructure / Contracts`
- 角色權限：`學生`、`家長`、`老師`、`管理員`
- 核心流程：每週登記、套用上週、行動點名、路線管理、跨路線調度、提醒寄信、全域通知、報表匯出
- 觀測與維運：`Serilog`、health checks、JWT Bearer Swagger、background service、自動 seed data
- 資料庫策略：正式以 `Azure SQL Database` 為主，測試採 `SQLite in-memory`

## 專案結構

```text
src/
  SchoolShuttleBus.Api
  SchoolShuttleBus.Application
  SchoolShuttleBus.Contracts
  SchoolShuttleBus.Domain
  SchoolShuttleBus.Infrastructure
tests/
  SchoolShuttleBus.Api.Tests
  SchoolShuttleBus.Application.Tests
  SchoolShuttleBus.Domain.Tests
docs/
  architecture.md
  api-overview.md
  erd-data-dictionary.md
  deployment-azure-app-service.md
  ci-cd.md
  demo-script.md
  testing-strategy.md
  adr/
```

## 已實作功能

- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`
- `GET /api/registrations/weeks/{weekStart}`
- `PUT /api/registrations/weeks/{weekStart}`
- `POST /api/registrations/weeks/{weekStart}/copy-last-week`
- `GET /api/registrations/students/{studentId}/summary`
- `GET /api/routes`
- `POST /api/routes`
- `PATCH /api/routes/{routeId}`
- `POST /api/routes/{routeId}/stops`
- `POST /api/routes/assignments/{routeId}`
- `GET /api/attendance/sessions`
- `POST /api/attendance/sessions`
- `PATCH /api/attendance/records/{recordId}`
- `POST /api/attendance/sessions/{sessionId}/complete`
- `POST /api/notifications/reminders/run`
- `GET /api/notifications/history`
- `POST /api/admin/dispatches`
- `POST /api/admin/broadcasts`
- `POST /api/admin/reports`
- `GET /api/admin/reports/{reportId}`

## 快速開始

1. 還原工具與套件

```powershell
dotnet tool restore
dotnet restore
```

2. 建立資料庫

```powershell
dotnet dotnet-ef database update --project src/SchoolShuttleBus.Infrastructure --startup-project src/SchoolShuttleBus.Api
```

3. 啟動 API

```powershell
dotnet run --project src/SchoolShuttleBus.Api
```

4. 開啟 Swagger

```text
https://localhost:5001/swagger
```

## Demo 帳號

所有 seed 帳號密碼皆為 `P@ssw0rd!`

- `admin@demo.local`
- `teacher@demo.local`
- `parent@demo.local`
- `student@demo.local`

## 本機設定

`src/SchoolShuttleBus.Api/appsettings.json` 內含開發預設值：

- `ConnectionStrings:SchoolShuttleBus`
- `Jwt:*`
- `Mailjet:*`
- `Reminders:*`

如果要切到真實 Mailjet 寄信，請將以下值改為真實憑證：

- `Mailjet:Enabled = true`
- `Mailjet:ApiKey`
- `Mailjet:ApiSecret`
- `Mailjet:FromEmail`
- `Mailjet:FromName`

## 測試

```powershell
dotnet test SchoolShuttleBus.sln
```

目前測試覆蓋：

- Domain 商業規則
- Application 衝突偵測
- API 整合流程：auth、registrations、routes、attendance、reports、manual reminders

## 文件索引

- [架構說明](docs/architecture.md)
- [API 概覽](docs/api-overview.md)
- [ERD / Data Dictionary](docs/erd-data-dictionary.md)
- [Azure 部署指南](docs/deployment-azure-app-service.md)
- [Azure Demo 服務清單](docs/azure-demo-services-checklist.md)
- [CI/CD 規劃](docs/ci-cd.md)
- [面試 Demo 劇本](docs/demo-script.md)
- [測試策略](docs/testing-strategy.md)
- [ADR-001 Azure SQL over Turso](docs/adr/001-azure-sql-over-turso.md)
