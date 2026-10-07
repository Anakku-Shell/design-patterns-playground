using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Patterns.Shop;

namespace Patterns.Modern.DependencyInjection.DotNet;

// Role: Client — the same constructor as the Classic level; the container calls it.
// Guide: §7.1
public sealed class CheckoutService(IEmailSender sender, TimeProvider clock)
{
    public string Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var text = string.Create(CultureInfo.InvariantCulture,
            $"Order {order.Id.ToString()[..8]} confirmed at {clock.GetUtcNow():HH:mm}");
        sender.Send(order.Customer.Email, text);
        return text;
    }
}

// Role: Service — the abstraction the client depends on.
public interface IEmailSender
{
    void Send(string recipient, string text);
}

// Role: ConcreteService — records what it would send.
public sealed class FakeEmailSender : IEmailSender
{
    private readonly List<string> _sent = [];

    public IReadOnlyList<string> Sent => _sent;

    public void Send(string recipient, string text) => _sent.Add($"{recipient}: {text}");
}

// Role: Injector — the registrations; the container reads constructors and builds the graph.
public static class CheckoutContainer
{
    public static IServiceCollection AddCheckout(this IServiceCollection services, TimeProvider clock)
    {
        services.AddSingleton(clock);                           // one for the whole application
        services.AddScoped<IEmailSender, FakeEmailSender>();    // one per scope (an HTTP request)
        services.AddTransient<CheckoutService>();               // a new one every time
        return services;
    }

    public static ServiceProvider Build(TimeProvider clock) =>
        new ServiceCollection().AddCheckout(clock).BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,  // a scoped service resolved from the root throws
            ValidateOnBuild = true, // every registration is checked now, not at first use
        });
}
