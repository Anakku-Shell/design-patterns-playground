using Microsoft.Extensions.DependencyInjection;
using Patterns.Behavioral.Mediator.DotNet;
using Patterns.Demo;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Behavioral.Mediator;

public sealed class MediatorDemo : IDemo
{
    public string Key => "mediator";
    public string Name => "Mediator";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§6.5";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        // A fixed id, so the demo prints the same text on every run.
        var order = SampleData.OrderOf((SampleData.Book, 2)) with { Id = new Guid("00000000-0000-0000-0000-000000000001") };

        narrator.Level(0, "Problem");
        narrator.Step("OrdersEndpoint(store, priceCheck, audit): the endpoint calls every collaborator itself");
        var endpoint = new Problem.OrdersEndpoint(new Problem.OrderStore(), new Problem.PriceCheck(), new Problem.AuditLog());
        narrator.Result(Invariant($"total {endpoint.TotalOf(endpoint.Place(order)):0.00}"));

        narrator.Level(1, "Classic");
        narrator.Step("dispatcher.Send(new PlaceOrder(order)), then Send(new GetOrderTotal(id))");
        var store = new Classic.OrderStore();
        var dispatcher = new Classic.Dispatcher();
        dispatcher.Register(new Classic.PlaceOrderHandler(store, new Classic.PriceCheck(), new Classic.AuditLog()));
        dispatcher.Register(new Classic.GetOrderTotalHandler(store));
        var id = dispatcher.Send(new Classic.PlaceOrder(order));
        narrator.Result(Invariant($"total {dispatcher.Send(new Classic.GetOrderTotal(id)):0.00}"));
        narrator.Step("a request nobody registered a handler for");
        try
        {
            new Classic.Dispatcher().Send(new Classic.GetOrderTotal(id));
        }
        catch (InvalidOperationException e)
        {
            narrator.Result(e.Message);
        }

        narrator.Level(2, ".NET");
        narrator.Step("the dispatcher asks the DI container for IRequestHandler<TRequest, TResponse>");
        using var services = new ServiceCollection().AddOrderRequests().BuildServiceProvider();
        var mediator = services.GetRequiredService<DotNet.Dispatcher>();
        var placed = mediator.Send(new DotNet.PlaceOrder(order));
        narrator.Result(Invariant($"total {mediator.Send(new DotNet.GetOrderTotal(placed)):0.00}"));
        narrator.Result($"audit (injected into the handler): {string.Join(", ", services.GetRequiredService<DotNet.AuditLog>().Entries)}");

        narrator.Takeaway("Callers send requests to one dispatcher; each handler has only the dependencies it needs.");
    }
}
