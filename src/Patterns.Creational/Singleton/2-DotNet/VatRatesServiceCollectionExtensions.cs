using Microsoft.Extensions.DependencyInjection;

namespace Patterns.Creational.Singleton.DotNet;

// Guide: §4.1
public static class VatRatesServiceCollectionExtensions
{
    // One instance for the lifetime of this container. Consumers receive it in their constructor,
    // so the dependency is visible, and a test can build its own container or just call new.
    public static IServiceCollection AddVatRates(this IServiceCollection services) =>
        services.AddSingleton<VatRateTable>();
}
