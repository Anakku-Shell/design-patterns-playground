using Patterns.Demo;
using Patterns.Shop;
using Patterns.Structural.Proxy.Classic;
using Patterns.Structural.Proxy.Common;
using static System.FormattableString;

namespace Patterns.Structural.Proxy;

public sealed class ProxyDemo : IDemo
{
    public string Key => "proxy";
    public string Name => "Proxy";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§5.7";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        string[] files = ["book.jpg", "headphones.jpg", "mug.jpg"];
        var clerk = new User("Luis", IsAdmin: false);

        narrator.Level(0, "Problem");
        narrator.Step("A product page with three images; the visitor looks at none");
        var store = new ImageStore();
        _ = new Problem.ProductPage(store, files);
        narrator.Result(Invariant($"images read: {store.Reads}"));

        narrator.Level(1, "Classic");
        narrator.Step("The same three images as LazyImageProxy; the visitor opens one, twice");
        store = new ImageStore();
        var images = files.Select(file => new LazyImageProxy(store, file)).ToList();
        _ = images[0].Content;
        _ = images[0].Content;
        narrator.Result(Invariant($"images read: {store.Reads}"));
        narrator.Step("A clerk changes a price through AdminOnlyPriceEditor");
        var editor = new PriceEditor();
        try
        {
            new AdminOnlyPriceEditor(editor, clerk).ChangePrice(SampleData.Book, 1.00m);
        }
        catch (UnauthorizedAccessException ex)
        {
            narrator.Result(Invariant($"refused: {ex.Message} Prices changed: {editor.Prices.Count}"));
        }

        narrator.Level(2, ".NET");
        narrator.Step("Lazy<byte[]> reads on the first .Value only");
        store = new ImageStore();
        var image = new DotNet.ProductImage(store, "book.jpg");
        var before = store.Reads;
        _ = image.Content;
        _ = image.Content;
        narrator.Result(Invariant($"reads before: {before}; after reading Content twice: {store.Reads}"));
        narrator.Step("DispatchProxy generates a logging proxy for IPriceEditor at run time");
        var log = new List<string>();
        var logged = DotNet.LoggingProxy.Create<DotNet.IPriceEditor>(new DotNet.PriceEditor(), log);
        logged.ChangePrice(SampleData.Book, 11.00m);
        narrator.Result($"log: {string.Join(", ", log)}");

        narrator.Takeaway("A proxy has the real object's interface and decides when, or whether, the call gets through.");
    }
}
