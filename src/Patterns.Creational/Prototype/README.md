# Prototype

**Relevance:** 🕰 Historical. Records and `with` do it for you; the pattern is worth studying for shallow vs deep copies.

**Intent:** create new objects by copying an existing one (the prototype).

**Read first:** [`1-Classic/OrderTemplate.cs`](1-Classic/OrderTemplate.cs) (compare `Clone` and `ShallowClone`).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | `IPrototype<T>.Clone()` that copies the list, and `ShallowClone()` with `MemberwiseClone` to show the shared-list trap. |
| [`2-DotNet/`](2-DotNet/) | A record with an `ImmutableArray`: `with` makes the next month's order. |

Historical patterns have no Problem level; the guide shows the "before" code as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- prototype`

**Tests:** `dotnet test --project tests/Patterns.Creational.Tests --filter-class "*.PrototypeTests"`

**Guide:** [§4.5 Prototype](../../../docs/DESIGN_PATTERNS_GUIDE.md#45-prototype)
