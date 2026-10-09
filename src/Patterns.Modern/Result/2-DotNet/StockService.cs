using System.Diagnostics.CodeAnalysis;
using Patterns.Shop;

namespace Patterns.Modern.Result.DotNet;

public sealed record Reservation(Guid ProductId, int Quantity);

// Role: Operation — the BCL's own result pattern, TryXxx (int.TryParse, Dictionary.TryGetValue): a bool says
// whether it worked, the out parameter carries the value. It cannot say why it failed.
// Guide: §7.6
public static class StockService
{
    public static bool TryReserve(Product product, int quantity, [NotNullWhen(true)] out Reservation? reservation)
    {
        ArgumentNullException.ThrowIfNull(product);
        reservation = quantity >= 1 && product.Stock >= quantity ? new Reservation(product.Id, quantity) : null;
        return reservation is not null;
    }
}
