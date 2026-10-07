using Microsoft.Extensions.DependencyInjection;
using Patterns.Modern.DependencyInjection.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.DependencyInjection.Classic;
using DotNet = Patterns.Modern.DependencyInjection.DotNet;
using Problem = Patterns.Modern.DependencyInjection.Problem;

namespace Patterns.Modern.Tests;

public sealed class DependencyInjectionTests
{
    private static readonly Order Order =
        SampleData.OrderOf((SampleData.Book, 2)) with { Id = new Guid("1a2b3c4d-0000-0000-0000-000000000000") };

    private const string Expected = "Order 1a2b3c4d confirmed at 10:30";

    [Fact]
    public void Classic_AndDotNet_ConfirmTheSame()
    {
        var classicSender = new Classic.FakeEmailSender();
        var classic = Classic.CompositionRoot.CreateCheckout(classicSender, new FixedTime(10, 30));

        using var provider = CheckoutContainer.Build(new FixedTime(10, 30));
        using var scope = provider.CreateScope();
        var dotnet = scope.ServiceProvider.GetRequiredService<DotNet.CheckoutService>();

        Assert.Equal(Expected, classic.Confirm(Order));
        Assert.Equal(Expected, dotnet.Confirm(Order));
        Assert.Equal(["ana@example.com: " + Expected], classicSender.Sent);
        var dotnetSender = (DotNet.FakeEmailSender)scope.ServiceProvider.GetRequiredService<DotNet.IEmailSender>(); // scoped: the one the checkout got
        Assert.Equal(["ana@example.com: " + Expected], dotnetSender.Sent);
    }

    [Fact]
    public void Problem_HidesItsDependencies()
    {
        // The only constructor takes nothing: there is nothing a test could replace.
        Assert.Empty(typeof(Problem.CheckoutService).GetConstructors().Single().GetParameters());
        Assert.Empty(typeof(Problem.LocatorCheckoutService).GetConstructors().Single().GetParameters());
    }

    [Fact]
    public void Problem_Locator_FailsOnlyWhenUsed()
    {
        // Construction succeeds; the missing registration is found at run time, inside Confirm.
        var checkout = new Problem.LocatorCheckoutService();

        var error = Assert.Throws<InvalidOperationException>(() => checkout.Confirm(Order));

        Assert.Equal("No service registered for IEmailSender.", error.Message);
    }

    [Fact]
    public void Lifetimes()
    {
        using var provider = CheckoutContainer.Build(new FixedTime(10, 30));
        using var first = provider.CreateScope();
        using var second = provider.CreateScope();

        // Singleton: one for the whole container.
        Assert.Same(first.ServiceProvider.GetRequiredService<TimeProvider>(), second.ServiceProvider.GetRequiredService<TimeProvider>());

        // Scoped: the same within a scope, a different one in another scope.
        Assert.Same(first.ServiceProvider.GetRequiredService<DotNet.IEmailSender>(), first.ServiceProvider.GetRequiredService<DotNet.IEmailSender>());
        Assert.NotSame(first.ServiceProvider.GetRequiredService<DotNet.IEmailSender>(), second.ServiceProvider.GetRequiredService<DotNet.IEmailSender>());

        // Transient: a new one every time.
        Assert.NotSame(first.ServiceProvider.GetRequiredService<DotNet.CheckoutService>(), first.ServiceProvider.GetRequiredService<DotNet.CheckoutService>());
    }

    [Fact]
    public void ResolvingScopedFromRoot_Throws()
    {
        using var provider = CheckoutContainer.Build(new FixedTime(10, 30));

        Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<DotNet.IEmailSender>());
    }

    [Fact]
    public void CaptiveDependency_FailsOnBuild()
    {
        // A singleton checkout would capture one scoped sender forever; ValidateOnBuild refuses it at start-up.
        var services = new ServiceCollection()
            .AddSingleton<TimeProvider>(new FixedTime(10, 30))
            .AddScoped<DotNet.IEmailSender, DotNet.FakeEmailSender>()
            .AddSingleton<DotNet.CheckoutService>();

        var error = Assert.Throws<AggregateException>(() =>
            services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true }));

        Assert.Contains("Cannot consume scoped service", error.InnerExceptions[0].Message, StringComparison.Ordinal);
    }
}
