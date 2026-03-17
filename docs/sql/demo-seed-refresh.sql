SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    /*
        Demo refresh script for Azure SQL
        - Login password for all seeded accounts: P@ssw0rd!
        - Recommended demo week: 2026-03-16 to 2026-03-20
        - Fixes the teacher attendance demo by reseeding registrations and a live roster session
    */

    DECLARE @PasswordHash nvarchar(max) = N'AQAAAAIAAYagAAAAELNOiqcqUn99yPLqVuLZyQO1tRQHad41tN7LKBPBm2aNfuGigtzGEPnyzurIrqztcg==';
    DECLARE @BaseCreatedAt datetimeoffset = '2026-03-15T16:00:00+00:00';

    DECLARE @AdministratorRoleId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetRoles] WHERE [Name] = N'Administrator'), '90000000-0000-0000-0000-000000000001');
    DECLARE @TeacherRoleId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetRoles] WHERE [Name] = N'Teacher'), '90000000-0000-0000-0000-000000000002');
    DECLARE @ParentRoleId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetRoles] WHERE [Name] = N'Parent'), '90000000-0000-0000-0000-000000000003');
    DECLARE @StudentRoleId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetRoles] WHERE [Name] = N'Student'), '90000000-0000-0000-0000-000000000004');

    DECLARE @AdminUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'E0001'), '11111111-1111-1111-1111-111111111111');
    DECLARE @TeacherUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'T0001'), '22222222-2222-2222-2222-222222222222');
    DECLARE @TeacherTwoUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'T0002'), '55555555-5555-5555-5555-555555555555');
    DECLARE @ParentUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'0900-000-003'), '33333333-3333-3333-3333-333333333333');
    DECLARE @ParentTwoUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'0900-000-005'), '66666666-6666-6666-6666-666666666662');
    DECLARE @ParentThreeUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'0900-000-006'), '66666666-6666-6666-6666-666666666663');
    DECLARE @ParentFourUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'0900-000-007'), '66666666-6666-6666-6666-666666666664');
    DECLARE @ParentFiveUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'0900-000-008'), '66666666-6666-6666-6666-666666666665');
    DECLARE @StudentUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'S10001'), '44444444-4444-4444-4444-444444444444');
    DECLARE @StudentTwoUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'S10002'), '77777777-7777-7777-7777-777777777772');
    DECLARE @StudentThreeUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'S10003'), '77777777-7777-7777-7777-777777777773');
    DECLARE @StudentFourUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'S10004'), '77777777-7777-7777-7777-777777777774');
    DECLARE @StudentFiveUserId uniqueidentifier = COALESCE((SELECT TOP (1) [Id] FROM [AspNetUsers] WHERE [UserName] = N'S10005'), '77777777-7777-7777-7777-777777777775');

    DECLARE @AdminStaffProfileId uniqueidentifier = 'bbbbbbbb-2222-2222-2222-222222222222';
    DECLARE @TeacherStaffProfileId uniqueidentifier = 'cccccccc-1111-1111-1111-111111111111';
    DECLARE @TeacherTwoStaffProfileId uniqueidentifier = '99999999-9999-9999-9999-999999999992';

    DECLARE @GuardianId uniqueidentifier = 'bbbbbbbb-1111-1111-1111-111111111111';
    DECLARE @GuardianTwoId uniqueidentifier = 'bbbbbbbb-1111-1111-1111-111111111112';
    DECLARE @GuardianThreeId uniqueidentifier = 'bbbbbbbb-1111-1111-1111-111111111113';
    DECLARE @GuardianFourId uniqueidentifier = 'bbbbbbbb-1111-1111-1111-111111111114';
    DECLARE @GuardianFiveId uniqueidentifier = 'bbbbbbbb-1111-1111-1111-111111111115';

    DECLARE @StudentProfileId uniqueidentifier = 'aaaaaaaa-1111-1111-1111-111111111111';
    DECLARE @StudentTwoId uniqueidentifier = 'aaaaaaaa-1111-1111-1111-111111111112';
    DECLARE @StudentThreeId uniqueidentifier = 'aaaaaaaa-1111-1111-1111-111111111113';
    DECLARE @StudentFourId uniqueidentifier = 'aaaaaaaa-1111-1111-1111-111111111114';
    DECLARE @StudentFiveId uniqueidentifier = 'aaaaaaaa-1111-1111-1111-111111111115';

    DECLARE @MorningRouteId uniqueidentifier = 'dddddddd-1111-1111-1111-111111111111';
    DECLARE @DismissalRouteId uniqueidentifier = 'eeeeeeee-1111-1111-1111-111111111111';
    DECLARE @KindergartenRouteId uniqueidentifier = 'ffffffff-1111-1111-1111-111111111111';
    DECLARE @BackupRouteId uniqueidentifier = '88888888-8888-8888-8888-888888888888';

    DECLARE @ReminderTemplateId uniqueidentifier = '78787878-7878-7878-7878-787878787871';
    DECLARE @ReminderJobId uniqueidentifier = '12121212-1212-1212-1212-121212121212';
    DECLARE @BroadcastAnnouncementId uniqueidentifier = '34343434-3434-3434-3434-343434343434';
    DECLARE @BroadcastJobId uniqueidentifier = '56565656-5656-5656-5656-565656565656';

    DECLARE @SessionMorningHistoryId uniqueidentifier = '67676767-6767-6767-6767-676767676761';
    DECLARE @SessionDismissalHistoryId uniqueidentifier = '67676767-6767-6767-6767-676767676762';
    DECLARE @SessionKindergartenHistoryId uniqueidentifier = '67676767-6767-6767-6767-676767676763';
    DECLARE @SessionMorningLiveId uniqueidentifier = '67676767-6767-6767-6767-676767676764';

    DECLARE @SeedRoles TABLE
    (
        [Id] uniqueidentifier PRIMARY KEY,
        [Name] nvarchar(256) NOT NULL,
        [NormalizedName] nvarchar(256) NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NULL
    );

    INSERT INTO @SeedRoles ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    VALUES
        (@AdministratorRoleId, N'Administrator', N'ADMINISTRATOR', N'8a97c35c-7aaf-4b83-b6b3-0dd3b8d4df01'),
        (@TeacherRoleId, N'Teacher', N'TEACHER', N'8a97c35c-7aaf-4b83-b6b3-0dd3b8d4df02'),
        (@ParentRoleId, N'Parent', N'PARENT', N'8a97c35c-7aaf-4b83-b6b3-0dd3b8d4df03'),
        (@StudentRoleId, N'Student', N'STUDENT', N'8a97c35c-7aaf-4b83-b6b3-0dd3b8d4df04');

    MERGE [AspNetRoles] AS [target]
    USING @SeedRoles AS [source]
        ON [target].[Id] = [source].[Id]
    WHEN MATCHED THEN
        UPDATE SET
            [Name] = [source].[Name],
            [NormalizedName] = [source].[NormalizedName],
            [ConcurrencyStamp] = [source].[ConcurrencyStamp]
    WHEN NOT MATCHED THEN
        INSERT ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
        VALUES ([source].[Id], [source].[Name], [source].[NormalizedName], [source].[ConcurrencyStamp]);

    DECLARE @SeedUsers TABLE
    (
        [Id] uniqueidentifier PRIMARY KEY,
        [UserName] nvarchar(256) NOT NULL,
        [NormalizedUserName] nvarchar(256) NOT NULL,
        [Email] nvarchar(256) NOT NULL,
        [NormalizedEmail] nvarchar(256) NOT NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [SecurityStamp] nvarchar(max) NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL
    );

    INSERT INTO @SeedUsers
    VALUES
        (@AdminUserId, N'E0001', N'E0001', N'admin@demo.local', N'ADMIN@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e001', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e101', N'0900-000-001', 1, 0, NULL, 0, 0),
        (@TeacherUserId, N'T0001', N'T0001', N'teacher@demo.local', N'TEACHER@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e002', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e102', N'0900-000-002', 1, 0, NULL, 0, 0),
        (@TeacherTwoUserId, N'T0002', N'T0002', N'teacher2@demo.local', N'TEACHER2@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e003', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e103', N'0900-000-009', 1, 0, NULL, 0, 0),
        (@ParentUserId, N'0900-000-003', N'0900-000-003', N'parent@demo.local', N'PARENT@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e004', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e104', N'0900-000-003', 1, 0, NULL, 0, 0),
        (@ParentTwoUserId, N'0900-000-005', N'0900-000-005', N'parent2@demo.local', N'PARENT2@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e005', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e105', N'0900-000-005', 1, 0, NULL, 0, 0),
        (@ParentThreeUserId, N'0900-000-006', N'0900-000-006', N'parent3@demo.local', N'PARENT3@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e006', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e106', N'0900-000-006', 1, 0, NULL, 0, 0),
        (@ParentFourUserId, N'0900-000-007', N'0900-000-007', N'parent4@demo.local', N'PARENT4@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e007', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e107', N'0900-000-007', 1, 0, NULL, 0, 0),
        (@ParentFiveUserId, N'0900-000-008', N'0900-000-008', N'parent5@demo.local', N'PARENT5@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e008', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e108', N'0900-000-008', 1, 0, NULL, 0, 0),
        (@StudentUserId, N'S10001', N'S10001', N'student@demo.local', N'STUDENT@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e009', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e109', N'0900-100-001', 1, 0, NULL, 0, 0),
        (@StudentTwoUserId, N'S10002', N'S10002', N'student2@demo.local', N'STUDENT2@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e010', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e110', N'0900-100-002', 1, 0, NULL, 0, 0),
        (@StudentThreeUserId, N'S10003', N'S10003', N'student3@demo.local', N'STUDENT3@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e011', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e111', N'0900-100-003', 1, 0, NULL, 0, 0),
        (@StudentFourUserId, N'S10004', N'S10004', N'student4@demo.local', N'STUDENT4@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e012', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e112', N'0900-100-004', 1, 0, NULL, 0, 0),
        (@StudentFiveUserId, N'S10005', N'S10005', N'student5@demo.local', N'STUDENT5@DEMO.LOCAL', 1, @PasswordHash, N'4f2b1a69-cff7-4f78-9560-e8d1ca50e013', N'4f2b1a69-cff7-4f78-9560-e8d1ca50e113', N'0900-100-005', 1, 0, NULL, 0, 0);

    MERGE [AspNetUsers] AS [target]
    USING @SeedUsers AS [source]
        ON [target].[Id] = [source].[Id]
    WHEN MATCHED THEN
        UPDATE SET
            [UserName] = [source].[UserName],
            [NormalizedUserName] = [source].[NormalizedUserName],
            [Email] = [source].[Email],
            [NormalizedEmail] = [source].[NormalizedEmail],
            [EmailConfirmed] = [source].[EmailConfirmed],
            [PasswordHash] = [source].[PasswordHash],
            [SecurityStamp] = [source].[SecurityStamp],
            [ConcurrencyStamp] = [source].[ConcurrencyStamp],
            [PhoneNumber] = [source].[PhoneNumber],
            [PhoneNumberConfirmed] = [source].[PhoneNumberConfirmed],
            [TwoFactorEnabled] = [source].[TwoFactorEnabled],
            [LockoutEnd] = [source].[LockoutEnd],
            [LockoutEnabled] = [source].[LockoutEnabled],
            [AccessFailedCount] = [source].[AccessFailedCount]
    WHEN NOT MATCHED THEN
        INSERT
        (
            [Id],
            [UserName],
            [NormalizedUserName],
            [Email],
            [NormalizedEmail],
            [EmailConfirmed],
            [PasswordHash],
            [SecurityStamp],
            [ConcurrencyStamp],
            [PhoneNumber],
            [PhoneNumberConfirmed],
            [TwoFactorEnabled],
            [LockoutEnd],
            [LockoutEnabled],
            [AccessFailedCount]
        )
        VALUES
        (
            [source].[Id],
            [source].[UserName],
            [source].[NormalizedUserName],
            [source].[Email],
            [source].[NormalizedEmail],
            [source].[EmailConfirmed],
            [source].[PasswordHash],
            [source].[SecurityStamp],
            [source].[ConcurrencyStamp],
            [source].[PhoneNumber],
            [source].[PhoneNumberConfirmed],
            [source].[TwoFactorEnabled],
            [source].[LockoutEnd],
            [source].[LockoutEnabled],
            [source].[AccessFailedCount]
        );

    DECLARE @ExistingDemoRouteIds TABLE ([Id] uniqueidentifier PRIMARY KEY);
    INSERT INTO @ExistingDemoRouteIds ([Id])
    SELECT [Id]
    FROM [Routes]
    WHERE [RouteName] IN
    (
        N'Linkou Morning Route A',
        N'Linkou Dismissal Route A',
        N'Kindergarten Door-to-Door',
        N'Linkou Backup Morning Route',
        N'Linkou Morning Route B',
        N'Linkou Dismissal Route B'
    )
    OR [Id] IN (@MorningRouteId, @DismissalRouteId, @KindergartenRouteId, @BackupRouteId);

    DECLARE @ExistingDemoStaffIds TABLE ([Id] uniqueidentifier PRIMARY KEY);
    INSERT INTO @ExistingDemoStaffIds ([Id])
    SELECT [Id]
    FROM [StaffProfiles]
    WHERE [EmployeeNumber] IN (N'E0001', N'T0001', N'T0002')
       OR [Id] IN (@AdminStaffProfileId, @TeacherStaffProfileId, @TeacherTwoStaffProfileId);

    DECLARE @ExistingDemoStudentIds TABLE ([Id] uniqueidentifier PRIMARY KEY);
    INSERT INTO @ExistingDemoStudentIds ([Id])
    SELECT [Id]
    FROM [Students]
    WHERE [StudentNumber] IN (N'S10001', N'S10002', N'S10003', N'S10004', N'S10005')
       OR [Id] IN (@StudentProfileId, @StudentTwoId, @StudentThreeId, @StudentFourId, @StudentFiveId);

    DECLARE @ExistingDemoGuardianIds TABLE ([Id] uniqueidentifier PRIMARY KEY);
    INSERT INTO @ExistingDemoGuardianIds ([Id])
    SELECT [Id]
    FROM [Guardians]
    WHERE [PhoneNumber] IN (N'0900-000-003', N'0900-000-005', N'0900-000-006', N'0900-000-007', N'0900-000-008')
       OR [Id] IN (@GuardianId, @GuardianTwoId, @GuardianThreeId, @GuardianFourId, @GuardianFiveId);

    DECLARE @ExistingDemoJobIds TABLE ([Id] uniqueidentifier PRIMARY KEY);
    INSERT INTO @ExistingDemoJobIds ([Id])
    SELECT [Id]
    FROM [NotificationJobs]
    WHERE [Id] IN (@ReminderJobId, @BroadcastJobId)
       OR [Subject] IN (N'Shuttle Bus Registration Reminder', N'Friday Dismissal Reminder');

    DECLARE @DemoUserIds TABLE ([Id] uniqueidentifier PRIMARY KEY);
    INSERT INTO @DemoUserIds ([Id])
    VALUES
        (@AdminUserId),
        (@TeacherUserId),
        (@TeacherTwoUserId),
        (@ParentUserId),
        (@ParentTwoUserId),
        (@ParentThreeUserId),
        (@ParentFourUserId),
        (@ParentFiveUserId),
        (@StudentUserId),
        (@StudentTwoUserId),
        (@StudentThreeUserId),
        (@StudentFourUserId),
        (@StudentFiveUserId);

    DELETE [record]
    FROM [AttendanceRecords] AS [record]
    INNER JOIN [AttendanceSessions] AS [session]
        ON [session].[Id] = [record].[AttendanceSessionId]
    WHERE [session].[RouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds)
       OR [session].[CreatedByStaffProfileId] IN (SELECT [Id] FROM @ExistingDemoStaffIds)
       OR [record].[StudentId] IN (SELECT [Id] FROM @ExistingDemoStudentIds);

    DELETE FROM [AttendanceSessions]
    WHERE [RouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds)
       OR [CreatedByStaffProfileId] IN (SELECT [Id] FROM @ExistingDemoStaffIds);

    DELETE FROM [DispatchOverrides]
    WHERE [StudentId] IN (SELECT [Id] FROM @ExistingDemoStudentIds)
       OR [RouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds);

    DELETE FROM [RideRegistrations]
    WHERE [StudentId] IN (SELECT [Id] FROM @ExistingDemoStudentIds)
       OR [RouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds);

    DELETE FROM [RouteAssignments]
    WHERE [RouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds)
       OR [StaffProfileId] IN (SELECT [Id] FROM @ExistingDemoStaffIds);

    DELETE FROM [RouteStops]
    WHERE [RouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds);

    DELETE FROM [StudentGuardianLinks]
    WHERE [StudentId] IN (SELECT [Id] FROM @ExistingDemoStudentIds)
       OR [GuardianId] IN (SELECT [Id] FROM @ExistingDemoGuardianIds);

    DELETE FROM [NotificationDeliveries]
    WHERE [NotificationJobId] IN (SELECT [Id] FROM @ExistingDemoJobIds)
       OR [RecipientUserId] IN (SELECT [Id] FROM @DemoUserIds);

    DELETE FROM [NotificationJobs]
    WHERE [Id] IN (SELECT [Id] FROM @ExistingDemoJobIds);

    DELETE FROM [BroadcastAnnouncements]
    WHERE [Id] = @BroadcastAnnouncementId
       OR ([Subject] = N'Friday Dismissal Reminder' AND [CreatedByUserId] = @AdminUserId);

    DELETE FROM [NotificationTemplates]
    WHERE [Id] = @ReminderTemplateId
       OR ([TemplateName] = N'Reminder' AND [Subject] = N'Shuttle Bus Registration Reminder');

    DELETE FROM [RefreshTokens]
    WHERE [UserId] IN (SELECT [Id] FROM @DemoUserIds);

    DELETE FROM [AspNetUserRoles]
    WHERE [UserId] IN (SELECT [Id] FROM @DemoUserIds);

    UPDATE [Students]
    SET
        [DefaultRouteId] = NULL,
        [UpdatedAtUtc] = SYSDATETIMEOFFSET()
    WHERE [DefaultRouteId] IN (SELECT [Id] FROM @ExistingDemoRouteIds);

    DELETE FROM [Students]
    WHERE [StudentNumber] IN (N'S10001', N'S10002', N'S10003', N'S10004', N'S10005')
       OR [Id] IN (@StudentProfileId, @StudentTwoId, @StudentThreeId, @StudentFourId, @StudentFiveId);

    DELETE FROM [Guardians]
    WHERE [PhoneNumber] IN (N'0900-000-003', N'0900-000-005', N'0900-000-006', N'0900-000-007', N'0900-000-008')
       OR [Id] IN (@GuardianId, @GuardianTwoId, @GuardianThreeId, @GuardianFourId, @GuardianFiveId);

    DELETE FROM [StaffProfiles]
    WHERE [EmployeeNumber] IN (N'E0001', N'T0001', N'T0002')
       OR [Id] IN (@AdminStaffProfileId, @TeacherStaffProfileId, @TeacherTwoStaffProfileId);

    DELETE FROM [Routes]
    WHERE [RouteName] IN
    (
        N'Linkou Morning Route A',
        N'Linkou Dismissal Route A',
        N'Kindergarten Door-to-Door',
        N'Linkou Backup Morning Route',
        N'Linkou Morning Route B',
        N'Linkou Dismissal Route B'
    )
       OR [Id] IN (@MorningRouteId, @DismissalRouteId, @KindergartenRouteId, @BackupRouteId);

    INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
    VALUES
        (@AdminUserId, @AdministratorRoleId),
        (@TeacherUserId, @TeacherRoleId),
        (@TeacherTwoUserId, @TeacherRoleId),
        (@ParentUserId, @ParentRoleId),
        (@ParentTwoUserId, @ParentRoleId),
        (@ParentThreeUserId, @ParentRoleId),
        (@ParentFourUserId, @ParentRoleId),
        (@ParentFiveUserId, @ParentRoleId),
        (@StudentUserId, @StudentRoleId),
        (@StudentTwoUserId, @StudentRoleId),
        (@StudentThreeUserId, @StudentRoleId),
        (@StudentFourUserId, @StudentRoleId),
        (@StudentFiveUserId, @StudentRoleId);

    INSERT INTO [Routes] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [RouteName], [RouteType], [Direction], [CampusName], [IsActive])
    VALUES
        (@MorningRouteId, @BaseCreatedAt, NULL, N'Linkou Morning Route A', 1, 1, N'Kang Chiao Linkou', 1),
        (@DismissalRouteId, @BaseCreatedAt, NULL, N'Linkou Dismissal Route A', 1, 2, N'Kang Chiao Linkou', 1),
        (@KindergartenRouteId, @BaseCreatedAt, NULL, N'Kindergarten Door-to-Door', 2, 2, N'Kang Chiao Linkou', 1),
        (@BackupRouteId, @BaseCreatedAt, NULL, N'Linkou Backup Morning Route', 1, 1, N'Kang Chiao Linkou', 0);

    INSERT INTO [RouteStops] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [RouteId], [Sequence], [StopName], [Address], [HandoffContactName], [HandoffContactPhone])
    VALUES
        (NEWID(), @BaseCreatedAt, NULL, @MorningRouteId, 1, N'Linkou Station', N'No. 8, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @MorningRouteId, 2, N'A9 Mitsui Stop', N'No. 1, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @MorningRouteId, 3, N'Campus Gate', N'No. 1, Xingfu Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @DismissalRouteId, 1, N'Campus Gate', N'No. 1, Xingfu Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @DismissalRouteId, 2, N'A9 Mitsui Stop', N'No. 1, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @DismissalRouteId, 3, N'Linkou Station', N'No. 8, Wenhua 3rd Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @KindergartenRouteId, 1, N'Child Home', N'No. 3, Demo Rd.', N'黃家長', N'0900-000-008'),
        (NEWID(), @BaseCreatedAt, NULL, @BackupRouteId, 1, N'Cultural 2nd Rd.', N'No. 66, Cultural 2nd Rd., Linkou Dist.', NULL, NULL),
        (NEWID(), @BaseCreatedAt, NULL, @BackupRouteId, 2, N'Campus Gate', N'No. 1, Xingfu Rd., Linkou Dist.', NULL, NULL);

    INSERT INTO [StaffProfiles] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [UserId], [EmployeeNumber], [FullName], [PhoneNumber], [CanManageAllRoutes])
    VALUES
        (@AdminStaffProfileId, @BaseCreatedAt, NULL, @AdminUserId, N'E0001', N'系統管理員', N'0900-000-001', 1),
        (@TeacherStaffProfileId, @BaseCreatedAt, NULL, @TeacherUserId, N'T0001', N'王老師', N'0900-000-002', 0),
        (@TeacherTwoStaffProfileId, @BaseCreatedAt, NULL, @TeacherTwoUserId, N'T0002', N'李老師', N'0900-000-009', 0);

    INSERT INTO [Guardians] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [UserId], [FullName], [PhoneNumber], [Email])
    VALUES
        (@GuardianId, @BaseCreatedAt, NULL, @ParentUserId, N'陳家長', N'0900-000-003', N'parent@demo.local'),
        (@GuardianTwoId, @BaseCreatedAt, NULL, @ParentTwoUserId, N'林家長', N'0900-000-005', N'parent2@demo.local'),
        (@GuardianThreeId, @BaseCreatedAt, NULL, @ParentThreeUserId, N'王家長', N'0900-000-006', N'parent3@demo.local'),
        (@GuardianFourId, @BaseCreatedAt, NULL, @ParentFourUserId, N'劉家長', N'0900-000-007', N'parent4@demo.local'),
        (@GuardianFiveId, @BaseCreatedAt, NULL, @ParentFiveUserId, N'黃家長', N'0900-000-008', N'parent5@demo.local');

    INSERT INTO [Students] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [UserId], [StudentNumber], [FullName], [Stage], [GradeLabel], [DefaultRouteId])
    VALUES
        (@StudentProfileId, @BaseCreatedAt, NULL, @StudentUserId, N'S10001', N'陳小明', 3, N'八年級', @MorningRouteId),
        (@StudentTwoId, @BaseCreatedAt, NULL, @StudentTwoUserId, N'S10002', N'林小雨', 2, N'五年級', @MorningRouteId),
        (@StudentThreeId, @BaseCreatedAt, NULL, @StudentThreeUserId, N'S10003', N'王樂樂', 2, N'四年級', @MorningRouteId),
        (@StudentFourId, @BaseCreatedAt, NULL, @StudentFourUserId, N'S10004', N'劉星宇', 4, N'高一', @DismissalRouteId),
        (@StudentFiveId, @BaseCreatedAt, NULL, @StudentFiveUserId, N'S10005', N'黃豆豆', 1, N'K2', @KindergartenRouteId);

    INSERT INTO [StudentGuardianLinks] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [StudentId], [GuardianId], [IsPrimaryContact])
    VALUES
        (NEWID(), @BaseCreatedAt, NULL, @StudentProfileId, @GuardianId, 1),
        (NEWID(), @BaseCreatedAt, NULL, @StudentTwoId, @GuardianTwoId, 1),
        (NEWID(), @BaseCreatedAt, NULL, @StudentThreeId, @GuardianThreeId, 1),
        (NEWID(), @BaseCreatedAt, NULL, @StudentFourId, @GuardianFourId, 1),
        (NEWID(), @BaseCreatedAt, NULL, @StudentFiveId, @GuardianFiveId, 1);

    INSERT INTO [RouteAssignments] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [RouteId], [StaffProfileId])
    VALUES
        (NEWID(), @BaseCreatedAt, NULL, @MorningRouteId, @TeacherStaffProfileId),
        (NEWID(), @BaseCreatedAt, NULL, @DismissalRouteId, @TeacherStaffProfileId),
        (NEWID(), @BaseCreatedAt, NULL, @KindergartenRouteId, @TeacherTwoStaffProfileId),
        (NEWID(), @BaseCreatedAt, NULL, @BackupRouteId, @TeacherTwoStaffProfileId);

    DECLARE @RideRegistrationSeed TABLE
    (
        [StudentId] uniqueidentifier NOT NULL,
        [Date] date NOT NULL,
        [Direction] int NOT NULL,
        [IsRegistered] bit NOT NULL,
        [IsPresent] bit NOT NULL,
        [RouteId] uniqueidentifier NULL
    );

    INSERT INTO @RideRegistrationSeed ([StudentId], [Date], [Direction], [IsRegistered], [IsPresent], [RouteId])
    VALUES
        (@StudentProfileId, '2026-03-16', 1, 1, 1, @MorningRouteId),
        (@StudentProfileId, '2026-03-16', 2, 1, 1, @DismissalRouteId),
        (@StudentProfileId, '2026-03-17', 1, 1, 1, @MorningRouteId),
        (@StudentProfileId, '2026-03-17', 2, 1, 0, @DismissalRouteId),
        (@StudentProfileId, '2026-03-18', 1, 1, 1, @MorningRouteId),
        (@StudentProfileId, '2026-03-18', 2, 1, 1, @DismissalRouteId),
        (@StudentProfileId, '2026-03-19', 1, 1, 0, @MorningRouteId),
        (@StudentProfileId, '2026-03-19', 2, 1, 1, @DismissalRouteId),
        (@StudentProfileId, '2026-03-20', 1, 1, 1, @MorningRouteId),
        (@StudentProfileId, '2026-03-20', 2, 1, 1, @DismissalRouteId),

        (@StudentTwoId, '2026-03-16', 1, 1, 1, @MorningRouteId),
        (@StudentTwoId, '2026-03-16', 2, 1, 1, @DismissalRouteId),
        (@StudentTwoId, '2026-03-17', 1, 1, 1, @MorningRouteId),
        (@StudentTwoId, '2026-03-17', 2, 1, 0, @DismissalRouteId),
        (@StudentTwoId, '2026-03-18', 1, 1, 0, @MorningRouteId),
        (@StudentTwoId, '2026-03-18', 2, 1, 1, @DismissalRouteId),
        (@StudentTwoId, '2026-03-19', 1, 0, 0, NULL),
        (@StudentTwoId, '2026-03-19', 2, 0, 0, NULL),
        (@StudentTwoId, '2026-03-20', 1, 0, 0, NULL),
        (@StudentTwoId, '2026-03-20', 2, 0, 0, NULL),

        (@StudentThreeId, '2026-03-16', 1, 1, 1, @MorningRouteId),
        (@StudentThreeId, '2026-03-16', 2, 1, 1, @DismissalRouteId),
        (@StudentThreeId, '2026-03-17', 1, 1, 0, @MorningRouteId),
        (@StudentThreeId, '2026-03-17', 2, 1, 1, @DismissalRouteId),
        (@StudentThreeId, '2026-03-18', 1, 1, 1, @MorningRouteId),
        (@StudentThreeId, '2026-03-18', 2, 1, 0, @DismissalRouteId),
        (@StudentThreeId, '2026-03-19', 1, 1, 1, @MorningRouteId),
        (@StudentThreeId, '2026-03-19', 2, 1, 1, @DismissalRouteId),
        (@StudentThreeId, '2026-03-20', 1, 1, 1, @MorningRouteId),
        (@StudentThreeId, '2026-03-20', 2, 0, 0, NULL),

        (@StudentFourId, '2026-03-16', 1, 0, 0, NULL),
        (@StudentFourId, '2026-03-16', 2, 1, 1, @DismissalRouteId),
        (@StudentFourId, '2026-03-17', 1, 0, 0, NULL),
        (@StudentFourId, '2026-03-17', 2, 0, 0, NULL),
        (@StudentFourId, '2026-03-18', 1, 0, 0, NULL),
        (@StudentFourId, '2026-03-18', 2, 1, 0, @DismissalRouteId),
        (@StudentFourId, '2026-03-19', 1, 0, 0, NULL),
        (@StudentFourId, '2026-03-19', 2, 0, 0, NULL),
        (@StudentFourId, '2026-03-20', 1, 0, 0, NULL),
        (@StudentFourId, '2026-03-20', 2, 1, 1, @DismissalRouteId),

        (@StudentFiveId, '2026-03-16', 1, 0, 0, NULL),
        (@StudentFiveId, '2026-03-16', 2, 1, 1, @KindergartenRouteId),
        (@StudentFiveId, '2026-03-17', 1, 0, 0, NULL),
        (@StudentFiveId, '2026-03-17', 2, 1, 1, @KindergartenRouteId),
        (@StudentFiveId, '2026-03-18', 1, 0, 0, NULL),
        (@StudentFiveId, '2026-03-18', 2, 1, 0, @KindergartenRouteId),
        (@StudentFiveId, '2026-03-19', 1, 0, 0, NULL),
        (@StudentFiveId, '2026-03-19', 2, 1, 1, @KindergartenRouteId),
        (@StudentFiveId, '2026-03-20', 1, 0, 0, NULL),
        (@StudentFiveId, '2026-03-20', 2, 1, 1, @KindergartenRouteId);

    INSERT INTO [RideRegistrations] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [StudentId], [Date], [Direction], [IsRegistered], [IsPresent], [RouteId])
    SELECT NEWID(), @BaseCreatedAt, NULL, [StudentId], [Date], [Direction], [IsRegistered], [IsPresent], [RouteId]
    FROM @RideRegistrationSeed;

    INSERT INTO [NotificationTemplates] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [TemplateName], [Subject], [Body])
    VALUES
        (@ReminderTemplateId, @BaseCreatedAt, NULL, N'Reminder', N'Shuttle Bus Registration Reminder', N'Please complete next week''s shuttle bus registration.');

    INSERT INTO [BroadcastAnnouncements] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [Audience], [Subject], [Body], [CreatedByUserId])
    VALUES
        (@BroadcastAnnouncementId, '2026-03-18T03:30:00+00:00', NULL, 2, N'Friday Dismissal Reminder', N'Friday traffic may be heavier than usual. Please arrive at your stop 5 minutes early.', @AdminUserId);

    INSERT INTO [NotificationJobs] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [JobType], [Channel], [Subject], [Body], [CreatedByUserId])
    VALUES
        (@ReminderJobId, '2026-03-17T08:00:00+00:00', NULL, N'Reminder', 1, N'Shuttle Bus Registration Reminder', N'Please complete next week''s shuttle bus registration.', @AdminUserId),
        (@BroadcastJobId, '2026-03-18T03:30:00+00:00', NULL, N'Broadcast', 1, N'Friday Dismissal Reminder', N'Friday traffic may be heavier than usual. Please arrive at your stop 5 minutes early.', @AdminUserId);

    INSERT INTO [NotificationDeliveries] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [NotificationJobId], [RecipientUserId], [RecipientEmail], [Status], [SentAtUtc], [ErrorMessage])
    VALUES
        (NEWID(), '2026-03-17T08:00:00+00:00', NULL, @ReminderJobId, @ParentUserId, N'parent@demo.local', N'Sent', '2026-03-17T08:00:00+00:00', NULL),
        (NEWID(), '2026-03-17T08:00:00+00:00', NULL, @ReminderJobId, @ParentTwoUserId, N'parent2@demo.local', N'Sent', '2026-03-17T08:00:00+00:00', NULL),
        (NEWID(), '2026-03-17T08:00:00+00:00', NULL, @ReminderJobId, @ParentFiveUserId, N'parent5@demo.local', N'Sent', '2026-03-17T08:00:00+00:00', NULL),
        (NEWID(), '2026-03-18T03:31:00+00:00', NULL, @BroadcastJobId, @ParentUserId, N'parent@demo.local', N'Sent', '2026-03-18T03:31:00+00:00', NULL),
        (NEWID(), '2026-03-18T03:31:00+00:00', NULL, @BroadcastJobId, @ParentTwoUserId, N'parent2@demo.local', N'Sent', '2026-03-18T03:31:00+00:00', NULL),
        (NEWID(), '2026-03-18T03:31:00+00:00', NULL, @BroadcastJobId, @ParentThreeUserId, N'parent3@demo.local', N'Sent', '2026-03-18T03:31:00+00:00', NULL),
        (NEWID(), '2026-03-18T03:31:00+00:00', NULL, @BroadcastJobId, @ParentFourUserId, N'parent4@demo.local', N'Sent', '2026-03-18T03:31:00+00:00', NULL),
        (NEWID(), '2026-03-18T03:31:00+00:00', NULL, @BroadcastJobId, @ParentFiveUserId, N'parent5@demo.local', N'Sent', '2026-03-18T03:31:00+00:00', NULL);

    INSERT INTO [AttendanceSessions] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [RouteId], [Date], [Direction], [IsCompleted], [CreatedByStaffProfileId])
    VALUES
        (@SessionMorningHistoryId, '2026-03-16T22:15:00+00:00', NULL, @MorningRouteId, '2026-03-16', 1, 1, @TeacherStaffProfileId),
        (@SessionDismissalHistoryId, '2026-03-16T09:20:00+00:00', NULL, @DismissalRouteId, '2026-03-16', 2, 1, @TeacherStaffProfileId),
        (@SessionKindergartenHistoryId, '2026-03-17T09:40:00+00:00', NULL, @KindergartenRouteId, '2026-03-17', 2, 1, @TeacherTwoStaffProfileId),
        (@SessionMorningLiveId, '2026-03-18T00:15:00+00:00', NULL, @MorningRouteId, '2026-03-18', 1, 0, @TeacherStaffProfileId);

    INSERT INTO [AttendanceRecords] ([Id], [CreatedAtUtc], [UpdatedAtUtc], [AttendanceSessionId], [StudentId], [Status], [EmergencyPhoneSnapshot])
    VALUES
        (NEWID(), '2026-03-16T22:15:30+00:00', NULL, @SessionMorningHistoryId, @StudentProfileId, 2, N'0900-000-003'),
        (NEWID(), '2026-03-16T22:15:30+00:00', NULL, @SessionMorningHistoryId, @StudentTwoId, 2, N'0900-000-005'),
        (NEWID(), '2026-03-16T22:15:30+00:00', NULL, @SessionMorningHistoryId, @StudentThreeId, 3, N'0900-000-006'),
        (NEWID(), '2026-03-16T09:20:30+00:00', NULL, @SessionDismissalHistoryId, @StudentProfileId, 2, N'0900-000-003'),
        (NEWID(), '2026-03-16T09:20:30+00:00', NULL, @SessionDismissalHistoryId, @StudentTwoId, 4, N'0900-000-005'),
        (NEWID(), '2026-03-16T09:20:30+00:00', NULL, @SessionDismissalHistoryId, @StudentThreeId, 2, N'0900-000-006'),
        (NEWID(), '2026-03-16T09:20:30+00:00', NULL, @SessionDismissalHistoryId, @StudentFourId, 2, N'0900-000-007'),
        (NEWID(), '2026-03-17T09:40:30+00:00', NULL, @SessionKindergartenHistoryId, @StudentFiveId, 2, N'0900-000-008'),
        (NEWID(), '2026-03-18T00:15:30+00:00', NULL, @SessionMorningLiveId, @StudentProfileId, 2, N'0900-000-003'),
        (NEWID(), '2026-03-18T00:15:30+00:00', NULL, @SessionMorningLiveId, @StudentTwoId, 1, N'0900-000-005'),
        (NEWID(), '2026-03-18T00:15:30+00:00', NULL, @SessionMorningLiveId, @StudentThreeId, 2, N'0900-000-006');

    COMMIT TRANSACTION;

    SELECT
        [Metric] = N'Demo refresh complete',
        [SeededStudents] = (SELECT COUNT(*) FROM [Students] WHERE [StudentNumber] IN (N'S10001', N'S10002', N'S10003', N'S10004', N'S10005')),
        [SeededRoutes] = (SELECT COUNT(*) FROM [Routes] WHERE [Id] IN (@MorningRouteId, @DismissalRouteId, @KindergartenRouteId, @BackupRouteId)),
        [SeededRegistrations] = (SELECT COUNT(*) FROM [RideRegistrations] WHERE [StudentId] IN (@StudentProfileId, @StudentTwoId, @StudentThreeId, @StudentFourId, @StudentFiveId)),
        [SeededAttendanceSessions] = (SELECT COUNT(*) FROM [AttendanceSessions] WHERE [Id] IN (@SessionMorningHistoryId, @SessionDismissalHistoryId, @SessionKindergartenHistoryId, @SessionMorningLiveId));
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    THROW;
END CATCH;
