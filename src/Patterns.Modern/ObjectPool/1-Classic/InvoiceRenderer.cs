using System.Globalization;
using Patterns.Shop;

namespace Patterns.Modern.ObjectPool.Classic;

// Role: Client — rents a builder, uses it, and always returns it (finally).
// Guide: §7.8
public sealed class InvoiceRenderer(InvoiceBufferPool pool)
{
    public string Render(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var text = pool.Rent();
        try
        {
            text.Append(CultureInfo.InvariantCulture, $"Invoice {order.Id.ToString()[..8]}\n");
            foreach (var line in order.Lines)
            {
                text.Append(CultureInfo.InvariantCulture, $"{line.Product.Name} x{line.Quantity} {line.LineTotal:0.00}\n");
            }
            text.Append(CultureInfo.InvariantCulture, $"Total {order.Total:0.00}\n");
            return text.ToString(); // copy out before returning: the builder belongs to the pool again
        }
        finally
        {
            pool.Return(text);
        }
    }
}
