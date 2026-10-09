using Microsoft.AspNetCore.Http;
using Patterns.Behavioral.ChainOfResponsibility.Classic;
using Patterns.Behavioral.ChainOfResponsibility.DotNet;
using Patterns.Shop;
using Xunit;
using Problem = Patterns.Behavioral.ChainOfResponsibility.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class ChainOfResponsibilityTests
{
    private static readonly Address NewYork = new("US", "New York", "10001");

    private static Order OrderFor(string name) => name switch
    {
        "empty" => SampleData.OrderOf(),
        "too many units" => SampleData.OrderOf((SampleData.Book, 11)),
        "out of stock" => SampleData.OrderOf((SampleData.Mug, 1)),
        "to the US" => SampleData.OrderOf((SampleData.Book, 2)) with { ShippingAddress = NewYork },
        "valid" => SampleData.OrderOf((SampleData.Book, 2)),
        "too many units and out of stock" => SampleData.OrderOf((SampleData.Book, 21)),
        "out of stock and to the US" => SampleData.OrderOf((SampleData.Mug, 1)) with { ShippingAddress = NewYork },
        _ => throw new ArgumentOutOfRangeException(nameof(name)),
    };

    private static OrderCheck CheckWithTheClassicChain(Order order)
    {
        var rules = new NotEmptyRule();
        rules.SetNext(new MaxQuantityRule()).SetNext(new StockRule()).SetNext(new ShippingCountryRule());
        return rules.Check(order);
    }

    [Theory]
    [InlineData("empty", false, "Order has no lines.")]
    [InlineData("too many units", false, "At most 10 units per product.")]
    [InlineData("out of stock", false, "Not enough stock for Coffee Mug.")]
    [InlineData("to the US", false, "We do not ship to US.")]
    [InlineData("valid", true, null)]
    // Two adjacent rules broken at once: the earlier one answers, which pins the order of all four.
    [InlineData("too many units and out of stock", false, "At most 10 units per product.")]
    [InlineData("out of stock and to the US", false, "Not enough stock for Coffee Mug.")]
    public void BothLevels_GiveTheSameCheck(string name, bool isValid, string? error)
    {
        var order = OrderFor(name);

        var problem = new Problem.OrderValidator().Validate(order);
        var classic = CheckWithTheClassicChain(order);

        Assert.Equal((isValid, error), (problem.IsValid, problem.Error));
        Assert.Equal((isValid, error), (classic.IsValid, classic.Error));
    }

    [Fact]
    public void FirstFailingRuleWins()
    {
        // Empty and to the US: two rules fail, the first one in the chain answers.
        var order = SampleData.OrderOf() with { ShippingAddress = NewYork };

        Assert.Equal("Order has no lines.", new Problem.OrderValidator().Validate(order).Error);
        Assert.Equal("Order has no lines.", CheckWithTheClassicChain(order).Error);
    }

    [Fact]
    public void Classic_TheOrderOfTheChainDecidesTheMessage()
    {
        var order = SampleData.OrderOf() with { ShippingAddress = NewYork };
        var countryFirst = new ShippingCountryRule();
        countryFirst.SetNext(new NotEmptyRule());

        Assert.Equal("We do not ship to US.", countryFirst.Check(order).Error);
    }

    [Fact]
    public async Task DotNet_MissingHeader_ShortCircuitsWith401()
    {
        var trace = new List<string>();
        var context = new DefaultHttpContext();

        await OrderPipeline.Build(trace)(context);

        Assert.Equal(["correlation", "auth"], trace);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }

    [Fact]
    public async Task DotNet_ValidRequest_RunsEveryMiddlewareInOrder()
    {
        var trace = new List<string>();
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Customer-Id"] = "ana";
        context.Response.Body = new MemoryStream();

        await OrderPipeline.Build(trace)(context);

        Assert.Equal(["correlation", "auth", "log", "endpoint"], trace);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        Assert.Equal("abc-123", context.Response.Headers["X-Correlation-Id"]);
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        Assert.Equal("order accepted", await reader.ReadToEndAsync(TestContext.Current.CancellationToken));
    }
}
