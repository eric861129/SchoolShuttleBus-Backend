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
        var teacherTwo = await CreateUserAsync(Guid.Parse("55555555-5555-5555-5555-555555555555"), "T0002", "teacher2@demo.local", RoleNames.Teacher, "0900-000-009");

        var parent = await CreateUserAsync(DemoSeedConstants.ParentUserId, "0900-000-003", "parent@demo.local", RoleNames.Parent, "0900-000-003");
        var parentTwo = await CreateUserAsync(Guid.Parse("66666666-6666-6666-6666-666666666662"), "0900-000-005", "parent2@demo.local", RoleNames.Parent, "0900-000-005");
        var parentThree = await CreateUserAsync(Guid.Parse("66666666-6666-6666-6666-666666666663"), "0900-000-006", "parent3@demo.local", RoleNames.Parent, "0900-000-006");
        var parentFour = await CreateUserAsync(Guid.Parse("66666666-6666-6666-6666-666666666664"), "0900-000-007", "parent4@demo.local", RoleNames.Parent, "0900-000-007");
        var parentFive = await CreateUserAsync(Guid.Parse("66666666-6666-6666-6666-666666666665"), "0900-000-008", "parent5@demo.local", RoleNames.Parent, "0900-000-008");

        var student = await CreateUserAsync(DemoSeedConstants.StudentUserId, "S10001", "student@demo.local", RoleNames.Student, "0900-100-001");
        var studentTwoUser = await CreateUserAsync(Guid.Parse("77777777-7777-7777-7777-777777777772"), "S10002", "student2@demo.local", RoleNames.Student, "0900-100-002");
        var studentThreeUser = await CreateUserAsync(Guid.Parse("77777777-7777-7777-7777-777777777773"), "S10003", "student3@demo.local", RoleNames.Student, "0900-100-003");
        var studentFourUser = await CreateUserAsync(Guid.Parse("77777777-7777-7777-7777-777777777774"), "S10004", "student4@demo.local", RoleNames.Student, "0900-100-004");
        var studentFiveUser = await CreateUserAsync(Guid.Parse("77777777-7777-7777-7777-777777777775"), "S10005", "student5@demo.local", RoleNames.Student, "0900-100-005");

        var routeToSchool = new Route
        {
            Id = DemoSeedConstants.MorningRouteId,
            RouteName = "Linkou Morning Route A",
            CampusName = "Kang Chiao Linkou",
            Direction = TripDirection.ToSchool,
            RouteType = RouteType.Standard,
            Stops =
            [
                new RouteStop { Sequence = 1, StopName = "Linkou Station", Address = "No. 8, Wenhua 3rd Rd., Linkou Dist." },
                new RouteStop { Sequence = 2, StopName = "A9 Mitsui Stop", Address = "No. 1, Wenhua 3rd Rd., Linkou Dist." },
                new RouteStop { Sequence = 3, StopName = "Campus Gate", Address = "No. 1, Xingfu Rd., Linkou Dist." }
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
                new RouteStop { Sequence = 1, StopName = "Campus Gate", Address = "No. 1, Xingfu Rd., Linkou Dist." },
                new RouteStop { Sequence = 2, StopName = "A9 Mitsui Stop", Address = "No. 1, Wenhua 3rd Rd., Linkou Dist." },
                new RouteStop { Sequence = 3, StopName = "Linkou Station", Address = "No. 8, Wenhua 3rd Rd., Linkou Dist." }
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
                    HandoffContactName = "黃家長",
                    HandoffContactPhone = "0900-000-008"
                }
            ]
        };

        var backupRoute = new Route
        {
            Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            RouteName = "Linkou Backup Morning Route",
            CampusName = "Kang Chiao Linkou",
            Direction = TripDirection.ToSchool,
            RouteType = RouteType.Standard,
            IsActive = false,
            Stops =
            [
                new RouteStop { Sequence = 1, StopName = "Cultural 2nd Rd.", Address = "No. 66, Cultural 2nd Rd., Linkou Dist." },
                new RouteStop { Sequence = 2, StopName = "Campus Gate", Address = "No. 1, Xingfu Rd., Linkou Dist." }
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

        var teacherTwoProfile = new StaffProfile
        {
            Id = Guid.Parse("99999999-9999-9999-9999-999999999992"),
            UserId = teacherTwo.Id,
            EmployeeNumber = "T0002",
            FullName = "李老師",
            PhoneNumber = "0900-000-009",
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

        var guardianTwo = new Guardian
        {
            Id = Guid.Parse("bbbbbbbb-1111-1111-1111-111111111112"),
            UserId = parentTwo.Id,
            FullName = "林家長",
            PhoneNumber = "0900-000-005",
            Email = parentTwo.Email ?? string.Empty
        };

        var guardianThree = new Guardian
        {
            Id = Guid.Parse("bbbbbbbb-1111-1111-1111-111111111113"),
            UserId = parentThree.Id,
            FullName = "王家長",
            PhoneNumber = "0900-000-006",
            Email = parentThree.Email ?? string.Empty
        };

        var guardianFour = new Guardian
        {
            Id = Guid.Parse("bbbbbbbb-1111-1111-1111-111111111114"),
            UserId = parentFour.Id,
            FullName = "劉家長",
            PhoneNumber = "0900-000-007",
            Email = parentFour.Email ?? string.Empty
        };

        var guardianFive = new Guardian
        {
            Id = Guid.Parse("bbbbbbbb-1111-1111-1111-111111111115"),
            UserId = parentFive.Id,
            FullName = "黃家長",
            PhoneNumber = "0900-000-008",
            Email = parentFive.Email ?? string.Empty
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

        var studentTwo = new Student
        {
            Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111112"),
            UserId = studentTwoUser.Id,
            StudentNumber = "S10002",
            FullName = "林小雨",
            Stage = StudentStage.Elementary,
            GradeLabel = "五年級",
            DefaultRoute = routeToSchool
        };

        var studentThree = new Student
        {
            Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111113"),
            UserId = studentThreeUser.Id,
            StudentNumber = "S10003",
            FullName = "王樂樂",
            Stage = StudentStage.Elementary,
            GradeLabel = "四年級",
            DefaultRoute = routeToSchool
        };

        var studentFour = new Student
        {
            Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111114"),
            UserId = studentFourUser.Id,
            StudentNumber = "S10004",
            FullName = "劉星宇",
            Stage = StudentStage.SeniorHigh,
            GradeLabel = "高一",
            DefaultRoute = routeHomebound
        };

        var studentFive = new Student
        {
            Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111115"),
            UserId = studentFiveUser.Id,
            StudentNumber = "S10005",
            FullName = "黃豆豆",
            Stage = StudentStage.Kindergarten,
            GradeLabel = "K2",
            DefaultRoute = doorToDoorRoute
        };

        var guardianLinks = new[]
        {
            new StudentGuardianLink { Student = studentProfile, Guardian = guardian, IsPrimaryContact = true },
            new StudentGuardianLink { Student = studentTwo, Guardian = guardianTwo, IsPrimaryContact = true },
            new StudentGuardianLink { Student = studentThree, Guardian = guardianThree, IsPrimaryContact = true },
            new StudentGuardianLink { Student = studentFour, Guardian = guardianFour, IsPrimaryContact = true },
            new StudentGuardianLink { Student = studentFive, Guardian = guardianFive, IsPrimaryContact = true }
        };

        var assignments = new[]
        {
            new RouteAssignment { Route = routeToSchool, StaffProfile = staffProfile },
            new RouteAssignment { Route = routeHomebound, StaffProfile = staffProfile },
            new RouteAssignment { Route = doorToDoorRoute, StaffProfile = teacherTwoProfile },
            new RouteAssignment { Route = backupRoute, StaffProfile = teacherTwoProfile }
        };

        var weekStart = new DateOnly(2026, 3, 16);
        var registrations = new List<RideRegistration>();

        registrations.AddRange(CreateWeekRegistrations(
            studentProfile,
            weekStart,
            routeToSchool,
            routeHomebound,
            [true, true, true, true, true],
            [true, true, true, true, true],
            [true, true, true, false, true],
            [true, false, true, true, true]));

        registrations.AddRange(CreateWeekRegistrations(
            studentTwo,
            weekStart,
            routeToSchool,
            routeHomebound,
            [true, true, true, false, false],
            [true, true, true, false, false],
            [true, true, false, false, false],
            [true, false, true, false, false]));

        registrations.AddRange(CreateWeekRegistrations(
            studentThree,
            weekStart,
            routeToSchool,
            routeHomebound,
            [true, true, true, true, true],
            [true, true, true, true, false],
            [true, false, true, true, true],
            [true, true, false, true, false]));

        registrations.AddRange(CreateWeekRegistrations(
            studentFour,
            weekStart,
            routeToSchool,
            routeHomebound,
            [false, false, false, false, false],
            [true, false, true, false, true],
            [false, false, false, false, false],
            [true, false, false, false, true]));

        registrations.AddRange(CreateWeekRegistrations(
            studentFive,
            weekStart,
            routeToSchool,
            doorToDoorRoute,
            [false, false, false, false, false],
            [true, true, true, true, true],
            [false, false, false, false, false],
            [true, true, false, true, true]));

        var reminderTemplate = new NotificationTemplate
        {
            TemplateName = "Reminder",
            Subject = "Shuttle Bus Registration Reminder",
            Body = "Please complete next week's shuttle bus registration."
        };

        var reminderJob = new NotificationJob
        {
            Id = Guid.Parse("12121212-1212-1212-1212-121212121212"),
            JobType = "Reminder",
            Channel = NotificationChannel.Email,
            Subject = reminderTemplate.Subject,
            Body = reminderTemplate.Body,
            CreatedByUserId = admin.Id,
            CreatedAtUtc = new DateTimeOffset(2026, 3, 17, 8, 0, 0, TimeSpan.Zero),
            Deliveries =
            [
                CreateDelivery("parent@demo.local", guardian.UserId, "Sent", new DateTimeOffset(2026, 3, 17, 8, 0, 0, TimeSpan.Zero)),
                CreateDelivery("parent2@demo.local", guardianTwo.UserId, "Sent", new DateTimeOffset(2026, 3, 17, 8, 0, 0, TimeSpan.Zero)),
                CreateDelivery("parent5@demo.local", guardianFive.UserId, "Sent", new DateTimeOffset(2026, 3, 17, 8, 0, 0, TimeSpan.Zero))
            ]
        };

        var broadcastAnnouncement = new BroadcastAnnouncement
        {
            Id = Guid.Parse("34343434-3434-3434-3434-343434343434"),
            Audience = BroadcastAudience.Parents,
            Subject = "Friday Dismissal Reminder",
            Body = "Friday traffic may be heavier than usual. Please arrive at your stop 5 minutes early.",
            CreatedByUserId = admin.Id,
            CreatedAtUtc = new DateTimeOffset(2026, 3, 18, 3, 30, 0, TimeSpan.Zero)
        };

        var broadcastJob = new NotificationJob
        {
            Id = Guid.Parse("56565656-5656-5656-5656-565656565656"),
            JobType = "Broadcast",
            Channel = NotificationChannel.Email,
            Subject = broadcastAnnouncement.Subject,
            Body = broadcastAnnouncement.Body,
            CreatedByUserId = admin.Id,
            CreatedAtUtc = new DateTimeOffset(2026, 3, 18, 3, 30, 0, TimeSpan.Zero),
            Deliveries =
            [
                CreateDelivery("parent@demo.local", guardian.UserId, "Sent", new DateTimeOffset(2026, 3, 18, 3, 31, 0, TimeSpan.Zero)),
                CreateDelivery("parent2@demo.local", guardianTwo.UserId, "Sent", new DateTimeOffset(2026, 3, 18, 3, 31, 0, TimeSpan.Zero)),
                CreateDelivery("parent3@demo.local", guardianThree.UserId, "Sent", new DateTimeOffset(2026, 3, 18, 3, 31, 0, TimeSpan.Zero)),
                CreateDelivery("parent4@demo.local", guardianFour.UserId, "Sent", new DateTimeOffset(2026, 3, 18, 3, 31, 0, TimeSpan.Zero)),
                CreateDelivery("parent5@demo.local", guardianFive.UserId, "Sent", new DateTimeOffset(2026, 3, 18, 3, 31, 0, TimeSpan.Zero))
            ]
        };

        var attendanceSessions = new[]
        {
            new AttendanceSession
            {
                Id = Guid.Parse("67676767-6767-6767-6767-676767676761"),
                Route = routeToSchool,
                Date = weekStart,
                Direction = TripDirection.ToSchool,
                IsCompleted = true,
                CreatedByStaffProfile = staffProfile,
                CreatedAtUtc = new DateTimeOffset(2026, 3, 16, 22, 15, 0, TimeSpan.Zero),
                Records =
                [
                    CreateAttendanceRecord(studentProfile, guardian.PhoneNumber, AttendanceStatus.Present),
                    CreateAttendanceRecord(studentTwo, guardianTwo.PhoneNumber, AttendanceStatus.Present),
                    CreateAttendanceRecord(studentThree, guardianThree.PhoneNumber, AttendanceStatus.Missing)
                ]
            },
            new AttendanceSession
            {
                Id = Guid.Parse("67676767-6767-6767-6767-676767676762"),
                Route = routeHomebound,
                Date = weekStart,
                Direction = TripDirection.Homebound,
                IsCompleted = true,
                CreatedByStaffProfile = staffProfile,
                CreatedAtUtc = new DateTimeOffset(2026, 3, 16, 9, 20, 0, TimeSpan.Zero),
                Records =
                [
                    CreateAttendanceRecord(studentProfile, guardian.PhoneNumber, AttendanceStatus.Present),
                    CreateAttendanceRecord(studentTwo, guardianTwo.PhoneNumber, AttendanceStatus.Excused),
                    CreateAttendanceRecord(studentThree, guardianThree.PhoneNumber, AttendanceStatus.Present),
                    CreateAttendanceRecord(studentFour, guardianFour.PhoneNumber, AttendanceStatus.Present)
                ]
            },
            new AttendanceSession
            {
                Id = Guid.Parse("67676767-6767-6767-6767-676767676763"),
                Route = doorToDoorRoute,
                Date = weekStart.AddDays(1),
                Direction = TripDirection.Homebound,
                IsCompleted = true,
                CreatedByStaffProfile = teacherTwoProfile,
                CreatedAtUtc = new DateTimeOffset(2026, 3, 17, 9, 40, 0, TimeSpan.Zero),
                Records =
                [
                    CreateAttendanceRecord(studentFive, guardianFive.PhoneNumber, AttendanceStatus.Present)
                ]
            }
        };

        dbContext.Routes.AddRange(routeToSchool, routeHomebound, doorToDoorRoute, backupRoute);
        dbContext.StaffProfiles.AddRange(adminStaffProfile, staffProfile, teacherTwoProfile);
        dbContext.Guardians.AddRange(guardian, guardianTwo, guardianThree, guardianFour, guardianFive);
        dbContext.Students.AddRange(studentProfile, studentTwo, studentThree, studentFour, studentFive);
        dbContext.StudentGuardianLinks.AddRange(guardianLinks);
        dbContext.RouteAssignments.AddRange(assignments);
        dbContext.RideRegistrations.AddRange(registrations);
        dbContext.NotificationTemplates.Add(reminderTemplate);
        dbContext.BroadcastAnnouncements.Add(broadcastAnnouncement);
        dbContext.NotificationJobs.AddRange(reminderJob, broadcastJob);
        dbContext.AttendanceSessions.AddRange(attendanceSessions);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<RideRegistration> CreateWeekRegistrations(
        Student student,
        DateOnly weekStart,
        Route toSchoolRoute,
        Route homeboundRoute,
        IReadOnlyList<bool> toSchoolRegistered,
        IReadOnlyList<bool> homeboundRegistered,
        IReadOnlyList<bool> toSchoolPresent,
        IReadOnlyList<bool> homeboundPresent)
    {
        foreach (var offset in Enumerable.Range(0, 5))
        {
            yield return new RideRegistration
            {
                Student = student,
                Date = weekStart.AddDays(offset),
                Direction = TripDirection.ToSchool,
                IsRegistered = toSchoolRegistered[offset],
                IsPresent = toSchoolRegistered[offset] && toSchoolPresent[offset],
                Route = toSchoolRegistered[offset] ? toSchoolRoute : null
            };

            yield return new RideRegistration
            {
                Student = student,
                Date = weekStart.AddDays(offset),
                Direction = TripDirection.Homebound,
                IsRegistered = homeboundRegistered[offset],
                IsPresent = homeboundRegistered[offset] && homeboundPresent[offset],
                Route = homeboundRegistered[offset] ? homeboundRoute : null
            };
        }
    }

    private static AttendanceRecord CreateAttendanceRecord(Student student, string emergencyPhone, AttendanceStatus status)
    {
        return new AttendanceRecord
        {
            Student = student,
            Status = status,
            EmergencyPhoneSnapshot = emergencyPhone,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };
    }

    private static NotificationDelivery CreateDelivery(string email, Guid recipientUserId, string status, DateTimeOffset sentAtUtc)
    {
        return new NotificationDelivery
        {
            RecipientEmail = email,
            RecipientUserId = recipientUserId,
            Status = status,
            SentAtUtc = sentAtUtc,
            CreatedAtUtc = sentAtUtc
        };
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
