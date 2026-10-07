using System.Globalization;
using System.Text;
using Microsoft.Extensions.Hosting;
using Patterns.Shop;

namespace Patterns.Behavioral.TemplateMethod.DotNet;

// Role: ConcreteClass — fills in the one step BackgroundService leaves open. The framework's StartAsync and
// StopAsync are the template: they start ExecuteAsync on a background task, cancel it and wait for it.
// Guide: §6.10
public sealed class NightlyExportService(IEnumerable<Order> orders, TextWriter output) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        output.Write(OrderCsv.Export(orders)); // export once, then finish
        return Task.CompletedTask;
    }
}

/// <summary>This level's own CSV writer (levels never share the pattern's types): the same rules as the others.</summary>
public static class OrderCsv
{
    public static string Export(IEnumerable<Order> orders)
    {
        var csv = new StringBuilder("id,customer,status,total\n");
        foreach (var order in orders.Where(o => o.Status != OrderStatus.Draft)
                                    .OrderByDescending(o => o.Total).ThenBy(o => o.Id))
        {
            csv.Append(CultureInfo.InvariantCulture, $"{order.Id},{order.Customer.Name},{order.Status},{order.Total:0.00}\n");
        }
        return csv.ToString();
    }
}
