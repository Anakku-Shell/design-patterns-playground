using System.Globalization;
using System.Text;
using Patterns.Shop;

namespace Patterns.Behavioral.TemplateMethod.Problem;

// Guide: §6.10
public sealed class CsvExporter
{
    public string Export(IEnumerable<Order> orders)
    {
        // PAIN: the filter and sort rules are copied in JsonExporter. Changing them (say, also skip
        // cancelled orders) in one exporter and forgetting the other is a matter of time.
        var selected = orders.Where(o => o.Status != OrderStatus.Draft)
                             .OrderByDescending(o => o.Total).ThenBy(o => o.Id);
        var csv = new StringBuilder("id,customer,status,total\n");
        foreach (var order in selected)
        {
            csv.Append(CultureInfo.InvariantCulture, $"{order.Id},{order.Customer.Name},{order.Status},{order.Total:0.00}\n");
        }
        return csv.ToString();
    }
}

public sealed class JsonExporter
{
    public string Export(IEnumerable<Order> orders)
    {
        // PAIN: the same selection as CsvExporter, typed a second time.
        var selected = orders.Where(o => o.Status != OrderStatus.Draft)
                             .OrderByDescending(o => o.Total).ThenBy(o => o.Id)
                             .ToList();
        var json = new StringBuilder("[");
        for (var i = 0; i < selected.Count; i++)
        {
            if (i > 0) { json.Append(','); }
            json.Append(CultureInfo.InvariantCulture, $"{{\"id\":\"{selected[i].Id}\",\"total\":{selected[i].Total:0.00}}}");
        }
        return json.Append(']').ToString();
    }
}
