using Microsoft.Extensions.DependencyInjection;
using Patterns.Behavioral.Mediator.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Behavioral.Mediator.Classic;
using DotNet = Patterns.Behavioral.Mediator.DotNet;
using Problem = Patterns.Behavioral.Mediator.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class MediatorTests
{
    private static Classic.Dispatcher ClassicDispatcher(Classic.AuditLog? audit = null)
    {
        var store = new Classic.OrderStore();
        var dispatcher = new Classic.Dispatcher();
        dispatcher.Register(new Classic.PlaceOrderHandler(store, new Classic.PriceCheck(), audit ?? new Classic.AuditLog()));
        dispatcher.Register(new Classic.GetOrderTotalHandler(store));
        return dispatcher;
    }

    private static ServiceProvider DotNetServices() => new ServiceCollection().AddOrderRequests().BuildServiceProvider();

    [Fact]
    public void AllLevels_PlaceThenGetTotal()
    {
        var order = SampleData.OrderOf((SampleData.Book, 2));

        var endpoint = new Problem.OrdersEndpoint(new Problem.OrderStore(), new Problem.PriceCheck(), new Problem.AuditLog());
        Assert.Equal(25.00m, endpoint.TotalOf(endpoint.Place(order)));

        var classic = ClassicDispatcher();
        Assert.Equal(25.00m, classic.Send(new Classic.GetOrderTotal(classic.Send(new Classic.PlaceOrder(order)))));

        using var services = DotNetServices();
        var dotNet = services.GetRequiredService<DotNet.Dispatcher>();
        Assert.Equal(25.00m, dotNet.Send(new DotNet.GetOrderTotal(dotNet.Send(new DotNet.PlaceOrder(order)))));
    }

    [Fact]
    public void AllLevels_RefuseAnEmptyOrder()
    {
        var empty = SampleData.OrderOf();
        const string message = "An order needs at least one line.";

        var endpoint = new Problem.OrdersEndpoint(new Problem.OrderStore(), new Problem.PriceCheck(), new Problem.AuditLog());
        Assert.Equal(message, Assert.Throws<InvalidOperationException>(() => endpoint.Place(empty)).Message);
        Assert.Equal(message, Assert.Throws<InvalidOperationException>(() => ClassicDispatcher().Send(new Classic.PlaceOrder(empty))).Message);
        using var services = DotNetServices();
        var dotNet = services.GetRequiredService<DotNet.Dispatcher>();
        // The handler's own exception, not a TargetInvocationException: the dispatcher calls it through reflection.
        Assert.Equal(message, Assert.Throws<InvalidOperationException>(() => dotNet.Send(new DotNet.PlaceOrder(empty))).Message);
    }

    [Fact]
    public void Classic_UnknownRequest_Throws()
    {
        var dispatcher = new Classic.Dispatcher();

        var error = Assert.Throws<InvalidOperationException>(() => dispatcher.Send(new Classic.GetOrderTotal(Guid.Empty)));

        Assert.Equal("No handler registered for GetOrderTotal.", error.Message);
    }

    [Fact]
    public void DotNet_UnknownRequest_Throws()
    {
        using var services = new ServiceCollection().AddSingleton<DotNet.Dispatcher>().BuildServiceProvider();
        var dispatcher = services.GetRequiredService<DotNet.Dispatcher>();

        var error = Assert.Throws<InvalidOperationException>(() => dispatcher.Send(new DotNet.GetOrderTotal(Guid.Empty)));

        Assert.Equal("No handler registered for GetOrderTotal.", error.Message);
    }

    [Fact]
    public void DotNet_HandlersComeFromTheContainer()
    {
        using var services = DotNetServices();
        var order = SampleData.OrderOf((SampleData.Book, 2));

        services.GetRequiredService<DotNet.Dispatcher>().Send(new DotNet.PlaceOrder(order));

        // PlaceOrderHandler received the container's AuditLog singleton through its constructor.
        Assert.Equal([$"placed {order.Id}"], services.GetRequiredService<DotNet.AuditLog>().Entries);
    }

    [Fact]
    public void Classic_PlaceOrderHandler_Audits()
    {
        var audit = new Classic.AuditLog();
        var order = SampleData.OrderOf((SampleData.Book, 2));

        ClassicDispatcher(audit).Send(new Classic.PlaceOrder(order));

        Assert.Equal([$"placed {order.Id}"], audit.Entries);
    }
}
