using Patterns.Shop;

namespace Patterns.Modern.Result.Classic;

public sealed record Reservation(Guid ProductId, int Quantity);

// Role: Operation — its return type says it can fail, and how.
// Guide: §7.6
// Static, like int.TryParse: reserving needs no state here (CA1822).
public static class StockService
{
    public static Result<Reservation> Reserve(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity < 1)
        {
            return Result.Failure<Reservation>(new("quantity.invalid", "Quantity must be at least 1."));
        }
        return product.Stock < quantity
            ? Result.Failure<Reservation>(new("stock.insufficient", $"Only {product.Stock} left of {product.Name}."))
            : Result.Success(new Reservation(product.Id, quantity));
    }
}
