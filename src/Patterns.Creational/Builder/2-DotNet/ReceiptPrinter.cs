using System.Globalization;
using System.Text;
using Patterns.Shop;

namespace Patterns.Creational.Builder.DotNet;

// Role: Director — drives .NET's StringBuilder, the builder of strings.
// Guide: §4.4
public static class ReceiptPrinter
{
    public static string Print(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // Strings are immutable: "a + b" in a loop copies everything each time. StringBuilder appends into
        // a growing buffer and creates the final string once, in ToString().
        var receipt = new StringBuilder();
        receipt.AppendLine(CultureInfo.InvariantCulture, $"Order for {order.Customer.Name}");
        foreach (var line in order.Lines)
        {
            receipt.AppendLine(CultureInfo.InvariantCulture,
                $"{line.Quantity} x {line.Product.Name} @ {line.Product.Price:0.00} = {line.LineTotal:0.00}");
        }
        receipt.Append(CultureInfo.InvariantCulture, $"Total: {order.Total:0.00}");
        return receipt.ToString();
    }
}
