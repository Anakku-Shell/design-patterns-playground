using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Patterns.Shop;

namespace Patterns.Modern.Options.DotNet;

// Role: Options — a plain class with one property per setting; the binder fills it by name.
// Guide: §7.2
public sealed class ShippingOptions
{
    public decimal StandardCost { get; set; }

    public decimal FreeShippingThreshold { get; set; }

    public IReadOnlyList<string> Countries { get; set; } = [];
}

// Role: Consumer — depends on the accessor, never on IConfiguration.
public sealed class ShippingCalculator(IOptions<ShippingOptions> options)
{
    public decimal CostFor(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var shipping = options.Value;
        return order.Total >= shipping.FreeShippingThreshold ? 0.00m : shipping.StandardCost;
    }
}

/// <summary>Binds the "Shipping" section, validates it, and registers the consumer.</summary>
public static class ShippingOptionsRegistration
{
    public static IServiceCollection AddShippingOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ShippingOptions>()
            .Bind(configuration.GetSection("Shipping"))
            .Validate(o => o.FreeShippingThreshold > 0, "FreeShippingThreshold must be positive.")
            .Validate(o => o.Countries.Count > 0, "At least one shipping country is required.")
            .ValidateOnStart(); // with a host: fail at start-up; with a bare provider: when first read
        services.AddSingleton<ShippingCalculator>();
        return services;
    }
}
