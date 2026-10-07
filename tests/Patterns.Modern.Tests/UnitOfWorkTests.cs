using System.Transactions;
using Patterns.Modern.UnitOfWork.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.UnitOfWork.Classic;
using DotNet = Patterns.Modern.UnitOfWork.DotNet;
using Problem = Patterns.Modern.UnitOfWork.Problem;

namespace Patterns.Modern.Tests;

public sealed class UnitOfWorkTests
{
    private static readonly Order Fine = SampleData.OrderOf((SampleData.Book, 2), (SampleData.Headphones, 1));

    // The book is in stock, the mug is not: the second change fails after the first one looked fine.
    private static readonly Order WithAMug = SampleData.OrderOf((SampleData.Book, 2), (SampleData.Mug, 1));

    private static string MissingMug => $"Not enough stock for {SampleData.Mug.Id}.";

    [Fact]
    public void Problem_LeavesHalfDoneWork()
    {
        var shop = new Problem.InMemoryShop(SampleData.Products);
        var checkout = new Problem.CheckoutService(new Problem.OrderRepository(shop), new Problem.StockRepository(shop));

        var error = Assert.Throws<InvalidOperationException>(() => checkout.Confirm(WithAMug));

        Assert.Equal(MissingMug, error.Message);
        Assert.Equal([WithAMug], shop.Orders);             // the order was saved…
        Assert.Equal(18, shop.Stock[SampleData.Book.Id]);  // …and the books taken, for an order that failed
    }

    [Fact]
    public void Commit_AppliesEverything()
    {
        var classicShop = new Classic.InMemoryShop(SampleData.Products);
        new Classic.CheckoutService(classicShop).Confirm(Fine);

        var dotnetShop = new DotNet.InMemoryShop(SampleData.Products);
        new DotNet.CheckoutService(dotnetShop).Confirm(Fine);

        Assert.Equal([Fine], classicShop.Orders);
        Assert.Equal([Fine], dotnetShop.Orders);
        Assert.Equal((18, 4), (classicShop.Stock[SampleData.Book.Id], classicShop.Stock[SampleData.Headphones.Id]));
        Assert.Equal((18, 4), (dotnetShop.Stock[SampleData.Book.Id], dotnetShop.Stock[SampleData.Headphones.Id]));
    }

    [Fact]
    public void Commit_WithMissingStock_AppliesNothing()
    {
        var classicShop = new Classic.InMemoryShop(SampleData.Products);
        var classicError = Assert.Throws<InvalidOperationException>(() => new Classic.CheckoutService(classicShop).Confirm(WithAMug));

        var dotnetShop = new DotNet.InMemoryShop(SampleData.Products);
        Assert.Throws<TransactionAbortedException>(() => new DotNet.CheckoutService(dotnetShop).Confirm(WithAMug));

        Assert.Equal(MissingMug, classicError.Message);
        Assert.Empty(classicShop.Orders);
        Assert.Empty(dotnetShop.Orders);
        Assert.Equal(20, classicShop.Stock[SampleData.Book.Id]);
        Assert.Equal(20, dotnetShop.Stock[SampleData.Book.Id]);
    }

    [Fact]
    public void Classic_ChecksTheSumOfChangesToOneProduct()
    {
        // 15 + 10 books: each change alone fits the 20 in stock, together they do not.
        var shop = new Classic.InMemoryShop(SampleData.Products);
        var work = new Classic.UnitOfWork(shop);
        work.DecreaseStock(SampleData.Book.Id, 15);
        work.DecreaseStock(SampleData.Book.Id, 10);

        Assert.Throws<InvalidOperationException>(work.Commit);
        Assert.Equal(20, shop.Stock[SampleData.Book.Id]);
    }

