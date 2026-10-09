# Null Object

**Relevance:** ⭐⭐ Useful. Small and everywhere: `NullLogger<T>.Instance`, empty collections, `Stream.Null`.

**Intent:** provide an object with neutral, do-nothing behaviour to use instead of `null`.

**Read first:** [`1-Classic/Discounts.cs`](1-Classic/Discounts.cs), [`1-Classic/CustomerProfiles.cs`](1-Classic/CustomerProfiles.cs), then [`2-DotNet/PriceService.cs`](2-DotNet/PriceService.cs).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `PriceService(IDiscount? discount, ILogger? logger)` with a null check at every use; a greeting with an "if guest" branch. |
| [`1-Classic/`](1-Classic/) | `NoDiscount.Instance` next to `PercentageDiscount`; `GuestCustomer.Instance` next to `RegisteredCustomer`, chosen once by `CustomerProfiles.For`. |
| [`2-DotNet/`](2-DotNet/) | An optional logger defaulting to `NullLogger<PriceService>.Instance`; logging through a `[LoggerMessage]` method with no null check. |

**Run it:** `dotnet run --project src/Patterns.Runner -- null-object`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.NullObjectTests"`

**Guide:** [§7.7 Null Object](../../../docs/DESIGN_PATTERNS_GUIDE.md#77-null-object)
