using Microsoft.Extensions.Logging;
using Patterns.Shop;

namespace Patterns.Modern.NullObject.Problem;

public interface IDiscount
{
    decimal Apply(decimal amount);
}

public sealed class PercentageDiscount(decimal percent) : IDiscount
{
    public decimal Apply(decimal amount) =>
        decimal.Round(amount * (100 - percent) / 100, 2, MidpointRounding.AwayFromZero);
}

// Guide: §7.7
public sealed partial class PriceService(IDiscount? discount, ILogger? logger)
{
    public decimal PriceOf(decimal amount)
    {
        // PAIN: a null check at every use. Forget one and it is a NullReferenceException, maybe only for the
        // orders without a discount, maybe only in the tool that runs without a logger.
        var price = discount is not null ? discount.Apply(amount) : amount;
        if (logger is not null)
        {
            LogPriced(logger, amount, price);
        }
        return price;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Priced {Amount} at {Price}")]
    private static partial void LogPriced(ILogger logger, decimal amount, decimal price);
}

public static class Greeter
{
    // PAIN: every place that shows the customer's name repeats this branch.
    public static string GreetingFor(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return customer.IsGuest ? "Hello, guest" : $"Hello, {customer.Name}";
    }
}
