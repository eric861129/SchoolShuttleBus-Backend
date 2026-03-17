using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Auth;

namespace SchoolShuttleBus.Infrastructure.Persistence;

/// <summary>
/// Seeds a deterministic demo dataset so the API can be exercised from Swagger,
/// integration tests, and interview walkthroughs without any manual setup.
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

        var admin = await CreateUserAsync(DemoSeedConstants.AdminUserId, "admin@demo.local", RoleNames.Administrator);
        var teacher = await CreateUserAsync(DemoSeedConstants.TeacherUserId, "teacher@demo.local", RoleNames.Teacher);
        var parent = await CreateUserAsync(DemoSeedConstants.ParentUserId, "parent@demo.local", RoleNames.Parent);
        var student = await CreateUserAsync(DemoSeedConstants.StudentUserId, "student@demo.local", RoleNames.Student);

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

        var staffProfile = new StaffProfile
        {
            Id = DemoSeedConstants.StaffProfileId,
            UserId = teacher.Id,
            FullName = "Demo Teacher",
            PhoneNumber = "0900-000-002",
            CanManageAllRoutes = false
        };

        var guardian = new Guardian
        {
            Id = DemoSeedConstants.GuardianId,
            UserId = parent.Id,
            FullName = "Demo Parent",
            PhoneNumber = "0900-000-003",
            Email = parent.Email ?? string.Empty
        };

        var studentProfile = new Student
        {
            Id = DemoSeedConstants.StudentId,
            UserId = student.Id,
            StudentNumber = "S10001",
            FullName = "Demo Student",
            Stage = StudentStage.JuniorHigh,
            GradeLabel = "Grade 8",
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
        dbContext.StaffProfiles.Add(staffProfile);
        dbContext.Guardians.Add(guardian);
        dbContext.Students.Add(studentProfile);
        dbContext.StudentGuardianLinks.Add(link);
        dbContext.RouteAssignments.Add(assignment);
        dbContext.RideRegistrations.AddRange(registrations);
        dbContext.NotificationTemplates.Add(template);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<AppUser> CreateUserAsync(Guid userId, string email, string roleName)
    {
        var user = new AppUser
        {
            Id = userId,
            UserName = email,
            Email = email,
            EmailConfirmed = true
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
