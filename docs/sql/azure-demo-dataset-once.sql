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
            N'E0001',
            N'T0001',
            N'T0002',
            N'0910-200-001',
            N'0910-200-002',
            N'0910-200-003',
            N'0910-200-004',
            N'0910-200-005',
            N'S20001',
            N'S20002',
            N'S20003',
            N'S20004',
            N'S20005'
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
    (@AdminUserId, N'E0001', N'E0001', N'guanliyuan@demo.local', N'GUANLIYUAN@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-100-001', 1, 0, NULL, 1, 0),
    (@Teacher1UserId, N'T0001', N'T0001', N'lin.laoshi@demo.local', N'LIN.LAOSHI@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-100-002', 1, 0, NULL, 1, 0),
    (@Teacher2UserId, N'T0002', N'T0002', N'wu.laoshi@demo.local', N'WU.LAOSHI@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-100-003', 1, 0, NULL, 1, 0),
    (@Parent1UserId, N'0910-200-001', N'0910-200-001', N'chen.jiazhang1@demo.local', N'CHEN.JIAZHANG1@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-200-001', 1, 0, NULL, 1, 0),
    (@Parent2UserId, N'0910-200-002', N'0910-200-002', N'chen.jiazhang2@demo.local', N'CHEN.JIAZHANG2@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-200-002', 1, 0, NULL, 1, 0),
    (@Parent3UserId, N'0910-200-003', N'0910-200-003', N'liu.jiazhang@demo.local', N'LIU.JIAZHANG@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-200-003', 1, 0, NULL, 1, 0),
    (@Parent4UserId, N'0910-200-004', N'0910-200-004', N'wang.jiazhang@demo.local', N'WANG.JIAZHANG@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-200-004', 1, 0, NULL, 1, 0),
    (@Parent5UserId, N'0910-200-005', N'0910-200-005', N'lin.jiazhang@demo.local', N'LIN.JIAZHANG@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-200-005', 1, 0, NULL, 1, 0),
    (@Student1UserId, N'S20001', N'S20001', N'xiaoming.chen@demo.local', N'XIAOMING.CHEN@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-300-001', 1, 0, NULL, 1, 0),
    (@Student2UserId, N'S20002', N'S20002', N'xiaomei.chen@demo.local', N'XIAOMEI.CHEN@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-300-002', 1, 0, NULL, 1, 0),
    (@Student3UserId, N'S20003', N'S20003', N'zihao.liu@demo.local', N'ZIHAO.LIU@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-300-003', 1, 0, NULL, 1, 0),
    (@Student4UserId, N'S20004', N'S20004', N'yuting.wang@demo.local', N'YUTING.WANG@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-300-004', 1, 0, NULL, 1, 0),
    (@Student5UserId, N'S20005', N'S20005', N'xiaoyu.lin@demo.local', N'XIAOYU.LIN@DEMO.LOCAL', 1, @SharedPasswordHash, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()), N'0910-300-005', 1, 0, NULL, 1, 0);

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

    INSERT INTO StaffProfiles (Id, UserId, EmployeeNumber, FullName, PhoneNumber, CanManageAllRoutes, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@AdminStaffProfileId, @AdminUserId, N'E0001', N'系統管理員', N'0910-100-001', 1, @UtcNow, NULL),
    (@Teacher1StaffProfileId, @Teacher1UserId, N'T0001', N'林老師', N'0910-100-002', 1, @UtcNow, NULL),
    (@Teacher2StaffProfileId, @Teacher2UserId, N'T0002', N'吳老師', N'0910-100-003', 0, @UtcNow, NULL);

    INSERT INTO Guardians (Id, UserId, FullName, PhoneNumber, Email, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@Guardian1Id, @Parent1UserId, N'陳媽媽', N'0910-200-001', N'chen.jiazhang1@demo.local', @UtcNow, NULL),
    (@Guardian2Id, @Parent2UserId, N'陳爸爸', N'0910-200-002', N'chen.jiazhang2@demo.local', @UtcNow, NULL),
    (@Guardian3Id, @Parent3UserId, N'劉媽媽', N'0910-200-003', N'liu.jiazhang@demo.local', @UtcNow, NULL),
    (@Guardian4Id, @Parent4UserId, N'王媽媽', N'0910-200-004', N'wang.jiazhang@demo.local', @UtcNow, NULL),
    (@Guardian5Id, @Parent5UserId, N'林媽媽', N'0910-200-005', N'lin.jiazhang@demo.local', @UtcNow, NULL);

    INSERT INTO Routes (Id, RouteName, RouteType, Direction, CampusName, IsActive, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@RouteMorningBId, N'林口上午線 B', 1, 1, N'康橋林口校區', 1, @UtcNow, NULL),
    (@RouteMorningCId, N'台北上午線 C', 1, 1, N'康橋林口校區', 1, @UtcNow, NULL),
    (@RouteDismissalBId, N'林口放學線 B', 1, 2, N'康橋林口校區', 1, @UtcNow, NULL),
    (@RouteDoorToDoorBId, N'幼兒園到府線 B', 2, 2, N'康橋林口校區', 1, @UtcNow, NULL);

    INSERT INTO RouteStops (Id, RouteId, Sequence, StopName, Address, HandoffContactName, HandoffContactPhone, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('63100000-0000-0000-0000-000000000001', @RouteMorningBId, 1, N'林口站', N'新北市林口區文化三路 8 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000002', @RouteMorningBId, 2, N'A9 三井站', N'新北市林口區文化三路一段 1 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000003', @RouteMorningBId, 3, N'校門口', N'新北市林口區幸福路 1 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000004', @RouteMorningCId, 1, N'行天宮站', N'台北市中山區民權東路二段 139 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000005', @RouteMorningCId, 2, N'圓山站', N'台北市大同區酒泉街 9 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000006', @RouteMorningCId, 3, N'校門口', N'新北市林口區幸福路 1 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000007', @RouteDismissalBId, 1, N'校門口', N'新北市林口區幸福路 1 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000008', @RouteDismissalBId, 2, N'A9 三井站', N'新北市林口區文化三路一段 1 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000009', @RouteDismissalBId, 3, N'林口站', N'新北市林口區文化三路 8 號', NULL, NULL, @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000010', @RouteDoorToDoorBId, 1, N'王雨婷住家', N'新北市林口區復興路 28 號', N'王媽媽', N'0910-200-004', @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000011', @RouteDoorToDoorBId, 2, N'林小宇住家', N'新北市林口區仁愛路 36 號', N'林媽媽', N'0910-200-005', @UtcNow, NULL),
    ('63100000-0000-0000-0000-000000000012', @RouteDoorToDoorBId, 3, N'陳小明臨時下車點', N'新北市林口區市民大道 100 號', N'陳媽媽', N'0910-200-001', @UtcNow, NULL);

    INSERT INTO Students (Id, UserId, StudentNumber, FullName, Stage, GradeLabel, DefaultRouteId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    (@Student1Id, @Student1UserId, N'S20001', N'陳小明', 2, N'四年級', @RouteMorningBId, @UtcNow, NULL),
    (@Student2Id, @Student2UserId, N'S20002', N'陳小美', 3, N'七年級', @RouteMorningBId, @UtcNow, NULL),
    (@Student3Id, @Student3UserId, N'S20003', N'劉子豪', 4, N'高一', @RouteMorningCId, @UtcNow, NULL),
    (@Student4Id, @Student4UserId, N'S20004', N'王雨婷', 2, N'五年級', @RouteMorningBId, @UtcNow, NULL),
    (@Student5Id, @Student5UserId, N'S20005', N'林小宇', 1, N'幼大班', @RouteMorningCId, @UtcNow, NULL);

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
    ('68100000-0000-0000-0000-000000000001', '68000000-0000-0000-0000-000000000001', @Student1Id, 2, N'0910-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000002', '68000000-0000-0000-0000-000000000001', @Student2Id, 3, N'0910-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000003', '68000000-0000-0000-0000-000000000001', @Student4Id, 2, N'0910-200-004', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000004', '68000000-0000-0000-0000-000000000002', @Student3Id, 2, N'0910-200-003', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000005', '68000000-0000-0000-0000-000000000002', @Student5Id, 2, N'0910-200-005', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000006', '68000000-0000-0000-0000-000000000003', @Student1Id, 2, N'0910-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000007', '68000000-0000-0000-0000-000000000003', @Student2Id, 4, N'0910-200-001', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000008', '68000000-0000-0000-0000-000000000003', @Student3Id, 2, N'0910-200-003', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000009', '68000000-0000-0000-0000-000000000003', @Student4Id, 2, N'0910-200-004', @UtcNow, NULL),
    ('68100000-0000-0000-0000-000000000010', '68000000-0000-0000-0000-000000000004', @Student5Id, 2, N'0910-200-005', @UtcNow, NULL);

    INSERT INTO DispatchOverrides (Id, StudentId, RouteId, Date, Direction, Status, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('69000000-0000-0000-0000-000000000001', @Student1Id, @RouteDoorToDoorBId, '2026-03-26', 2, 1, @UtcNow, NULL);

    INSERT INTO NotificationTemplates (Id, TemplateName, Subject, Body, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000001', N'每週搭車提醒', N'校車搭乘登記提醒', N'請於週五晚上六點前完成下週校車登記。', @UtcNow, NULL);

    INSERT INTO NotificationJobs (Id, JobType, Channel, Subject, Body, CreatedByUserId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000002', N'Broadcast', 1, N'週五登記提醒', N'請在本週五前完成下週校車搭乘登記。', @AdminUserId, @UtcNow, NULL);

    INSERT INTO NotificationDeliveries (Id, NotificationJobId, RecipientUserId, RecipientEmail, Status, SentAtUtc, ErrorMessage, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', @Parent1UserId, N'chen.jiazhang1@demo.local', N'Sent', @UtcNow, NULL, @UtcNow, NULL),
    ('70000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000002', @Parent3UserId, N'liu.jiazhang@demo.local', N'Sent', @UtcNow, NULL, @UtcNow, NULL),
    ('70000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', @Parent5UserId, N'lin.jiazhang@demo.local', N'Sent', @UtcNow, NULL, @UtcNow, NULL);

    INSERT INTO BroadcastAnnouncements (Id, Audience, Subject, Body, CreatedByUserId, CreatedAtUtc, UpdatedAtUtc)
    VALUES
    ('70000000-0000-0000-0000-000000000006', 2, N'天候通知', N'因大雨影響，本週五放學班車預計延後十五分鐘發車。', @AdminUserId, @UtcNow, NULL);

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
