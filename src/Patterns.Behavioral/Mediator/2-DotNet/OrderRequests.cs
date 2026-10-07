using Patterns.Shop;

namespace Patterns.Behavioral.Mediator.DotNet;

// Role: Request — place an order; the answer is its id.
// Guide: §6.5 (the same requests and handlers as the Classic level; here the container builds the handlers)
public sealed record PlaceOrder(Order Order) : IRequest<Guid>;

// Role: Request — the total of a placed order.
public sealed record GetOrderTotal(Guid OrderId) : IRequest<decimal>;

// Role: ConcreteHandler — places the order, with only the collaborators it needs.
public sealed class PlaceOrderHandler(OrderStore store, PriceCheck priceCheck, AuditLog audit)
    : IRequestHandler<PlaceOrder, Guid>
{
    public Guid Handle(PlaceOrder request)
    {
        ArgumentNullException.ThrowIfNull(request);
        priceCheck.Verify(request.Order);
        store.Save(request.Order);
        audit.Record($"placed {request.Order.Id}");
        return request.Order.Id;
    }
}

// Role: ConcreteHandler — reads a total; needs the store and nothing else.
public sealed class GetOrderTotalHandler(OrderStore store) : IRequestHandler<GetOrderTotal, decimal>
{
    public decimal Handle(GetOrderTotal request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return store.Get(request.OrderId).Total;
    }
}
