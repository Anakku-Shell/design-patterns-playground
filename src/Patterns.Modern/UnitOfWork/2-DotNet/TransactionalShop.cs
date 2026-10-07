using System.Transactions;
using Patterns.Shop;

namespace Patterns.Modern.UnitOfWork.DotNet;

// Role: Store — the data; changed only when the ambient transaction commits.
// Guide: §7.4
public sealed class InMemoryShop
{
    private readonly List<Order> _orders = [];

    public InMemoryShop(IEnumerable<Product> products) => Stock = products.ToDictionary(p => p.Id, p => p.Stock);

    public Dictionary<Guid, int> Stock { get; }

    public IReadOnlyList<Order> Orders => _orders;

    public void AddOrder(Order order) => _orders.Add(order);
}

// Role: UnitOfWork — a resource that enlists in the ambient transaction (TransactionScope) and keeps its
// changes pending until the transaction manager says commit (two-phase commit: Prepare, then Commit).
public sealed class TransactionalShop(InMemoryShop shop) : IEnlistmentNotification
{
    private readonly List<Order> _newOrders = [];
    private readonly List<(Guid ProductId, int Units)> _stockChanges = [];
    private Transaction? _enlistedIn; // the one transaction whose changes this object holds

    public void RegisterOrder(Order order)
    {
        EnlistOnce();
        _newOrders.Add(order);
    }

    public void DecreaseStock(Guid productId, int units)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(units); // a negative decrease would add stock
        EnlistOnce();
        _stockChanges.Add((productId, units));
    }

    // Phase 1: can we commit? Every enlisted resource is asked before anyone writes.
    public void Prepare(PreparingEnlistment preparingEnlistment)
    {
        ArgumentNullException.ThrowIfNull(preparingEnlistment);
        var enough = _stockChanges.GroupBy(c => c.ProductId)
            .All(change => shop.Stock.TryGetValue(change.Key, out var inStock) && inStock >= change.Sum(c => c.Units));
        if (enough)
        {
            preparingEnlistment.Prepared();
        }
        else
        {
            Discard();
            preparingEnlistment.ForceRollback(); // disposing the scope throws TransactionAbortedException
        }
    }

    // Phase 2: everybody agreed; apply.
    public void Commit(Enlistment enlistment)
    {
        ArgumentNullException.ThrowIfNull(enlistment);
        foreach (var (productId, units) in _stockChanges)
        {
            shop.Stock[productId] -= units;
        }
        foreach (var order in _newOrders)
        {
            shop.AddOrder(order);
        }
        Discard();
        enlistment.Done();
    }

    public void Rollback(Enlistment enlistment)
    {
        ArgumentNullException.ThrowIfNull(enlistment);
        Discard();
        enlistment.Done();
    }

    public void InDoubt(Enlistment enlistment)
    {
        ArgumentNullException.ThrowIfNull(enlistment);
        Discard();
        enlistment.Done();
    }

    private void EnlistOnce()
    {
        var transaction = Transaction.Current
            ?? throw new InvalidOperationException("TransactionalShop must be used inside a TransactionScope.");
        if (_enlistedIn is null)
        {
            transaction.EnlistVolatile(this, EnlistmentOptions.None); // first change of this transaction
            _enlistedIn = transaction;
        }
        else if (!_enlistedIn.Equals(transaction))
        {
            // One transaction at a time: a nested RequiresNew scope would otherwise lose its changes.
            throw new InvalidOperationException("TransactionalShop already holds changes of another transaction.");
        }
    }

    private void Discard()
    {
        _newOrders.Clear();
        _stockChanges.Clear();
        _enlistedIn = null;
    }
}

// Role: Client — one TransactionScope per operation; Complete() is the "commit" vote.
public sealed class CheckoutService(InMemoryShop shop)
{
    public void Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var transactional = new TransactionalShop(shop);
        using var scope = new TransactionScope();
        transactional.RegisterOrder(order);
        foreach (var line in order.Lines)
        {
            transactional.DecreaseStock(line.Product.Id, line.Quantity);
        }
        scope.Complete(); // without it, disposing the scope rolls everything back
    }
}
