namespace Patterns.Creational.AbstractFactory.Classic;

// Role: AbstractFactory — creates the three objects of one payment provider, which must match.
// Guide: §4.3
public interface IPaymentProviderFactory
{
    IPaymentCharger CreateCharger();
    IRefunder CreateRefunder();
    IReceiptFormatter CreateReceiptFormatter();
}

// Role: AbstractProduct — takes the money and returns a transaction id.
public interface IPaymentCharger
{
    string Charge(decimal amount);
}

// Role: AbstractProduct — gives the money back; only for transactions of its own family.
public interface IRefunder
{
    string Refund(string transactionId);
}

// Role: AbstractProduct — the text of the receipt.
public interface IReceiptFormatter
{
    string Format(string transactionId, decimal amount);
}
