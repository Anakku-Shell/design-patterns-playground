namespace Patterns.Modern.Tests;

/// <summary>A clock stopped at one time of day (UTC), so texts that print the time are exact in tests.</summary>
internal sealed class FixedTime(int hour, int minute) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => new(2026, 10, 7, hour, minute, 0, TimeSpan.Zero);
}
