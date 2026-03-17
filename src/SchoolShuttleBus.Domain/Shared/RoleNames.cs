namespace SchoolShuttleBus.Domain.Shared;

public static class RoleNames
{
    public const string Student = nameof(Student);
    public const string Parent = nameof(Parent);
    public const string Teacher = nameof(Teacher);
    public const string Administrator = "Administrator";

    public static readonly string[] All =
    [
        Student,
        Parent,
        Teacher,
        Administrator
    ];
}
