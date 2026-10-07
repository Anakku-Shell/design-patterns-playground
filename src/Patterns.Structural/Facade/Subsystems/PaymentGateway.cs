using System.Globalization;
using Patterns.Shop;

namespace Patterns.Structural.Facade.Subsystems;

/// <summary>Takes the money. A simulated subsystem, shared by every level. Guide: §5.5.</summary>
// Role: Subsystem — charges the customer.
// Guide: §5.5
public sealed class PaymentGateway
{
    public int Charges { get; private set; }

    /// <summary>Returns the payment id: PAY-0001, PAY-0002…</summary>
    public string Charge(Customer customer, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Charges++;
        return string.Create(CultureInfo.InvariantCulture, $"PAY-{Charges:0000}");
    }
}
