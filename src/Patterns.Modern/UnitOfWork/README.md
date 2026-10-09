# Unit of Work

**Relevance:** ⭐⭐ Useful. Every EF Core application uses one through `DbContext` and `SaveChanges`; writing one by hand is rare.

**Intent:** keep track of the changes made during a business operation and commit them together, all or nothing.

**Read first:** [`1-Classic/UnitOfWork.cs`](1-Classic/UnitOfWork.cs), then [`2-DotNet/TransactionalShop.cs`](2-DotNet/TransactionalShop.cs) and the test `Problem_LeavesHalfDoneWork`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `OrderRepository.Save` and `StockRepository.Decrease` write at once: the order is saved, then the out-of-stock mug fails, and the shop keeps half the work. |
| [`1-Classic/`](1-Classic/) | `UnitOfWork` records `RegisterOrder` and `DecreaseStock`; `Commit` validates every change (summed per product), then applies all of them. |
| [`2-DotNet/`](2-DotNet/) | `System.Transactions`: `TransactionalShop` enlists in the ambient `TransactionScope` as an `IEnlistmentNotification` (two-phase commit); no `Complete()` means rollback. |

**Run it:** `dotnet run --project src/Patterns.Runner -- unit-of-work`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.UnitOfWorkTests"`

**Guide:** [§7.4 Unit of Work](../../../docs/DESIGN_PATTERNS_GUIDE.md#74-unit-of-work)
