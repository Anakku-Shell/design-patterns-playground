# Decorator

**Relevance:** ⭐⭐⭐ Essential. `DelegatingHandler`, streams, middleware and decorated services in DI are everyday .NET.

**Intent:** attach additional responsibilities to an object dynamically, by wrapping it in objects with the same interface.

**Read first:** [`1-Classic/PriceCalculators.cs`](1-Classic/PriceCalculators.cs), then the test `Order_Matters` (`116.00` vs `114.95`).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `PriceCalculator.Price(order, applyVat, coupon)`: one flag per rule and a fixed order. |
| [`1-Classic/`](1-Classic/) | `BasePrice` wrapped by `VatDecorator` and `CouponDecorator`, in any number and order. |
| [`2-DotNet/`](2-DotNet/) | `DelegatingHandler`s around an `HttpClient`: `CorrelationIdHandler` and `RequestLogHandler`, chained by hand in `CarrierHttpClient`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- decorator`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.DecoratorTests"`

**Guide:** [§5.4 Decorator](../../../docs/DESIGN_PATTERNS_GUIDE.md#54-decorator)
