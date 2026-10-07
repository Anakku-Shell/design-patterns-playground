using Microsoft.Extensions.Logging;
using Patterns.Modern.NullObject.Classic;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.NullObject.Classic;
using DotNet = Patterns.Modern.NullObject.DotNet;
using Problem = Patterns.Modern.NullObject.Problem;

namespace Patterns.Modern.Tests;

public sealed class NullObjectTests
{
    [Fact]
    public void AllLevels_PriceWithoutADiscount()
    {
        Assert.Equal(100.00m, new Problem.PriceService(null, null).PriceOf(100.00m));
        Assert.Equal(100.00m, new Classic.PriceService(Classic.NoDiscount.Instance).PriceOf(100.00m));
        Assert.Equal(100.00m, new DotNet.PriceService(DotNet.NoDiscount.Instance).PriceOf(100.00m));
    }

    [Fact]
    public void AllLevels_PriceWithTenPercentOff()
    {
        Assert.Equal(90.00m, new Problem.PriceService(new Problem.PercentageDiscount(10), null).PriceOf(100.00m));
        Assert.Equal(90.00m, new Classic.PriceService(new Classic.PercentageDiscount(10)).PriceOf(100.00m));
        Assert.Equal(90.00m, new DotNet.PriceService(new DotNet.PercentageDiscount(10)).PriceOf(100.00m));
    }

    [Fact]
    public void PercentageDiscount_RoundsToCents()
    {
        // 12.345 → 12.35 away from zero (to even would give 12.34).
        Assert.Equal(12.35m, new Classic.PercentageDiscount(50).Apply(24.69m));
        Assert.Equal(12.35m, new DotNet.PercentageDiscount(50).Apply(24.69m));
    }

    [Fact]
    public void NoDiscount_ChangesNothing()
    {
        Assert.Equal(37.50m, Classic.NoDiscount.Instance.Apply(37.50m));
        Assert.Equal(0.00m, Classic.NoDiscount.Instance.Apply(0.00m));
        Assert.Empty(typeof(Classic.NoDiscount).GetConstructors()); // no public constructor: Instance is the only one
    }

    [Fact]
    public void Guest_IsGreetedWithoutNullChecks()
    {
        Assert.Equal("Hello, Ana", Greeter.GreetingFor(CustomerProfiles.For(SampleData.Ana)));
        Assert.Equal("Hello, guest", Greeter.GreetingFor(CustomerProfiles.For(SampleData.Guest)));
        Assert.Same(GuestCustomer.Instance, CustomerProfiles.For(SampleData.Guest));
    }

    [Fact]
    public void Problem_GreetsWithABranch()
    {
        Assert.Equal("Hello, Ana", Problem.Greeter.GreetingFor(SampleData.Ana));
        Assert.Equal("Hello, guest", Problem.Greeter.GreetingFor(SampleData.Guest));
    }

    [Fact]
    public void DotNet_WorksWithoutALogger()
    {
        var service = new DotNet.PriceService(new DotNet.PercentageDiscount(10)); // no logger: NullLogger inside

        Assert.Equal(90.00m, service.PriceOf(100.00m));
    }

    [Fact]
    public void DotNet_LogsWhenGivenALogger()
    {
        var logger = new ListLogger<DotNet.PriceService>();

        new DotNet.PriceService(new DotNet.PercentageDiscount(10), logger).PriceOf(100.00m);

        Assert.Equal(["Priced 100.00 at 90.00"], logger.Messages);
    }

    private sealed class ListLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
            Messages.Add(formatter(state, exception));
    }
}
