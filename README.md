# School Shuttle Bus Backend

校車管理系統後端示範專案，使用 `.NET 8`、`ASP.NET Core Web API`、`EF Core`、`ASP.NET Core Identity`、`JWT` 與 `Azure SQL Database` 建置。這份專案以面試 Demo 與作品集展示為主，文件也整理成可直接提供給面試官閱讀的版本。

## 專案亮點

- 模組化單體後端：`Api / Application / Domain / Infrastructure / Contracts`
- 角色權限：`學生`、`家長`、`老師`、`管理員`
- 核心流程：每週登記、套用上週、行動點名、路線管理、跨路線調度、提醒寄信、全域通知、報表匯出
- 展示友善：Swagger 可直接操作、內建 demo seed data、角色切換容易
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
  demo-guide.md
  notebooklm/
```

## 已實作功能

- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`
- `GET /api/auth/context`
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
- `GET /api/admin/lookups`
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

所有 seed 帳號密碼皆為 `P@ssw0rd!`，登入欄位請使用 `account`：

- `E0001` 管理員
- `T0001` 教師
- `0900-000-003` 家長
- `S10001` 學生

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
- [Demo 指南](docs/demo-guide.md)
- [NotebookLM 文件包](docs/notebooklm/README.md)
