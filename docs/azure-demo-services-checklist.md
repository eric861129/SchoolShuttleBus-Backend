# Azure Demo 服務清單與設定指南

## 目的

這份文件是給面試 Demo 用的 Azure 部署清單。假設你**已經建立好免費版 Azure SQL Database**，接下來只補最小但夠完整的雲端服務，讓你可以展示：

- API 已上雲
- 資料庫已連線
- 設定有環境化
- 健康檢查與日誌可觀察
- 背景提醒工作有合理部署方式

## 建議建立的 Azure 服務

### 1. Resource Group

建議先建立一個專用 Resource Group，例如：

- `rg-school-shuttlebus-demo`

用途：

- 把 App Service、App Service Plan、Application Insights、Azure SQL 放在同一組資源裡
- 面試時比較好講資源邊界
- 後續刪除 Demo 環境也簡單

### 2. Azure SQL Database

你已經有這個，可以直接沿用。請確認以下設定：

- 你有對應的 **SQL logical server**
- 你知道 server name、database name、admin login
- Firewall 已加入你的本機 IP，方便本地 migration / 連線測試
- 如果 App Service 要直接連，最簡單的 Demo 做法是先允許 Azure services 存取 SQL server

這個專案需要的重點設定：

- 連線字串放到 App Service 的 `Application settings`
- key 名稱請用 `ConnectionStrings__SchoolShuttleBus`
- API 啟動時會執行 migration，所以 App Service 需要有足夠權限連到 Azure SQL

### 3. App Service Plan

這是 Web App 的運算資源。

Demo 建議：

- 如果你只示範「手動提醒」與一般 API：可以用最低成本方案
- 如果你要示範 `ReminderBackgroundService` 的自動排程：建議 **Basic B1 以上**

原因：

- 免費 / 共享方案容易休眠
- 背景工作在休眠時不會穩定執行
- `Always On` 也通常需要 Basic 以上

建議命名：

- `asp-school-shuttlebus-demo`

### 4. Web App (Azure App Service)

這是主要 API 服務。

建議命名：

- `app-school-shuttlebus-demo`

建立時建議選項：

- Publish: `Code`
- Runtime stack: `.NET 8 (LTS)`
- Operating System: `Windows` 或 `Linux` 都可以
- Region: 與 Azure SQL 盡量同區域
- Pricing plan: 綁到你剛建立的 App Service Plan

我對這個專案的建議：

- 想最穩定照 GitHub Actions + publish profile 跑：`Windows Code` 很直觀
- 想強調比較現代雲端：可以選 `Linux`
- 面試 Demo 優先考量是穩定與容易解釋，不是一定要最雲原生

### 5. Application Insights

強烈建議建立，因為非常適合面試時展示「雲端可觀察性」。

建議命名：

- `appi-school-shuttlebus-demo`

用途：

- 看 request / failure / traces
- Demo 時可以說明 API 上線後如何查錯與看健康狀況

如果你要簡化：

- 這個不是 API 正常運作的必要條件
- 但它是面試加分項

### 6. 可選：Key Vault

Demo 第一版可以**先不建**。

因為目前專案已經支援把設定直接放在 App Service `Application settings`。如果你想在面試時多講一層成熟度，可以補充：

- v1 Demo: App Service settings
- v2 Production: Key Vault + Managed Identity

## App Service 需要設定什麼

進入：

- `Web App -> Settings -> Environment variables`

至少要設定以下值：

### 資料庫

- `ConnectionStrings__SchoolShuttleBus`

內容請使用 Azure SQL 的完整 connection string，例如：

```text
Server=tcp:<your-server>.database.windows.net,1433;Initial Catalog=<your-db>;Persist Security Info=False;User ID=<user>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### JWT

- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__SigningKey`

建議：

- `Jwt__Issuer = SchoolShuttleBus.Api`
- `Jwt__Audience = SchoolShuttleBus.Clients`
- `Jwt__SigningKey` 請換成一組長度足夠的隨機字串，不要沿用開發預設值

### Mailjet

如果你要實際寄信：

