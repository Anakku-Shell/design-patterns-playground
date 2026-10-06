using Patterns.Creational.Builder.Classic;
using Patterns.Creational.Builder.DotNet;
using Patterns.Demo;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Creational.Builder;

public sealed class BuilderDemo : IDemo
{
    public string Key => "builder";
    public string Name => "Builder";
    public PatternCategory Category => PatternCategory.Creational;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§4.4";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("A mutable order filled in field by field, without an address yet");
        var mutable = new Problem.MutableOrder { Customer = SampleData.Ana };
        mutable.Lines.Add(new OrderLine(SampleData.Book, 2));
        narrator.Result($"the object exists, and IsValid() says {mutable.IsValid()}");

        narrator.Level(1, "Classic");
        narrator.Step("OrderBuilder.For(Ana).ShipTo(Madrid).Add(Book, 2).Add(Headphones).Build()");
        var built = OrderBuilder.For(SampleData.Ana).ShipTo(SampleData.Madrid)
            .Add(SampleData.Book, 2).Add(SampleData.Headphones).Build();
        narrator.Result(Invariant($"{built.Order.Lines.Count} lines, total {built.Order.Total:0.00}"));
        narrator.Step("The same builder without ShipTo");
        try
        {
            OrderBuilder.For(SampleData.Ana).Add(SampleData.Book).Build();
        }
        catch (InvalidOperationException ex)
        {
            narrator.Result($"refused: {ex.Message}");
        }

        narrator.Level(2, ".NET");
        narrator.Step("StringBuilder builds the receipt; UriBuilder builds the tracking link");
        foreach (var line in ReceiptPrinter.Print(built.Order).Split(Environment.NewLine))
        {
            narrator.Result(line);
        }
        narrator.Result(TrackingLink.For(built.Order.Id).ToString());

        narrator.Takeaway("A builder collects the parts and the rules, and hands out the object only when it is valid.");
    }
}
