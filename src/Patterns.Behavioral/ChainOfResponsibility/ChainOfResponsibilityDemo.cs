using Microsoft.AspNetCore.Http;
using Patterns.Behavioral.ChainOfResponsibility.Classic;
using Patterns.Behavioral.ChainOfResponsibility.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.ChainOfResponsibility;

public sealed class ChainOfResponsibilityDemo : IDemo
{
    public string Key => "chain-of-responsibility";
    public string Name => "Chain of Responsibility";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§6.1";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var valid = SampleData.OrderOf((SampleData.Book, 2));
        var outOfStock = SampleData.OrderOf((SampleData.Mug, 1));

        narrator.Level(0, "Problem");
        narrator.Step("OrderValidator.Validate: four ifs in one method");
        narrator.Result(Describe(new Problem.OrderValidator().Validate(outOfStock).Error));

        narrator.Level(1, "Classic");
        narrator.Step("NotEmpty → MaxQuantity → Stock → ShippingCountry, built in one line");
        var rules = new NotEmptyRule();
        rules.SetNext(new MaxQuantityRule()).SetNext(new StockRule()).SetNext(new ShippingCountryRule());
        narrator.Result(Describe(rules.Check(outOfStock).Error));
        narrator.Step("2 books to Madrid: every rule passes and calls the next");
        narrator.Result(Describe(rules.Check(valid).Error));

        narrator.Level(2, ".NET");
        foreach (var withHeader in new[] { false, true })
        {
            narrator.Step(withHeader ? "middleware pipeline, with X-Customer-Id" : "middleware pipeline, without X-Customer-Id");
            var trace = new List<string>();
            var context = new DefaultHttpContext();
            if (withHeader) { context.Request.Headers["X-Customer-Id"] = "ana"; }
            // IDemo.Run is synchronous; a console demo can wait for the result.
            OrderPipeline.Build(trace)(context).GetAwaiter().GetResult();
            narrator.Result($"{string.Join(" → ", trace)} ({context.Response.StatusCode})");
        }

        narrator.Takeaway("Each link does one check and decides whether the request goes on; the order of the chain is one visible line.");
    }

    private static string Describe(string? error) => error ?? "valid";
}
