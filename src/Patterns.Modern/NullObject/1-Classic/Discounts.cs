namespace Patterns.Modern.NullObject.Classic;

// Role: AbstractObject — the interface the client uses.
// Guide: §7.7
public interface IDiscount
{
    decimal Apply(decimal amount);
}

// Role: RealObject — takes a percentage off, rounded to cents.
public sealed class PercentageDiscount(decimal percent) : IDiscount
{
    public decimal Apply(decimal amount) =>
        decimal.Round(amount * (100 - percent) / 100, 2, MidpointRounding.AwayFromZero);
}

// Role: NullObject — a discount that discounts nothing.
public sealed class NoDiscount : IDiscount
{
    private NoDiscount()
    {
    }

    public static NoDiscount Instance { get; } = new(); // stateless: one shared instance is enough

    public decimal Apply(decimal amount) => amount;
}

// Role: Client — always has a discount object, so it never checks for null.
public sealed class PriceService(IDiscount discount)
{
    public decimal PriceOf(decimal amount) => discount.Apply(amount);
}
