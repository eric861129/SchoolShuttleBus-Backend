using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Application.Notifications;
using SchoolShuttleBus.Contracts.Admin;
using SchoolShuttleBus.Contracts.Notifications;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Notifications;
using SchoolShuttleBus.Domain.Registrations;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class NotificationService(
    SchoolShuttleBusDbContext dbContext,
    IEmailDispatcher emailDispatcher,
    ILocalTimeProvider localTimeProvider) : INotificationService
{
    public async Task<Result<ReminderRunResponse>> RunRemindersAsync(Guid? actorUserId, CancellationToken cancellationToken)
    {
        var localDate = localTimeProvider.Today;
        var targetWeekStart = localTimeProvider.NextWeekStart;
        var students = await dbContext.Students
            .Include(student => student.GuardianLinks)
                .ThenInclude(link => link.Guardian)
            .ToListAsync(cancellationToken);

        var reminderJob = new NotificationJob
        {
            JobType = "Reminder",
            Channel = NotificationChannel.Email,
            Subject = "Shuttle bus registration reminder",
            Body = "Please complete next week's shuttle bus registration.",
            CreatedByUserId = actorUserId
        };

        foreach (var student in students)
        {
            var count = await dbContext.RideRegistrations.CountAsync(
                registration => registration.StudentId == student.Id &&
                                registration.Date >= targetWeekStart &&
                                registration.Date <= targetWeekStart.AddDays(4),
                cancellationToken);

            var shouldSend = ReminderPolicy.ShouldSendReminder(
                ReminderSnapshot.Create(
                    UserRole.Parent,
                    localDate,
                    RegistrationWindowPolicy.HasSubmittedWeek(count)));
            if (!shouldSend)
            {
                continue;
            }

            var recipients = student.GuardianLinks
                .Select(link => link.Guardian.Email)
                .Append((await dbContext.Users.SingleAsync(user => user.Id == student.UserId, cancellationToken)).Email ?? string.Empty)
                .Where(static email => !string.IsNullOrWhiteSpace(email))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (var recipient in recipients)
            {
                await emailDispatcher.SendAsync(recipient, reminderJob.Subject, reminderJob.Body, cancellationToken);
                reminderJob.Deliveries.Add(new NotificationDelivery
                {
                    RecipientEmail = recipient,
                    RecipientUserId = null,
                    Status = "Sent",
                    SentAtUtc = DateTimeOffset.UtcNow
                });
            }
        }

        dbContext.NotificationJobs.Add(reminderJob);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<ReminderRunResponse>.Ok(new ReminderRunResponse(reminderJob.Id, reminderJob.Deliveries.Count));
    }

    public async Task<IReadOnlyCollection<NotificationDeliveryResponse>> GetHistoryAsync(CancellationToken cancellationToken)
    {
        return await dbContext.NotificationDeliveries
            .OrderByDescending(delivery => delivery.CreatedAtUtc)
            .Select(delivery => new NotificationDeliveryResponse(
                delivery.Id,
                delivery.RecipientEmail,
                delivery.Status,
                delivery.SentAtUtc,
                delivery.ErrorMessage))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Result<BroadcastResponse>> CreateBroadcastAsync(CreateBroadcastRequest request, Guid actorUserId, CancellationToken cancellationToken)
    {
        var announcement = new BroadcastAnnouncement
        {
            Audience = request.Audience,
            Subject = request.Subject,
            Body = request.Body,
            CreatedByUserId = actorUserId
        };

        var job = new NotificationJob
        {
            JobType = "Broadcast",
            Channel = NotificationChannel.Email,
            Subject = request.Subject,
            Body = request.Body,
            CreatedByUserId = actorUserId
        };

        var recipients = await ResolveAudienceEmailsAsync(request.Audience, cancellationToken);
        foreach (var email in recipients)
        {
            await emailDispatcher.SendAsync(email, request.Subject, request.Body, cancellationToken);
            job.Deliveries.Add(new NotificationDelivery
            {
                RecipientEmail = email,
                Status = "Sent",
                SentAtUtc = DateTimeOffset.UtcNow
            });
        }

        dbContext.BroadcastAnnouncements.Add(announcement);
        dbContext.NotificationJobs.Add(job);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<BroadcastResponse>.Ok(new BroadcastResponse(announcement.Id, job.Id, job.Deliveries.Count));
    }

    private async Task<IReadOnlyCollection<string>> ResolveAudienceEmailsAsync(BroadcastAudience audience, CancellationToken cancellationToken)
    {
        IQueryable<Auth.AppUser> query = dbContext.Users;
        if (audience != BroadcastAudience.All)
        {
            var roleName = audience switch
            {
                BroadcastAudience.Parents => RoleNames.Parent,
                BroadcastAudience.Students => RoleNames.Student,
                BroadcastAudience.Teachers => RoleNames.Teacher,
                _ => RoleNames.Administrator
            };

            var userIds = dbContext.UserRoles
                .Join(dbContext.Roles.Where(role => role.Name == roleName),
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, _) => userRole.UserId);

            query = query.Where(user => userIds.Contains(user.Id));
        }

        return await query
            .Where(user => user.Email != null)
            .Select(user => user.Email!)
            .Distinct()
            .ToArrayAsync(cancellationToken);
    }
}
