using System.Text;
using Patterns.Demo;
using Patterns.Shop;
using Patterns.Structural.Adapter.Classic;
using Patterns.Structural.Adapter.DotNet;
using Patterns.Structural.Adapter.External;
using static System.FormattableString;

namespace Patterns.Structural.Adapter;

public sealed class AdapterDemo : IDemo
{
    public string Key => "adapter";
    public string Name => "Adapter";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§5.1";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var order = SampleData.OrderOf((SampleData.Book, 2));
        var carrier = new LegacyCarrierClient();

        narrator.Level(0, "Problem");
        narrator.Step("CheckoutSummary builds the carrier's payload and parses its answer itself");
        narrator.Result(new Problem.CheckoutSummary(carrier).Describe(order));
        narrator.Result($"sent \"{carrier.LastPayload}\"; OrderTracking has its own copy of the same code");

        narrator.Level(1, "Classic");
        narrator.Step("LegacyCarrierAdapter turns an Order into the payload and the answer into a ShippingQuote");
        var adapter = new LegacyCarrierAdapter(carrier);
        narrator.Result(adapter.Quote(order).ToString());
        narrator.Result(new CheckoutSummary(adapter).Describe(order));
        narrator.Step("The same order shipped to Lisbon");
        narrator.Result(adapter.Quote(order with { ShippingAddress = SampleData.Lisbon }).ToString());

        narrator.Level(2, ".NET");
        narrator.Step("StreamReader adapts the bytes of the carrier's rate file to lines of text");
        using var file = new MemoryStream(Encoding.UTF8.GetBytes("ES;2\nPT;4"));
        foreach (var (country, days) in CarrierRateFile.ReadDeliveryDays(file))
        {
            narrator.Result(Invariant($"{country}: {days} days"));
        }

        narrator.Takeaway("An adapter keeps someone else's interface in one class, so the rest of the code speaks its own.");
    }
}
