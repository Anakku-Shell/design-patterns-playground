using Patterns.Demo;
using Patterns.Modern.NullObject.Classic;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Modern.NullObject;

public sealed class NullObjectDemo : IDemo
{
    public string Key => "null-object";
    public string Name => "Null Object";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§7.7";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("PriceService(IDiscount? discount, ILogger? logger): a null check at every use");
        narrator.Result(Invariant($"no discount, no logger: {new Problem.PriceService(null, null).PriceOf(100.00m):0.00}"));
        narrator.Result(Invariant($"10% off:                {new Problem.PriceService(new Problem.PercentageDiscount(10), null).PriceOf(100.00m):0.00}"));
        narrator.Result($"{Problem.Greeter.GreetingFor(SampleData.Ana)} / {Problem.Greeter.GreetingFor(SampleData.Guest)} (an \"if guest\" branch)");

        narrator.Level(1, "Classic");
        narrator.Step("NoDiscount.Instance does nothing; PriceService always has a discount and never checks");
        narrator.Result(Invariant($"NoDiscount.Instance:        {new Classic.PriceService(NoDiscount.Instance).PriceOf(100.00m):0.00}"));
        narrator.Result(Invariant($"new PercentageDiscount(10): {new Classic.PriceService(new Classic.PercentageDiscount(10)).PriceOf(100.00m):0.00}"));
        narrator.Step("CustomerProfiles.For decides once; GuestCustomer.Instance is the null object");
        narrator.Result($"{Greeter.GreetingFor(CustomerProfiles.For(SampleData.Ana))} / {Greeter.GreetingFor(CustomerProfiles.For(SampleData.Guest))}");

        narrator.Level(2, ".NET");
        narrator.Step("PriceService(discount, ILogger<PriceService>? logger = null): logger ?? NullLogger<PriceService>.Instance");
        narrator.Result(Invariant($"without a logger: {new DotNet.PriceService(new DotNet.PercentageDiscount(10)).PriceOf(100.00m):0.00} (logged to nowhere)"));
        narrator.Step("other null objects in the BCL");
        TextWriter.Null.Write("this text goes nowhere");
        narrator.Result("TextWriter.Null, Stream.Null, Enumerable.Empty<T>(), CancellationToken.None, Task.CompletedTask");

        narrator.Takeaway("Decide \"is there one?\" once, where the object is created, and pass a do-nothing object instead of null.");
    }
}
