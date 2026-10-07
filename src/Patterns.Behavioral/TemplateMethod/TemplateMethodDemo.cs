using Patterns.Behavioral.TemplateMethod.Classic;
using Patterns.Behavioral.TemplateMethod.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.TemplateMethod;

public sealed class TemplateMethodDemo : IDemo
{
    public string Key => "template-method";
    public string Name => "Template Method";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§6.10";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        Order[] orders =
        [
            // Fixed ids, so the demo prints the same text on every run.
            SampleData.OrderOf((SampleData.Book, 2)) with { Id = new Guid("00000000-0000-0000-0000-000000000001"), Status = OrderStatus.Placed },
            SampleData.OrderOf((SampleData.Headphones, 1)) with { Id = new Guid("00000000-0000-0000-0000-000000000002"), Status = OrderStatus.Paid },
            SampleData.OrderOf((SampleData.Mug, 1)) with { Id = new Guid("00000000-0000-0000-0000-000000000003") }, // a draft: skipped by every exporter
        ];

        narrator.Level(0, "Problem");
        narrator.Step("CsvExporter and JsonExporter each repeat the filter, the sort and the loop");
        Print(narrator, new Problem.JsonExporter().Export(orders));

        narrator.Level(1, "Classic");
        narrator.Step("OrderExporter.Export is the template; CsvOrderExporter fills in Header and Row");
        Print(narrator, new CsvOrderExporter().Export(orders));
        narrator.Step("JsonOrderExporter also overrides the Footer hook");
        Print(narrator, new JsonOrderExporter().Export(orders));

        narrator.Level(2, ".NET");
        narrator.Step("NightlyExportService : BackgroundService fills in ExecuteAsync; StartAsync runs it");
        using var file = new StringWriter();
        using (var service = new NightlyExportService(orders, file))
        {
            // IDemo.Run is synchronous; a console demo can wait for the result.
            service.StartAsync(CancellationToken.None).GetAwaiter().GetResult();
            service.ExecuteTask!.GetAwaiter().GetResult();
            service.StopAsync(CancellationToken.None).GetAwaiter().GetResult();
        }
        Print(narrator, file.ToString());

        narrator.Takeaway("The base class owns the steps and calls the parts that vary: \"don't call us, we'll call you\".");
    }

    private static void Print(Narrator narrator, string text)
    {
        foreach (var line in text.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            narrator.Result(line);
        }
    }
}
