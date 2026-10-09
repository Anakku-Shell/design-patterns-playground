using Patterns.Shop;

namespace Patterns.Behavioral.Mediator.Problem;

/// <summary>The placed orders, in memory. Guide: §6.5.</summary>
public sealed class OrderStore
{
    private readonly Dictionary<Guid, Order> _orders = [];

    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _orders[order.Id] = order;
    }

    public Order Get(Guid orderId) =>
        _orders.TryGetValue(orderId, out var order) ? order : throw new KeyNotFoundException($"No order {orderId}.");
}

/// <summary>Checks an order before it is placed: it has lines, and big orders go to a person first.</summary>
public sealed class PriceCheck
{
    private readonly decimal _manualReviewAbove = 10_000.00m;

    public void Verify(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (order.Lines.Count == 0) { throw new InvalidOperationException("An order needs at least one line."); }
        if (order.Total > _manualReviewAbove) { throw new InvalidOperationException("Orders over 10000.00 need a manual review."); }
    }
}

/// <summary>What happened, for the auditors.</summary>
public sealed class AuditLog
{
    private readonly List<string> _entries = [];

    public IReadOnlyList<string> Entries => _entries;

    public void Record(string entry) => _entries.Add(entry);
}
