using Patterns.Shop;

namespace Patterns.Modern.Result.Problem;

public sealed record Reservation(Guid ProductId, int Quantity);

// Guide: §7.6
public sealed class StockService
{
    // PAIN: the signature says "returns a Reservation"; nothing says it can fail, and callers learn about
    // InsufficientStockException by reading the body or by crashing. An everyday outcome (not enough books)
    // travels as an exception, and callers write try/catch for ordinary control flow.
    public Reservation Reserve(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);
        return product.Stock >= quantity
            ? new Reservation(product.Id, quantity)
            : throw new InsufficientStockException(product, quantity);
    }
}

public sealed class InsufficientStockException : Exception
{
    public InsufficientStockException(Product product, int quantity)
        : base($"Only {product.Stock} left of {product.Name}.")
    {
        Requested = quantity;
    }

    public int Requested { get; }
}
