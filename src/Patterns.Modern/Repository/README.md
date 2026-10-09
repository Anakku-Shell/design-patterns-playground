# Repository

**Relevance:** ⭐⭐ Useful, and debated. You meet repositories in most enterprise .NET code; whether to write one over EF Core is a team decision.

**Intent:** mediate between the business code and the data storage through a collection-like interface.

**Read first:** [`1-Classic/ProductRepository.cs`](1-Classic/ProductRepository.cs), then [`2-DotNet/ProductQueries.cs`](2-DotNet/ProductQueries.cs) and the guide's "repository over EF Core?" table.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `CatalogService` filters an `IList<Product>` (a stand-in for a table) inline in every method; duplicates go in unchecked. |
| [`1-Classic/`](1-Classic/) | `IProductRepository` (`GetById`, `ListByCategory`, `Add`) with `InMemoryProductRepository`; `CatalogService` uses the interface only. |
| [`2-DotNet/`](2-DotNet/) | `ProductQueries` over `IQueryable<Product>`, the shape `DbSet<T>` gives you, with a case conversion a provider can translate. |

**Run it:** `dotnet run --project src/Patterns.Runner -- repository`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.RepositoryTests"`

**Guide:** [§7.3 Repository](../../../docs/DESIGN_PATTERNS_GUIDE.md#73-repository)
