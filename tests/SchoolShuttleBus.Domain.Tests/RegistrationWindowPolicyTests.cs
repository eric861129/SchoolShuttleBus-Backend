using FluentAssertions;
using SchoolShuttleBus.Domain.Registrations;

namespace SchoolShuttleBus.Domain.Tests;

public sealed class RegistrationWindowPolicyTests
{
    [Fact]
    public void CanSubmit_ShouldAllowThursdayFirstSubmission_WhenNextWeekIsNotSubmittedYet()
    {
        var allowed = RegistrationWindowPolicy.CanSubmit(
            new DateOnly(2026, 3, 19),
            new DateOnly(2026, 3, 23),
            hasSubmittedWeek: false);

        allowed.Should().BeTrue();
    }

    [Fact]
    public void CanSubmit_ShouldBlockThursdayChanges_WhenNextWeekAlreadySubmitted()
    {
        var allowed = RegistrationWindowPolicy.CanSubmit(
            new DateOnly(2026, 3, 19),
            new DateOnly(2026, 3, 23),
            hasSubmittedWeek: true);

        allowed.Should().BeFalse();
    }

    [Fact]
    public void CanSubmit_ShouldBlockFridayRegistrations_ForNextWeek()
    {
        var allowed = RegistrationWindowPolicy.CanSubmit(
            new DateOnly(2026, 3, 20),
            new DateOnly(2026, 3, 23),
            hasSubmittedWeek: false);

        allowed.Should().BeFalse();
    }

    [Theory]
    [InlineData(10, true)]
    [InlineData(9, false)]
    public void HasSubmittedWeek_ShouldReflectCompleteTripSlots(int tripSlotCount, bool expected)
    {
        RegistrationWindowPolicy.HasSubmittedWeek(tripSlotCount).Should().Be(expected);
    }
}
