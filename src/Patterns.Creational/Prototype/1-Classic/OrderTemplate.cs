using Patterns.Shop;

namespace Patterns.Creational.Prototype.Classic;

// Role: Prototype — declares the copy operation, with the real type (unlike ICloneable, which returns object).
// Guide: §4.5
public interface IPrototype<out T>
{
    T Clone();
}

// Role: ConcretePrototype — a monthly order that knows how to copy itself.
public sealed class OrderTemplate(string name, IEnumerable<OrderLine> lines) : IPrototype<OrderTemplate>
{
    public string Name { get; set; } = name;

    // Mutable on purpose: it is what makes the difference between a shallow and a deep copy visible.
    public IList<OrderLine> Lines { get; } = new List<OrderLine>(lines);

    // Deep enough: a new list. The OrderLine records inside are immutable, so sharing them is safe.
    public OrderTemplate Clone() => new(Name, Lines);

    // MemberwiseClone copies the fields one by one: the copy gets the SAME list object. The trap.
    public OrderTemplate ShallowClone() => (OrderTemplate)MemberwiseClone();
}
