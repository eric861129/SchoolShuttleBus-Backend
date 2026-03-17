
# 【產品需求規格書 PRD】康橋智慧交通車管理系統 (Demo 版)

## 一、 專案概述 (Project Overview)

本專案旨在為康橋國際學校打造一套現代化的「智慧交通車管理系統」。針對學校涵蓋幼兒園至高中「一條龍」的學制特性，解決傳統紙本或被動點名帶來的行政負擔與安全隱憂。系統提供精準的乘車登記、即時點名與未到通報機制，並透過自動化提醒降低行政催收成本，全面提升學生乘車安全與行政效率。

## 二、 系統架構與技術選型 (System Architecture)

本系統採用**前後端分離**架構，並導入 DevOps 精神建立 CI/CD 自動化流水線，展現現代化雲端開發標準。

* **前端 (Frontend)**：`Vue.js`
* **UI/UX**：Mobile-first (手機端優先) 設計，支援 RWD (響應式網頁)，完美適配手機、平板與桌機。
* **部署位置**：`GitHub Pages` (透過 GitHub Actions 自動打包部署)。


* **後端 (Backend)**：`.NET Core Web API`
* **架構**：RESTful API 設計，負責商業邏輯、權限驗證與資料處理。
* **部署位置**：`Microsoft Azure` (App Service / Container Apps，透過 CI/CD 自動發佈)。


* **資料庫 (Database)**：`Turso` (輕量級邊緣資料庫，適合 Demo 與高讀取情境)。
* **版控與 CI/CD**：`GitHub` + `GitHub Actions`。

## 三、 使用者角色與權限 (User Roles & Permissions)

| 角色名稱 | 適用對象 | 權限說明 |
| --- | --- | --- |
| **學生端** | 國中、高中生 | 可查看自己的乘車登記狀態；可自行登記下週搭乘狀況。 |
| **家長端** | 幼兒園至高中生家長 | **必用(幼/小)**：代為登記/異動子女搭乘狀況。<br>

<br>**選用(國/高)**：查看子女自行登記之狀況，或協助修改。 |
| **管理端(小)** | 隨車老師、導護老師 | **車長權限**：僅能查看與管理「自己負責綁定之該輛交通車」的點名清單、乘車狀態與緊急聯絡資訊。 |
| **管理端(大)** | 交通組行政、高階主管 | **總控權限**：可查看全校所有車輛登記狀況、跨路線調度、發送全域推播、匯出報表與異動所有資料。 |

## 四、 核心功能需求 (Functional Requirements)

### 1. 智慧登記與提醒模組 (Registration & Alert)

* **乘車登記**：家長或學生可勾選「下週一至週五」的「上學」與「放學」搭乘意願（預設可載入上週固定班表）。
* **自動防呆提醒 (Alert)**：系統會自動偵測，若接近 **週四、週五** 仍未完成下週乘車登記，系統將自動觸發推播/Email 提醒家長或學生及早登記。
* **狀態查詢**：隨時隨地透過手機查看當週與下週的乘車排程。

### 2. 行動點名與安全聯絡模組 (Roll Call & Safety)

* **隨車點名介面**：針對手機單手操作優化的 Checkbox 點名表。老師上車後快速勾選「已上車」。
* **一鍵聯絡功能**：當點名發現「未到」時，名單旁會直接顯示該學生的**緊急聯絡電話**與**家長聯絡方式**，老師點擊即可直接撥號，無需切換系統查通訊錄。

### 3. 彈性路線與站點管理 (Route Management)

* **混合學制共乘**：系統支援同一台車混搭國小至高中生，點名表自動依站點或年級排序。
* **多節點路線設定 (Point-to-Point)**：支援如 `站點A -> 站點B -> 林口康橋`，以及放學反向 `林口康橋 -> 站點B -> 站點A` 的獨立路線建置。
* **幼兒園專車特例處理**：系統內建「幼兒園門到門 (Door-to-Door)」標籤，這類路線的站點將直接對應幼生住家地址，點名表會特別標示交接家長資訊。

## 五、 資料庫初步設計 (Database Schema Draft)

採用關聯式設計，以輕量級 Turso 實作，以下為核心 Table 規劃：

* **`Parents` (家長表)**：`ParentID`, `Name`, `Phone`, `Email`...
* **`Students` (學生表)**：`StudentID`, `Name`, `Grade` (年級，用於判斷是否為幼兒園), `ParentID` (Foreign Key，1對多關係), `DefaultRouteID`...
* **`Teachers` (老師表)**：`TeacherID`, `Name`, `Phone`, `RoleLevel` (大權限/小權限), `AssignedRouteID` (綁定之車輛路線)...
* **`BusRoutes` (路線表)**：`RouteID`, `RouteName`, `Type` (一般/幼兒園特例), `Stops` (JSON格式記錄站點順序)...
* **`Registrations` (乘車登記表)**：`RegID`, `StudentID`, `Date` (日期), `Direction` (上學/放學), `IsRegistered` (是否搭乘), `IsPresent` (實際點名是否出席)...

## 六、 CI/CD 部署流程規劃 (Deployment Pipeline)

面試時展示此流程，可強烈證明您具備 DevOps 的實戰能力：

1. **Frontend Pipeline (Vue.js)**：
* 開發者 `git push` 至 GitHub `main` 分支。
* 觸發 GitHub Actions，執行 `npm run build`。
* 將打包後的 `dist` 資料夾自動推播至 `gh-pages` 分支，完成免費且穩定的前端發佈。


2. **Backend Pipeline (.NET Core Web API)**：
* 開發者 `git push` 至 GitHub。
* 觸發 GitHub Actions，執行 `.NET Build` 與 `Test`。
* 透過 Publish Profile 將編譯好的 Package 自動部署至 **Azure App Service**。
* API 連接位於邊緣節點的 **Turso DB**，確保極低的讀取延遲。

