using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Patterns.Modern.NullObject.DotNet;

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

    public static NoDiscount Instance { get; } = new();

    public decimal Apply(decimal amount) => amount;
}

// Role: Client — the logger is optional; NullLogger<T>.Instance, the framework's null object, stands in for it.
public sealed partial class PriceService(IDiscount discount, ILogger<PriceService>? logger = null)
{
    private readonly ILogger<PriceService> _logger = logger ?? NullLogger<PriceService>.Instance;

    public decimal PriceOf(decimal amount)
    {
        var price = discount.Apply(amount);
        LogPriced(_logger, amount, price); // no null check
        return price;
    }

    // [LoggerMessage] generates a fast, allocation-free logging method at compile time (hence partial).
    [LoggerMessage(Level = LogLevel.Information, Message = "Priced {Amount} at {Price}")]
    private static partial void LogPriced(ILogger logger, decimal amount, decimal price);
}
