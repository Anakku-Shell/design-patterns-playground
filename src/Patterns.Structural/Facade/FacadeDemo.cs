using Patterns.Demo;
using Patterns.Shop;
using Patterns.Structural.Facade.Classic;
using Patterns.Structural.Facade.DotNet;
using Patterns.Structural.Facade.Subsystems;
using static System.FormattableString;

namespace Patterns.Structural.Facade;

public sealed class FacadeDemo : IDemo
{
    public string Key => "facade";
    public string Name => "Facade";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§5.5";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var twoBooks = SampleData.OrderOf((SampleData.Book, 2));

        narrator.Level(0, "Problem");
        narrator.Step("WebCheckout calls inventory, payments, shipping and mailer itself (MobileCheckout too)");
        var inventory = new Inventory(SampleData.Products);
        var (paymentId, tracking) = new Problem.WebCheckout(inventory, new PaymentGateway(), new Shipping(), new Mailer())
            .Place(twoBooks);
        narrator.Result(Invariant($"{paymentId}, {tracking}; book stock {inventory.StockOf(SampleData.Book.Id)}"));

        narrator.Level(1, "Classic");
        narrator.Step("CheckoutFacade.PlaceOrder(2 books): one call, the order of the steps inside");
        inventory = new Inventory(SampleData.Products);
        var payments = new PaymentGateway();
        var mailer = new Mailer();
        var facade = new CheckoutFacade(inventory, payments, new Shipping(), mailer);
        var receipt = facade.PlaceOrder(twoBooks);
        narrator.Result(Invariant($"{receipt.PaymentId}, {receipt.TrackingNumber}; book stock {inventory.StockOf(SampleData.Book.Id)}"));
        narrator.Result($"mailed: {mailer.Sent[^1]}");
        narrator.Step("Six headphones (five in stock)");
        var chargesBefore = payments.Charges;
        try
        {
            facade.PlaceOrder(SampleData.OrderOf((SampleData.Headphones, 6)));
        }
        catch (InvalidOperationException ex)
        {
            narrator.Result(Invariant($"refused: {ex.Message} New charges: {payments.Charges - chargesBefore}"));
        }

        narrator.Level(2, ".NET");
        narrator.Step("File.WriteAllText: one call hides a FileStream, a StreamWriter and their disposal");
        var path = Path.GetTempFileName();
        try
        {
            InvoiceFile.SaveSimple(path, "Invoice 0001: 25.00");
            narrator.Result($"read back: {InvoiceFile.ReadTheLongWay(path)}");
        }
        finally
        {
            File.Delete(path);
        }

        narrator.Takeaway("A facade gives clients one simple call and keeps the sequence of subsystem calls in one place.");
    }
}
