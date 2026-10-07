using Patterns.Demo;
using Patterns.Shop;
using Patterns.Structural.Decorator.Classic;
using Patterns.Structural.Decorator.DotNet;
using static System.FormattableString;

namespace Patterns.Structural.Decorator;

public sealed class DecoratorDemo : IDemo
{
    public string Key => "decorator";
    public string Name => "Decorator";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§5.4";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var order = SampleData.OrderOf((SampleData.Book, 8));

        narrator.Level(0, "Problem");
        narrator.Step("Price(order, applyVat: true, coupon: 5): one method, flags, a fixed order");
        narrator.Result(Invariant($"{new Problem.PriceCalculator().Price(order, applyVat: true, coupon: 5m):0.00}"));

        narrator.Level(1, "Classic");
        narrator.Step("8 books through BasePrice");
        narrator.Result(Invariant($"{new BasePrice().PriceOf(order):0.00}"));
        narrator.Step("Coupon(Vat(BasePrice)): VAT first, then the coupon");
        narrator.Result(Invariant($"{new CouponDecorator(new VatDecorator(new BasePrice(), 0.21m), 5m).PriceOf(order):0.00}"));
        narrator.Step("Vat(Coupon(BasePrice)): the same two rules, the other way round");
        narrator.Result(Invariant($"{new VatDecorator(new CouponDecorator(new BasePrice(), 5m), 0.21m).PriceOf(order):0.00}"));

        narrator.Level(2, ".NET");
        narrator.Step("HttpClient → CorrelationIdHandler → RequestLogHandler → carrier");
        var log = new List<string>();
        using (var client = CarrierHttpClient.Create(() => "abc-123", log))
        {
            // IDemo.Run is synchronous; a console demo can wait for the result.
            using var response = client.GetAsync(new Uri("https://carrier.example.com/quote")).GetAwaiter().GetResult();
        }
        foreach (var line in log)
        {
            narrator.Result(line);
        }

        narrator.Takeaway("Decorators keep the interface and add one behaviour each; where you stack them decides the order.");
    }
}
