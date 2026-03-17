using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolShuttleBus.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChineseCommentsToDomainTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Students",
                comment: "學生主檔");

            migrationBuilder.AlterTable(
                name: "StudentGuardianLinks",
                comment: "學生與家長關聯");

            migrationBuilder.AlterTable(
                name: "StaffProfiles",
                comment: "教職員主檔");

            migrationBuilder.AlterTable(
                name: "RouteStops",
                comment: "路線停靠站");

            migrationBuilder.AlterTable(
                name: "Routes",
                comment: "校車路線主檔");

            migrationBuilder.AlterTable(
                name: "RouteAssignments",
                comment: "路線指派資料");

            migrationBuilder.AlterTable(
                name: "RideRegistrations",
                comment: "搭車登記資料");

            migrationBuilder.AlterTable(
                name: "ReportExports",
                comment: "報表匯出紀錄");

            migrationBuilder.AlterTable(
                name: "RefreshTokens",
                comment: "重新整理權杖");

            migrationBuilder.AlterTable(
                name: "NotificationTemplates",
                comment: "通知範本");

            migrationBuilder.AlterTable(
                name: "NotificationJobs",
                comment: "通知工作");

            migrationBuilder.AlterTable(
                name: "NotificationDeliveries",
                comment: "通知發送紀錄");

            migrationBuilder.AlterTable(
                name: "Guardians",
                comment: "家長主檔");

            migrationBuilder.AlterTable(
                name: "DispatchOverrides",
                comment: "派車覆寫設定");

            migrationBuilder.AlterTable(
                name: "BroadcastAnnouncements",
                comment: "廣播公告");

            migrationBuilder.AlterTable(
                name: "AuditLogs",
                comment: "稽核紀錄");

            migrationBuilder.AlterTable(
                name: "AttendanceSessions",
                comment: "點名場次");

            migrationBuilder.AlterTable(
                name: "AttendanceRecords",
                comment: "點名紀錄");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                comment: "對應登入使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "Students",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                comment: "學號",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Stage",
                table: "Students",
                type: "int",
                nullable: false,
                comment: "學制階段",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "GradeLabel",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                comment: "年級標籤",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                comment: "學生姓名",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultRouteId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true,
                comment: "預設路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Students",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                comment: "學生識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "StudentGuardianLinks",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "StudentGuardianLinks",
                type: "uniqueidentifier",
                nullable: false,
                comment: "學生識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimaryContact",
                table: "StudentGuardianLinks",
                type: "bit",
                nullable: false,
                comment: "是否為主要聯絡人",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "GuardianId",
                table: "StudentGuardianLinks",
                type: "uniqueidentifier",
                nullable: false,
                comment: "家長識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "StudentGuardianLinks",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StudentGuardianLinks",
                type: "uniqueidentifier",
                nullable: false,
                comment: "關聯識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "StaffProfiles",
                type: "uniqueidentifier",
                nullable: false,
                comment: "對應登入使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "StaffProfiles",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "StaffProfiles",
                type: "nvarchar(max)",
                nullable: false,
                comment: "聯絡電話",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "StaffProfiles",
                type: "nvarchar(max)",
                nullable: false,
                comment: "教職員姓名",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNumber",
                table: "StaffProfiles",
                type: "nvarchar(450)",
                nullable: false,
                comment: "工號",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "StaffProfiles",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<bool>(
                name: "CanManageAllRoutes",
                table: "StaffProfiles",
                type: "bit",
                nullable: false,
                comment: "是否可管理全部路線",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StaffProfiles",
                type: "uniqueidentifier",
                nullable: false,
                comment: "教職員識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "RouteStops",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StopName",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: false,
                comment: "站點名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Sequence",
                table: "RouteStops",
                type: "int",
                nullable: false,
                comment: "停靠順序",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "RouteStops",
                type: "uniqueidentifier",
                nullable: false,
                comment: "所屬路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "HandoffContactPhone",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: true,
                comment: "交接聯絡人電話",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HandoffContactName",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: true,
                comment: "交接聯絡人姓名",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "RouteStops",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: false,
                comment: "站點地址",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RouteStops",
                type: "uniqueidentifier",
                nullable: false,
                comment: "站點識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "Routes",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RouteType",
                table: "Routes",
                type: "int",
                nullable: false,
                comment: "路線類型",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "路線名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Routes",
                type: "bit",
                nullable: false,
                comment: "是否啟用",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "Routes",
                type: "int",
                nullable: false,
                comment: "行車方向",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Routes",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "CampusName",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                comment: "校區名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: false,
                comment: "路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "RouteAssignments",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StaffProfileId",
                table: "RouteAssignments",
                type: "uniqueidentifier",
                nullable: false,
                comment: "教職員識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "RouteAssignments",
                type: "uniqueidentifier",
                nullable: false,
                comment: "路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "RouteAssignments",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RouteAssignments",
                type: "uniqueidentifier",
                nullable: false,
                comment: "指派識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "RideRegistrations",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "RideRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                comment: "學生識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "RideRegistrations",
                type: "uniqueidentifier",
                nullable: true,
                comment: "指派路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRegistered",
                table: "RideRegistrations",
                type: "bit",
                nullable: false,
                comment: "是否已登記搭車",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPresent",
                table: "RideRegistrations",
                type: "bit",
                nullable: false,
                comment: "是否實際到場搭乘",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "RideRegistrations",
                type: "int",
                nullable: false,
                comment: "行車方向",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "RideRegistrations",
                type: "date",
                nullable: false,
                comment: "搭車日期",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "RideRegistrations",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RideRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                comment: "登記識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "ReportExports",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReportType",
                table: "ReportExports",
                type: "int",
                nullable: false,
                comment: "報表類型",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FiltersJson",
                table: "ReportExports",
                type: "nvarchar(max)",
                nullable: false,
                comment: "匯出條件 JSON",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ReportExports",
                type: "nvarchar(max)",
                nullable: false,
                comment: "檔名",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ExportFormat",
                table: "ReportExports",
                type: "int",
                nullable: false,
                comment: "匯出格式",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "ReportExports",
                type: "uniqueidentifier",
                nullable: false,
                comment: "建立匯出的使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "ReportExports",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "ReportExports",
                type: "nvarchar(max)",
                nullable: false,
                comment: "內容類型",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Content",
                table: "ReportExports",
                type: "varbinary(max)",
                nullable: false,
                comment: "匯出內容",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ReportExports",
                type: "uniqueidentifier",
                nullable: false,
                comment: "匯出識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                comment: "對應使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                comment: "權杖字串",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RevokedAtUtc",
                table: "RefreshTokens",
                type: "datetimeoffset",
                nullable: true,
                comment: "撤銷時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpiresAtUtc",
                table: "RefreshTokens",
                type: "datetimeoffset",
                nullable: false,
                comment: "到期時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                comment: "權杖識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "NotificationTemplates",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                table: "NotificationTemplates",
                type: "nvarchar(max)",
                nullable: false,
                comment: "範本名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "NotificationTemplates",
                type: "nvarchar(max)",
                nullable: false,
                comment: "通知主旨",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "NotificationTemplates",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "NotificationTemplates",
                type: "nvarchar(max)",
                nullable: false,
                comment: "通知內容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NotificationTemplates",
                type: "uniqueidentifier",
                nullable: false,
                comment: "範本識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "NotificationJobs",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "NotificationJobs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "通知主旨",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "JobType",
                table: "NotificationJobs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "工作類型",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "NotificationJobs",
                type: "uniqueidentifier",
                nullable: true,
                comment: "建立通知的使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "NotificationJobs",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<int>(
                name: "Channel",
                table: "NotificationJobs",
                type: "int",
                nullable: false,
                comment: "發送管道",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "NotificationJobs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "通知內容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NotificationJobs",
                type: "uniqueidentifier",
                nullable: false,
                comment: "通知工作識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "NotificationDeliveries",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "NotificationDeliveries",
                type: "nvarchar(max)",
                nullable: false,
                comment: "發送狀態",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SentAtUtc",
                table: "NotificationDeliveries",
                type: "datetimeoffset",
                nullable: true,
                comment: "發送時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipientUserId",
                table: "NotificationDeliveries",
                type: "uniqueidentifier",
                nullable: true,
                comment: "收件者使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientEmail",
                table: "NotificationDeliveries",
                type: "nvarchar(max)",
                nullable: false,
                comment: "收件者電子郵件",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "NotificationJobId",
                table: "NotificationDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                comment: "所屬通知工作識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "NotificationDeliveries",
                type: "nvarchar(max)",
                nullable: true,
                comment: "失敗訊息",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "NotificationDeliveries",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NotificationDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                comment: "發送紀錄識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Guardians",
                type: "uniqueidentifier",
                nullable: false,
                comment: "對應登入使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "Guardians",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                comment: "手機號碼",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                comment: "家長姓名",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                comment: "電子郵件",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Guardians",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Guardians",
                type: "uniqueidentifier",
                nullable: false,
                comment: "家長識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "DispatchOverrides",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "DispatchOverrides",
                type: "uniqueidentifier",
                nullable: false,
                comment: "學生識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "DispatchOverrides",
                type: "int",
                nullable: false,
                comment: "覆寫狀態",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "DispatchOverrides",
                type: "uniqueidentifier",
                nullable: false,
                comment: "指派路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "DispatchOverrides",
                type: "int",
                nullable: false,
                comment: "行車方向",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "DispatchOverrides",
                type: "date",
                nullable: false,
                comment: "覆寫日期",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "DispatchOverrides",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "DispatchOverrides",
                type: "uniqueidentifier",
                nullable: false,
                comment: "覆寫識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "BroadcastAnnouncements",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "BroadcastAnnouncements",
                type: "nvarchar(max)",
                nullable: false,
                comment: "公告主旨",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "BroadcastAnnouncements",
                type: "uniqueidentifier",
                nullable: false,
                comment: "建立公告的使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "BroadcastAnnouncements",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "BroadcastAnnouncements",
                type: "nvarchar(max)",
                nullable: false,
                comment: "公告內容",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Audience",
                table: "BroadcastAnnouncements",
                type: "int",
                nullable: false,
                comment: "公告對象",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "BroadcastAnnouncements",
                type: "uniqueidentifier",
                nullable: false,
                comment: "公告識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "AuditLogs",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetadataJson",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "補充資料 JSON",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EntityName",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "目標實體名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: true,
                comment: "目標實體識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "AuditLogs",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActorUserId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: true,
                comment: "執行者使用者識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                comment: "動作名稱",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: false,
                comment: "稽核識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "AttendanceSessions",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "AttendanceSessions",
                type: "uniqueidentifier",
                nullable: false,
                comment: "路線識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "AttendanceSessions",
                type: "bit",
                nullable: false,
                comment: "是否已完成點名",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "AttendanceSessions",
                type: "int",
                nullable: false,
                comment: "行車方向",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "AttendanceSessions",
                type: "date",
                nullable: false,
                comment: "點名日期",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByStaffProfileId",
                table: "AttendanceSessions",
                type: "uniqueidentifier",
                nullable: false,
                comment: "建立場次的教職員識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "AttendanceSessions",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AttendanceSessions",
                type: "uniqueidentifier",
                nullable: false,
                comment: "場次識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "AttendanceRecords",
                type: "datetimeoffset",
                nullable: true,
                comment: "最後更新時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "AttendanceRecords",
                type: "uniqueidentifier",
                nullable: false,
                comment: "學生識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AttendanceRecords",
                type: "int",
                nullable: false,
                comment: "點名狀態",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyPhoneSnapshot",
                table: "AttendanceRecords",
                type: "nvarchar(max)",
                nullable: false,
                comment: "緊急聯絡電話快照",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "AttendanceRecords",
                type: "datetimeoffset",
                nullable: false,
                comment: "建立時間（UTC）",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<Guid>(
                name: "AttendanceSessionId",
                table: "AttendanceRecords",
                type: "uniqueidentifier",
                nullable: false,
                comment: "所屬點名場次識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AttendanceRecords",
                type: "uniqueidentifier",
                nullable: false,
                comment: "點名紀錄識別碼",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Students",
                oldComment: "學生主檔");

            migrationBuilder.AlterTable(
                name: "StudentGuardianLinks",
                oldComment: "學生與家長關聯");

            migrationBuilder.AlterTable(
                name: "StaffProfiles",
                oldComment: "教職員主檔");

            migrationBuilder.AlterTable(
                name: "RouteStops",
                oldComment: "路線停靠站");

            migrationBuilder.AlterTable(
                name: "Routes",
                oldComment: "校車路線主檔");

            migrationBuilder.AlterTable(
                name: "RouteAssignments",
                oldComment: "路線指派資料");

            migrationBuilder.AlterTable(
                name: "RideRegistrations",
                oldComment: "搭車登記資料");

            migrationBuilder.AlterTable(
                name: "ReportExports",
                oldComment: "報表匯出紀錄");

            migrationBuilder.AlterTable(
                name: "RefreshTokens",
                oldComment: "重新整理權杖");

            migrationBuilder.AlterTable(
                name: "NotificationTemplates",
                oldComment: "通知範本");

            migrationBuilder.AlterTable(
                name: "NotificationJobs",
                oldComment: "通知工作");

            migrationBuilder.AlterTable(
                name: "NotificationDeliveries",
                oldComment: "通知發送紀錄");

            migrationBuilder.AlterTable(
                name: "Guardians",
                oldComment: "家長主檔");

            migrationBuilder.AlterTable(
                name: "DispatchOverrides",
                oldComment: "派車覆寫設定");

            migrationBuilder.AlterTable(
                name: "BroadcastAnnouncements",
                oldComment: "廣播公告");

            migrationBuilder.AlterTable(
                name: "AuditLogs",
                oldComment: "稽核紀錄");

            migrationBuilder.AlterTable(
                name: "AttendanceSessions",
                oldComment: "點名場次");

            migrationBuilder.AlterTable(
                name: "AttendanceRecords",
                oldComment: "點名紀錄");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "對應登入使用者識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "Students",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "學號");

            migrationBuilder.AlterColumn<int>(
                name: "Stage",
                table: "Students",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "學制階段");

            migrationBuilder.AlterColumn<string>(
                name: "GradeLabel",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "年級標籤");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "學生姓名");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefaultRouteId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "預設路線識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Students",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "學生識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "StudentGuardianLinks",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "StudentGuardianLinks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "學生識別碼");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimaryContact",
                table: "StudentGuardianLinks",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否為主要聯絡人");

            migrationBuilder.AlterColumn<Guid>(
                name: "GuardianId",
                table: "StudentGuardianLinks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "家長識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "StudentGuardianLinks",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StudentGuardianLinks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "關聯識別碼");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "StaffProfiles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "對應登入使用者識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "StaffProfiles",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "StaffProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "聯絡電話");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "StaffProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "教職員姓名");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNumber",
                table: "StaffProfiles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "工號");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "StaffProfiles",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<bool>(
                name: "CanManageAllRoutes",
                table: "StaffProfiles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否可管理全部路線");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StaffProfiles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "教職員識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "RouteStops",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "StopName",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "站點名稱");

            migrationBuilder.AlterColumn<int>(
                name: "Sequence",
                table: "RouteStops",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "停靠順序");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "RouteStops",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "所屬路線識別碼");

            migrationBuilder.AlterColumn<string>(
                name: "HandoffContactPhone",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "交接聯絡人電話");

            migrationBuilder.AlterColumn<string>(
                name: "HandoffContactName",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "交接聯絡人姓名");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "RouteStops",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "站點地址");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RouteStops",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "站點識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "Routes",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<int>(
                name: "RouteType",
                table: "Routes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "路線類型");

            migrationBuilder.AlterColumn<string>(
                name: "RouteName",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "路線名稱");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Routes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否啟用");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "Routes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "行車方向");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Routes",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "CampusName",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "校區名稱");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "路線識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "RouteAssignments",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "StaffProfileId",
                table: "RouteAssignments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "教職員識別碼");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "RouteAssignments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "路線識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "RouteAssignments",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RouteAssignments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "指派識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "RideRegistrations",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "RideRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "學生識別碼");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "RideRegistrations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "指派路線識別碼");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRegistered",
                table: "RideRegistrations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否已登記搭車");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPresent",
                table: "RideRegistrations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否實際到場搭乘");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "RideRegistrations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "行車方向");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "RideRegistrations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "搭車日期");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "RideRegistrations",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RideRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "登記識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "ReportExports",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<int>(
                name: "ReportType",
                table: "ReportExports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "報表類型");

            migrationBuilder.AlterColumn<string>(
                name: "FiltersJson",
                table: "ReportExports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "匯出條件 JSON");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ReportExports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "檔名");

            migrationBuilder.AlterColumn<int>(
                name: "ExportFormat",
                table: "ReportExports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "匯出格式");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "ReportExports",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "建立匯出的使用者識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "ReportExports",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "ReportExports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "內容類型");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Content",
                table: "ReportExports",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldComment: "匯出內容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ReportExports",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "匯出識別碼");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "對應使用者識別碼");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "權杖字串");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RevokedAtUtc",
                table: "RefreshTokens",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "撤銷時間（UTC）");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ExpiresAtUtc",
                table: "RefreshTokens",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "到期時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "權杖識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "NotificationTemplates",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateName",
                table: "NotificationTemplates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "範本名稱");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "NotificationTemplates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "通知主旨");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "NotificationTemplates",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "NotificationTemplates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "通知內容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NotificationTemplates",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "範本識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "NotificationJobs",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "NotificationJobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "通知主旨");

            migrationBuilder.AlterColumn<string>(
                name: "JobType",
                table: "NotificationJobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "工作類型");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "NotificationJobs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "建立通知的使用者識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "NotificationJobs",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<int>(
                name: "Channel",
                table: "NotificationJobs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "發送管道");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "NotificationJobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "通知內容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NotificationJobs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "通知工作識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "NotificationDeliveries",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "NotificationDeliveries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "發送狀態");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SentAtUtc",
                table: "NotificationDeliveries",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "發送時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipientUserId",
                table: "NotificationDeliveries",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "收件者使用者識別碼");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientEmail",
                table: "NotificationDeliveries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "收件者電子郵件");

            migrationBuilder.AlterColumn<Guid>(
                name: "NotificationJobId",
                table: "NotificationDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "所屬通知工作識別碼");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "NotificationDeliveries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "失敗訊息");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "NotificationDeliveries",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "NotificationDeliveries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "發送紀錄識別碼");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Guardians",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "對應登入使用者識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "Guardians",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "手機號碼");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "家長姓名");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Guardians",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "電子郵件");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "Guardians",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Guardians",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "家長識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "DispatchOverrides",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "DispatchOverrides",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "學生識別碼");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "DispatchOverrides",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "覆寫狀態");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "DispatchOverrides",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "指派路線識別碼");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "DispatchOverrides",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "行車方向");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "DispatchOverrides",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "覆寫日期");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "DispatchOverrides",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "DispatchOverrides",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "覆寫識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "BroadcastAnnouncements",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "BroadcastAnnouncements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "公告主旨");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "BroadcastAnnouncements",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "建立公告的使用者識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "BroadcastAnnouncements",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "BroadcastAnnouncements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "公告內容");

            migrationBuilder.AlterColumn<int>(
                name: "Audience",
                table: "BroadcastAnnouncements",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "公告對象");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "BroadcastAnnouncements",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "公告識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "AuditLogs",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<string>(
                name: "MetadataJson",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "補充資料 JSON");

            migrationBuilder.AlterColumn<string>(
                name: "EntityName",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "目標實體名稱");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "目標實體識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "AuditLogs",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActorUserId",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true,
                oldComment: "執行者使用者識別碼");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "動作名稱");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AuditLogs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "稽核識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "AttendanceSessions",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "RouteId",
                table: "AttendanceSessions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "路線識別碼");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "AttendanceSessions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "是否已完成點名");

            migrationBuilder.AlterColumn<int>(
                name: "Direction",
                table: "AttendanceSessions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "行車方向");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "AttendanceSessions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "點名日期");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByStaffProfileId",
                table: "AttendanceSessions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "建立場次的教職員識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "AttendanceSessions",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AttendanceSessions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "場次識別碼");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAtUtc",
                table: "AttendanceRecords",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldComment: "最後更新時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "AttendanceRecords",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "學生識別碼");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AttendanceRecords",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "點名狀態");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyPhoneSnapshot",
                table: "AttendanceRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "緊急聯絡電話快照");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAtUtc",
                table: "AttendanceRecords",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldComment: "建立時間（UTC）");

            migrationBuilder.AlterColumn<Guid>(
                name: "AttendanceSessionId",
                table: "AttendanceRecords",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "所屬點名場次識別碼");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AttendanceRecords",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "點名紀錄識別碼");
        }
    }
}
