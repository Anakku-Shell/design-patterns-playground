using Patterns.Demo;
using Patterns.Structural.Flyweight.Classic;
using Patterns.Structural.Flyweight.DotNet;
using static System.FormattableString;

namespace Patterns.Structural.Flyweight;

public sealed class FlyweightDemo : IDemo
{
    public string Key => "flyweight";
    public string Name => "Flyweight";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Historical;
    public string GuideSection => "§5.6";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(1, "Classic");
        narrator.Step("10,000 catalog entries, each asking the factory for its category");
        var factory = new CategoryInfoFactory();
        string[] categories = ["Books", "Electronics", "Home"];
        var entries = Enumerable.Range(1, 10_000)
            .Select(i => new CatalogEntry(Invariant($"SKU-{i:00000}"), 10m, factory.Get(categories[i % 3])))
            .ToList();
        narrator.Result(Invariant($"{entries.Count:N0} entries, {factory.Created} CategoryInfo objects created"));
        narrator.Result($"entries 3 and 6 share one Books object: {ReferenceEquals(entries[2].Category, entries[5].Category)}");

        narrator.Level(2, ".NET");
        narrator.Step("Two equal SKUs built at run time, then the same through string.Intern");
        narrator.Result($"built: same object? {ReferenceEquals(SkuText.Build("BOOK-", "001"), SkuText.Build("BOOK-", "001"))}");
        narrator.Result($"interned: same object? {ReferenceEquals(SkuText.BuildInterned("BOOK-", "001"), SkuText.BuildInterned("BOOK-", "001"))}");

        narrator.Takeaway("Share the immutable part, keep the per-use part outside; in .NET, measure before you need it.");
    }
}
