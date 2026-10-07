using Patterns.Shop;
using Patterns.Structural.Facade.Classic;
using Patterns.Structural.Facade.DotNet;
using Patterns.Structural.Facade.Subsystems;
using Xunit;
using Problem = Patterns.Structural.Facade.Problem;

namespace Patterns.Structural.Tests;

public sealed class FacadeTests
{
    private static readonly Order TwoBooks = SampleData.OrderOf((SampleData.Book, 2));

    public static TheoryData<string> Checkouts => ["web", "mobile", "facade"];

    [Theory]
    [MemberData(nameof(Checkouts))]
    public void EveryCheckout_PlacesTheOrderTheSameWay(string checkout)
    {
        var shop = new Systems();

        var (paymentId, tracking) = checkout switch
        {
            "web" => new Problem.WebCheckout(shop.Inventory, shop.Payments, shop.Shipping, shop.Mailer).Place(TwoBooks),
            "mobile" => new Problem.MobileCheckout(shop.Inventory, shop.Payments, shop.Shipping, shop.Mailer).Place(TwoBooks),
            _ => Receipt(shop.Facade().PlaceOrder(TwoBooks)),
        };

        Assert.Equal(("PAY-0001", "TRK-0001"), (paymentId, tracking));
        Assert.Equal(18, shop.Inventory.StockOf(SampleData.Book.Id));
        Assert.Equal(["Order confirmed. Tracking number: TRK-0001."], shop.Mailer.Sent);
    }

    [Fact]
    public void Facade_ReturnsTheOrderId()
    {
        Assert.Equal(TwoBooks.Id, new Systems().Facade().PlaceOrder(TwoBooks).OrderId);
    }

    [Fact]
    public void NotEnoughStock_ChargesNothing()
    {
        var shop = new Systems();
        var sixHeadphones = SampleData.OrderOf((SampleData.Book, 1), (SampleData.Headphones, 6));

        var error = Assert.Throws<InvalidOperationException>(() => shop.Facade().PlaceOrder(sixHeadphones));

        Assert.Equal("Not enough stock for Wireless Headphones.", error.Message);
        Assert.Equal(0, shop.Payments.Charges);
        Assert.Equal(20, shop.Inventory.StockOf(SampleData.Book.Id)); // all-or-nothing: the book was not reserved
        Assert.Equal(5, shop.Inventory.StockOf(SampleData.Headphones.Id));
        Assert.Empty(shop.Mailer.Sent);
    }

    [Fact]
    public void DotNet_BothWaysWriteTheSameFile()
    {
        const string invoice = "Invoice 0001\nClean Code x 2 = 25.00\nTotal: 25.00";
        var simple = Path.GetTempFileName();
        var longWay = Path.GetTempFileName();
        try
        {
            InvoiceFile.SaveSimple(simple, invoice);
            InvoiceFile.SaveTheLongWay(longWay, invoice);

            Assert.Equal(File.ReadAllBytes(simple), File.ReadAllBytes(longWay));
            Assert.Equal(invoice, InvoiceFile.ReadSimple(longWay));
            Assert.Equal(invoice, InvoiceFile.ReadTheLongWay(simple));
        }
        finally
        {
            File.Delete(simple);
            File.Delete(longWay);
        }
    }

    private static (string, string) Receipt(CheckoutReceipt receipt) => (receipt.PaymentId, receipt.TrackingNumber);

    // Fresh subsystems for each test: stock and counters start from SampleData every time.
    private sealed class Systems
    {
        public Inventory Inventory { get; } = new(SampleData.Products);
        public PaymentGateway Payments { get; } = new();
        public Shipping Shipping { get; } = new();
        public Mailer Mailer { get; } = new();

        public CheckoutFacade Facade() => new(Inventory, Payments, Shipping, Mailer);
    }
}
