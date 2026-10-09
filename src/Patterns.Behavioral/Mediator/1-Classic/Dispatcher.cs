namespace Patterns.Behavioral.Mediator.Classic;

// Role: Request — a message that declares the type of its answer.
// Guide: §6.5
public interface IRequest<TResponse>;

// Role: Handler — the code for one request type.
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    TResponse Handle(TRequest request);
}

// Role: Mediator — routes each request to the one handler registered for its type.
public sealed class Dispatcher
{
    // Request type → a function that calls its handler. Storing a function hides the generic types,
    // so Send only needs to know TResponse.
    private readonly Dictionary<Type, Func<object, object?>> _handlers = [];

    public void Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        ArgumentNullException.ThrowIfNull(handler);
        _handlers[typeof(TRequest)] = request => handler.Handle((TRequest)request);
    }

    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!_handlers.TryGetValue(request.GetType(), out var handle))
        {
            throw new InvalidOperationException($"No handler registered for {request.GetType().Name}.");
        }
        return (TResponse)handle(request)!;
    }
}
