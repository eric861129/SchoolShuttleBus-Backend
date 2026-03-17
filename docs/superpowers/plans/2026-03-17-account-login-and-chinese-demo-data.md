# Account Login And Chinese Demo Data Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace email-based login with account-based login using student number, employee number, and guardian phone number, then align seeded/demo SQL data to Chinese-first demo content.

**Architecture:** Keep JWT issuance and authorization unchanged. Only change account lookup rules, add an employee number field for staff, and update seeded/demo data so integration tests and Azure demo data both use the new account identifiers.

**Tech Stack:** ASP.NET Core 8, EF Core 8, ASP.NET Identity, xUnit integration tests, SQL Server

---

### Task 1: Lock Login Contract With Failing Tests

**Files:**
- Modify: `tests/SchoolShuttleBus.Api.Tests/AuthEndpointsTests.cs`
- Modify: `tests/SchoolShuttleBus.Api.Tests/TestAuthExtensions.cs`

- [ ] Add tests that log in with `account` instead of `email`
- [ ] Add coverage for one admin/teacher/parent/student account shape
- [ ] Run targeted auth tests and confirm they fail for the expected reason

### Task 2: Implement Account-Based Lookup

**Files:**
- Modify: `src/SchoolShuttleBus.Contracts/Auth/AuthContracts.cs`
- Modify: `src/SchoolShuttleBus.Infrastructure/Auth/AuthService.cs`
- Modify: `src/SchoolShuttleBus.Api/Controllers/AuthController.cs`

- [ ] Replace `LoginRequest.Email` with `LoginRequest.Account`
- [ ] Update login lookup to resolve users by student number, employee number, or guardian phone number
- [ ] Keep password verification and token generation unchanged
- [ ] Re-run auth tests until green

### Task 3: Add Employee Number To Staff Data Model

**Files:**
- Modify: `src/SchoolShuttleBus.Domain/Entities/StaffProfile.cs`
- Modify: `src/SchoolShuttleBus.Infrastructure/Persistence/SchoolShuttleBusDbContext.cs`
- Create: `src/SchoolShuttleBus.Infrastructure/Persistence/Migrations/<timestamp>_AddEmployeeNumberToStaffProfiles.cs`
- Modify: `src/SchoolShuttleBus.Infrastructure/Persistence/Migrations/SchoolShuttleBusDbContextModelSnapshot.cs`

- [ ] Add `EmployeeNumber` to `StaffProfile`
- [ ] Add a unique index for employee number
- [ ] Create migration for SQL Server schema update
- [ ] Run migration-aware tests/build checks

### Task 4: Align Seeded Test Data

**Files:**
- Modify: `src/SchoolShuttleBus.Infrastructure/Persistence/SeedDataService.cs`
- Modify: `src/SchoolShuttleBus.Infrastructure/Persistence/DemoSeedConstants.cs` if needed

- [ ] Update built-in seed accounts to Chinese names and account identifiers
- [ ] Ensure `AspNetUsers.UserName` matches the account identifier
- [ ] Keep integration tests using the new seeded accounts

### Task 5: Refresh One-Time Azure Demo SQL

**Files:**
- Modify: `docs/sql/azure-demo-dataset-once.sql`
- Modify: `docs/sql/azure-demo-dataset-once.md`

- [ ] Replace email-style usernames with student number / employee number / phone number accounts
- [ ] Convert demo names and labels to Chinese-first values
- [ ] Update the documented Swagger login examples

### Task 6: Verify End To End

**Files:**
- Verify only

- [ ] Run targeted auth integration tests
- [ ] Run build/tests covering migration and auth changes
- [ ] Hit local or deployed login endpoint shape if needed
- [ ] Summarize required Azure SQL follow-up for the new migration/demo SQL
