using Patterns.Demo;

namespace Patterns.Runner;

/// <summary>
/// Every demo, in guide order: the four category lists concatenated. No reflection or scanning, so
/// "how does a demo get run?" is one "go to definition" away. Guide: §2.4.
/// </summary>
public static class Catalog
{
    public static IReadOnlyList<IDemo> All { get; } =
    [
        .. Creational.Demos.All,
        .. Structural.Demos.All,
        .. Behavioral.Demos.All,
        .. Modern.Demos.All,
    ];
}
