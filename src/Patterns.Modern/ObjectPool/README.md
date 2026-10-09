# Object Pool

**Relevance:** ⭐ Niche, measure first. The framework pools what matters (connections, arrays, handlers); you write pooling code only on measured hot paths.

**Intent:** reuse objects that are expensive to create by keeping them in a pool, lending them out and taking them back.

**Read first:** [`1-Classic/InvoiceBufferPool.cs`](1-Classic/InvoiceBufferPool.cs), then [`2-DotNet/InvoiceRenderer.cs`](2-DotNet/InvoiceRenderer.cs) and the test `ArrayPool_MayReturnALargerArray`.

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | `InvoiceBufferPool(maxRetained)` with `Rent`, `Return` (clears; drops when full) and `Created`; `InvoiceRenderer` rents and returns in `finally`. |
| [`2-DotNet/`](2-DotNet/) | `DefaultObjectPoolProvider().CreateStringBuilderPool()` (`Microsoft.Extensions.ObjectPool`) and `ArrayPool<byte>.Shared` in `InvoiceEncoder`, which keeps only the bytes it wrote. |

**Run it:** `dotnet run --project src/Patterns.Runner -- object-pool`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.ObjectPoolTests"`

**Guide:** [§7.8 Object Pool](../../../docs/DESIGN_PATTERNS_GUIDE.md#78-object-pool)
