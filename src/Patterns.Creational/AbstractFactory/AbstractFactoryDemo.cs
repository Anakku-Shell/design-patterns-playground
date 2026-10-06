using Microsoft.Extensions.DependencyInjection;
using Patterns.Creational.AbstractFactory.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Creational.AbstractFactory;

public sealed class AbstractFactoryDemo : IDemo
{
    public string Key => "abstract-factory";
    public string Name => "Abstract Factory";
    public PatternCategory Category => PatternCategory.Creational;
    public Relevance Relevance => Relevance.Niche;
    public string GuideSection => "§4.3";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        var twoBooks = SampleData.OrderOf((SampleData.Book, 2));
        narrator.Title(this);

        narrator.Level(1, "Classic");
        narrator.Step("Pay for two books with the card family, then with the wallet family");
        narrator.Result(new Classic.CheckoutPayment(new Classic.CardProviderFactory()).Pay(twoBooks));
        narrator.Result(new Classic.CheckoutPayment(new Classic.WalletProviderFactory()).Pay(twoBooks));
        narrator.Step("Refund a wallet transaction with the card refunder");
        try
        {
            new Classic.CardProviderFactory().CreateRefunder().Refund("WALLET-0001");
        }
        catch (ArgumentException ex)
        {
            narrator.Result($"refused: {ex.Message}");
        }

        narrator.Level(2, ".NET");
        narrator.Step("Both families registered as keyed services; the key chooses the whole family");
        using var provider = new ServiceCollection().AddPaymentProviders().BuildServiceProvider();
        foreach (var key in new[] { "card", "wallet" })
        {
            var family = provider.GetRequiredKeyedService<IPaymentProviderFactory>(key);
            narrator.Result($"{key}: {new CheckoutPayment(family).Pay(twoBooks)}");
        }

        narrator.Takeaway("Choosing the factory once chooses every product, so a card charger never meets a wallet receipt.");
    }
}
