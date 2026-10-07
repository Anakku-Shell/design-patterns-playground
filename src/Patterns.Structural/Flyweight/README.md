# Flyweight

**Relevance:** 🕰 Historical. The garbage collector, value types and cheap memory solve it in almost all application code; string interning is the runtime's own flyweight.

**Intent:** use sharing to support large numbers of fine-grained objects efficiently.

**Read first:** [`1-Classic/CategoryInfoFactory.cs`](1-Classic/CategoryInfoFactory.cs) (the intrinsic state in `CategoryInfo`, the extrinsic in `CatalogEntry`).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | `CategoryInfoFactory` hands out one shared, immutable `CategoryInfo` per category: 10,000 entries, 3 objects. |
| [`2-DotNet/`](2-DotNet/) | `string.Intern`: equal strings built at run time become one shared instance (`SkuText`). |

Historical patterns have no Problem level; the guide shows the "before" code as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- flyweight`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.FlyweightTests"`

**Guide:** [§5.6 Flyweight](../../../docs/DESIGN_PATTERNS_GUIDE.md#56-flyweight)
