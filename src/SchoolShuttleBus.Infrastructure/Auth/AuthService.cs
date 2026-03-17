using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Contracts.Auth;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Auth;

public sealed class AuthService(
    UserManager<AppUser> userManager,
    SchoolShuttleBusDbContext dbContext,
    ITokenFactory tokenFactory,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Result<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var account = request.Account.Trim();
        var user = await ResolveUserByAccountAsync(account, cancellationToken);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<TokenResponse>.Fail("Invalid account or password.");
        }

        return Result<TokenResponse>.Ok(await CreateTokenEnvelopeAsync(user, cancellationToken));
    }

    public async Task<Result<TokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await dbContext.RefreshTokens
            .Include(token => token.User)
            .SingleOrDefaultAsync(token => token.Token == request.RefreshToken, cancellationToken);

        if (refreshToken is null || !refreshToken.IsActive)
        {
            return Result<TokenResponse>.Fail("Refresh token is invalid or expired.");
        }

        refreshToken.RevokedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<TokenResponse>.Ok(await CreateTokenEnvelopeAsync(refreshToken.User, cancellationToken));
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeTokens = await dbContext.RefreshTokens
            .Where(token => token.UserId == userId && token.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.RevokedAtUtc = DateTimeOffset.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);
        if (user is null)
        {
            return Result<CurrentUserResponse>.Fail("User not found.");
        }

        var roles = await userManager.GetRolesAsync(user);
        return Result<CurrentUserResponse>.Ok(new CurrentUserResponse(user.Id, user.Email ?? string.Empty, roles.ToArray()));
    }

    public async Task<Result<CurrentUserContextResponse>> GetCurrentUserContextAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result<CurrentUserContextResponse>.Fail("User not found.");
        }

        var roles = (await userManager.GetRolesAsync(user)).ToArray();
        var students = await GetAccessibleStudentsAsync(userId, roles, cancellationToken);
        var staffProfile = await GetStaffProfileAsync(userId, roles, cancellationToken);
        var displayName = await ResolveDisplayNameAsync(userId, user.Email ?? string.Empty, roles, students, staffProfile, cancellationToken);

        return Result<CurrentUserContextResponse>.Ok(new CurrentUserContextResponse(
            user.Id,
            user.Email ?? string.Empty,
            roles,
            displayName,
            students,
            staffProfile));
    }

    private async Task<AppUser?> ResolveUserByAccountAsync(string account, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(account))
        {
            return null;
        }

        var normalizedAccount = account.Trim().ToUpperInvariant();

        var studentUserId = await dbContext.Students
            .Where(student => student.StudentNumber.ToUpper() == normalizedAccount)
            .Select(student => (Guid?)student.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (studentUserId is { } studentId)
        {
            return await userManager.FindByIdAsync(studentId.ToString());
        }

        var staffUserId = await dbContext.StaffProfiles
            .Where(profile => profile.EmployeeNumber.ToUpper() == normalizedAccount)
            .Select(profile => (Guid?)profile.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (staffUserId is { } staffId)
        {
            return await userManager.FindByIdAsync(staffId.ToString());
        }

        var guardianUserId = await dbContext.Guardians
            .Where(guardian => guardian.PhoneNumber == account)
            .Select(guardian => (Guid?)guardian.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (guardianUserId is { } guardianId)
        {
            return await userManager.FindByIdAsync(guardianId.ToString());
        }

        return await userManager.FindByNameAsync(account);
    }

    private async Task<AccessibleStudentResponse[]> GetAccessibleStudentsAsync(Guid userId, IReadOnlyCollection<string> roles, CancellationToken cancellationToken)
    {
        if (roles.Contains(RoleNames.Student))
        {
            return await dbContext.Students
                .AsNoTracking()
                .Where(student => student.UserId == userId)
                .OrderBy(student => student.StudentNumber)
                .ThenBy(student => student.FullName)
                .Select(student => new AccessibleStudentResponse(
                    student.Id,
                    student.StudentNumber,
                    student.FullName,
                    student.Stage,
                    student.GradeLabel))
                .ToArrayAsync(cancellationToken);
        }

        if (roles.Contains(RoleNames.Parent))
        {
            return await dbContext.Students
                .AsNoTracking()
                .Where(student => student.GuardianLinks.Any(link => link.Guardian.UserId == userId))
                .OrderBy(student => student.StudentNumber)
                .ThenBy(student => student.FullName)
                .Select(student => new AccessibleStudentResponse(
                    student.Id,
                    student.StudentNumber,
                    student.FullName,
                    student.Stage,
                    student.GradeLabel))
                .ToArrayAsync(cancellationToken);
        }

        return [];
    }

    private async Task<StaffProfileSummaryResponse?> GetStaffProfileAsync(Guid userId, IReadOnlyCollection<string> roles, CancellationToken cancellationToken)
    {
        if (!roles.Contains(RoleNames.Teacher) && !roles.Contains(RoleNames.Administrator))
        {
            return null;
        }

        return await dbContext.StaffProfiles
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => new StaffProfileSummaryResponse(
                profile.Id,
                profile.EmployeeNumber,
                profile.FullName,
                profile.CanManageAllRoutes))
            .SingleOrDefaultAsync(cancellationToken);
    }

    private async Task<string> ResolveDisplayNameAsync(
        Guid userId,
        string email,
        IReadOnlyCollection<string> roles,
        IReadOnlyCollection<AccessibleStudentResponse> students,
        StaffProfileSummaryResponse? staffProfile,
        CancellationToken cancellationToken)
    {
        if (roles.Contains(RoleNames.Student))
        {
            return students.Select(student => student.StudentName).FirstOrDefault() ?? email;
        }

        if (roles.Contains(RoleNames.Parent))
        {
            var guardianName = await dbContext.Guardians
                .AsNoTracking()
                .Where(guardian => guardian.UserId == userId)
                .Select(guardian => guardian.FullName)
                .SingleOrDefaultAsync(cancellationToken);

            return string.IsNullOrWhiteSpace(guardianName) ? email : guardianName;
        }

        if (roles.Contains(RoleNames.Teacher) || roles.Contains(RoleNames.Administrator))
        {
            return string.IsNullOrWhiteSpace(staffProfile?.FullName) ? email : staffProfile.FullName;
        }

        return email;
    }

    private async Task<TokenResponse> CreateTokenEnvelopeAsync(AppUser user, CancellationToken cancellationToken)
    {
        var roles = await userManager.GetRolesAsync(user);
        var expiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var roleArray = roles.ToArray();
        var accessToken = tokenFactory.CreateAccessToken(user.Id, user.Email ?? string.Empty, roleArray, expiresAtUtc);
        var refreshTokenValue = tokenFactory.CreateRefreshToken();

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new TokenResponse(user.Id, user.Email ?? string.Empty, accessToken, refreshTokenValue, expiresAtUtc);
    }
}
