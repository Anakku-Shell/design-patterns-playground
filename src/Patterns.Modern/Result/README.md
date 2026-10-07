# Result

**Relevance:** ⭐⭐ Useful. Increasingly common in .NET APIs and domain code; `TryXxx` is everywhere in the BCL.

**Intent:** return the outcome of an operation that can fail in an expected way as a value (success or error), instead of throwing an exception.

**Read first:** [`1-Classic/Result.cs`](1-Classic/Result.cs), then [`1-Classic/StockService.cs`](1-Classic/StockService.cs) and the test `Bind_StopsAtFirstFailure`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `StockService.Reserve` throws `InsufficientStockException` for an everyday outcome; callers use `try`/`catch` for flow. |
| [`1-Classic/`](1-Classic/) | `Result<T>` (`IsSuccess`, `Value`, `Error`, `Map`, `Bind`, `Match`) created through `Result.Success` and `Result.Failure`; errors are `BusinessError(Code, Message)`. |
| [`2-DotNet/`](2-DotNet/) | The BCL's own result pattern: `TryReserve(product, quantity, [NotNullWhen(true)] out Reservation? reservation)`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- result`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.ResultTests"`

**Guide:** [§7.6 Result](../../../docs/DESIGN_PATTERNS_GUIDE.md#76-result)
