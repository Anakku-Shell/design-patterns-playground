using System.Globalization;
using Patterns.Shop;

namespace Patterns.Structural.Facade.Subsystems;

/// <summary>Books the carrier. A simulated subsystem, shared by every level. Guide: §5.5.</summary>
// Role: Subsystem — schedules the shipment.
// Guide: §5.5
public sealed class Shipping
{
    private int _scheduled;

    /// <summary>Returns the tracking number: TRK-0001, TRK-0002…</summary>
    public string Schedule(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _scheduled++;
        return string.Create(CultureInfo.InvariantCulture, $"TRK-{_scheduled:0000}");
    }
}
