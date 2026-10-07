using Patterns.Shop;
using Patterns.Structural.Proxy.Common;
using Xunit;
using Classic = Patterns.Structural.Proxy.Classic;
using DotNet = Patterns.Structural.Proxy.DotNet;
using Problem = Patterns.Structural.Proxy.Problem;

namespace Patterns.Structural.Tests;

public sealed class ProxyTests
{
    private static readonly User Admin = new("Marta", IsAdmin: true);
    private static readonly User Clerk = new("Luis", IsAdmin: false);
    private const string OnlyAdmins = "Only administrators can change prices.";

    [Fact]
    public void Admin_ChangesThePrice_InProblemAndClassic()
    {
        var page = new Problem.PriceAdminPage(Admin);
        page.ChangePrice(SampleData.Book, 11.00m);

        var editor = new Classic.PriceEditor();
        new Classic.AdminOnlyPriceEditor(editor, Admin).ChangePrice(SampleData.Book, 11.00m);

        Assert.Equal(11.00m, page.Prices[SampleData.Book.Id]);
        Assert.Equal(11.00m, editor.Prices[SampleData.Book.Id]);
    }

    [Fact]
    public void NonAdmin_IsRefused_InProblemAndClassic()
    {
        var editor = new Classic.PriceEditor();

        Assert.Equal(OnlyAdmins, Assert.Throws<UnauthorizedAccessException>(
            () => new Problem.PriceAdminPage(Clerk).ChangePrice(SampleData.Book, 1m)).Message);
        Assert.Equal(OnlyAdmins, Assert.Throws<UnauthorizedAccessException>(
            () => new Classic.AdminOnlyPriceEditor(editor, Clerk).ChangePrice(SampleData.Book, 1m)).Message);
        Assert.Empty(editor.Prices);
    }

    [Fact]
    public void Problem_ReadsEveryImageUpFront()
    {
        var store = new ImageStore();

        _ = new Problem.ProductPage(store, ["book.jpg", "mug.jpg", "headphones.jpg"]);

        Assert.Equal(3, store.Reads);
    }

    [Fact]
    public void LazyProxy_DoesNotReadUntilUsed()
    {
        var store = new ImageStore();
        var image = new Classic.LazyImageProxy(store, "book.jpg");

        Assert.Equal(0, store.Reads);
        Assert.Equal(1024, image.Content.Length);
        Assert.Equal(1024, image.Content.Length);
        Assert.Equal(1, store.Reads);
    }

    [Fact]
    public void StoredImage_ReadsAtOnce()
    {
        var store = new ImageStore();

        _ = new Classic.StoredImage(store, "book.jpg");

        Assert.Equal(1, store.Reads);
    }

    [Fact]
    public void DotNet_Lazy_ReadsOnce()
    {
        var store = new ImageStore();
        var image = new DotNet.ProductImage(store, "book.jpg");

        Assert.Equal(0, store.Reads);
        Assert.Same(image.Content, image.Content);
        Assert.Equal(1, store.Reads);
    }

    [Fact]
    public void DotNet_DispatchProxy_LogsCalls()
    {
        var log = new List<string>();
        var real = new DotNet.PriceEditor();
        var editor = DotNet.LoggingProxy.Create<DotNet.IPriceEditor>(real, log);

        editor.ChangePrice(SampleData.Book, 11.00m);

        Assert.Equal(["ChangePrice called"], log);
        Assert.Equal(11.00m, real.Prices[SampleData.Book.Id]);
    }

    [Fact]
    public void DotNet_DispatchProxy_LetsTheTargetsOwnExceptionThrough()
    {
        var log = new List<string>();
        var editor = DotNet.LoggingProxy.Create<DotNet.IPriceEditor>(new DotNet.PriceEditor(), log);

        // The exact type, not a TargetInvocationException wrapping it.
        Assert.Throws<ArgumentNullException>(() => editor.ChangePrice(null!, 1m));
        Assert.Equal(["ChangePrice called"], log);
    }
}
