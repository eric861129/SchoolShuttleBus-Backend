using FluentAssertions;
using SchoolShuttleBus.Domain.Notifications;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Tests;

public sealed class ReminderPolicyTests
{
    [Fact]
    public void ShouldSendReminder_ShouldReturnTrueOnThursdayWhenNextWeekHasMissingRegistration()
    {
        var snapshot = ReminderSnapshot.Create(
            UserRole.Parent,
            new DateOnly(2026, 3, 19),
            hasCompletedNextWeekRegistration: false);

        ReminderPolicy.ShouldSendReminder(snapshot).Should().BeTrue();
    }

    [Fact]
    public void ShouldSendReminder_ShouldReturnFalseWhenRegistrationAlreadyCompleted()
    {
        var snapshot = ReminderSnapshot.Create(
            UserRole.Student,
            new DateOnly(2026, 3, 20),
            hasCompletedNextWeekRegistration: true);

        ReminderPolicy.ShouldSendReminder(snapshot).Should().BeFalse();
    }
}
