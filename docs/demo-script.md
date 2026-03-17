# 面試 Demo 劇本

## 1. 開場

- 開 Swagger
- 說明這是 backend-first 架構
- 展示 solution 分層與 docs 結構

## 2. Auth

- 用 `admin@demo.local / P@ssw0rd!` 登入
- 在 Swagger Authorize 貼上 JWT
- 呼叫 `/api/auth/me`

## 3. 家長登記流程

- 用 `parent@demo.local` 登入
- 查 `GET /api/registrations/weeks/2026-03-16`
- 呼叫 `copy-last-week` 複製到下週
- 查 summary

## 4. 老師點名流程

- 用 `teacher@demo.local` 登入
- 呼叫 `GET /api/routes`
- 建立 `POST /api/attendance/sessions`
- 更新單筆 attendance record
- 完成 session

## 5. 管理員流程

- 建立 dispatch override
- 手動觸發 reminders
- 發送 broadcast
- 產生 report 並下載 CSV

## 6. 工程亮點收尾

- 展示 migration、tests、GitHub Actions workflow
- 展示 Azure SQL / App Service / Mailjet 設定說明
