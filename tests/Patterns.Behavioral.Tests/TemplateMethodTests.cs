using Patterns.Behavioral.TemplateMethod.Classic;
using Patterns.Behavioral.TemplateMethod.DotNet;
using Patterns.Shop;
using Xunit;
using Problem = Patterns.Behavioral.TemplateMethod.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class TemplateMethodTests
{
    private static Order Fixed(int id, Product product, int quantity, OrderStatus status) =>
        new(new Guid($"00000000-0000-0000-0000-{id:000000000000}"), SampleData.Ana, [new OrderLine(product, quantity)], SampleData.Madrid, status);

    private static readonly Order A = Fixed(1, SampleData.Book, 2, OrderStatus.Placed);       // 25.00
    private static readonly Order B = Fixed(2, SampleData.Headphones, 1, OrderStatus.Paid);   // 59.90
    private static readonly Order C = Fixed(3, SampleData.Mug, 1, OrderStatus.Draft);         // 8.00, a draft

    private static readonly Order[] Orders = [A, C, B];

    private const string ExpectedCsv =
        "id,customer,status,total\n" +
        "00000000-0000-0000-0000-000000000002,Ana,Paid,59.90\n" +
        "00000000-0000-0000-0000-000000000001,Ana,Placed,25.00\n";

    private const string ExpectedJson =
        """[{"id":"00000000-0000-0000-0000-000000000002","total":59.90},{"id":"00000000-0000-0000-0000-000000000001","total":25.00}]""";

    [Fact]
    public void Csv_IsTheSameInBothLevels()
    {
        Assert.Equal(ExpectedCsv, new Problem.CsvExporter().Export(Orders));
        Assert.Equal(ExpectedCsv, new CsvOrderExporter().Export(Orders));
    }

    [Fact]
    public void Json_IsTheSameInBothLevels()
    {
        Assert.Equal(ExpectedJson, new Problem.JsonExporter().Export(Orders));
        Assert.Equal(ExpectedJson, new JsonOrderExporter().Export(Orders));
    }

    [Fact]
    public void Drafts_AreSkipped()
    {
        var onlyDraft = new[] { C };

        Assert.Equal("id,customer,status,total\n", new CsvOrderExporter().Export(onlyDraft));
        Assert.Equal("[]", new JsonOrderExporter().Export(onlyDraft));
        Assert.Equal("id,customer,status,total\n", new Problem.CsvExporter().Export(onlyDraft));
        Assert.Equal("[]", new Problem.JsonExporter().Export(onlyDraft));
    }

    [Fact]
    public void SameTotal_IsSortedById()
    {
        var second = Fixed(2, SampleData.Book, 2, OrderStatus.Placed);
        var first = Fixed(1, SampleData.Book, 2, OrderStatus.Paid);

        var csv = new CsvOrderExporter().Export([second, first]);

        Assert.Equal(
            "id,customer,status,total\n" +
            "00000000-0000-0000-0000-000000000001,Ana,Paid,25.00\n" +
            "00000000-0000-0000-0000-000000000002,Ana,Placed,25.00\n",
            csv);
        Assert.Equal(csv, new Problem.CsvExporter().Export([second, first]));
        Assert.Equal(csv, OrderCsv.Export([second, first]));
        var json = new JsonOrderExporter().Export([second, first]);
        Assert.StartsWith("""[{"id":"00000000-0000-0000-0000-000000000001""", json, StringComparison.Ordinal);
        Assert.Equal(json, new Problem.JsonExporter().Export([second, first]));
    }

    [Fact]
    public void Exports_IgnoreTheCurrentCulture()
    {
        var csv = CultureScope.WithDecimalComma(() => new CsvOrderExporter().Export(Orders));
        var json = CultureScope.WithDecimalComma(() => new JsonOrderExporter().Export(Orders));
        var problemCsv = CultureScope.WithDecimalComma(() => new Problem.CsvExporter().Export(Orders));
        var problemJson = CultureScope.WithDecimalComma(() => new Problem.JsonExporter().Export(Orders));
        var service = CultureScope.WithDecimalComma(() => OrderCsv.Export(Orders));

        Assert.Equal(ExpectedCsv, csv);
        Assert.Equal(ExpectedJson, json);
        Assert.Equal(ExpectedCsv, problemCsv);
        Assert.Equal(ExpectedJson, problemJson);
        Assert.Equal(ExpectedCsv, service);
    }

    [Fact]
    public async Task DotNet_ServiceRunsTheExportOnce()
    {
        using var output = new StringWriter();
        using var service = new NightlyExportService(Orders, output);
        var ct = TestContext.Current.CancellationToken;

        await service.StartAsync(ct);
        // .NET 10 runs ExecuteAsync entirely in the background: wait for it before checking.
        await service.ExecuteTask!;
        await service.StopAsync(ct);

        Assert.Equal(ExpectedCsv, output.ToString());
    }
}
