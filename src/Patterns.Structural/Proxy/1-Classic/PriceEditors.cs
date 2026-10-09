using Patterns.Shop;
using Patterns.Structural.Proxy.Common;

namespace Patterns.Structural.Proxy.Classic;

// Role: Subject — changes prices.
// Guide: §5.7
public interface IPriceEditor
{
    void ChangePrice(Product product, decimal newPrice);
}

// Role: RealSubject — stores the new price. No security code at all: the proxy around it does that.
public sealed class PriceEditor : IPriceEditor
{
    private readonly Dictionary<Guid, decimal> _prices = [];

    public IReadOnlyDictionary<Guid, decimal> Prices => _prices;

    public void ChangePrice(Product product, decimal newPrice)
    {
        ArgumentNullException.ThrowIfNull(product);
        _prices[product.Id] = newPrice;
    }
}

// Role: Proxy (protection) — lets only administrators through to the real editor.
public sealed class AdminOnlyPriceEditor(IPriceEditor inner, User user) : IPriceEditor
{
    public void ChangePrice(Product product, decimal newPrice)
    {
        if (!user.IsAdmin) { throw new UnauthorizedAccessException("Only administrators can change prices."); }
        inner.ChangePrice(product, newPrice);
    }
}
