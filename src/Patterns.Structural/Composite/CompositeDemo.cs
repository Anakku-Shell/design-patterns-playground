using Microsoft.Extensions.Configuration;
using Patterns.Demo;
using Patterns.Shop;
using Patterns.Structural.Composite.Classic;
using Patterns.Structural.Composite.DotNet;
using static System.FormattableString;

namespace Patterns.Structural.Composite;

public sealed class CompositeDemo : IDemo
{
    public string Key => "composite";
    public string Name => "Composite";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§5.3";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("One CatalogEntry class for both; every operation checks \"product or children?\"");
        var audioEntry = new Problem.CatalogEntry();
        audioEntry.Children.Add(new Problem.CatalogEntry { Product = SampleData.Headphones });
        var kitEntry = new Problem.CatalogEntry();
        kitEntry.Children.Add(new Problem.CatalogEntry { Product = SampleData.Book });
        kitEntry.Children.Add(new Problem.CatalogEntry { Product = SampleData.Mug });
        kitEntry.Children.Add(audioEntry);
        narrator.Result(Invariant($"Starter kit: {Problem.CatalogEntry.PriceOf(kitEntry):0.00}, {Problem.CatalogEntry.ProductCountOf(kitEntry)} products"));

        narrator.Level(1, "Classic");
        narrator.Step("Starter kit = [Book, Mug, Audio = [Headphones]]; the client just asks kit.Price");
        var audio = new Bundle("Audio");
        audio.Add(new ProductItem(SampleData.Headphones));
        var kit = new Bundle("Starter kit");
        kit.Add(new ProductItem(SampleData.Book));
        kit.Add(new ProductItem(SampleData.Mug));
        kit.Add(audio);
        narrator.Result(Invariant($"{kit.Name}: {kit.Price:0.00}, {kit.ProductCount} products"));
        narrator.Step("audio.Add(kit): the kit already contains audio");
        try
        {
            audio.Add(kit);
        }
        catch (InvalidOperationException ex)
        {
            narrator.Result($"refused: {ex.Message}");
        }

        narrator.Level(2, ".NET");
        narrator.Step("IConfiguration is a tree of sections: read the children of Shipping:Carriers");
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Shipping:Carriers:Fast:Days"] = "1",
                ["Shipping:Carriers:Slow:Days"] = "5",
            })
            .Build();
        foreach (var (carrier, days) in CarrierSettings.Read(configuration))
        {
            narrator.Result(Invariant($"{carrier}: Days = {days}"));
        }

        narrator.Takeaway("A group answers like a single item by asking its children, so clients never check which one they hold.");
    }
}
