# ADR-001: Use Azure SQL Database Instead Of Turso

## Status

Accepted

## Context

原始需求草稿以 Turso 作為 Demo database，但本專案是 `.NET` 後端面試作品，重點在於：

- 用 ASP.NET Core + EF Core 展示完整 enterprise-style backend
- 降低 .NET 生態整合風險
- 提供更直觀的 Azure 雲端故事

## Decision

第一版資料庫改採 `Azure SQL Database`。

## Consequences

- Positive
  可以直接使用 EF Core + SQL Server provider + migration
- Positive
  與 Azure App Service 部署故事一致
- Negative
  失去 edge database 的敘事點

## Follow-up

- 文件保留未來若要切回 Turso / libSQL 的比較與風險說明
