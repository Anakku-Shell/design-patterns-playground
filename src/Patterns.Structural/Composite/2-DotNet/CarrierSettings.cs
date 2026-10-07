using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Patterns.Structural.Composite.DotNet;

// Role: Client of a framework composite — every IConfigurationSection is itself an IConfiguration: a leaf
// with a value, or a node with children, navigated the same way.
// Guide: §5.3
public static class CarrierSettings
{
    // Reads Shipping → Carriers → <name> → Days into { name: days }.
    public static IReadOnlyDictionary<string, int> Read(IConfiguration configuration) =>
        configuration.GetSection("Shipping:Carriers")
            .GetChildren() // each child section is again an IConfiguration
            .ToDictionary(carrier => carrier.Key,
                          carrier => int.Parse(carrier["Days"]!, CultureInfo.InvariantCulture));
}
