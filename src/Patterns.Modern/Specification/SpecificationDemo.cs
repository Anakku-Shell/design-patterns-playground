using Patterns.Demo;
using Patterns.Modern.Specification.Classic;
using Patterns.Modern.Specification.DotNet;
using Patterns.Shop;

namespace Patterns.Modern.Specification;

public sealed class SpecificationDemo : IDemo
{
    public string Key => "specification";
    public string Name => "Specification";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§7.5";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("ProductFinder: one method per combination");
        var finder = new Problem.ProductFinder(SampleData.Products);
        narrator.Result($"FindCheapInStock(20): {Names(finder.FindCheapInStock(20m))}");
        narrator.Result($"FindOutOfStock():     {Names(finder.FindOutOfStock())}");
        narrator.Result($"FindBooksOrHome():    {Names(finder.FindBooksOrHome())}");

        narrator.Level(1, "Classic");
        narrator.Step("three rules (InStock, InCategory, CheaperThan) combined with And, Or, Not");
        Show(narrator, "new InStock().And(new CheaperThan(20))", new InStock().And(new CheaperThan(20m)));
        Show(narrator, "new InStock().Not()", new InStock().Not());
        Show(narrator, "new InCategory(\"books\").Or(new InCategory(\"home\"))", new InCategory("books").Or(new InCategory("home")));
        Show(narrator, "…the same .And(new InStock())", new InCategory("books").Or(new InCategory("home")).And(new InStock()));

        narrator.Level(2, ".NET");
        narrator.Step("ExpressionSpecification: the combined rule is one expression tree, usable in IQueryable.Where");
        var cheapInStock = new InStockExpression().And(new CheaperThanExpression(20m)).ToExpression();
        narrator.Result($"one lambda: parameter {cheapInStock.Parameters.Single().Name}, body {cheapInStock.Body.NodeType} of two comparisons");
        narrator.Result($"products.AsQueryable().Where(expression): {Names(SampleData.Products.AsQueryable().Where(cheapInStock))}");

        narrator.Takeaway("Name each business rule once and combine rules instead of writing a method per combination; use expressions when the database must run them.");
    }

    private static void Show(Narrator narrator, string label, Specification<Product> spec) =>
        narrator.Result($"{label}: {Names(SampleData.Products.Where(spec.IsSatisfiedBy))}");

    private static string Names(IEnumerable<Product> products) => string.Join(", ", products.Select(p => p.Name));
}
