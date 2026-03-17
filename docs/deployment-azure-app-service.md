# Azure App Service 部署指南

## 目標

- App runtime: Azure App Service
- Database: Azure SQL Database
- Secrets: App Service Configuration
- Optional mail provider: Mailjet

## 需要的設定

- `ConnectionStrings__SchoolShuttleBus`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__SigningKey`
- `Mailjet__Enabled`
- `Mailjet__ApiKey`
- `Mailjet__ApiSecret`
- `Mailjet__FromEmail`
- `Mailjet__FromName`
- `Reminders__Enabled`
- `Reminders__TimeZoneId`
- `Reminders__RunHourLocal`

## 建議步驟

1. 建立 Azure SQL Database
2. 建立 Azure App Service
3. 將上述設定加入 App Service Configuration
4. 在 GitHub Secrets 補上 publish profile 與 SQL connection string
5. 使用 workflow 自動 build / test / migrate / deploy

## 啟動時行為

- 應用程式啟動會執行 `dbContext.Database.MigrateAsync()`
- 首次啟動後自動 seed demo 帳號與資料
- `ReminderBackgroundService` 會依設定時區與時段自動執行
