using Microsoft.Extensions.DependencyInjection;
using Patterns.Behavioral.Strategy.DotNet;
using Patterns.Demo;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Behavioral.Strategy;

public sealed class StrategyDemo : IDemo
{
    public string Key => "strategy";
    public string Name => "Strategy";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§6.9";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var threeBooks = SampleData.OrderOf((SampleData.Book, 3));
        var fourBooks = SampleData.OrderOf((SampleData.Book, 4));
        string[] methods = ["standard", "express", "pickup"];

        narrator.Level(0, "Problem");
        narrator.Step("CostFor(3 books, method): one switch with a case per method");
        var problem = new Problem.ShippingCalculator();
        narrator.Result(string.Join(", ", methods.Select(m => Invariant($"{m} {problem.CostFor(threeBooks, m):0.00}"))));

        narrator.Level(1, "Classic");
        narrator.Step("new ShippingCalculator(new ExpressShipping()): the context does not know which strategy it has");
        narrator.Result(Invariant($"express {new Classic.ShippingCalculator(new Classic.ExpressShipping()).CostFor(threeBooks):0.00}"));
        narrator.Step("StandardShipping at exactly 50.00 (4 books)");
        narrator.Result(Invariant($"{new Classic.StandardShipping().CostFor(fourBooks):0.00}: free, the rule is >="));

        narrator.Level(2, ".NET");
        narrator.Step("keyed services: the strategy is picked at run time by its name");
        using var services = new ServiceCollection().AddShippingStrategies().BuildServiceProvider();
        var calculator = new DotNet.ShippingCalculator(services);
        narrator.Result(string.Join(", ", methods.Select(m => Invariant($"{m} {calculator.CostFor(threeBooks, m):0.00}"))));
        narrator.Step("a Func<Order, decimal> is a strategy too: PriceWith(3 books, ShippingRules.Express)");
        narrator.Result(Invariant($"{ShippingRules.PriceWith(threeBooks, ShippingRules.Express):0.00}"));

        narrator.Takeaway("Each algorithm behind one interface; a new one is a new class (or lambda) and the code that uses it does not change.");
    }
}
