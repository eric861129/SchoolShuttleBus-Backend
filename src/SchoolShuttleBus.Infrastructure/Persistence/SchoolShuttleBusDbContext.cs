using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Infrastructure.Auth;

namespace SchoolShuttleBus.Infrastructure.Persistence;

public sealed class SchoolShuttleBusDbContext(DbContextOptions<SchoolShuttleBusDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Student> Students => Set<Student>();

    public DbSet<Guardian> Guardians => Set<Guardian>();

    public DbSet<StaffProfile> StaffProfiles => Set<StaffProfile>();

    public DbSet<StudentGuardianLink> StudentGuardianLinks => Set<StudentGuardianLink>();

    public DbSet<Route> Routes => Set<Route>();

    public DbSet<RouteStop> RouteStops => Set<RouteStop>();

    public DbSet<RouteAssignment> RouteAssignments => Set<RouteAssignment>();

    public DbSet<RideRegistration> RideRegistrations => Set<RideRegistration>();

    public DbSet<AttendanceSession> AttendanceSessions => Set<AttendanceSession>();

    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

    public DbSet<DispatchOverride> DispatchOverrides => Set<DispatchOverride>();

    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

    public DbSet<NotificationJob> NotificationJobs => Set<NotificationJob>();

    public DbSet<NotificationDelivery> NotificationDeliveries => Set<NotificationDelivery>();

    public DbSet<BroadcastAnnouncement> BroadcastAnnouncements => Set<BroadcastAnnouncement>();

    public DbSet<ReportExport> ReportExports => Set<ReportExport>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var student = builder.Entity<Student>();
        student.ToTable(tableBuilder => tableBuilder.HasComment("學生主檔"));
        student.Property(item => item.Id).HasComment("學生識別碼");
        student.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        student.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        student.Property(item => item.UserId).HasComment("對應登入使用者識別碼");
        student.Property(item => item.StudentNumber).HasComment("學號");
        student.Property(item => item.FullName).HasComment("學生姓名");
        student.Property(item => item.Stage).HasComment("學制階段");
        student.Property(item => item.GradeLabel).HasComment("年級標籤");
        student.Property(item => item.DefaultRouteId).HasComment("預設路線識別碼");

        var guardian = builder.Entity<Guardian>();
        guardian.ToTable(tableBuilder => tableBuilder.HasComment("家長主檔"));
        guardian.Property(item => item.Id).HasComment("家長識別碼");
        guardian.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        guardian.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        guardian.Property(item => item.UserId).HasComment("對應登入使用者識別碼");
        guardian.Property(item => item.FullName).HasComment("家長姓名");
        guardian.Property(item => item.PhoneNumber).HasComment("手機號碼");
        guardian.Property(item => item.Email).HasComment("電子郵件");

        var staffProfile = builder.Entity<StaffProfile>();
        staffProfile.ToTable(tableBuilder => tableBuilder.HasComment("教職員主檔"));
        staffProfile.Property(item => item.Id).HasComment("教職員識別碼");
        staffProfile.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        staffProfile.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        staffProfile.Property(item => item.UserId).HasComment("對應登入使用者識別碼");
        staffProfile.Property(item => item.EmployeeNumber).HasComment("工號");
        staffProfile.Property(item => item.FullName).HasComment("教職員姓名");
        staffProfile.Property(item => item.PhoneNumber).HasComment("聯絡電話");
        staffProfile.Property(item => item.CanManageAllRoutes).HasComment("是否可管理全部路線");

        var studentGuardianLink = builder.Entity<StudentGuardianLink>();
        studentGuardianLink.ToTable(tableBuilder => tableBuilder.HasComment("學生與家長關聯"));
        studentGuardianLink.Property(item => item.Id).HasComment("關聯識別碼");
        studentGuardianLink.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        studentGuardianLink.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        studentGuardianLink.Property(item => item.StudentId).HasComment("學生識別碼");
        studentGuardianLink.Property(item => item.GuardianId).HasComment("家長識別碼");
        studentGuardianLink.Property(item => item.IsPrimaryContact).HasComment("是否為主要聯絡人");
        studentGuardianLink.HasIndex(link => new { link.StudentId, link.GuardianId })
            .IsUnique();

        var route = builder.Entity<Route>();
        route.ToTable(tableBuilder => tableBuilder.HasComment("校車路線主檔"));
        route.Property(item => item.Id).HasComment("路線識別碼");
        route.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        route.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        route.Property(item => item.RouteName).HasComment("路線名稱");
        route.Property(item => item.RouteType).HasComment("路線類型");
        route.Property(item => item.Direction).HasComment("行車方向");
        route.Property(item => item.CampusName).HasComment("校區名稱");
        route.Property(item => item.IsActive).HasComment("是否啟用");

        var routeStop = builder.Entity<RouteStop>();
        routeStop.ToTable(tableBuilder => tableBuilder.HasComment("路線停靠站"));
        routeStop.Property(item => item.Id).HasComment("站點識別碼");
        routeStop.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        routeStop.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        routeStop.Property(item => item.RouteId).HasComment("所屬路線識別碼");
        routeStop.Property(item => item.Sequence).HasComment("停靠順序");
        routeStop.Property(item => item.StopName).HasComment("站點名稱");
        routeStop.Property(item => item.Address).HasComment("站點地址");
        routeStop.Property(item => item.HandoffContactName).HasComment("交接聯絡人姓名");
        routeStop.Property(item => item.HandoffContactPhone).HasComment("交接聯絡人電話");
        routeStop.HasIndex(stop => new { stop.RouteId, stop.Sequence })
            .IsUnique();

        var routeAssignment = builder.Entity<RouteAssignment>();
        routeAssignment.ToTable(tableBuilder => tableBuilder.HasComment("路線指派資料"));
        routeAssignment.Property(item => item.Id).HasComment("指派識別碼");
        routeAssignment.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        routeAssignment.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        routeAssignment.Property(item => item.RouteId).HasComment("路線識別碼");
        routeAssignment.Property(item => item.StaffProfileId).HasComment("教職員識別碼");

        var rideRegistration = builder.Entity<RideRegistration>();
        rideRegistration.ToTable(tableBuilder => tableBuilder.HasComment("搭車登記資料"));
        rideRegistration.Property(item => item.Id).HasComment("登記識別碼");
        rideRegistration.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        rideRegistration.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        rideRegistration.Property(item => item.StudentId).HasComment("學生識別碼");
        rideRegistration.Property(item => item.Date).HasComment("搭車日期");
        rideRegistration.Property(item => item.Direction).HasComment("行車方向");
        rideRegistration.Property(item => item.IsRegistered).HasComment("是否已登記搭車");
        rideRegistration.Property(item => item.IsPresent).HasComment("是否實際到場搭乘");
        rideRegistration.Property(item => item.RouteId).HasComment("指派路線識別碼");
        rideRegistration.HasIndex(registration => new { registration.StudentId, registration.Date, registration.Direction })
            .IsUnique();

        var attendanceSession = builder.Entity<AttendanceSession>();
        attendanceSession.ToTable(tableBuilder => tableBuilder.HasComment("點名場次"));
        attendanceSession.Property(item => item.Id).HasComment("場次識別碼");
        attendanceSession.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        attendanceSession.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        attendanceSession.Property(item => item.RouteId).HasComment("路線識別碼");
        attendanceSession.Property(item => item.Date).HasComment("點名日期");
        attendanceSession.Property(item => item.Direction).HasComment("行車方向");
        attendanceSession.Property(item => item.IsCompleted).HasComment("是否已完成點名");
        attendanceSession.Property(item => item.CreatedByStaffProfileId).HasComment("建立場次的教職員識別碼");

        var attendanceRecord = builder.Entity<AttendanceRecord>();
        attendanceRecord.ToTable(tableBuilder => tableBuilder.HasComment("點名紀錄"));
        attendanceRecord.Property(item => item.Id).HasComment("點名紀錄識別碼");
        attendanceRecord.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        attendanceRecord.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        attendanceRecord.Property(item => item.AttendanceSessionId).HasComment("所屬點名場次識別碼");
        attendanceRecord.Property(item => item.StudentId).HasComment("學生識別碼");
        attendanceRecord.Property(item => item.Status).HasComment("點名狀態");
        attendanceRecord.Property(item => item.EmergencyPhoneSnapshot).HasComment("緊急聯絡電話快照");
        attendanceRecord.HasIndex(record => new { record.AttendanceSessionId, record.StudentId })
            .IsUnique();

        var dispatchOverride = builder.Entity<DispatchOverride>();
        dispatchOverride.ToTable(tableBuilder => tableBuilder.HasComment("派車覆寫設定"));
        dispatchOverride.Property(item => item.Id).HasComment("覆寫識別碼");
        dispatchOverride.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        dispatchOverride.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        dispatchOverride.Property(item => item.StudentId).HasComment("學生識別碼");
        dispatchOverride.Property(item => item.RouteId).HasComment("指派路線識別碼");
        dispatchOverride.Property(item => item.Date).HasComment("覆寫日期");
        dispatchOverride.Property(item => item.Direction).HasComment("行車方向");
        dispatchOverride.Property(item => item.Status).HasComment("覆寫狀態");
        dispatchOverride.HasIndex(overrideItem => new { overrideItem.StudentId, overrideItem.Date, overrideItem.Direction, overrideItem.Status });

        var notificationTemplate = builder.Entity<NotificationTemplate>();
        notificationTemplate.ToTable(tableBuilder => tableBuilder.HasComment("通知範本"));
        notificationTemplate.Property(item => item.Id).HasComment("範本識別碼");
        notificationTemplate.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        notificationTemplate.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        notificationTemplate.Property(item => item.TemplateName).HasComment("範本名稱");
        notificationTemplate.Property(item => item.Subject).HasComment("通知主旨");
        notificationTemplate.Property(item => item.Body).HasComment("通知內容");

        var notificationJob = builder.Entity<NotificationJob>();
        notificationJob.ToTable(tableBuilder => tableBuilder.HasComment("通知工作"));
        notificationJob.Property(item => item.Id).HasComment("通知工作識別碼");
        notificationJob.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        notificationJob.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        notificationJob.Property(item => item.JobType).HasComment("工作類型");
        notificationJob.Property(item => item.Channel).HasComment("發送管道");
        notificationJob.Property(item => item.Subject).HasComment("通知主旨");
        notificationJob.Property(item => item.Body).HasComment("通知內容");
        notificationJob.Property(item => item.CreatedByUserId).HasComment("建立通知的使用者識別碼");

        var notificationDelivery = builder.Entity<NotificationDelivery>();
        notificationDelivery.ToTable(tableBuilder => tableBuilder.HasComment("通知發送紀錄"));
        notificationDelivery.Property(item => item.Id).HasComment("發送紀錄識別碼");
        notificationDelivery.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        notificationDelivery.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        notificationDelivery.Property(item => item.NotificationJobId).HasComment("所屬通知工作識別碼");
        notificationDelivery.Property(item => item.RecipientUserId).HasComment("收件者使用者識別碼");
        notificationDelivery.Property(item => item.RecipientEmail).HasComment("收件者電子郵件");
        notificationDelivery.Property(item => item.Status).HasComment("發送狀態");
        notificationDelivery.Property(item => item.SentAtUtc).HasComment("發送時間（UTC）");
        notificationDelivery.Property(item => item.ErrorMessage).HasComment("失敗訊息");

        var broadcastAnnouncement = builder.Entity<BroadcastAnnouncement>();
        broadcastAnnouncement.ToTable(tableBuilder => tableBuilder.HasComment("廣播公告"));
        broadcastAnnouncement.Property(item => item.Id).HasComment("公告識別碼");
        broadcastAnnouncement.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        broadcastAnnouncement.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        broadcastAnnouncement.Property(item => item.Audience).HasComment("公告對象");
        broadcastAnnouncement.Property(item => item.Subject).HasComment("公告主旨");
        broadcastAnnouncement.Property(item => item.Body).HasComment("公告內容");
        broadcastAnnouncement.Property(item => item.CreatedByUserId).HasComment("建立公告的使用者識別碼");

        var reportExport = builder.Entity<ReportExport>();
        reportExport.ToTable(tableBuilder => tableBuilder.HasComment("報表匯出紀錄"));
        reportExport.Property(item => item.Id).HasComment("匯出識別碼");
        reportExport.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        reportExport.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        reportExport.Property(item => item.ReportType).HasComment("報表類型");
        reportExport.Property(item => item.ExportFormat).HasComment("匯出格式");
        reportExport.Property(item => item.FileName).HasComment("檔名");
        reportExport.Property(item => item.ContentType).HasComment("內容類型");
        reportExport.Property(item => item.FiltersJson).HasComment("匯出條件 JSON");
        reportExport.Property(item => item.Content).HasComment("匯出內容");
        reportExport.Property(item => item.CreatedByUserId).HasComment("建立匯出的使用者識別碼");

        var auditLog = builder.Entity<AuditLog>();
        auditLog.ToTable(tableBuilder => tableBuilder.HasComment("稽核紀錄"));
        auditLog.Property(item => item.Id).HasComment("稽核識別碼");
        auditLog.Property(item => item.CreatedAtUtc).HasComment("建立時間（UTC）");
        auditLog.Property(item => item.UpdatedAtUtc).HasComment("最後更新時間（UTC）");
        auditLog.Property(item => item.ActorUserId).HasComment("執行者使用者識別碼");
        auditLog.Property(item => item.Action).HasComment("動作名稱");
        auditLog.Property(item => item.EntityName).HasComment("目標實體名稱");
        auditLog.Property(item => item.EntityId).HasComment("目標實體識別碼");
        auditLog.Property(item => item.MetadataJson).HasComment("補充資料 JSON");

        var refreshToken = builder.Entity<RefreshToken>();
        refreshToken.ToTable(tableBuilder => tableBuilder.HasComment("重新整理權杖"));
        refreshToken.Property(item => item.Id).HasComment("權杖識別碼");
        refreshToken.Property(item => item.UserId).HasComment("對應使用者識別碼");
        refreshToken.Property(item => item.Token).HasComment("權杖字串");
        refreshToken.Property(item => item.ExpiresAtUtc).HasComment("到期時間（UTC）");
        refreshToken.Property(item => item.RevokedAtUtc).HasComment("撤銷時間（UTC）");
        refreshToken.HasIndex(token => token.Token)
            .IsUnique();

        staffProfile.HasIndex(profile => profile.EmployeeNumber)
            .IsUnique();
    }
}
