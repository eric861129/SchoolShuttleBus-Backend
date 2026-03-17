using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Auth;

namespace SchoolShuttleBus.Infrastructure.Persistence;

/// <summary>
/// 建立可重現的示範資料，讓 API 能直接用於 Swagger 展示、
/// 整合測試與面試 Demo，而不需要額外手動準備資料。
/// </summary>
public sealed class SeedDataService(
    SchoolShuttleBusDbContext dbContext,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        foreach (var roleName in RoleNames.All)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

        if (await userManager.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var admin = await CreateUserAsync(DemoSeedConstants.AdminUserId, "E0001", "admin@demo.local", RoleNames.Administrator, "0900-000-001");
        var teacher = await CreateUserAsync(DemoSeedConstants.TeacherUserId, "T0001", "teacher@demo.local", RoleNames.Teacher, "0900-000-002");
        var parent = await CreateUserAsync(DemoSeedConstants.ParentUserId, "0900-000-003", "parent@demo.local", RoleNames.Parent, "0900-000-003");
        var student = await CreateUserAsync(DemoSeedConstants.StudentUserId, "S10001", "student@demo.local", RoleNames.Student, "0900-000-004");

        var routeToSchool = new Route
        {
            Id = DemoSeedConstants.MorningRouteId,
            RouteName = "Linkou Morning Route A",
            CampusName = "Kang Chiao Linkou",
            Direction = TripDirection.ToSchool,
            RouteType = RouteType.Standard,
            Stops =
            [
                new RouteStop { Sequence = 1, StopName = "Stop A", Address = "No. 1, Demo Rd." },
                new RouteStop { Sequence = 2, StopName = "Stop B", Address = "No. 2, Demo Rd." }
            ]
        };

        var routeHomebound = new Route
        {
            Id = DemoSeedConstants.DismissalRouteId,
            RouteName = "Linkou Dismissal Route A",
            CampusName = "Kang Chiao Linkou",
            Direction = TripDirection.Homebound,
            RouteType = RouteType.Standard,
            Stops =
            [
                new RouteStop { Sequence = 1, StopName = "Campus", Address = "No. 1, Campus Rd." },
                new RouteStop { Sequence = 2, StopName = "Stop B", Address = "No. 2, Demo Rd." },
                new RouteStop { Sequence = 3, StopName = "Stop A", Address = "No. 1, Demo Rd." }
            ]
        };

        var doorToDoorRoute = new Route
        {
            Id = DemoSeedConstants.KindergartenRouteId,
            RouteName = "Kindergarten Door-to-Door",
            CampusName = "Kang Chiao Linkou",
            Direction = TripDirection.Homebound,
            RouteType = RouteType.DoorToDoor,
            Stops =
            [
                new RouteStop
                {
                    Sequence = 1,
                    StopName = "Child Home",
                    Address = "No. 3, Demo Rd.",
                    HandoffContactName = "Parent Pickup",
                    HandoffContactPhone = "0900-000-003"
                }
            ]
        };

        var adminStaffProfile = new StaffProfile
        {
            Id = DemoSeedConstants.AdminStaffProfileId,
            UserId = admin.Id,
            EmployeeNumber = "E0001",
            FullName = "系統管理員",
            PhoneNumber = "0900-000-001",
            CanManageAllRoutes = true
        };

        var staffProfile = new StaffProfile
        {
            Id = DemoSeedConstants.StaffProfileId,
            UserId = teacher.Id,
            EmployeeNumber = "T0001",
            FullName = "王老師",
            PhoneNumber = "0900-000-002",
            CanManageAllRoutes = false
        };

        var guardian = new Guardian
        {
            Id = DemoSeedConstants.GuardianId,
            UserId = parent.Id,
            FullName = "陳家長",
            PhoneNumber = "0900-000-003",
            Email = parent.Email ?? string.Empty
        };

        var studentProfile = new Student
        {
            Id = DemoSeedConstants.StudentId,
            UserId = student.Id,
            StudentNumber = "S10001",
            FullName = "陳小明",
            Stage = StudentStage.JuniorHigh,
            GradeLabel = "八年級",
            DefaultRoute = routeToSchool
        };

        var link = new StudentGuardianLink
        {
            Student = studentProfile,
            Guardian = guardian,
            IsPrimaryContact = true
        };

        var assignment = new RouteAssignment
        {
            Route = routeToSchool,
            StaffProfile = staffProfile
        };

        var weekStart = new DateOnly(2026, 3, 16);
        var registrations = Enumerable.Range(0, 5)
            .SelectMany(offset =>
            {
                var date = weekStart.AddDays(offset);
                return new[]
                {
                    new RideRegistration
                    {
                        Student = studentProfile,
                        Date = date,
                        Direction = TripDirection.ToSchool,
                        IsRegistered = true,
                        IsPresent = offset < 2,
                        Route = routeToSchool
                    },
                    new RideRegistration
                    {
                        Student = studentProfile,
                        Date = date,
                        Direction = TripDirection.Homebound,
                        IsRegistered = true,
                        IsPresent = false,
                        Route = routeHomebound
                    }
                };
            })
            .ToList();

        var template = new NotificationTemplate
        {
            TemplateName = "Reminder",
            Subject = "Shuttle Bus Registration Reminder",
            Body = "Please complete next week's shuttle bus registration."
        };

        _ = admin;

        dbContext.Routes.AddRange(routeToSchool, routeHomebound, doorToDoorRoute);
        dbContext.StaffProfiles.AddRange(adminStaffProfile, staffProfile);
        dbContext.Guardians.Add(guardian);
        dbContext.Students.Add(studentProfile);
        dbContext.StudentGuardianLinks.Add(link);
        dbContext.RouteAssignments.Add(assignment);
        dbContext.RideRegistrations.AddRange(registrations);
        dbContext.NotificationTemplates.Add(template);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<AppUser> CreateUserAsync(Guid userId, string userName, string email, string roleName, string phoneNumber)
    {
        var user = new AppUser
        {
            Id = userId,
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user, "P@ssw0rd!");
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create seeded user {email}: {string.Join(", ", createResult.Errors.Select(error => error.Description))}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException($"Failed to add role {roleName} for {email}: {string.Join(", ", roleResult.Errors.Select(error => error.Description))}");
        }

        return user;
    }
}
