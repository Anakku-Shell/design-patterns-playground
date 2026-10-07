using System.Buffers;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Patterns.Modern.ObjectPool.Classic;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.ObjectPool.Classic;
using DotNet = Patterns.Modern.ObjectPool.DotNet;

namespace Patterns.Modern.Tests;

public sealed class ObjectPoolTests
{
    private static readonly Order Order =
        SampleData.OrderOf((SampleData.Book, 2), (SampleData.Headphones, 1)) with { Id = new Guid("1a2b3c4d-0000-0000-0000-000000000000") };

    private const string Invoice = "Invoice 1a2b3c4d\nClean Code x2 25.00\nWireless Headphones x1 59.90\nTotal 84.90\n";

    [Fact]
    public void RentAfterReturn_ReusesTheSameEmptyObject()
    {
        var pool = new InvoiceBufferPool(maxRetained: 4);
        var first = pool.Rent();
        first.Append("Invoice 0001");
        pool.Return(first);

        var second = pool.Rent();

        Assert.Same(first, second);
        Assert.Equal(0, second.Length); // cleared: the next user must not see the previous invoice
        Assert.Equal(1, pool.Created);
    }

    [Fact]
    public void FullPool_DropsReturnedObjects()
    {
        var pool = new InvoiceBufferPool(maxRetained: 1);
        var a = pool.Rent();
        var b = pool.Rent();
        pool.Return(a);
        pool.Return(b); // the pool already holds one: b is left to the garbage collector

        Assert.Same(a, pool.Rent());
        Assert.NotSame(b, pool.Rent()); // empty again: a new one is created
        Assert.Equal(3, pool.Created);
    }

    [Fact]
    public void NegativeMaxRetained_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InvoiceBufferPool(maxRetained: -1));
    }

    [Fact]
    public void Classic_AndDotNet_RenderTheSameInvoice()
    {
        var classicPool = new InvoiceBufferPool(maxRetained: 2);
        var classic = new Classic.InvoiceRenderer(classicPool);
        var dotnet = new DotNet.InvoiceRenderer(new DefaultObjectPoolProvider().CreateStringBuilderPool());

        Assert.Equal(Invoice, classic.Render(Order));
        Assert.Equal(Invoice, classic.Render(Order)); // the second time on a reused, cleared builder
        Assert.Equal(Invoice, dotnet.Render(Order));
        Assert.Equal(1, classicPool.Created);
    }

    [Fact]
    public void DotNet_StringBuilderPool_ReusesAndClears()
    {
        var pool = new DefaultObjectPoolProvider().CreateStringBuilderPool();
        var first = pool.Get();
        first.Append("Invoice 0001");
        pool.Return(first);

        var second = pool.Get();

        Assert.Same(first, second);
        Assert.Equal(0, second.Length);
    }

    [Fact]
    public void ArrayPool_MayReturnALargerArray()
    {
        var buffer = ArrayPool<byte>.Shared.Rent(100);
        try
        {
            Assert.True(buffer.Length >= 100); // usually 128: the size buckets are powers of two
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    [Fact]
    public void DotNet_EncodedInvoice_HasOnlyTheBytesWritten()
    {
        // The rented buffer is longer than the text; the result must stop at what was written.
        var bytes = DotNet.InvoiceEncoder.ToUtf8("Invoice 0001");

        Assert.Equal(Encoding.UTF8.GetBytes("Invoice 0001"), bytes);
    }
}
