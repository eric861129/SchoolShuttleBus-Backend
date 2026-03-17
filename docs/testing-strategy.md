# 測試策略

## 測試層級

- `Domain.Tests`
  商業規則與 policy
- `Application.Tests`
  衝突判斷與協調邏輯
- `Api.Tests`
  真實 HTTP 整合測試，採 `WebApplicationFactory` + `SQLite in-memory`

## 目前覆蓋場景

- 週登記起始日驗證
- 週四/週五提醒判斷
- 跨路線調度衝突檢查
- 登入與 `me`
- 家長查週登記
- 套用上週
- 管理員產生報表
- 管理員手動提醒
- 老師查路線
- 老師建立點名 session

## 未來可擴充

- 更多 authorization 邊界測試
- migration 回歸測試
- 背景工作時區測試
- Mailjet 實際 API contract test
