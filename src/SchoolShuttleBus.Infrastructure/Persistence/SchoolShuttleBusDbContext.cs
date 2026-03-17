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

        builder.Entity<StudentGuardianLink>()
            .HasIndex(link => new { link.StudentId, link.GuardianId })
            .IsUnique();

        builder.Entity<RouteStop>()
            .HasIndex(stop => new { stop.RouteId, stop.Sequence })
            .IsUnique();

        builder.Entity<RideRegistration>()
            .HasIndex(registration => new { registration.StudentId, registration.Date, registration.Direction })
            .IsUnique();

        builder.Entity<StaffProfile>()
            .HasIndex(profile => profile.EmployeeNumber)
            .IsUnique();

        builder.Entity<AttendanceRecord>()
            .HasIndex(record => new { record.AttendanceSessionId, record.StudentId })
            .IsUnique();

        builder.Entity<DispatchOverride>()
            .HasIndex(overrideItem => new { overrideItem.StudentId, overrideItem.Date, overrideItem.Direction, overrideItem.Status });

        builder.Entity<RefreshToken>()
            .HasIndex(token => token.Token)
            .IsUnique();
    }
}
