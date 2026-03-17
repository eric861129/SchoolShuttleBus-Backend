# 校車管理系統二面 Demo 指南

## 文件用途

這份文件同時提供：

- 面試現場可直接照著講的講稿重點
- Swagger / Azure / CI/CD 的實際操作流程
- Demo 帳號與建議切換順序
- 面試官常見追問的回答方向

建議 Demo 時間控制在 10 到 15 分鐘。

---

## 一、專案一句話介紹

這是一套以 ASP.NET Core 8 開發的校車管理後端系統，主要解決學生每週搭車登記、老師車上點名、管理者路線與調度管理，以及通知提醒與報表輸出的需求；整體已完成資料庫建模、JWT 驗證、Swagger 文件、Azure 雲端部署與 GitHub Actions CI/CD。

---

## 二、Demo 前準備

### 1. 確認站台可開啟

- API Swagger：
  `https://app-school-shuttlebus-demo-e9c3h5c9btdafeak.westus2-01.azurewebsites.net/swagger/index.html`

### 2. 可用登入帳號

共同密碼：

- `P@ssw0rd!`

建議 Demo 使用帳號：

- 管理員：`E0001`
- 教師：`T0001`
- 家長：`0910-200-001`
- 學生：`S20001`

### 3. 建議展示順序

1. 管理員
2. 教師
3. 家長
4. 學生

這樣最容易先把整體架構講清楚，再逐步切到使用者情境。

---

## 三、開場講稿

可以直接這樣講：

「這個專案是我設計的一套校車管理後端系統，主要服務對象包含管理員、教師、家長與學生。系統支援每週搭乘登記、點名、路線管理、通知提醒以及報表匯出。技術上我使用 ASP.NET Core 8、Entity Framework Core、SQL Server 與 JWT 驗證，並部署到 Azure App Service，資料庫使用 Azure SQL，整體部署流程則透過 GitHub Actions 完成 CI/CD。」 

如果要再補一句：

「這次 Demo 我會先從 Swagger 展示 API 與權限角色，再切換不同角色帳號，展示資料如何隨角色改變。」

---

## 四、系統架構講稿

### 1. 分層架構

可以這樣說：

「整個專案採分層設計，包含 Domain、Application、Infrastructure、API 與 Contracts。Domain 放核心業務模型與列舉，Application 定義服務介面與規則，Infrastructure 實作資料存取、驗證與通知，API 負責對外暴露 REST 端點，Contracts 則定義 API request/response 模型。」

### 2. 驗證與授權

可以這樣說：

「系統使用 JWT 做身分驗證，登入後會依角色授權。這次我把登入方式改成更貼近真實情境的 account 登入：學生用學號、教職員用工號、家長用手機號碼，而不是單純用 email。」

### 3. 資料庫與資料

可以這樣說：

「資料庫由 EF Core migration 管理，啟動時可自動套用 migration。除了內建 seed，我另外準備了一份一次性 Azure SQL demo 資料腳本，包含中文姓名、中文路線、學生、家長、教師、點名與搭車資料，方便面試展示。」

### 4. 雲端部署

可以這樣說：

「目前 API 已部署在 Azure App Service，資料庫是 Azure SQL。CI/CD 使用 GitHub Actions，push 到 main 後會自動 build 並部署到 Azure。」

---

## 五、實際操作流程

## 1. 進 Swagger

操作：

- 打開 `/swagger/index.html`

講稿：

「我先從 Swagger 開始，因為這裡可以快速展示 API 分類、授權方式，以及我對文件化與可維護性的處理。」

補充重點：

- 顯示所有 controller 分類
- Swagger 說明文字已改為中文
- 可以直接用 Bearer token 驗證各角色 API

## 2. 登入並取得 Token

操作：

- 展開 `POST /api/auth/login`
- 輸入：

```json
{
  "account": "E0001",
  "password": "P@ssw0rd!"
}
```

- Execute
- 複製 `accessToken`
- 點 Swagger 右上角 `Authorize`
- 貼上 `Bearer <accessToken>`

講稿：

「這裡可以看到登入欄位不是 email，而是 account。這是我刻意設計成更貼近真實校務流程的登入模式。管理員用工號登入，學生用學號，家長則用手機號碼。」

## 3. 驗證目前登入者

操作：

- 呼叫 `GET /api/auth/me`

講稿：

「這支 API 可以快速驗證目前登入者資訊與角色，方便前端啟動時決定功能權限與可見畫面。」

## 4. 管理員流程

建議展示：

- `GET /api/admin/lookups`
- `POST /api/admin/dispatches`
- `POST /api/admin/broadcasts`
- `POST /api/admin/reports`

