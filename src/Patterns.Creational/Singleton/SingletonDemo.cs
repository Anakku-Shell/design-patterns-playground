using Microsoft.Extensions.DependencyInjection;
using Patterns.Creational.Singleton.DotNet;
using Patterns.Demo;
using static System.FormattableString;

namespace Patterns.Creational.Singleton;

public sealed class SingletonDemo : IDemo
{
    public string Key => "singleton";
    public string Name => "Singleton";
    public PatternCategory Category => PatternCategory.Creational;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§4.1";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("Read Spain's VAT rate from a public static dictionary");
        narrator.Result(Invariant($"ES {Problem.VatRates.RateFor("ES"):0.00}"));
        narrator.Step("Any code can overwrite it: VatRates.Rates[\"ES\"] = 0.99 (and put it back)");
        try
        {
            Problem.VatRates.Rates["ES"] = 0.99m;
            narrator.Result(Invariant($"ES {Problem.VatRates.RateFor("ES"):0.00} for everybody"));
        }
        finally
        {
            Problem.VatRates.Rates["ES"] = 0.21m; // global state: whoever changes it must remember to restore it
        }

        narrator.Level(1, "Classic");
        narrator.Step("A private constructor and a static Lazy<T> instance");
        narrator.Result(Invariant($"ES {Classic.VatRateTable.Instance.RateFor("ES"):0.00}; same object every time: {ReferenceEquals(Classic.VatRateTable.Instance, Classic.VatRateTable.Instance)}"));

        narrator.Level(2, ".NET");
        narrator.Step("An ordinary class registered with AddSingleton");
        using var first = new ServiceCollection().AddVatRates().BuildServiceProvider();
        using var second = new ServiceCollection().AddVatRates().BuildServiceProvider();
        var table = first.GetRequiredService<VatRateTable>();
        narrator.Result(Invariant($"ES {table.RateFor("ES"):0.00}; same object in one container: {ReferenceEquals(table, first.GetRequiredService<VatRateTable>())}"));
        narrator.Step("Two containers, as two tests would build");
        narrator.Result(Invariant($"same object across containers: {ReferenceEquals(table, second.GetRequiredService<VatRateTable>())}"));

        narrator.Takeaway("A DI singleton is one instance per container, received in the constructor: shared, but visible and replaceable.");
    }
}
