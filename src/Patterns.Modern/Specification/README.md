# Specification

**Relevance:** ⭐⭐ Useful. Common in DDD codebases and with Ardalis.Specification; in simpler code, a few reusable `Expression` fields cover most needs.

**Intent:** encapsulate a business rule as an object that answers "does this candidate satisfy me?", and combine rules with and, or and not.

**Read first:** [`1-Classic/Specification.cs`](1-Classic/Specification.cs), then [`2-DotNet/ExpressionSpecification.cs`](2-DotNet/ExpressionSpecification.cs) and the test `JoiningBodies_WithoutReplacingTheParameter_FailsToCompile`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `ProductFinder` with one method per combination (`FindCheapInStock`, `FindOutOfStock`, `FindBooksOrHome`…). |
| [`1-Classic/`](1-Classic/) | `Specification<T>` with `IsSatisfiedBy` and the combinators `And`, `Or`, `Not`; the rules `InStock`, `InCategory`, `CheaperThan`. In memory only. |
| [`2-DotNet/`](2-DotNet/) | `ExpressionSpecification<T>` with `ToExpression()`; the combinators merge expression trees under one parameter through a small `ExpressionVisitor`, so `IQueryable<T>.Where` (and EF Core) can use them. |

**Run it:** `dotnet run --project src/Patterns.Runner -- specification`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.SpecificationTests"`

**Guide:** [§7.5 Specification](../../../docs/DESIGN_PATTERNS_GUIDE.md#75-specification)
