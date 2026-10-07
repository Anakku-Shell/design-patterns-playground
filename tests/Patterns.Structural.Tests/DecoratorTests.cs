using Patterns.Shop;
using Patterns.Structural.Decorator.Classic;
using Patterns.Structural.Decorator.DotNet;
using Xunit;
using Problem = Patterns.Structural.Decorator.Problem;

namespace Patterns.Structural.Tests;

public sealed class DecoratorTests
{
    private static readonly Order EightBooks = SampleData.OrderOf((SampleData.Book, 8)); // 100.00

    [Fact]
    public void NoRules_GivesTheTotal()
    {
        Assert.Equal(100.00m, new Problem.PriceCalculator().Price(EightBooks, applyVat: false, coupon: null));
        Assert.Equal(100.00m, new BasePrice().PriceOf(EightBooks));
    }

    [Fact]
    public void Vat_AddsTwentyOnePercent()
    {
        Assert.Equal(121.00m, new Problem.PriceCalculator().Price(EightBooks, applyVat: true, coupon: null));
        Assert.Equal(121.00m, new VatDecorator(new BasePrice(), 0.21m).PriceOf(EightBooks));
    }

    [Fact]
    public void Coupon_SubtractsItsAmount()
    {
        Assert.Equal(95.00m, new Problem.PriceCalculator().Price(EightBooks, applyVat: false, coupon: 5m));
        Assert.Equal(95.00m, new CouponDecorator(new BasePrice(), 5m).PriceOf(EightBooks));
    }

    [Fact]
    public void Order_Matters()
    {
        var vatThenCoupon = new CouponDecorator(new VatDecorator(new BasePrice(), 0.21m), 5m);
        var couponThenVat = new VatDecorator(new CouponDecorator(new BasePrice(), 5m), 0.21m);

        Assert.Equal(116.00m, vatThenCoupon.PriceOf(EightBooks));
        Assert.Equal(114.95m, couponThenVat.PriceOf(EightBooks));
        // The Problem level has one fixed order: VAT first.
        Assert.Equal(116.00m, new Problem.PriceCalculator().Price(EightBooks, applyVat: true, coupon: 5m));
    }

    [Fact]
    public void Vat_RoundsHalfAwayFromZero()
    {
        var oneBook = SampleData.OrderOf((SampleData.Book, 1)); // 12.50 × 1.21 = 15.125

        Assert.Equal(15.13m, new VatDecorator(new BasePrice(), 0.21m).PriceOf(oneBook)); // to even would give 15.12
        Assert.Equal(15.13m, new Problem.PriceCalculator().Price(oneBook, applyVat: true, coupon: null));
    }

    [Fact]
    public void Coupon_RoundsToCents()
    {
        // 100.00 − 2.335 = 97.665: rounded half away from zero to cents (to even would give 97.66).
        Assert.Equal(97.67m, new CouponDecorator(new BasePrice(), 2.335m).PriceOf(EightBooks));
    }

    [Fact]
    public void Coupon_NeverGoesBelowZero()
    {
        Assert.Equal(0.00m, new CouponDecorator(new BasePrice(), 500m).PriceOf(EightBooks));
        Assert.Equal(0.00m, new Problem.PriceCalculator().Price(EightBooks, applyVat: false, coupon: 500m));
    }

    [Fact]
    public async Task DotNet_HandlersRunInOrder_AndTheInnerSeesTheHeader()
    {
        var log = new List<string>();
        using var client = CarrierHttpClient.Create(() => "abc-123", log);

        using var response = await client.GetAsync(new Uri("https://carrier.example.com/quote"), TestContext.Current.CancellationToken);

        Assert.Equal("ok", await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        Assert.Equal(
            ["GET https://carrier.example.com/quote", "carrier saw X-Correlation-Id: abc-123"],
            log);
    }
}
