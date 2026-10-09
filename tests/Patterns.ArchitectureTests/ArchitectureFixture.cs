using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Patterns.Demo;
using Patterns.Runner;
using Patterns.Shop;

namespace Patterns.ArchitectureTests;

/// <summary>
/// Loads the compiled assemblies once and answers "who uses whom" for the rules in
/// <see cref="PatternRulesTests"/>. Guide: §2.6.
/// </summary>
internal static class ArchitectureFixture
{
    public static readonly System.Reflection.Assembly ShopAssembly = typeof(SampleData).Assembly;
    public static readonly System.Reflection.Assembly DemoAssembly = typeof(IDemo).Assembly;

    public static readonly System.Reflection.Assembly[] CategoryAssemblies =
    [
        typeof(Creational.Demos).Assembly,
        typeof(Structural.Demos).Assembly,
        typeof(Behavioral.Demos).Assembly,
        typeof(Modern.Demos).Assembly,
    ];

    public static readonly System.Reflection.Assembly RunnerAssembly = typeof(RunnerApp).Assembly;

    public static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies([ShopAssembly, DemoAssembly, .. CategoryAssemblies, RunnerAssembly])
        .Build();

    /// <summary>
    /// The types we wrote in the given assemblies, in any namespace (the global one included, so nothing
    /// escapes the rules). The compiler also adds a few types to every assembly: nullable-annotation attributes
    /// in <c>System.Runtime.CompilerServices</c> and <c>Microsoft.CodeAnalysis</c>, and helpers named
    /// <c>&lt;…&gt;</c>. Those are not ours, so they are skipped.
    /// </summary>
    public static IEnumerable<IType> OwnTypes(params System.Reflection.Assembly[] assemblies)
    {
        var names = assemblies.Select(a => a.FullName).ToHashSet();
        return Architecture.Types.Where(t =>
            names.Contains(t.Assembly.FullName)
            && !t.Name.StartsWith('<')
            && t.Namespace.FullName is not ("System.Runtime.CompilerServices" or "Microsoft.CodeAnalysis"));
    }

    /// <summary>Every (type, used type) pair: fields, parameters, calls, base types, attributes…</summary>
    public static IEnumerable<(IType From, IType To)> Uses(IEnumerable<IType> types) =>
        from type in types
        from dependency in type.Dependencies
        select (type, dependency.Target);

    public static string Describe((IType From, IType To) use) => $"{use.From.FullName} uses {use.To.FullName}";
}
