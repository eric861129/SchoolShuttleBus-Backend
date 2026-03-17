# 校車管理系統角色流程與 Demo 情境

## 文件目的

這份文件的重點不是技術名詞，而是把系統講成一個有角色、有情境、有前後因果的故事。這很適合提供給 NotebookLM 生成簡報大綱、操作型影片腳本或 Demo 旁白。

## Demo 固定前提

- API 展示入口：`/swagger/index.html`
- 共同密碼：`P@ssw0rd!`
- 管理員帳號：`E0001`
- 教師帳號：`T0001`
- 家長帳號：`0900-000-003`
- 學生帳號：`S10001`
- 建議展示週次：`2026-03-16`

## 整體故事線

這個系統可以被理解成一條完整的校車營運流程：

1. 家長先替孩子填寫下週要不要搭車
2. 管理者維護路線、站點與教師指派
3. 如果有特殊狀況，管理者可做單次調度
4. 教師在搭車當天依照名單建立點名 session
5. 教師逐筆更新學生出席狀態
6. 系統保留通知與報表資料，方便管理端追蹤

## 角色一：管理員

### 角色定位

管理員是最能展現系統完整度的角色，因為他能接觸到路線、調度、公告、報表與查詢資料。

### 建議展示流程

1. 使用 `E0001` 登入
2. 呼叫 `GET /api/auth/me`
3. 呼叫 `GET /api/admin/lookups`
4. 呼叫 `POST /api/admin/dispatches`
5. 呼叫 `POST /api/admin/broadcasts`
6. 呼叫 `POST /api/admin/reports`
7. 呼叫 `GET /api/admin/reports/{reportId}`
8. 呼叫 `POST /api/notifications/reminders/run`
9. 呼叫 `GET /api/notifications/history`

### 這段流程想傳達的重點

- 系統不是只有查詢，還支援管理端操作
- 管理員能看到用於前端表單的 lookup 資料
- 報表可以真正生成並下載
- 提醒功能不是假按鈕，而是有通知任務與寄送紀錄

### 管理員 Demo 講稿方向

可以這樣說：

「管理員主要負責跨角色協調，所以我讓這個角色可以處理路線、調度、公告與報表。像 dispatch override 就是針對臨時改站或特殊接送需求做單次覆寫，不需要去改動學生的長期預設路線。」

## 角色二：教師

### 角色定位

教師負責車上點名，是最能體現「資料不是靜態存在，而是會被營運流程使用」的角色。

### 建議展示流程

1. 使用 `T0001` 登入
2. 呼叫 `GET /api/auth/context`
3. 呼叫 `GET /api/routes`
4. 呼叫 `GET /api/attendance/sessions`
5. 呼叫 `POST /api/attendance/sessions`
6. 呼叫 `PATCH /api/attendance/records/{recordId}`
7. 呼叫 `POST /api/attendance/sessions/{sessionId}/complete`

### 這段流程想傳達的重點

- 教師只會看到自己被指派的路線
- 點名 session 會根據登記資料建立名單
- 點名紀錄中保留緊急聯絡電話快照，方便營運現場處理問題

### 教師 Demo 講稿方向

可以這樣說：

「教師端不是單純查名單，而是把前面家長登記與管理端路線資料真正串進營運流程。建立點名場次後，教師就能用這份名單逐筆更新學生搭乘狀態。」

## 角色三：家長

### 角色定位

家長是最貼近真實使用者的角色，能展示系統如何處理週期性操作與減少重複輸入。

### 建議展示流程

1. 使用 `0900-000-003` 登入
2. 呼叫 `GET /api/auth/context`
3. 呼叫 `GET /api/registrations/weeks/2026-03-16?studentId=<studentId>`
4. 呼叫 `PUT /api/registrations/weeks/2026-03-16`
5. 呼叫 `POST /api/registrations/weeks/2026-03-23/copy-last-week?studentId=<studentId>`
6. 呼叫 `GET /api/registrations/students/{studentId}/summary`

### 這段流程想傳達的重點

- 家長不是被動查看，而是主動管理孩子的每週搭乘安排
- 系統針對固定週期需求提供 `copy last week`
- 可快速看到已登記次數與實際出席次數

### 家長 Demo 講稿方向

可以這樣說：

「家長每週都要處理搭車安排，所以我把一週當成主要操作單位。這樣比較貼近真實使用情境，也能透過 copy-last-week 降低重複輸入成本。」

## 角色四：學生

### 角色定位

學生角色適合拿來展示權限邊界與自助查詢能力。

### 建議展示流程

1. 使用 `S10001` 登入
2. 呼叫 `GET /api/auth/context`
3. 呼叫 `GET /api/registrations/weeks/2026-03-16?studentId=<studentId>`
4. 呼叫 `GET /api/registrations/students/{studentId}/summary`

### 這段流程想傳達的重點

- 學生也能看到自己的搭乘資料
- 權限範圍會因角色不同而有所限制
- 同一組 API 可以服務不同角色，但資料可見範圍不同

## 最佳 Demo 順序

最推薦的順序是：

1. 管理員
2. 教師
3. 家長
4. 學生

原因如下：

- 先用管理員把整體系統範圍講清楚
- 再用教師帶出營運現場使用情境
- 接著用家長展示週期性業務流程
- 最後用學生收斂到個人視角與權限邊界

## Demo 時可以主動強調的設計點

- 使用 `account` 登入而不是 email，因為更符合校務現場
- 家長、學生、教師、管理員共享同一套平台，但資料邊界不同
- 每週登記、調度、點名、通知與報表是互相連動的，不是各自獨立的 CRUD 模組
- Swagger 不只是測試工具，也是後端展示介面

## 適合交給 NotebookLM 的生成任務

可以要求 NotebookLM：

- 生成一份 8 頁左右的角色流程簡報
- 生成一段 5 分鐘的產品流程解說稿
- 生成一段依角色切換的 Demo 旁白
- 生成一份「先管理端、再教師、再家長」的展示大綱
