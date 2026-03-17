using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Admin;
using SchoolShuttleBus.Contracts.Notifications;

namespace SchoolShuttleBus.Application.Notifications;

public interface INotificationService
{
    Task<Result<ReminderRunResponse>> RunRemindersAsync(Guid? actorUserId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<NotificationDeliveryResponse>> GetHistoryAsync(CancellationToken cancellationToken);

    Task<Result<BroadcastResponse>> CreateBroadcastAsync(CreateBroadcastRequest request, Guid actorUserId, CancellationToken cancellationToken);
}
