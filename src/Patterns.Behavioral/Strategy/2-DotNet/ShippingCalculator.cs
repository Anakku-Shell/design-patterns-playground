using Microsoft.Extensions.DependencyInjection;
using Patterns.Shop;

namespace Patterns.Behavioral.Strategy.DotNet;

// Role: Context — asks the container for the strategy registered under the method's name.
// Guide: §6.9
public sealed class ShippingCalculator(IServiceProvider services)
{
    /// <summary>The same signature as the Problem level; the switch became the container's registrations.</summary>
    public decimal CostFor(Order order, string method) =>
        services.GetRequiredKeyedService<IShippingStrategy>(method).CostFor(order); // unknown key: InvalidOperationException
}

/// <summary>Registers one strategy per shipping method, keyed by its name.</summary>
public static class ShippingServiceCollectionExtensions
{
    public static IServiceCollection AddShippingStrategies(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IShippingStrategy, StandardShipping>("standard");
        services.AddKeyedSingleton<IShippingStrategy, ExpressShipping>("express");
        services.AddKeyedSingleton<IShippingStrategy, StorePickup>("pickup");
        return services;
    }
}

/// <summary>A strategy is often just a function: <c>Func&lt;Order, decimal&gt;</c> is the interface, a lambda the class.</summary>
public static class ShippingRules
{
    public static Func<Order, decimal> Standard { get; } = order => order.Total >= 50.00m ? 0.00m : 4.99m;

    public static Func<Order, decimal> Express { get; } = order => 9.99m + 1.00m * order.Units;

    public static Func<Order, decimal> Pickup { get; } = _ => 0.00m;

    /// <summary>The order total plus shipping, priced by whichever function it is given.</summary>
    public static decimal PriceWith(Order order, Func<Order, decimal> shipping)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(shipping);
        return order.Total + shipping(order);
    }
}
