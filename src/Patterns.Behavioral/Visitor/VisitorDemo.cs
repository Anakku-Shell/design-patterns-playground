using System.Linq.Expressions;
using Patterns.Behavioral.Visitor.Classic;
using Patterns.Behavioral.Visitor.DotNet;
using Patterns.Demo;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Behavioral.Visitor;

public sealed class VisitorDemo : IDemo
{
    public string Key => "visitor";
    public string Name => "Visitor";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Niche;
    public string GuideSection => "§6.11";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var kit = new BundleNode("Starter kit",
        [
            new ProductNode(SampleData.Book),
            new ProductNode(SampleData.Mug),
            new BundleNode("Audio", [new ProductNode(SampleData.Headphones)]),
        ]);

        narrator.Level(1, "Classic");
        narrator.Step("kit.Accept(new OutlineVisitor()): Accept picks the node, Visit picks the operation");
        var outline = new OutlineVisitor();
        kit.Accept(outline);
        foreach (var line in outline.Text.Split('\n'))
        {
            narrator.Result(line);
        }
        narrator.Step("kit.Accept(new VatVisitor()): books 4 %, the rest 21 %, rounded once at the end");
        var vat = new VatVisitor();
        kit.Accept(vat);
        narrator.Result(Invariant($"{vat.Total:0.00}"));

        narrator.Level(2, ".NET");
        narrator.Step("records + a switch expression: VatCalculator.VatOf(kit)");
        var items = new BundleItem("Starter kit",
        [
            new ProductItem(SampleData.Book),
            new ProductItem(SampleData.Mug),
            new BundleItem("Audio", [new ProductItem(SampleData.Headphones)]),
        ]);
        narrator.Result(Invariant($"{VatCalculator.VatOf(items):0.00}"));
        narrator.Step("ExpressionVisitor over o => o.Total > 100m && o.Units < 5");
        Expression<Func<Order, bool>> rule = o => o.Total > 100m && o.Units < 5;
        var collector = new ConstantCollector();
        collector.Visit(rule);
        narrator.Result($"constants: {string.Join(", ", collector.Constants.Select(c => Invariant($"{c}")))}");

        narrator.Takeaway("Visitor makes new operations easy and new node types hard; for your own types, pattern matching usually replaces it.");
    }
}
