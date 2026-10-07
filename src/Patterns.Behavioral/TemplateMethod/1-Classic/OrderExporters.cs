using System.Globalization;
using System.Text;
using Patterns.Shop;

namespace Patterns.Behavioral.TemplateMethod.Classic;

// Role: AbstractClass — the export algorithm, with the format left to subclasses.
// Guide: §6.10
public abstract class OrderExporter
{
    // The template method: not virtual, so every exporter follows the same rules.
    public string Export(IEnumerable<Order> orders)
    {
        var selected = orders.Where(o => o.Status != OrderStatus.Draft)
                             .OrderByDescending(o => o.Total).ThenBy(o => o.Id)
                             .ToList();
        var text = new StringBuilder(Header());
        for (var i = 0; i < selected.Count; i++)
        {
            text.Append(Row(selected[i], isLast: i == selected.Count - 1));
        }
        return text.Append(Footer()).ToString();
    }

    protected abstract string Header();

    protected abstract string Row(Order order, bool isLast);

    protected virtual string Footer() => ""; // a hook: optional, empty by default
}

// Role: ConcreteClass — the CSV format.
public sealed class CsvOrderExporter : OrderExporter
{
    protected override string Header() => "id,customer,status,total\n";

    protected override string Row(Order order, bool isLast) => string.Create(CultureInfo.InvariantCulture,
        $"{order.Id},{order.Customer.Name},{order.Status},{order.Total:0.00}\n");
}

// Role: ConcreteClass — the JSON format (written by hand to keep the example small).
public sealed class JsonOrderExporter : OrderExporter
{
    protected override string Header() => "[";

    protected override string Row(Order order, bool isLast) => string.Create(CultureInfo.InvariantCulture,
        $"{{\"id\":\"{order.Id}\",\"total\":{order.Total:0.00}}}{(isLast ? "" : ",")}");

    protected override string Footer() => "]";
}