- `Mailjet__Enabled = true`
- `Mailjet__ApiKey`
- `Mailjet__ApiSecret`
- `Mailjet__FromEmail`
- `Mailjet__FromName`

如果你只是先把雲端 API 跑起來，不急著驗證實際 Email：

- `Mailjet__Enabled = false`

此時系統會退回 demo/log 模式，不會真的送出郵件。

### 背景提醒

- `Reminders__Enabled`
- `Reminders__TimeZoneId`
- `Reminders__RunHourLocal`

建議值：

- `Reminders__Enabled = true`
- `Reminders__TimeZoneId = Asia/Taipei`
- `Reminders__RunHourLocal = 9`

如果你使用免費 / 共享方案，建議先用：

- `Reminders__Enabled = false`

因為 App 休眠後背景服務不穩定，面試時改用手動 endpoint：

- `POST /api/notifications/reminders/run`

## App Service 建議額外設定

### Health Check

進入：

- `Web App -> Monitoring -> Health check`

設定：

- path 使用 `/health/ready`

理由：

- 這個專案已經有 readiness health endpoint
- 面試時可以直接展示「平台如何知道 App 是健康的」

### Always On

進入：

- `Web App -> Settings -> Configuration / General settings`

設定：

- `Always On = On`

前提：

- 通常需要 Basic 以上方案

理由：

- 背景服務要穩定執行
- 避免 Demo 前第一次打 API 太慢

### HTTPS Only

建議：

- `HTTPS Only = On`

理由：

- 這是最基本的安全設定
- JWT / connection string 都不該走 HTTP

## Azure SQL 建議檢查項目

### Firewall

請確認至少有以下其中一種：

- 你的本機 IP 已加入 firewall
- Azure services 被允許存取 SQL server

Demo 最簡單做法：

- 本機開發時加自己 IP
- 雲端 Demo 時先允許 Azure services

如果你要更嚴謹：

- 改成只允許 App Service outbound IP

### Connection Policy / Networking

Demo 階段先以「能穩定連線」為主，不必一開始就做最嚴格網路隔離。Private Endpoint 可以作為面試時的加值延伸，而不是 v1 必做。

## 最小可用 Azure Demo 組合

如果你已經有 Azure SQL，最小建議如下：

1. Resource Group
2. App Service Plan
3. Web App
4. Azure SQL Database

這樣就能完成：

- API 上雲
- DB 連線
- Swagger Demo
- JWT 登入
- 手動提醒
- 報表匯出

## 面試加分 Azure Demo 組合

如果你想多講一點成熟度，再加：

1. Application Insights
2. Always On
3. Health Check
4. 真實 Mailjet 設定

這樣你就能說：

- 有基本 observability
- 有背景工作部署考量
- 有健康檢查
- 有真實外部整合

## 建議的建立順序

1. 確認 Azure SQL 可用
2. 建立 Resource Group
3. 建立 App Service Plan
4. 建立 Web App
5. 設定 Environment variables
6. 設定 SQL Firewall
7. 設定 Health Check / Always On
8. 建立 Application Insights
9. 從 GitHub Actions 或本機發佈

## 你目前的建議路線

因為你已經有免費版 Azure SQL，最務實的做法是：

1. 先建 `App Service Plan`
2. 再建 `Web App`
3. 把 `ConnectionStrings__SchoolShuttleBus` 和 JWT 設好
4. 先讓 API 成功上線
5. 視預算決定要不要把 Plan 升到 Basic 來開 `Always On`
6. 最後再補 `Application Insights`

## 官方參考文件

- Azure App Service overview: https://learn.microsoft.com/en-us/azure/app-service/overview
- Configure app settings in App Service: https://learn.microsoft.com/en-us/azure/app-service/configure-common
- Monitor ASP.NET Core with Application Insights: https://learn.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core
- Configure health check in App Service: https://learn.microsoft.com/en-us/azure/app-service/monitor-instances-health-check
- Azure SQL Database firewall rules: https://learn.microsoft.com/en-us/azure/azure-sql/database/firewall-configure
