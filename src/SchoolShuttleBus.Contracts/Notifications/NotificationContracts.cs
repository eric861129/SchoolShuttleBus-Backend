namespace SchoolShuttleBus.Contracts.Notifications;

public sealed record ReminderRunResponse(Guid NotificationJobId, int DeliveryCount);

public sealed record NotificationDeliveryResponse(Guid NotificationDeliveryId, string RecipientEmail, string Status, DateTimeOffset? SentAtUtc, string? ErrorMessage);
