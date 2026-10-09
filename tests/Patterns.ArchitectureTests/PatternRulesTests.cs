using System.Text.RegularExpressions;
using Xunit;
using static Patterns.ArchitectureTests.ArchitectureFixture;

namespace Patterns.ArchitectureTests;

/// <summary>
/// The structural rules of the repository, one test per rule (guide §2.6). If a change makes one fail,
/// the design is what changes, never the test.
/// </summary>
public sealed partial class PatternRulesTests
{
    // Patterns.<Category>.<Pattern>.<Level>…: the third segment names the pattern.
    [GeneratedRegex(@"^Patterns\.(Creational|Structural|Behavioral|Modern)\.[^.]+")]
    private static partial Regex PatternNamespace();

    // Patterns.<Category> itself: only Demos lives there.
    [GeneratedRegex(@"^Patterns\.(Creational|Structural|Behavioral|Modern)$")]
    private static partial Regex CategoryRootNamespace();

    [GeneratedRegex(@"^Patterns\.[^.]+\.[^.]+\.Problem(\..+)?$")]
    private static partial Regex ProblemNamespace();

    [GeneratedRegex(@"^Patterns\.[^.]+\.[^.]+\.Classic(\..+)?$")]
    private static partial Regex ClassicNamespace();

    [GeneratedRegex(@"^Patterns\.[^.]+\.[^.]+\.DotNet(\..+)?$")]
    private static partial Regex DotNetNamespace();

    private static string NamespaceOf(ArchUnitNET.Domain.IType type) => type.Namespace.FullName;

    private static bool IsInNamespace(ArchUnitNET.Domain.IType type, string root) =>
        NamespaceOf(type) == root || NamespaceOf(type).StartsWith(root + ".", StringComparison.Ordinal);

    // Why: the shop is shared by every pattern. If it used a pattern (or a library), changing that pattern
    // could break all the others.
    [Fact]
    public void Shop_DependsOnNothing()
    {
        var violations = Uses(OwnTypes(ShopAssembly))
            .Where(u => !IsInNamespace(u.To, "System") && !IsInNamespace(u.To, "Patterns.Shop"))
            .Select(Describe);

        Assert.Empty(violations);
    }

    // Why: the demo contract between patterns and the runner must stay neutral and tiny.
    [Fact]
    public void Demo_DependsOnNothing()
    {
        var violations = Uses(OwnTypes(DemoAssembly))
            .Where(u => !IsInNamespace(u.To, "System") && !IsInNamespace(u.To, "Patterns.Demo"))
            .Select(Describe);

        Assert.Empty(violations);
    }

    // Why: each pattern must be understandable on its own. Patterns share a project per category, so the
    // compiler would allow one to use another; this test does not.
    [Fact]
    public void Pattern_DoesNotUseAnotherPattern()
    {
        var violations =
            from use in Uses(OwnTypes(CategoryAssemblies))
            let user = PatternNamespace().Match(NamespaceOf(use.From))
            let used = PatternNamespace().Match(NamespaceOf(use.To))
            where user.Success
                  // another pattern, or a "shared" helper put next to Demos in the category root
                  && ((used.Success && user.Value != used.Value) || CategoryRootNamespace().IsMatch(NamespaceOf(use.To)))
            select Describe(use);

        Assert.Empty(violations);
    }

    // Why: the Problem level is the "before" picture; the solutions must not lean on it.
    [Fact]
    public void Classic_DoesNotUseProblem() => AssertLevelDoesNotUseProblem(ClassicNamespace());

    // Why: same as above, for the .NET level.
    [Fact]
    public void DotNet_DoesNotUseProblem() => AssertLevelDoesNotUseProblem(DotNetNamespace());

    private static void AssertLevelDoesNotUseProblem(Regex level)
    {
        var violations = Uses(OwnTypes(CategoryAssemblies))
            .Where(u => level.IsMatch(NamespaceOf(u.From)) && ProblemNamespace().IsMatch(NamespaceOf(u.To)))
            .Select(Describe);

        Assert.Empty(violations);
    }

    // Why: "by hand" means by hand. The Classic level uses only the base class library, so every moving
    // part of the pattern is visible in our code.
    [Fact]
    public void Classic_DoesNotUseMicrosoftExtensions()
    {
        var violations = Uses(OwnTypes(CategoryAssemblies))
            .Where(u => ClassicNamespace().IsMatch(NamespaceOf(u.From))
                        && (IsInNamespace(u.To, "Microsoft.Extensions") || IsInNamespace(u.To, "Microsoft.AspNetCore")))
            .Select(Describe);

        Assert.Empty(violations);
    }

    // Why: the code shows the patterns and what .NET provides, nothing else. Third-party libraries (MediatR,
    // AutoMapper, Scrutor…) are named in the guide, never referenced.
    [Fact]
    public void PatternCode_DoesNotUseThirdPartyLibraries()
    {
        var violations = Uses(OwnTypes([ShopAssembly, DemoAssembly, .. CategoryAssemblies, RunnerAssembly]))
            .Where(u => !FromAllowedNamespace(u.To) || !FromAllowedAssembly(u.To))
            .Select(Describe);

        Assert.Empty(violations);
    }

    // Generic parameters (T) have no namespace.
    private static bool FromAllowedNamespace(ArchUnitNET.Domain.IType type) =>
        NamespaceOf(type).Length == 0
        || IsInNamespace(type, "System") || IsInNamespace(type, "Microsoft") || IsInNamespace(type, "Patterns");

    // The namespace alone is not enough: a third-party package can publish types under System.* or Microsoft.*.
    // The assembly that declares the type tells who shipped it.
    private static bool FromAllowedAssembly(ArchUnitNET.Domain.IType type)
    {
        var assembly = type.Assembly?.Name;
        return assembly is null or "mscorlib" or "netstandard"
               || assembly.StartsWith("System", StringComparison.Ordinal)
               || assembly.StartsWith("Microsoft.", StringComparison.Ordinal)
               || assembly.StartsWith("Patterns.", StringComparison.Ordinal);
    }
}
