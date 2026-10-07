using Patterns.Shop;

namespace Patterns.Behavioral.Iterator.Classic;

// Role: Iterator — walks a sequence one element at a time.
// Guide: §6.4
public interface IIterator<out T>
{
    bool HasNext { get; }

    T GetNext(); // GoF calls it Next(); that is a Visual Basic keyword, which analyzer CA1716 rejects
}

// Role: ConcreteAggregate — keeps its storage private and hands out iterators.
public sealed class OrderHistory(IEnumerable<Order> orders)
{
    private readonly List<Order> _orders = [.. orders];

    public IIterator<IReadOnlyList<Order>> Pages(int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        return new PageIterator(_orders, pageSize);
    }

    // Role: ConcreteIterator — remembers where it is; the caller never sees an index.
    private sealed class PageIterator(List<Order> orders, int pageSize) : IIterator<IReadOnlyList<Order>>
    {
        private int _position;

        public bool HasNext => _position < orders.Count;

        public IReadOnlyList<Order> GetNext()
        {
            if (!HasNext) { throw new InvalidOperationException("No more pages."); }
            var page = orders.GetRange(_position, Math.Min(pageSize, orders.Count - _position));
            _position += page.Count;
            return page;
        }
    }
}
