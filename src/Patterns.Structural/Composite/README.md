# Composite

**Relevance:** ⭐⭐ Useful. Trees are everywhere: bundles, menus, configuration, UI controls, expressions.

**Intent:** compose objects into tree structures and let clients treat individual objects and groups of objects the same way.

**Read first:** [`1-Classic/CatalogItems.cs`](1-Classic/CatalogItems.cs) (look at `Bundle.Add` and its cycle check).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | One `CatalogEntry` for products and bundles; every operation repeats the "one or many?" check. |
| [`1-Classic/`](1-Classic/) | `ICatalogItem` with a leaf (`ProductItem`) and a composite (`Bundle`) that refuses cycles. |
| [`2-DotNet/`](2-DotNet/) | `IConfiguration` sections as a tree: `CarrierSettings` reads the children of `Shipping:Carriers`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- composite`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.CompositeTests"`

**Guide:** [§5.3 Composite](../../../docs/DESIGN_PATTERNS_GUIDE.md#53-composite)
