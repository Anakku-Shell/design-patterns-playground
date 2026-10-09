using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Patterns.Shop;

namespace Patterns.Creational.AbstractFactory.DotNet;

// The same roles as the Classic level, written compactly: what this level adds is how the family is chosen
// (by key, in the container). Guide: §4.3

// Role: AbstractFactory — creates the matching charger, refunder and receipt formatter of one provider.
public interface IPaymentProviderFactory
{
    IPaymentCharger CreateCharger();
    IRefunder CreateRefunder();
    IReceiptFormatter CreateReceiptFormatter();
}

// Role: AbstractProduct — takes the money, returns a transaction id.
public interface IPaymentCharger
{
    string Charge(decimal amount);
}

// Role: AbstractProduct — gives the money back, for its own family only.
public interface IRefunder
{
    string Refund(string transactionId);
}

// Role: AbstractProduct — the receipt text.
public interface IReceiptFormatter
{
    string Format(string transactionId, decimal amount);
}

// Role: ConcreteFactory — one class per family; its products only differ in the prefix and the receipt label.
public sealed class CardProviderFactory : IPaymentProviderFactory
{
    public IPaymentCharger CreateCharger() => new PrefixCharger("CARD");
    public IRefunder CreateRefunder() => new PrefixRefunder("CARD");
    public IReceiptFormatter CreateReceiptFormatter() => new LabelReceiptFormatter("Card payment");
}

// Role: ConcreteFactory — the wallet family.
public sealed class WalletProviderFactory : IPaymentProviderFactory
{
    public IPaymentCharger CreateCharger() => new PrefixCharger("WALLET");
    public IRefunder CreateRefunder() => new PrefixRefunder("WALLET");
    public IReceiptFormatter CreateReceiptFormatter() => new LabelReceiptFormatter("Wallet payment");
}

// Role: ConcreteProduct — the charger of either family, configured with its prefix.
internal sealed class PrefixCharger(string prefix) : IPaymentCharger
{
    private int _sequence;

    public string Charge(decimal amount) => string.Create(CultureInfo.InvariantCulture, $"{prefix}-{++_sequence:0000}");
}

// Role: ConcreteProduct — the refunder of either family; accepts only ids with its prefix.
internal sealed class PrefixRefunder(string prefix) : IRefunder
{
    public string Refund(string transactionId)
    {
        ArgumentNullException.ThrowIfNull(transactionId);
        return transactionId.StartsWith(prefix + "-", StringComparison.Ordinal)
            ? $"Refunded {transactionId}"
            : throw new ArgumentException($"Transaction {transactionId} does not belong to {prefix}.");
    }
}

// Role: ConcreteProduct — the receipt of either family, configured with its label.
internal sealed class LabelReceiptFormatter(string label) : IReceiptFormatter
{
    public string Format(string transactionId, decimal amount) =>
        string.Create(CultureInfo.InvariantCulture, $"{label} {transactionId}: {amount:0.00}");
}

// Role: Client — pays with whatever family it was given.
public sealed class CheckoutPayment(IPaymentProviderFactory provider)
{
    private readonly IPaymentCharger _charger = provider.CreateCharger();
    private readonly IReceiptFormatter _formatter = provider.CreateReceiptFormatter();

    public string Pay(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return _formatter.Format(_charger.Charge(order.Total), order.Total);
    }
}

// Guide: §4.3
public static class PaymentProviderServiceCollectionExtensions
{
    // The family is chosen once, by key ("card", "wallet"), where the factory is resolved.
    public static IServiceCollection AddPaymentProviders(this IServiceCollection services) =>
        services
            .AddKeyedSingleton<IPaymentProviderFactory, CardProviderFactory>("card")
            .AddKeyedSingleton<IPaymentProviderFactory, WalletProviderFactory>("wallet");
}
