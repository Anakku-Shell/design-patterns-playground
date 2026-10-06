using Patterns.Shop;

namespace Patterns.Creational.Builder.Classic;

// Role: Product — the finished, valid order plus the optional gift note (Order itself has no place for it).
// Guide: §4.4
public sealed record BuiltOrder(Order Order, string? GiftNote);

// Role: Builder — assembles an order step by step; an invalid order never leaves this class.
public sealed class OrderBuilder
{
    private readonly Customer _customer;
    private readonly List<OrderLine> _lines = [];
    private Address? _address;
    private string? _giftNote;

    private OrderBuilder(Customer customer) => _customer = customer ?? throw new ArgumentNullException(nameof(customer));

    public static OrderBuilder For(Customer customer) => new(customer);

    public OrderBuilder ShipTo(Address address)
    {
        _address = address ?? throw new ArgumentNullException(nameof(address));
        return this; // returning the builder lets calls chain
    }

    public OrderBuilder Add(Product product, int quantity = 1)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);

        var index = _lines.FindIndex(line => line.Product == product);
        if (index >= 0)
        {
            _lines[index] = _lines[index] with { Quantity = _lines[index].Quantity + quantity }; // same product: add up
        }
        else
        {
            _lines.Add(new OrderLine(product, quantity));
        }
        return this;
    }

    public OrderBuilder WithGiftNote(string note)
    {
        _giftNote = note;
        return this;
    }

    public BuiltOrder Build()
    {
        if (_address is null) { throw new InvalidOperationException("An order needs a shipping address."); }
        if (_lines.Count == 0) { throw new InvalidOperationException("An order needs at least one line."); }

        // A copy of the list: later calls on this builder cannot change the order already built.
        return new BuiltOrder(new Order(Guid.NewGuid(), _customer, [.. _lines], _address), _giftNote);
    }
}
