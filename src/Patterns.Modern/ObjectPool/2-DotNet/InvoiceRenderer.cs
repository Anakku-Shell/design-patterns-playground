using System.Buffers;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Patterns.Shop;

namespace Patterns.Modern.ObjectPool.DotNet;

// Role: Client — the same rent/use/return shape over Microsoft.Extensions.ObjectPool. The pool comes from
// new DefaultObjectPoolProvider().CreateStringBuilderPool(): its policy clears builders and drops huge ones.
// Guide: §7.8
public sealed class InvoiceRenderer(ObjectPool<StringBuilder> builders)
{
    public string Render(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var text = builders.Get();
        try
        {
            text.Append(CultureInfo.InvariantCulture, $"Invoice {order.Id.ToString()[..8]}\n");
            foreach (var line in order.Lines)
            {
                text.Append(CultureInfo.InvariantCulture, $"{line.Product.Name} x{line.Quantity} {line.LineTotal:0.00}\n");
            }
            text.Append(CultureInfo.InvariantCulture, $"Total {order.Total:0.00}\n");
            return text.ToString();
        }
        finally
        {
            builders.Return(text);
        }
    }
}

// Role: Client of ArrayPool<T>.Shared, the BCL's array pool.
public static class InvoiceEncoder
{
    public static byte[] ToUtf8(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        var buffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(text.Length)); // may be LONGER
        try
        {
            var written = Encoding.UTF8.GetBytes(text, buffer);
            return buffer.AsSpan(0, written).ToArray(); // only what was written, never buffer.Length
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer, clearArray: true); // invoices contain personal data
        }
    }
}
