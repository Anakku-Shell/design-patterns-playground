# Mediator

**Relevance:** ⭐⭐ Useful. The request/handler form (what MediatR does) is common in .NET APIs; the GoF "colleagues talk through a mediator" form appears in UI code.

**Intent:** define an object that encapsulates how a set of objects interact, so they do not refer to each other directly.

**Read first:** [`1-Classic/Dispatcher.cs`](1-Classic/Dispatcher.cs), then [`2-DotNet/Dispatcher.cs`](2-DotNet/Dispatcher.cs) and the test `DotNet_HandlersComeFromTheContainer`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `OrdersEndpoint(store, priceCheck, audit)` calls each collaborator itself; its constructor grows with every feature. |
| [`1-Classic/`](1-Classic/) | `IRequest<TResponse>`, `IRequestHandler<TRequest, TResponse>` and a `Dispatcher` that keeps one function per request type; `PlaceOrder` and `GetOrderTotal` with their handlers. |
| [`2-DotNet/`](2-DotNet/) | The same requests; the `Dispatcher` asks the DI container for `IRequestHandler<,>` built with `MakeGenericType`, so handlers get their dependencies by constructor injection (`AddOrderRequests`). |

**Run it:** `dotnet run --project src/Patterns.Runner -- mediator`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.MediatorTests"`

**Guide:** [§6.5 Mediator](../../../docs/DESIGN_PATTERNS_GUIDE.md#65-mediator)