講稿：

「管理員主要負責調度、公告與報表。像這支 dispatch override 可以對特定學生某天某個方向的行程進行路線覆寫，這在臨時改站、特殊接送需求時很常見。」

## 5. 教師流程

先重新登入：

```json
{
  "account": "T0001",
  "password": "P@ssw0rd!"
}
```

建議展示：

- `GET /api/routes`
- `GET /api/attendance/sessions`
- `POST /api/attendance/sessions`
- `PATCH /api/attendance/records/{recordId}`

講稿：

「教師登入後能看到自己被指派的路線，並依據當日搭乘登記資料建立點名場次。點名紀錄也會保留緊急聯絡電話快照，這樣就算後續家長資料變更，當下的點名紀錄仍能保留原始聯絡資訊。」

## 6. 家長流程

先重新登入：

```json
{
  "account": "0910-200-001",
  "password": "P@ssw0rd!"
}
```

建議展示：

- `GET /api/registrations/weeks/{weekStart}?studentId=...`
- `PUT /api/registrations/weeks/{weekStart}`
- `POST /api/registrations/weeks/{weekStart}/copy-last-week`
- `GET /api/registrations/students/{studentId}/summary`

講稿：

「家長可以替自己的孩子管理每週搭車登記，也可以直接複製上一週設定，減少重複輸入。摘要 API 可以快速看到已登記與實際到車次數。」

## 7. 學生流程

先重新登入：

```json
{
  "account": "S20001",
  "password": "P@ssw0rd!"
}
```

建議展示：

- `GET /api/registrations/weeks/{weekStart}`
- `GET /api/registrations/students/{studentId}/summary`

講稿：

「學生角色也能查看自己的搭乘資料，但可操作範圍會受角色授權限制，這部分是由 JWT 與角色判斷控制。」

---

## 六、推薦 Demo 重點

面試時建議特別強調這幾件事：

- 登入模式有依真實身份改成 account，而不是單純 email
- Swagger 文件與 XML doc 已完整中文化，方便溝通與交接
- 資料模型有完整角色與關聯，不只是簡單 CRUD
- EF Core migration 與 Azure SQL 已打通
- GitHub Actions 已完成雲端部署自動化

---

## 七、CI/CD 講稿

可以這樣說：

「這個專案已經部署到 Azure App Service，資料庫使用 Azure SQL。CI/CD 是透過 GitHub Actions 完成，當 main 分支更新時會自動 build、publish 並部署到 App Service。部署初期我也實際處理過 Azure 連線字串與 App Service 啟動問題，所以這個流程不只是理論設計，而是真的跑過的。」

如果面試官追問：

- Workflow 在哪裡：`.github/workflows/main_app-school-shuttlebus-demo.yml`
- 部署方式：Publish Profile
- 啟動問題怎麼排查：App Service logs、Kudu、connection string 檢查

---

## 八、Azure 講稿

可以這樣說：

「Azure 這邊我使用 App Service 部署 API，Azure SQL 放資料庫，並把必要設定如 JWT、資料庫連線字串放在 App Service 的環境變數中。這樣可以把部署設定與程式碼分離，也比較符合正式環境的配置方式。」

---

## 九、常見追問與回答方向

### 1. 為什麼用分層架構？

回答方向：

「因為這個專案角色多、規則多，如果把 controller、商業邏輯與資料存取全部混在一起，後續會很難維護。分層可以讓授權、業務規則與資料庫邏輯分開，方便測試與擴充。」

### 2. 為什麼登入改成 account？

回答方向：

「在校務場景裡，學生更自然使用學號、教師使用工號、家長則常用手機號碼。用 account 可以把登入體驗做成和真實場景一致，同時又保留 JWT 與角色授權機制。」

### 3. 怎麼保證資料一致性？

回答方向：

「我用 EF Core migration 管理 schema，資料表也有唯一索引與外鍵，例如學生與家長連結、搭乘登記、點名紀錄都有限制，避免重複與脫節資料。」

### 4. 如果要上正式環境，下一步會做什麼？

回答方向：

- 補更完整的角色權限策略
- 增加審計與操作紀錄
- 將通知服務接到正式供應商
- 補前端管理介面
- 加強整合測試與觀測機制

---

## 十、收尾講稿

可以這樣結尾：

「這個專案我不只完成了 API 與資料庫設計，也把 Swagger 文件、角色驗證、Azure 部署與 CI/CD 串起來，讓它是一個可以真正展示、測試與延伸的後端系統。若再往下一步做，我會優先補上更完整的前端與正式通知流程，讓它更接近可上線版本。」
