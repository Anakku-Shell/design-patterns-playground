using System.Collections.Immutable;
using Patterns.Shop;

namespace Patterns.Creational.Prototype.DotNet;

// Role: ConcretePrototype — a record: `with` is its built-in copy operation.
// Guide: §4.5
public sealed record RecurringOrder(Guid Id, string Name, ImmutableArray<OrderLine> Lines, DateOnly NextDelivery)
{
    // The whole pattern in one expression: copy, then change what differs. `with` is a shallow copy, which
    // is safe here only because ImmutableArray cannot change (a List<OrderLine> would be shared).
    public RecurringOrder ForNextMonth() => this with { Id = Guid.NewGuid(), NextDelivery = NextDelivery.AddMonths(1) };
}
