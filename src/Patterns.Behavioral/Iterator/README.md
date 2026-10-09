# Iterator

**Relevance:** ⭐⭐⭐ Essential. `IEnumerable<T>`, `foreach`, `yield return` and `IAsyncEnumerable<T>` are everyday C#. Understand the pattern; never hand-write it.

**Intent:** provide a way to access the elements of a collection one after another without exposing how it is stored.

**Read first:** [`1-Classic/OrderHistory.cs`](1-Classic/OrderHistory.cs), then [`2-DotNet/OrderHistory.cs`](2-DotNet/OrderHistory.cs) and the test `Yield_IsLazy`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `OrderHistory.Orders` is a public `List`; `HistoryScreen` pages it with `GetRange` and `Math.Min`, as every caller would. |
| [`1-Classic/`](1-Classic/) | `IIterator<T>` (`HasNext`, `GetNext`); `OrderHistory.Pages(3)` returns a private nested `PageIterator`. |
| [`2-DotNet/`](2-DotNet/) | `Pages` with `yield return` (lazy: one page built per request); `RemoteOrders.StreamAsync` as an `IAsyncEnumerable<Order>` over `FakeOrderApi`, one call per page as you consume. |

**Run it:** `dotnet run --project src/Patterns.Runner -- iterator`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.IteratorTests"`

**Guide:** [§6.4 Iterator](../../../docs/DESIGN_PATTERNS_GUIDE.md#64-iterator)
