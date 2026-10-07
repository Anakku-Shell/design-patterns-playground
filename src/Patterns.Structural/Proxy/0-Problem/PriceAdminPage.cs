using Patterns.Shop;
using Patterns.Structural.Proxy.Common;

namespace Patterns.Structural.Proxy.Problem;

// Guide: §5.7
public sealed class PriceAdminPage(User user)
{
    private readonly Dictionary<Guid, decimal> _prices = [];

    public IReadOnlyDictionary<Guid, decimal> Prices => _prices;

    public void ChangePrice(Product product, decimal newPrice)
    {
        ArgumentNullException.ThrowIfNull(product);

        // PAIN: the same check is copied into every method that changes something; one forgotten
        // copy is a security hole.
        if (!user.IsAdmin) { throw new UnauthorizedAccessException("Only administrators can change prices."); }
        _prices[product.Id] = newPrice;
    }

    public void ResetPrice(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        // PAIN: the second copy.
        if (!user.IsAdmin) { throw new UnauthorizedAccessException("Only administrators can change prices."); }
        _prices.Remove(product.Id);
    }
}
