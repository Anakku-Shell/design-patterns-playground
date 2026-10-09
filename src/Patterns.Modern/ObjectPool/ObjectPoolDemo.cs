using System.Buffers;
using Microsoft.Extensions.ObjectPool;
using Patterns.Demo;
using Patterns.Modern.ObjectPool.Classic;
using Patterns.Modern.ObjectPool.DotNet;
using Patterns.Shop;

namespace Patterns.Modern.ObjectPool;

public sealed class ObjectPoolDemo : IDemo
{
    public string Key => "object-pool";
    public string Name => "Object Pool";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Niche;
    public string GuideSection => "§7.8";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        // A fixed id, so the demo prints the same text on every run.
        var order = SampleData.OrderOf((SampleData.Book, 2), (SampleData.Headphones, 1)) with { Id = new Guid("1a2b3c4d-0000-0000-0000-000000000000") };

        narrator.Level(1, "Classic");
        narrator.Step("InvoiceRenderer rents a StringBuilder from InvoiceBufferPool(maxRetained: 2) and returns it in finally");
        var pool = new InvoiceBufferPool(maxRetained: 2);
        var renderer = new Classic.InvoiceRenderer(pool);
        foreach (var line in renderer.Render(order).Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            narrator.Result(line);
        }
        for (var i = 0; i < 999; i++)
        {
            renderer.Render(order);
        }
        narrator.Result($"1,000 invoices rendered, builders created: {pool.Created}");

        narrator.Level(2, ".NET");
        narrator.Step("new DefaultObjectPoolProvider().CreateStringBuilderPool(): Get, use, Return");
        var dotnet = new DotNet.InvoiceRenderer(new DefaultObjectPoolProvider().CreateStringBuilderPool());
        narrator.Result($"same invoice text: {dotnet.Render(order) == renderer.Render(order)}");
        narrator.Step("ArrayPool<byte>.Shared.Rent(100)");
        var buffer = ArrayPool<byte>.Shared.Rent(100);
        try
        {
            narrator.Result($"asked for 100 bytes, got an array of {buffer.Length}: track the length you use");
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
        narrator.Result($"InvoiceEncoder.ToUtf8(\"Invoice 0001\"): {InvoiceEncoder.ToUtf8("Invoice 0001").Length} bytes, only those written");

        narrator.Takeaway("Pool large or expensive objects on measured hot paths: reset them on return, bound the pool, never use one after returning it.");
    }
}
