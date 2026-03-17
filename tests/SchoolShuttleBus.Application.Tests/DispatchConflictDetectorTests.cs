using FluentAssertions;
using SchoolShuttleBus.Application.Dispatching;
using SchoolShuttleBus.Contracts.Dispatching;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Application.Tests;

public sealed class DispatchConflictDetectorTests
{
    [Fact]
    public void Detect_ShouldRejectOverlappingOverrideForSameStudentAndTrip()
    {
        var detector = new DispatchConflictDetector();
        var existingOverrides = new[]
        {
            new DispatchOverrideWindow(
                StudentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Date: new DateOnly(2026, 3, 23),
                Direction: TripDirection.Homebound,
                RouteId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"))
        };

        var command = new DispatchOverrideWindow(
            StudentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Date: new DateOnly(2026, 3, 23),
            Direction: TripDirection.Homebound,
            RouteId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));

        var result = detector.Detect(existingOverrides, command);

        result.HasConflict.Should().BeTrue();
    }

    [Fact]
    public void Detect_ShouldAllowDifferentDirectionOnSameDate()
    {
        var detector = new DispatchConflictDetector();
        var existingOverrides = new[]
        {
            new DispatchOverrideWindow(
                StudentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Date: new DateOnly(2026, 3, 23),
                Direction: TripDirection.ToSchool,
                RouteId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"))
        };

        var command = new DispatchOverrideWindow(
            StudentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Date: new DateOnly(2026, 3, 23),
            Direction: TripDirection.Homebound,
            RouteId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));

        var result = detector.Detect(existingOverrides, command);

        result.HasConflict.Should().BeFalse();
    }
}
