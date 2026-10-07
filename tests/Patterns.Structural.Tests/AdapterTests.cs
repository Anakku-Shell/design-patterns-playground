using System.Globalization;
using System.Text;
using Patterns.Shop;
using Patterns.Structural.Adapter.External;
using Xunit;
using Classic = Patterns.Structural.Adapter.Classic;
using DotNet = Patterns.Structural.Adapter.DotNet;
using Problem = Patterns.Structural.Adapter.Problem;

namespace Patterns.Structural.Tests;

public sealed class AdapterTests
{
    private static readonly Order TwoBooksToMadrid = SampleData.OrderOf((SampleData.Book, 2));

    [Fact]
    public void ProblemAndClassic_QuoteTheSameShipping()
    {
        var carrier = new LegacyCarrierClient();

        Assert.Equal("Shipping 4.00 in 2 days", new Problem.CheckoutSummary(carrier).Describe(TwoBooksToMadrid));
        Assert.Equal(2, new Problem.OrderTracking(carrier).DeliveryDays(TwoBooksToMadrid));
        Assert.Equal(new Classic.ShippingQuote(4.00m, 2), new Classic.LegacyCarrierAdapter(carrier).Quote(TwoBooksToMadrid));
        Assert.Equal("Shipping 4.00 in 2 days",
            new Classic.CheckoutSummary(new Classic.LegacyCarrierAdapter(carrier)).Describe(TwoBooksToMadrid));
    }

    [Fact]
    public void Lisbon_TakesFourDays()
    {
        var toLisbon = TwoBooksToMadrid with { ShippingAddress = SampleData.Lisbon };

        Assert.Equal(new Classic.ShippingQuote(4.00m, 4), new Classic.LegacyCarrierAdapter(new LegacyCarrierClient()).Quote(toLisbon));
        Assert.Equal("Shipping 4.00 in 4 days", new Problem.CheckoutSummary(new LegacyCarrierClient()).Describe(toLisbon));
    }

    [Fact]
    public void Adapter_ParsesWithInvariantCulture()
    {
        var carrier = new LegacyCarrierClient();

        var quote = new Classic.LegacyCarrierAdapter(carrier).Quote(TwoBooksToMadrid);

        Assert.Equal("ES;2;2500", carrier.LastPayload); // 25.00 sent as 2500 cents
        Assert.Equal(4.00m, quote.Price);                // read back from "PRICE=4.00"
    }

    [Fact]
    public void Adapter_IgnoresTheCurrentCulture()
    {
        var carrier = new LegacyCarrierClient();

        var quote = WithDecimalComma(() => new Classic.LegacyCarrierAdapter(carrier).Quote(TwoBooksToMadrid));
        var summary = WithDecimalComma(() => new Problem.CheckoutSummary(carrier).Describe(TwoBooksToMadrid));

        Assert.Equal(new Classic.ShippingQuote(4.00m, 2), quote); // not 400: "." is not read as a group separator
        Assert.Equal("Shipping 4.00 in 2 days", summary);
    }

    [Fact]
    public void Carrier_RejectsAMalformedPayload()
    {
        Assert.Throws<FormatException>(() => new LegacyCarrierClient().RequestQuote("ES;two;2500"));
    }

    [Fact]
    public void RateFile_IsReadThroughStreamReader()
    {
        using var file = new MemoryStream(Encoding.UTF8.GetBytes("ES;2\nPT;4"));

        var days = DotNet.CarrierRateFile.ReadDeliveryDays(file);

        Assert.Equal(new Dictionary<string, int> { ["ES"] = 2, ["PT"] = 4 }, days);
    }

    // The repository runs with invariant globalization, so the current culture is always invariant and a
    // missing CultureInfo.InvariantCulture would go unnoticed. A cloned culture with a decimal comma (like
    // es-ES) still works in that mode and makes the mistake visible.
    private static T WithDecimalComma<T>(Func<T> action)
    {
        var previous = CultureInfo.CurrentCulture;
        var comma = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        comma.NumberFormat.NumberDecimalSeparator = ",";
        comma.NumberFormat.NumberGroupSeparator = ".";
        CultureInfo.CurrentCulture = comma;
        try
        {
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }
}
