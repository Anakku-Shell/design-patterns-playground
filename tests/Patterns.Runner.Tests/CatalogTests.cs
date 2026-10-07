using Patterns.Demo;
using Xunit;

namespace Patterns.Runner.Tests;

public sealed class CatalogTests
{
    // The patterns built so far, in guide order. Each code phase appends its cards (spec §3).
    private static readonly (string Key, Relevance Relevance)[] Expected =
    [
        ("singleton", Relevance.Useful),
        ("factory-method", Relevance.Useful),
        ("abstract-factory", Relevance.Niche),
        ("builder", Relevance.Essential),
        ("prototype", Relevance.Historical),
        ("adapter", Relevance.Essential),
        ("bridge", Relevance.Niche),
        ("composite", Relevance.Useful),
        ("decorator", Relevance.Essential),
        ("facade", Relevance.Essential),
        ("flyweight", Relevance.Historical),
        ("proxy", Relevance.Useful),
        ("chain-of-responsibility", Relevance.Essential),
        ("command", Relevance.Useful),
        ("interpreter", Relevance.Historical),
        ("iterator", Relevance.Essential),
        ("mediator", Relevance.Useful),
        ("memento", Relevance.Niche),
        ("observer", Relevance.Essential),
        ("state", Relevance.Useful),
        ("strategy", Relevance.Essential),
        ("template-method", Relevance.Useful),
        ("visitor", Relevance.Niche),
        ("dependency-injection", Relevance.Essential),
        ("options", Relevance.Essential),
        ("repository", Relevance.Useful),
        ("unit-of-work", Relevance.Useful),
        ("specification", Relevance.Useful),
        ("result", Relevance.Useful),
        ("null-object", Relevance.Useful),
        ("object-pool", Relevance.Niche),
    ];

    [Fact]
    public void Catalog_MatchesTheExpectedPatterns()
    {
        Assert.Equal(Expected, Catalog.All.Select(d => (d.Key, d.Relevance)));
    }

    [Fact]
    public void Keys_AreUnique()
    {
        var keys = Catalog.All.Select(d => d.Key).ToList();

        Assert.Equal(keys.Count, keys.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void Keys_AreKebabCase()
    {
        Assert.All(Catalog.All, d => Assert.Matches("^[a-z]+(-[a-z]+)*$", d.Key));
    }

    [Fact]
    public void EveryDemo_RunsAndWritesOutput()
    {
        // A loop instead of a [Theory]: with an empty catalog a theory has no rows, and the loop simply passes.
        foreach (var demo in Catalog.All)
        {
            var output = new StringWriter();

            demo.Run(output);

            Assert.False(string.IsNullOrWhiteSpace(output.ToString()), $"{demo.Key} wrote nothing.");
        }
    }

    [Fact]
    public void EveryDemo_PointsToItsGuideSection()
    {
        Assert.All(Catalog.All, d => Assert.Matches(@"^§[4-7]\.\d+$", d.GuideSection));
    }
}