    [Fact]
    public void Classic_NothingIsWrittenBeforeCommit()
    {
        var shop = new Classic.InMemoryShop(SampleData.Products);
        var work = new Classic.UnitOfWork(shop);

        work.RegisterOrder(Fine);
        work.DecreaseStock(SampleData.Book.Id, 2);

        Assert.Empty(shop.Orders);
        Assert.Equal(20, shop.Stock[SampleData.Book.Id]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void DecreaseStock_RejectsZeroOrNegativeUnits(int units)
    {
        // -5 would add stock; 0 for an unknown product would pass validation and fail half-way through applying.
        var classic = new Classic.UnitOfWork(new Classic.InMemoryShop(SampleData.Products));
        var dotnet = new TransactionalShop(new DotNet.InMemoryShop(SampleData.Products));

        Assert.Throws<ArgumentOutOfRangeException>(() => classic.DecreaseStock(SampleData.Mug.Id, units));
        using var scope = new TransactionScope();
        Assert.Throws<ArgumentOutOfRangeException>(() => dotnet.DecreaseStock(SampleData.Mug.Id, units));
    }

    [Fact]
    public void Commit_WithUnknownProduct_AppliesNothing()
    {
        var unknown = Guid.NewGuid();
        var classicShop = new Classic.InMemoryShop(SampleData.Products);
        var work = new Classic.UnitOfWork(classicShop);
        work.DecreaseStock(SampleData.Book.Id, 2);
        work.DecreaseStock(unknown, 1);

        var dotnetShop = new DotNet.InMemoryShop(SampleData.Products);
        var transactional = new TransactionalShop(dotnetShop);

        var error = Assert.Throws<InvalidOperationException>(work.Commit);
        Assert.Throws<TransactionAbortedException>(() =>
        {
            using var scope = new TransactionScope();
            transactional.DecreaseStock(SampleData.Book.Id, 2);
            transactional.DecreaseStock(unknown, 1);
            scope.Complete();
        });

        Assert.Equal($"Not enough stock for {unknown}.", error.Message);
        Assert.Equal(20, classicShop.Stock[SampleData.Book.Id]);
        Assert.Equal(20, dotnetShop.Stock[SampleData.Book.Id]);
    }

    [Fact]
    public void DotNet_ScopeWithoutComplete_RollsBack()
    {
        var shop = new DotNet.InMemoryShop(SampleData.Products);
        var transactional = new TransactionalShop(shop);

        using (new TransactionScope())
        {
            transactional.RegisterOrder(Fine);
            transactional.DecreaseStock(SampleData.Book.Id, 2);
            // no scope.Complete(): disposing the scope rolls back
        }

        Assert.Empty(shop.Orders);
        Assert.Equal(20, shop.Stock[SampleData.Book.Id]);

        // The rollback discarded the pending changes: the same object works in the next scope.
        using (var scope = new TransactionScope())
        {
            transactional.DecreaseStock(SampleData.Book.Id, 1);
            scope.Complete();
        }
        Assert.Empty(shop.Orders);
        Assert.Equal(19, shop.Stock[SampleData.Book.Id]);
    }

    [Fact]
    public void DotNet_ChangesInAnotherTransaction_Throw()
    {
        // One TransactionalShop holds the changes of one transaction at a time.
        var transactional = new TransactionalShop(new DotNet.InMemoryShop(SampleData.Products));
        using var outer = new TransactionScope();
        transactional.DecreaseStock(SampleData.Book.Id, 1);

        using var inner = new TransactionScope(TransactionScopeOption.RequiresNew);
        var error = Assert.Throws<InvalidOperationException>(() => transactional.DecreaseStock(SampleData.Book.Id, 5));

        Assert.Equal("TransactionalShop already holds changes of another transaction.", error.Message);
    }

    [Fact]
    public void DotNet_OutsideAScope_Throws()
    {
        var transactional = new TransactionalShop(new DotNet.InMemoryShop(SampleData.Products));

        var error = Assert.Throws<InvalidOperationException>(() => transactional.DecreaseStock(SampleData.Book.Id, 1));

        Assert.Equal("TransactionalShop must be used inside a TransactionScope.", error.Message);
    }
}
