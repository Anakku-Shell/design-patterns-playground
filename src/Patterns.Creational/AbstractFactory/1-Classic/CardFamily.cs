using System.Globalization;

namespace Patterns.Creational.AbstractFactory.Classic;

// Role: ConcreteFactory — the card family. Everything it creates speaks "CARD".
// Guide: §4.3
public sealed class CardProviderFactory : IPaymentProviderFactory
{
    public IPaymentCharger CreateCharger() => new CardCharger();
    public IRefunder CreateRefunder() => new CardRefunder();
    public IReceiptFormatter CreateReceiptFormatter() => new CardReceiptFormatter();
}

// Role: ConcreteProduct — the card charger; numbers its transactions CARD-0001, CARD-0002…
public sealed class CardCharger : IPaymentCharger
{
    private int _sequence;

    public string Charge(decimal amount) => string.Create(CultureInfo.InvariantCulture, $"CARD-{++_sequence:0000}");
}

// Role: ConcreteProduct — the card refunder; refuses transactions made by another provider.
public sealed class CardRefunder : IRefunder
{
    public string Refund(string transactionId)
    {
        ArgumentNullException.ThrowIfNull(transactionId);
        return transactionId.StartsWith("CARD-", StringComparison.Ordinal)
            ? $"Refunded {transactionId}"
            // No paramName: it would append " (Parameter '…')" to the message.
            : throw new ArgumentException($"Transaction {transactionId} was not made by card.");
    }
}

// Role: ConcreteProduct — the card receipt, in invariant culture: 25.00, never 25,00.
public sealed class CardReceiptFormatter : IReceiptFormatter
{
    public string Format(string transactionId, decimal amount) =>
        string.Create(CultureInfo.InvariantCulture, $"Card payment {transactionId}: {amount:0.00}");
}
