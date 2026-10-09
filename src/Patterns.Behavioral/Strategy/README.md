# Strategy

**Relevance:** ⭐⭐⭐ Essential. Interfaces injected by DI, keyed services, `IComparer<T>` and every lambda passed to LINQ are strategies.

**Intent:** define a family of algorithms, encapsulate each one, and make them interchangeable.

**Read first:** [`1-Classic/ShippingStrategies.cs`](1-Classic/ShippingStrategies.cs), then [`2-DotNet/ShippingCalculator.cs`](2-DotNet/ShippingCalculator.cs) and the test `Standard_AtExactly50_IsFree`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `ShippingCalculator.CostFor(order, method)`: a `switch` with one case per shipping method. |
| [`1-Classic/`](1-Classic/) | `IShippingStrategy` with `StandardShipping`, `ExpressShipping` and `StorePickup`; the context `ShippingCalculator` receives one. |
| [`2-DotNet/`](2-DotNet/) | Keyed services (`AddKeyedSingleton<IShippingStrategy, ExpressShipping>("express")`) and a calculator that resolves the strategy by name; `ShippingRules`: the same strategies as `Func<Order, decimal>`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- strategy`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.StrategyTests"`

**Guide:** [§6.9 Strategy](../../../docs/DESIGN_PATTERNS_GUIDE.md#69-strategy)
