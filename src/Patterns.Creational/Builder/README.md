# Builder

**Relevance:** ⭐⭐⭐ Essential. `StringBuilder`, `WebApplication.CreateBuilder` and test data builders are everyday .NET.

**Intent:** separate the construction of a complex object from its representation, so it is built step by step and only handed out when complete.

**Read first:** [`1-Classic/OrderBuilder.cs`](1-Classic/OrderBuilder.cs), then the test data builder [`AnOrder`](../../../tests/Patterns.Creational.Tests/AnOrder.cs).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | A mutable order that is invalid until someone remembers to call `IsValid()`. |
| [`1-Classic/`](1-Classic/) | A fluent `OrderBuilder`: merges repeated products, refuses incomplete orders, returns a `BuiltOrder` (the immutable `Order` plus the optional gift note). |
| [`2-DotNet/`](2-DotNet/) | `StringBuilder` (the receipt) and `UriBuilder` (the tracking link). |

**Run it:** `dotnet run --project src/Patterns.Runner -- builder`

**Tests:** `dotnet test --project tests/Patterns.Creational.Tests --filter-class "*.BuilderTests"`

**Guide:** [§4.4 Builder](../../../docs/DESIGN_PATTERNS_GUIDE.md#44-builder)
