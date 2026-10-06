using Microsoft.Extensions.DependencyInjection;
using Patterns.Creational.AbstractFactory.Classic;
using Patterns.Shop;
using Xunit;
using DotNet = Patterns.Creational.AbstractFactory.DotNet;

namespace Patterns.Creational.Tests;

public sealed class AbstractFactoryTests
{
    private static readonly Order TwoBooks = SampleData.OrderOf((SampleData.Book, 2));

    [Fact]
    public void CardFamily_ProducesACardReceipt()
    {
        Assert.Equal("Card payment CARD-0001: 25.00", new CheckoutPayment(new CardProviderFactory()).Pay(TwoBooks));
    }

    [Fact]
    public void WalletFamily_ProducesAWalletReceipt()
    {
        Assert.Equal("Wallet payment WALLET-0001: 25.00", new CheckoutPayment(new WalletProviderFactory()).Pay(TwoBooks));
    }

    [Fact]
    public void Charger_NumbersItsTransactions()
    {
        var checkout = new CheckoutPayment(new CardProviderFactory());

        checkout.Pay(TwoBooks);

        Assert.Equal("Card payment CARD-0002: 25.00", checkout.Pay(TwoBooks));
    }

    [Fact]
    public void Refunder_RefundsItsOwnFamily()
    {
        Assert.Equal("Refunded CARD-0001", new CardProviderFactory().CreateRefunder().Refund("CARD-0001"));
        Assert.Equal("Refunded WALLET-0001", new WalletProviderFactory().CreateRefunder().Refund("WALLET-0001"));
    }

    [Fact]
    public void Refunder_RejectsTransactionOfAnotherFamily()
    {
        Assert.Equal("Transaction WALLET-0001 was not made by card.",
            Assert.Throws<ArgumentException>(() => new CardProviderFactory().CreateRefunder().Refund("WALLET-0001")).Message);
        Assert.Equal("Transaction CARD-0001 was not made with the wallet.",
            Assert.Throws<ArgumentException>(() => new WalletProviderFactory().CreateRefunder().Refund("CARD-0001")).Message);
    }

    [Theory]
    [InlineData("card", "Card payment CARD-0001: 25.00")]
    [InlineData("wallet", "Wallet payment WALLET-0001: 25.00")]
    public void DotNet_KeyedFamily_IsResolved(string key, string receipt)
    {
        using var provider = AddProviders();

        var family = provider.GetRequiredKeyedService<DotNet.IPaymentProviderFactory>(key);

        Assert.Equal(receipt, new DotNet.CheckoutPayment(family).Pay(TwoBooks));
    }

    [Fact]
    public void DotNet_Refunder_AcceptsItsFamily_AndRejectsTheOther()
    {
        using var provider = AddProviders();
        var card = provider.GetRequiredKeyedService<DotNet.IPaymentProviderFactory>("card").CreateRefunder();

        Assert.Equal("Refunded CARD-0001", card.Refund("CARD-0001"));
        Assert.Equal("Transaction CARDS-0001 does not belong to CARD.",
            Assert.Throws<ArgumentException>(() => card.Refund("CARDS-0001")).Message);
    }

    private static ServiceProvider AddProviders() =>
        DotNet.PaymentProviderServiceCollectionExtensions.AddPaymentProviders(new ServiceCollection()).BuildServiceProvider();
}
