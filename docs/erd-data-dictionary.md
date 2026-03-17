# ERD / Data Dictionary

## 核心資料表

- `AspNetUsers`
  Identity 使用者主表
- `AspNetRoles`
  角色表
- `Students`
  學生基本資料與預設路線
- `Guardians`
  家長資料
- `StaffProfiles`
  老師 / 行政資料
- `StudentGuardianLinks`
  學生與家長關聯
- `Routes`
  路線主檔
- `RouteStops`
  路線站點
- `RouteAssignments`
  路線與老師指派
- `RideRegistrations`
  每日上學/放學登記
- `AttendanceSessions`
  點名 session
- `AttendanceRecords`
  單一學生點名結果
- `DispatchOverrides`
  跨路線調度
- `NotificationJobs`
  一次通知任務
- `NotificationDeliveries`
  單封通知寄送紀錄
- `BroadcastAnnouncements`
  全域廣播記錄
- `ReportExports`
  已產出的報表
- `AuditLogs`
  稽核日誌
- `RefreshTokens`
  refresh token 儲存表

## 關聯摘要

- `Students` 1..n `RideRegistrations`
- `Students` 1..n `AttendanceRecords`
- `Students` n..n `Guardians` via `StudentGuardianLinks`
- `Routes` 1..n `RouteStops`
- `Routes` 1..n `RouteAssignments`
- `Routes` 1..n `AttendanceSessions`
- `AttendanceSessions` 1..n `AttendanceRecords`
- `NotificationJobs` 1..n `NotificationDeliveries`
