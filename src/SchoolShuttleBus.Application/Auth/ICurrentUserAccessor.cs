namespace SchoolShuttleBus.Application.Auth;

public interface ICurrentUserAccessor
{
    Guid? UserId { get; }

    IReadOnlyCollection<string> Roles { get; }

    bool IsInRole(string roleName);
}
