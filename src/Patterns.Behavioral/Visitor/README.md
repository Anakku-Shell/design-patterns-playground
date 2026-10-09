# Visitor

**Relevance:** ⭐ Niche. You write a visitor when a library hands you one (`ExpressionVisitor`, Roslyn's syntax visitors); for your own types, pattern matching made it unnecessary in most cases.

**Intent:** represent an operation to be performed on the elements of an object structure, so new operations can be added without changing the element classes.

**Read first:** [`1-Classic/CatalogVisitors.cs`](1-Classic/CatalogVisitors.cs) (and `Accept` in [`1-Classic/CatalogNodes.cs`](1-Classic/CatalogNodes.cs): double dispatch), then [`2-DotNet/CatalogItems.cs`](2-DotNet/CatalogItems.cs).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | `CatalogNode` with `ProductNode` and `BundleNode`, each with `Accept`; `ICatalogVisitor` with `VatVisitor` (14.76 for the Starter kit) and `OutlineVisitor` (an indented outline). |
| [`2-DotNet/`](2-DotNet/) | (a) records `ProductItem`/`BundleItem` and `VatCalculator.VatOf` with a `switch` expression; (b) `ConstantCollector : ExpressionVisitor`, which finds `100` and `5` in `o => o.Total > 100m && o.Units < 5`. |

Niche patterns have no Problem level; the guide shows the "before" code as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- visitor`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.VisitorTests"`

**Guide:** [§6.11 Visitor](../../../docs/DESIGN_PATTERNS_GUIDE.md#611-visitor)
