using System.Transactions;
using Patterns.Demo;
using Patterns.Modern.UnitOfWork.DotNet;
using Patterns.Shop;

namespace Patterns.Modern.UnitOfWork;

public sealed class UnitOfWorkDemo : IDemo
{
    public string Key => "unit-of-work";
    public string Name => "Unit of Work";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§7.4";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var withAMug = SampleData.OrderOf((SampleData.Book, 2), (SampleData.Mug, 1)); // the mug is out of stock
        var fine = SampleData.OrderOf((SampleData.Book, 2), (SampleData.Headphones, 1));

        narrator.Level(0, "Problem");
        narrator.Step("confirm 2 books and 1 mug: OrderRepository.Save, then StockRepository.Decrease per line, each writing at once");
        var problemShop = new Problem.InMemoryShop(SampleData.Products);
        try
        {
            new Problem.CheckoutService(new Problem.OrderRepository(problemShop), new Problem.StockRepository(problemShop)).Confirm(withAMug);
        }
        catch (InvalidOperationException e)
        {
            narrator.Result($"failed: {e.Message}");
        }
        narrator.Result($"orders saved: {problemShop.Orders.Count}, books in stock: {problemShop.Stock[SampleData.Book.Id]} (half-done work)");

        narrator.Level(1, "Classic");
        narrator.Step("UnitOfWork: RegisterOrder and DecreaseStock only record; Commit validates everything, then applies everything");
        var classicShop = new Classic.InMemoryShop(SampleData.Products);
        try
        {
            new Classic.CheckoutService(classicShop).Confirm(withAMug);
        }
        catch (InvalidOperationException e)
        {
            narrator.Result($"failed: {e.Message}");
        }
        narrator.Result($"orders saved: {classicShop.Orders.Count}, books in stock: {classicShop.Stock[SampleData.Book.Id]} (nothing applied)");
        new Classic.CheckoutService(classicShop).Confirm(fine);
        narrator.Result($"2 books and the headphones: orders saved: {classicShop.Orders.Count}, books in stock: {classicShop.Stock[SampleData.Book.Id]}");

        narrator.Level(2, ".NET");
        narrator.Step("TransactionScope + TransactionalShop (an IEnlistmentNotification): Prepare votes, Commit applies");
        var dotnetShop = new DotNet.InMemoryShop(SampleData.Products);
        try
        {
            new DotNet.CheckoutService(dotnetShop).Confirm(withAMug);
        }
        catch (TransactionAbortedException e)
        {
            narrator.Result($"failed: {e.GetType().Name}");
        }
        narrator.Result($"orders saved: {dotnetShop.Orders.Count}, books in stock: {dotnetShop.Stock[SampleData.Book.Id]}");
        narrator.Step("the fine order, but the scope is disposed without Complete()");
        var transactional = new TransactionalShop(dotnetShop);
        using (new TransactionScope())
        {
            transactional.RegisterOrder(fine);
            transactional.DecreaseStock(SampleData.Book.Id, 2);
        }
        narrator.Result($"orders saved: {dotnetShop.Orders.Count}, books in stock: {dotnetShop.Stock[SampleData.Book.Id]} (rolled back)");

        narrator.Takeaway("Collect the changes of one operation and commit them together; with EF Core, DbContext and SaveChanges do it for you.");
    }
}
