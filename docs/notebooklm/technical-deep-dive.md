# 校車管理系統技術深潛說明

## 文件目的

這份文件提供較完整的工程說明，讓 NotebookLM 在產生技術型簡報或影片內容時，不只會講功能，也能講出後端設計選擇與其原因。

## 技術棧

- Runtime: `.NET 8`
- Web Framework: `ASP.NET Core Web API`
- Authentication: `ASP.NET Core Identity + JWT`
- ORM: `Entity Framework Core`
- Database: `Azure SQL Database`
- Logging: `Serilog`
- API Docs: `Swagger + XML comments`
- Testing: `xUnit + FluentAssertions + WebApplicationFactory + SQLite in-memory`

## 架構風格

本專案採用模組化單體設計，而不是在第一版就拆成多服務。這樣做的原因是：

- 面試 Demo 更重視邏輯邊界是否清楚，而不是服務數量
- 校車管理流程彼此關聯高，先放在同一個部署單位更容易維護
- 模組分層清楚時，未來若要拆分服務仍有基礎

## 分層責任

### Api

- 對外暴露 REST endpoint
- 處理授權屬性
- 設定 Swagger、ProblemDetails、CORS、Health Checks

### Application

- 定義服務介面
- 放置協調層與跨模組規則
- 例如調度衝突檢查器

### Domain

- 放核心實體與商業規則
- 包含搭乘登記、點名、通知、調度、報表等模型
- 放列舉與規則，例如 `TripDirection`、`RouteType`、`AttendanceStatus`

### Infrastructure

- 實作資料存取
- 實作登入、JWT、通知、背景工作
- 管理 EF Core DbContext、migration 與 seed data

### Contracts

- 定義 API request / response DTO
- 讓外部契約與內部實體分離

## 驗證與授權設計

### 為什麼使用 account 登入

本專案刻意不用 email 當作唯一登入入口，而是改成 account：

- 學生用學號，例如 `S10001`
- 教師用工號，例如 `T0001`
- 管理員用工號，例如 `E0001`
- 家長用手機號碼，例如 `0900-000-003`

這樣的設計更貼近校務現場，也讓 Demo 更有真實感。

### JWT 流程

- `POST /api/auth/login` 取得 access token 與 refresh token
- `POST /api/auth/refresh` 換發新的權杖
- `POST /api/auth/logout` 註銷目前有效 refresh token
- `GET /api/auth/me` 取得目前登入者基本資訊
- `GET /api/auth/context` 提供前端啟動所需的角色上下文資料

### auth context 的價值

`/api/auth/context` 是這個專案很適合展示的一個設計點，因為它能一次回傳：

- 使用者基本資料
- 顯示名稱
- 可存取學生清單
- 教職員 profile 摘要

這讓前端在啟動時能快速知道要顯示哪些功能與資料範圍。

## 核心資料模型

### 使用者與角色

- `AspNetUsers`
- `AspNetRoles`
- `RefreshTokens`

### 人員資料

- `Students`
- `Guardians`
- `StaffProfiles`
- `StudentGuardianLinks`

### 營運資料

- `Routes`
- `RouteStops`
- `RouteAssignments`
- `RideRegistrations`
- `AttendanceSessions`
- `AttendanceRecords`
- `DispatchOverrides`

### 通知與報表

- `NotificationJobs`
- `NotificationDeliveries`
- `BroadcastAnnouncements`
- `ReportExports`
- `AuditLogs`

## 重要商業規則

### 每週登記

- 以週為主單位管理搭車安排
- 每日區分上學與放學兩個方向
- 上下學可對應不同路線
- 可複製上一週設定

### 點名

- 點名 session 依路線、日期、方向建立
- 建立時會根據當天搭乘登記產生名單
- 單一學生點名結果可更新為不同出席狀態
- 點名記錄保留緊急聯絡資訊快照

### 調度

- 管理員可針對學生在某一天某方向建立單次覆寫
- 調度邏輯有衝突檢查
- Application 層已有相關測試覆蓋

### 通知

- 支援手動執行提醒
- 支援背景提醒服務
- 可查詢通知寄送歷程
- Mailjet 未啟用時可退回 demo/log 模式

### 報表

- 管理員可建立報表
- 第一版採 CSV 匯出，降低展示期的格式複雜度
- 報表建立後可再透過下載端點取得內容

## 啟動時行為

應用程式啟動時會：

1. 建立必要服務
2. 設定 Swagger 與 Bearer Token 驗證
3. 啟用 Serilog request logging
4. 套用資料庫 migration
5. 執行 seed data
6. 暴露 `/health` 與 `/health/ready`

這代表專案不需要手動灌很多資料，就能直接進 Swagger 開始展示。

## Demo Seed Data

目前內建的示範資料包含：

- 管理員、教師、家長、學生各一組帳號
- 一位學生與其家長關聯
- 管理員與教師 staff profile
- 上學、放學與 Door-to-Door 路線
- 一週的搭車登記資料
- 用於提醒功能的通知模板

建議展示週次是 `2026-03-16` 這一週，因為測試與 seed data 都圍繞這組資料。

## API 設計亮點

### 對面試展示友善

- 所有主要功能都可從 Swagger 直接操作
- Swagger 已設定 Bearer security scheme
- Controller XML 註解可直接生成 API 說明

### 對前端串接友善

- DTO 與 Domain Entity 分離
- `auth/context` 與 `admin/lookups` 這類聚合型端點降低前端初始查詢成本

### 對維護友善

- 啟動時 migration + seed
- ProblemDetails 統一錯誤格式
- CORS 政策與 health check 已納入

## 測試策略與目前覆蓋

### Domain Tests

- `RegistrationWeekTests`
- `ReminderPolicyTests`

### Application Tests

- `DispatchConflictDetectorTests`

### API Integration Tests

- 驗證登入、refresh 與目前使用者資訊
- 驗證 `auth/context` 角色上下文
- 驗證家長查詢週登記與套用上週
- 驗證教師查詢路線與建立點名 session
- 驗證管理員建立報表、執行提醒與查詢 lookups

## 為什麼這個專案適合作為作品集

- 有清楚的商業情境，不是抽象題目
- 有多角色、多模組、多資料關係
- 有 API 文件、測試與可展示資料
- 有工程設計取捨，而不是單純把功能塞進 controller

## 適合讓 NotebookLM 強化的技術簡報主題

1. 為什麼採用模組化單體
2. account-based login 的設計原因
3. 每週登記、調度、點名如何串成完整流程
4. 啟動即 migration + seed 的展示價值
5. 如何用測試證明系統不只是畫面或假資料
