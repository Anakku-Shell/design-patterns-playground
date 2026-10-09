using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Patterns.Behavioral.Mediator.DotNet;

// Role: Request — a message that declares the type of its answer.
// Guide: §6.5
public interface IRequest<TResponse>;

// Role: Handler — the code for one request type; a service registered in the container.
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    TResponse Handle(TRequest request);
}

// Role: Mediator — asks the container for the handler of each request: the core of what MediatR does.
public sealed class Dispatcher(IServiceProvider provider)
{
    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        ArgumentNullException.ThrowIfNull(request);
        // Build the closed generic type IRequestHandler<GetOrderTotal, decimal> at run time.
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for {request.GetType().Name}.");
        // Reflection; DoNotWrapExceptions keeps the handler's own exception type for the caller.
        return (TResponse)handlerType.GetMethod(nameof(IRequestHandler<PlaceOrder, Guid>.Handle))!
            .Invoke(handler, BindingFlags.DoNotWrapExceptions, binder: null, [request], culture: null)!;
    }
}

/// <summary>Registers the store, the collaborators, the handlers and the dispatcher.</summary>
public static class OrderRequestsServiceCollectionExtensions
{
    public static IServiceCollection AddOrderRequests(this IServiceCollection services)
    {
        services.AddSingleton<OrderStore>();
        services.AddSingleton<PriceCheck>();
        services.AddSingleton<AuditLog>();
        services.AddTransient<IRequestHandler<PlaceOrder, Guid>, PlaceOrderHandler>();
        services.AddTransient<IRequestHandler<GetOrderTotal, decimal>, GetOrderTotalHandler>();
        // Transient, like the handlers: a singleton would resolve them from the root provider and keep
        // disposable handlers alive until shutdown.
        services.AddTransient<Dispatcher>();
        return services;
    }
}
