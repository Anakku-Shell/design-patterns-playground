using System.Globalization;

namespace Patterns.Creational.AbstractFactory.Classic;

// Role: ConcreteFactory — the wallet family. Everything it creates speaks "WALLET".
// Guide: §4.3
public sealed class WalletProviderFactory : IPaymentProviderFactory
{
    public IPaymentCharger CreateCharger() => new WalletCharger();
    public IRefunder CreateRefunder() => new WalletRefunder();
    public IReceiptFormatter CreateReceiptFormatter() => new WalletReceiptFormatter();
}

// Role: ConcreteProduct — the wallet charger; numbers its transactions WALLET-0001, WALLET-0002…
public sealed class WalletCharger : IPaymentCharger
{
    private int _sequence;

    public string Charge(decimal amount) => string.Create(CultureInfo.InvariantCulture, $"WALLET-{++_sequence:0000}");
}

// Role: ConcreteProduct — the wallet refunder; refuses transactions made by another provider.
public sealed class WalletRefunder : IRefunder
{
    public string Refund(string transactionId)
    {
        ArgumentNullException.ThrowIfNull(transactionId);
        return transactionId.StartsWith("WALLET-", StringComparison.Ordinal)
            ? $"Refunded {transactionId}"
            : throw new ArgumentException($"Transaction {transactionId} was not made with the wallet.");
    }
}

// Role: ConcreteProduct — the wallet receipt, in invariant culture.
public sealed class WalletReceiptFormatter : IReceiptFormatter
{
    public string Format(string transactionId, decimal amount) =>
        string.Create(CultureInfo.InvariantCulture, $"Wallet payment {transactionId}: {amount:0.00}");
}
