using FluentAssertions;
using SchoolShuttleBus.Domain.Registrations;

namespace SchoolShuttleBus.Domain.Tests;

public sealed class RegistrationWeekTests
{
    [Fact]
    public void Create_ShouldRejectDatesThatAreNotMonday()
    {
        Action action = () => RegistrationWeek.Create(new DateOnly(2026, 3, 18));

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Monday*");
    }

    [Fact]
    public void Create_ShouldBuildFiveWeekdaysStartingFromMonday()
    {
        var week = RegistrationWeek.Create(new DateOnly(2026, 3, 16));

        week.Days.Should().HaveCount(5);
        week.Days.Select(static day => day.Date).Should().ContainInOrder(
        [
            new DateOnly(2026, 3, 16),
            new DateOnly(2026, 3, 17),
            new DateOnly(2026, 3, 18),
            new DateOnly(2026, 3, 19),
            new DateOnly(2026, 3, 20)
        ]);
    }
}
