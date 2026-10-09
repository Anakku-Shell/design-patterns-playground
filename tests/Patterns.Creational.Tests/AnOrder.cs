using Patterns.Shop;

namespace Patterns.Creational.Tests;

/// <summary>
/// Builder's second face: a test data builder. Every field has a sensible default (Ana, Madrid), so a test
/// says only what it is about. Guide: §4.4.
/// </summary>
internal sealed class AnOrder
{
    private readonly (Product Product, int Quantity)[] _lines;
    private Address _address = SampleData.Madrid;

    private AnOrder((Product Product, int Quantity)[] lines) => _lines = lines;

    public static AnOrder WithLines(params (Product Product, int Quantity)[] lines) => new(lines);

    public AnOrder ShippedTo(Address address)
    {
        _address = address;
        return this;
    }

    public Order Build() =>
        new(Guid.NewGuid(), SampleData.Ana, [.. _lines.Select(l => new OrderLine(l.Product, l.Quantity))], _address);
}
