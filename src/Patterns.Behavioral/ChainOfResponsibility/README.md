# Chain of Responsibility

**Relevance:** ⭐⭐⭐ Essential. Every ASP.NET Core application is a chain of middleware, and every `HttpClient` a chain of handlers.

**Intent:** avoid coupling the sender of a request to its receiver by giving several objects a chance to handle it; chain them and pass the request along.

**Read first:** [`1-Classic/OrderRules.cs`](1-Classic/OrderRules.cs), then [`2-DotNet/OrderPipeline.cs`](2-DotNet/OrderPipeline.cs) and the test `DotNet_MissingHeader_ShortCircuitsWith401`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `OrderValidator.Validate`: four `if`s in one method, in a fixed order. |
| [`1-Classic/`](1-Classic/) | `OrderRule` with `SetNext` and `Check`; one class per rule (`NotEmptyRule`, `MaxQuantityRule`, `StockRule`, `ShippingCountryRule`); the first failure wins. |
| [`2-DotNet/`](2-DotNet/) | ASP.NET Core middleware run in memory: `ApplicationBuilder`, three `app.Use` and a terminal `app.Run`; "auth" answers 401 without calling `next`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- chain-of-responsibility`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.ChainOfResponsibilityTests"`

**Guide:** [§6.1 Chain of Responsibility](../../../docs/DESIGN_PATTERNS_GUIDE.md#61-chain-of-responsibility)
