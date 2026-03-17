SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @UtcNow datetimeoffset = '2026-03-17T08:00:00+00:00';
    DECLARE @WeekStart date = '2026-03-23';
    DECLARE @SharedPasswordHash nvarchar(max) = N'AQAAAAIAAYagAAAAEBSabhZ8yVQgGoL7feO34xInbljHWBG5hbE93mTIMXhYIuwekDxaxB3/ZlAH04KzHQ==';

    DECLARE @AdminUserId uniqueidentifier = '51000000-0000-0000-0000-000000000001';
    DECLARE @Teacher1UserId uniqueidentifier = '52000000-0000-0000-0000-000000000001';
    DECLARE @Teacher2UserId uniqueidentifier = '52000000-0000-0000-0000-000000000002';
    DECLARE @Parent1UserId uniqueidentifier = '53000000-0000-0000-0000-000000000001';
    DECLARE @Parent2UserId uniqueidentifier = '53000000-0000-0000-0000-000000000002';
    DECLARE @Parent3UserId uniqueidentifier = '53000000-0000-0000-0000-000000000003';
    DECLARE @Parent4UserId uniqueidentifier = '53000000-0000-0000-0000-000000000004';
    DECLARE @Parent5UserId uniqueidentifier = '53000000-0000-0000-0000-000000000005';
    DECLARE @Student1UserId uniqueidentifier = '54000000-0000-0000-0000-000000000001';
    DECLARE @Student2UserId uniqueidentifier = '54000000-0000-0000-0000-000000000002';
    DECLARE @Student3UserId uniqueidentifier = '54000000-0000-0000-0000-000000000003';
    DECLARE @Student4UserId uniqueidentifier = '54000000-0000-0000-0000-000000000004';
    DECLARE @Student5UserId uniqueidentifier = '54000000-0000-0000-0000-000000000005';

    DECLARE @AdminStaffProfileId uniqueidentifier = '61000000-0000-0000-0000-000000000001';
    DECLARE @Teacher1StaffProfileId uniqueidentifier = '61000000-0000-0000-0000-000000000002';
    DECLARE @Teacher2StaffProfileId uniqueidentifier = '61000000-0000-0000-0000-000000000003';

    DECLARE @Guardian1Id uniqueidentifier = '62000000-0000-0000-0000-000000000001';
    DECLARE @Guardian2Id uniqueidentifier = '62000000-0000-0000-0000-000000000002';
    DECLARE @Guardian3Id uniqueidentifier = '62000000-0000-0000-0000-000000000003';
    DECLARE @Guardian4Id uniqueidentifier = '62000000-0000-0000-0000-000000000004';
    DECLARE @Guardian5Id uniqueidentifier = '62000000-0000-0000-0000-000000000005';

    DECLARE @RouteMorningBId uniqueidentifier = '63000000-0000-0000-0000-000000000001';
    DECLARE @RouteMorningCId uniqueidentifier = '63000000-0000-0000-0000-000000000002';
    DECLARE @RouteDismissalBId uniqueidentifier = '63000000-0000-0000-0000-000000000003';
    DECLARE @RouteDoorToDoorBId uniqueidentifier = '63000000-0000-0000-0000-000000000004';

    DECLARE @Student1Id uniqueidentifier = '64000000-0000-0000-0000-000000000001';
    DECLARE @Student2Id uniqueidentifier = '64000000-0000-0000-0000-000000000002';
    DECLARE @Student3Id uniqueidentifier = '64000000-0000-0000-0000-000000000003';
    DECLARE @Student4Id uniqueidentifier = '64000000-0000-0000-0000-000000000004';
    DECLARE @Student5Id uniqueidentifier = '64000000-0000-0000-0000-000000000005';

    DECLARE @RoleAdminId uniqueidentifier =
        (SELECT TOP (1) Id FROM AspNetRoles WHERE NormalizedName = N'ADMINISTRATOR');
    DECLARE @RoleTeacherId uniqueidentifier =
        (SELECT TOP (1) Id FROM AspNetRoles WHERE NormalizedName = N'TEACHER');
    DECLARE @RoleParentId uniqueidentifier =
        (SELECT TOP (1) Id FROM AspNetRoles WHERE NormalizedName = N'PARENT');
    DECLARE @RoleStudentId uniqueidentifier =
        (SELECT TOP (1) Id FROM AspNetRoles WHERE NormalizedName = N'STUDENT');

    IF @RoleAdminId IS NULL
    BEGIN
        SET @RoleAdminId = '71000000-0000-0000-0000-000000000001';
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleAdminId, N'Administrator', N'ADMINISTRATOR', CONVERT(nvarchar(36), NEWID()));
    END;

    IF @RoleTeacherId IS NULL
    BEGIN
        SET @RoleTeacherId = '71000000-0000-0000-0000-000000000002';
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleTeacherId, N'Teacher', N'TEACHER', CONVERT(nvarchar(36), NEWID()));
    END;

    IF @RoleParentId IS NULL
    BEGIN
        SET @RoleParentId = '71000000-0000-0000-0000-000000000003';
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleParentId, N'Parent', N'PARENT', CONVERT(nvarchar(36), NEWID()));
    END;

    IF @RoleStudentId IS NULL
    BEGIN
        SET @RoleStudentId = '71000000-0000-0000-0000-000000000004';
        INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
        VALUES (@RoleStudentId, N'Student', N'STUDENT', CONVERT(nvarchar(36), NEWID()));
    END;

    IF EXISTS
    (
        SELECT 1
        FROM AspNetUsers
        WHERE UserName IN
        (
            N'ops.admin@demo.local',
            N'olivia.lin.teacher@demo.local',
            N'brian.wu.teacher@demo.local',
            N'grace.chen.parent@demo.local',
            N'kevin.chen.parent@demo.local',
            N'sophia.liu.parent@demo.local',
            N'iris.wang.parent@demo.local',
            N'nina.lin.parent@demo.local',
            N'ethan.chen.student@demo.local',
            N'emma.chen.student@demo.local',
            N'leo.liu.student@demo.local',
            N'mia.wang.student@demo.local',
            N'noah.lin.student@demo.local'
        )
    )
    BEGIN
        THROW 50001, 'Demo dataset already appears to be imported. Stop before re-running this script.', 1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Students
        WHERE StudentNumber IN (N'S20001', N'S20002', N'S20003', N'S20004', N'S20005')
    )
    BEGIN
        THROW 50002, 'Student numbers from the one-time demo dataset already exist.', 1;
    END;

    INSERT INTO AspNetUsers
    (
        Id,
        UserName,
        NormalizedUserName,
        Email,
        NormalizedEmail,
        EmailConfirmed,
        PasswordHash,
        SecurityStamp,
        ConcurrencyStamp,
        PhoneNumber,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEnd,
        LockoutEnabled,
        AccessFailedCount
    )
    VALUES
    (@AdminUserId, N'ops.admin@demo.local', N'OPS.ADMIN@DEMO.LOCAL', N'ops.admin@demo.local', N'OPS.ADMIN@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-100-001', 1, 0, NULL, 1, 0),
    (@Teacher1UserId, N'olivia.lin.teacher@demo.local', N'OLIVIA.LIN.TEACHER@DEMO.LOCAL', N'olivia.lin.teacher@demo.local', N'OLIVIA.LIN.TEACHER@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-100-002', 1, 0, NULL, 1, 0),
    (@Teacher2UserId, N'brian.wu.teacher@demo.local', N'BRIAN.WU.TEACHER@DEMO.LOCAL', N'brian.wu.teacher@demo.local', N'BRIAN.WU.TEACHER@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-100-003', 1, 0, NULL, 1, 0),
    (@Parent1UserId, N'grace.chen.parent@demo.local', N'GRACE.CHEN.PARENT@DEMO.LOCAL', N'grace.chen.parent@demo.local', N'GRACE.CHEN.PARENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-200-001', 1, 0, NULL, 1, 0),
    (@Parent2UserId, N'kevin.chen.parent@demo.local', N'KEVIN.CHEN.PARENT@DEMO.LOCAL', N'kevin.chen.parent@demo.local', N'KEVIN.CHEN.PARENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-200-002', 1, 0, NULL, 1, 0),
    (@Parent3UserId, N'sophia.liu.parent@demo.local', N'SOPHIA.LIU.PARENT@DEMO.LOCAL', N'sophia.liu.parent@demo.local', N'SOPHIA.LIU.PARENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-200-003', 1, 0, NULL, 1, 0),
    (@Parent4UserId, N'iris.wang.parent@demo.local', N'IRIS.WANG.PARENT@DEMO.LOCAL', N'iris.wang.parent@demo.local', N'IRIS.WANG.PARENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-200-004', 1, 0, NULL, 1, 0),
    (@Parent5UserId, N'nina.lin.parent@demo.local', N'NINA.LIN.PARENT@DEMO.LOCAL', N'nina.lin.parent@demo.local', N'NINA.LIN.PARENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-200-005', 1, 0, NULL, 1, 0),
    (@Student1UserId, N'ethan.chen.student@demo.local', N'ETHAN.CHEN.STUDENT@DEMO.LOCAL', N'ethan.chen.student@demo.local', N'ETHAN.CHEN.STUDENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-300-001', 1, 0, NULL, 1, 0),
    (@Student2UserId, N'emma.chen.student@demo.local', N'EMMA.CHEN.STUDENT@DEMO.LOCAL', N'emma.chen.student@demo.local', N'EMMA.CHEN.STUDENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-300-002', 1, 0, NULL, 1, 0),
    (@Student3UserId, N'leo.liu.student@demo.local', N'LEO.LIU.STUDENT@DEMO.LOCAL', N'leo.liu.student@demo.local', N'LEO.LIU.STUDENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-300-003', 1, 0, NULL, 1, 0),
    (@Student4UserId, N'mia.wang.student@demo.local', N'MIA.WANG.STUDENT@DEMO.LOCAL', N'mia.wang.student@demo.local', N'MIA.WANG.STUDENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-300-004', 1, 0, NULL, 1, 0),
    (@Student5UserId, N'noah.lin.student@demo.local', N'NOAH.LIN.STUDENT@DEMO.LOCAL', N'noah.lin.student@demo.local', N'NOAH.LIN.STUDENT@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0900-300-005', 1, 0, NULL, 1, 0);

    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES
    (@AdminUserId, @RoleAdminId),
    (@Teacher1UserId, @RoleTeacherId),
    (@Teacher2UserId, @RoleTeacherId),
    (@Parent1UserId, @RoleParentId),
    (@Parent2UserId, @RoleParentId),
    (@Parent3UserId, @RoleParentId),
    (@Parent4UserId, @RoleParentId),
    (@Parent5UserId, @RoleParentId),
    (@Student1UserId, @RoleStudentId),
    (@Student2UserId, @RoleStudentId),
    (@Student3UserId, @RoleStudentId),
    (@Student4UserId, @RoleStudentId),
    (@Student5UserId, @RoleStudentId);

    INSERT INTO StaffProfiles (Id, UserId, FullName, PhoneNumber, CanManageAllRoutes, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@AdminStaffProfileId, @AdminUserId, N'Demo Operations Admin', N'0900-100-001', 1, @UtcNow, NULL),
    (@Teacher1StaffProfileId, @Teacher1UserId, N'Olivia Lin', N'0900-100-002', 1, @UtcNow, NULL),
    (@Teacher2StaffProfileId, @Teacher2UserId, N'Brian Wu', N'0900-100-003', 0, @UtcNow, NULL);

    INSERT INTO Guardians (Id, UserId, FullName, PhoneNumber, Email, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@Guardian1Id, @Parent1UserId, N'Grace Chen', N'0900-200-001', N'grace.chen.parent@demo.local', @UtcNow, NULL),
    (@Guardian2Id, @Parent2UserId, N'Kevin Chen', N'0900-200-002', N'kevin.chen.parent@demo.local', @UtcNow, NULL),
    (@Guardian3Id, @Parent3UserId, N'Sophia Liu', N'0900-200-003', N'sophia.liu.parent@demo.local', @UtcNow, NULL),
    (@Guardian4Id, @Parent4UserId, N'Iris Wang', N'0900-200-004', N'iris.wang.parent@demo.local', @UtcNow, NULL),
    (@Guardian5Id, @Parent5UserId, N'Nina Lin', N'0900-200-005', N'nina.lin.parent@demo.local', @UtcNow, NULL);

    INSERT INTO Routes (Id, RouteName, RouteType, Direction, CampusName, IsActive, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@RouteMorningBId, N'Linkou Morning Route B', 1, 1, N'Kang Chiao Linkou', 1, @UtcNow, NULL),
    (@RouteMorningCId, N'Taipei Morning Route C', 1, 1, N'Kang Chiao Linkou', 1, @UtcNow, NULL),
    (@RouteDismissalBId, N'Linkou Dismissal Route B', 1, 2, N'Kang Chiao Linkou', 1, @UtcNow, NULL),
    (@RouteDoorToDoorBId, N'Kindergarten Door-to-Door B', 2, 2, N'Kang Chiao Linkou', 1, @UtcNow, NULL);

    INSERT INTO RouteStops (Id, RouteId, Sequence, StopName, Address, HandoffContactName, HandoffContactPhone, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('63100000-0000-0000-0000-000000000001', @RouteMorningBId, 1, N'Linkou Station', N'No. 8, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000002', @RouteMorningBId, 2, N'A9 Mitsui Stop', N'No. 1, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000003', @RouteMorningBId, 3, N'Campus Gate', N'No. 1, Xingfu Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000004', @RouteMorningCId, 1, N'Xingtian Temple', N'No. 139, Minquan E. Rd., Taipei City', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000005', @RouteMorningCId, 2, N'Yuanshan Station', N'No. 163, Jihe Rd., Taipei City', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000006', @RouteMorningCId, 3, N'Campus Gate', N'No. 1, Xingfu Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000007', @RouteDismissalBId, 1, N'Campus Gate', N'No. 1, Xingfu Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000008', @RouteDismissalBId, 2, N'A9 Mitsui Stop', N'No. 1, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000009', @RouteDismissalBId, 3, N'Linkou Station', N'No. 8, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000010', @RouteDoorToDoorBId, 1, N'Mia Home', N'No. 28, Fuxing Rd., Linkou Dist.', N'Iris Wang', N'0900-200-004', @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000011', @RouteDoorToDoorBId, 2, N'Noah Home', N'No. 36, Renai Rd., Linkou Dist.', N'Nina Lin', N'0900-200-005', @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000012', @RouteDoorToDoorBId, 3, N'Ethan Temporary Drop-Off', N'No. 100, Civic Blvd., Linkou Dist.', N'Grace Chen', N'0900-200-001', @UtcNow, NULL);

    INSERT INTO Students (Id, UserId, StudentNumber, FullName, Stage, GradeLabel, DefaultRouteId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@Student1Id, @Student1UserId, N'S20001', N'Ethan Chen', 2, N'Grade 4', @RouteMorningBId, @UtcNow, NULL),
    (@Student2Id, @Student2UserId, N'S20002', N'Emma Chen', 3, N'Grade 7', @RouteMorningBId, @UtcNow, NULL),
    (@Student3Id, @Student3UserId, N'S20003', N'Leo Liu', 4, N'Grade 10', @RouteMorningCId, @UtcNow, NULL),
    (@Student4Id, @Student4UserId, N'S20004', N'Mia Wang', 2, N'Grade 5', @RouteMorningBId, @UtcNow, NULL),
    (@Student5Id, @Student5UserId, N'S20005', N'Noah Lin', 1, N'K2', @RouteMorningCId, @UtcNow, NULL);

    INSERT INTO StudentGuardianLinks (Id, StudentId, GuardianId, IsPrimaryContact, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('65000000-0000-0000-0000-000000000001', @Student1Id, @Guardian1Id, 1, @UtcNow, NULL),
    ('65000000-0000-0000-0000-000000000002', @Student1Id, @Guardian2Id, 0, @UtcNow, NULL),
    ('65000000-0000-0000-0000-000000000003', @Student2Id, @Guardian1Id, 1, @UtcNow, NULL),
    ('65000000-0000-0000-0000-000000000004', @Student2Id, @Guardian2Id, 0, @UtcNow, NULL),
    ('65000000-0000-0000-0000-000000000005', @Student3Id, @Guardian3Id, 1, @UtcNow, NULL),
    ('65000000-0000-0000-0000-000000000006', @Student4Id, @Guardian4Id, 1, @UtcNow, NULL),
    ('65000000-0000-0000-0000-000000000007', @Student5Id, @Guardian5Id, 1, @UtcNow, NULL);

    INSERT INTO RouteAssignments (Id, RouteId, StaffProfileId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('66000000-0000-0000-0000-000000000001', @RouteMorningBId, @Teacher1StaffProfileId, @UtcNow, NULL),
    ('66000000-0000-0000-0000-000000000002', @RouteDismissalBId, @Teacher1StaffProfileId, @UtcNow, NULL),
    ('66000000-0000-0000-0000-000000000003', @RouteMorningCId, @Teacher2StaffProfileId, @UtcNow, NULL),
    ('66000000-0000-0000-0000-000000000004', @RouteDoorToDoorBId, @Teacher2StaffProfileId, @UtcNow, NULL);

    INSERT INTO RideRegistrations (Id, StudentId, Date, Direction, IsRegistered, IsPresent, RouteId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('67000000-0000-0000-0000-000000000001', @Student1Id, '2026-03-23', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000002', @Student1Id, '2026-03-23', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000003', @Student1Id, '2026-03-24', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000004', @Student1Id, '2026-03-24', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000005', @Student1Id, '2026-03-25', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000006', @Student1Id, '2026-03-25', 2, 1, 0, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000007', @Student1Id, '2026-03-26', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000008', @Student1Id, '2026-03-26', 2, 1, 1, @RouteDoorToDoorBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000009', @Student1Id, '2026-03-27', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000010', @Student1Id, '2026-03-27', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000011', @Student2Id, '2026-03-23', 1, 1, 0, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000012', @Student2Id, '2026-03-23', 2, 1, 0, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000013', @Student2Id, '2026-03-24', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000014', @Student2Id, '2026-03-24', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000015', @Student2Id, '2026-03-25', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000016', @Student2Id, '2026-03-25', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000017', @Student2Id, '2026-03-27', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000018', @Student2Id, '2026-03-27', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000019', @Student3Id, '2026-03-23', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000020', @Student3Id, '2026-03-23', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000021', @Student3Id, '2026-03-24', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000022', @Student3Id, '2026-03-25', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000023', @Student3Id, '2026-03-25', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000024', @Student3Id, '2026-03-26', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000025', @Student3Id, '2026-03-27', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000026', @Student3Id, '2026-03-27', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000027', @Student4Id, '2026-03-23', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000028', @Student4Id, '2026-03-23', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000029', @Student4Id, '2026-03-24', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000030', @Student4Id, '2026-03-24', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000031', @Student4Id, '2026-03-25', 1, 1, 0, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000032', @Student4Id, '2026-03-25', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000033', @Student4Id, '2026-03-26', 1, 1, 1, @RouteMorningBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000034', @Student4Id, '2026-03-26', 2, 1, 1, @RouteDismissalBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000035', @Student5Id, '2026-03-23', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000036', @Student5Id, '2026-03-23', 2, 1, 1, @RouteDoorToDoorBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000037', @Student5Id, '2026-03-24', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000038', @Student5Id, '2026-03-24', 2, 1, 1, @RouteDoorToDoorBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000039', @Student5Id, '2026-03-25', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000040', @Student5Id, '2026-03-25', 2, 1, 1, @RouteDoorToDoorBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000041', @Student5Id, '2026-03-26', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000042', @Student5Id, '2026-03-26', 2, 1, 1, @RouteDoorToDoorBId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000043', @Student5Id, '2026-03-27', 1, 1, 1, @RouteMorningCId, @UtcNow, NULL),
    ('67000000-0000-0000-0000-000000000044', @Student5Id, '2026-03-27', 2, 1, 1, @RouteDoorToDoorBId, @UtcNow, NULL);

    INSERT INTO AttendanceSessions (Id, RouteId, Date, Direction, IsCompleted, CreatedByStaffProfileId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('68000000-0000-0000-0000-000000000001', @RouteMorningBId, '2026-03-23', 1, 1, @Teacher1StaffProfileId, @UtcNow, NULL),
    ('68000000-0000-0000-0000-000000000002', @RouteMorningCId, '2026-03-23', 1, 1, @Teacher2StaffProfileId, @UtcNow, NULL),
    ('68000000-0000-0000-0000-000000000003', @RouteDismissalBId, '2026-03-23', 2, 1, @Teacher1StaffProfileId, @UtcNow, NULL),
    ('68000000-0000-0000-0000-000000000004', @RouteDoorToDoorBId, '2026-03-23', 2, 1, @Teacher2StaffProfileId, @UtcNow, NULL);

    INSERT INTO AttendanceRecords (Id, AttendanceSessionId, StudentId, Status, EmergencyPhoneSnapshot, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('68100000-0000-0000-0000-000000000001', '68000000-0000-0000-0000-000000000001', @Student1Id, 2, N'0900-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000002', '68000000-0000-0000-0000-000000000001', @Student2Id, 3, N'0900-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000003', '68000000-0000-0000-0000-000000000001', @Student4Id, 2, N'0900-200-004', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000004', '68000000-0000-0000-0000-000000000002', @Student3Id, 2, N'0900-200-003', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000005', '68000000-0000-0000-0000-000000000002', @Student5Id, 2, N'0900-200-005', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000006', '68000000-0000-0000-0000-000000000003', @Student1Id, 2, N'0900-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000007', '68000000-0000-0000-0000-000000000003', @Student2Id, 4, N'0900-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000008', '68000000-0000-0000-0000-000000000003', @Student3Id, 2, N'0900-200-003', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000009', '68000000-0000-0000-0000-000000000003', @Student4Id, 2, N'0900-200-004', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000010', '68000000-0000-0000-0000-000000000004', @Student5Id, 2, N'0900-200-005', @UtcNow, NULL);

    INSERT INTO DispatchOverrides (Id, StudentId, RouteId, Date, Direction, Status, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('69000000-0000-0000-0000-000000000001', @Student1Id, @RouteDoorToDoorBId, '2026-03-26', 2, 1, @UtcNow, NULL);

    INSERT INTO NotificationTemplates (Id, TemplateName, Subject, Body, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000001', N'Weekly Reminder - EN', N'Shuttle bus registration reminder', N'Please confirm next week''s bus registrations before Friday 18:00.', @UtcNow, NULL);

    INSERT INTO NotificationJobs (Id, JobType, Channel, Subject, Body, CreatedByUserId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000002', N'Broadcast', 1, N'Friday registration reminder', N'Please finish next week''s registration before Friday evening.', @AdminUserId, @UtcNow, NULL);

    INSERT INTO NotificationDeliveries (Id, NotificationJobId, RecipientUserId, RecipientEmail, Status, SentAtUtc, ErrorMessage, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', @Parent1UserId, N'grace.chen.parent@demo.local', N'Sent', @UtcNow, NULL, @UtcNow, NULL),
    ('70000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000002', @Parent3UserId, N'sophia.liu.parent@demo.local', N'Sent', @UtcNow, NULL, @UtcNow, NULL),
    ('70000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', @Parent5UserId, N'nina.lin.parent@demo.local', N'Sent', @UtcNow, NULL, @UtcNow, NULL);

    INSERT INTO BroadcastAnnouncements (Id, Audience, Subject, Body, CreatedByUserId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000006', 2, N'Weather notice', N'Friday dismissal will be delayed by 15 minutes because of heavy rain.', @AdminUserId, @UtcNow, NULL);

    COMMIT TRANSACTION;

    SELECT UserName, PhoneNumber
    FROM AspNetUsers
    WHERE Id IN
    (
        @AdminUserId,
        @Teacher1UserId,
        @Teacher2UserId,
        @Parent1UserId,
        @Parent2UserId,
        @Parent3UserId,
        @Parent4UserId,
        @Parent5UserId,
        @Student1UserId,
        @Student2UserId,
        @Student3UserId,
        @Student4UserId,
        @Student5UserId
    )
    ORDER BY UserName;

    SELECT StudentNumber, FullName, GradeLabel, DefaultRouteId
    FROM Students
    WHERE Id IN (@Student1Id, @Student2Id, @Student3Id, @Student4Id, @Student5Id)
    ORDER BY StudentNumber;

    SELECT RouteName, Direction, RouteType, CampusName
    FROM Routes
    WHERE Id IN (@RouteMorningBId, @RouteMorningCId, @RouteDismissalBId, @RouteDoorToDoorBId)
    ORDER BY RouteName;

    SELECT COUNT(*) AS ImportedRideRegistrations
    FROM RideRegistrations
    WHERE Id BETWEEN '67000000-0000-0000-0000-000000000001' AND '67000000-0000-0000-0000-000000000044';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    THROW;
END CATCH;
