# Abstract Factory

**Relevance:** ⭐ Niche. DI and keyed services cover most "pick an implementation" needs; the pattern still earns its place when a family must stay consistent (`DbProviderFactory`).

**Intent:** provide an interface for creating families of related objects without naming their concrete classes.

**Read first:** [`1-Classic/PaymentInterfaces.cs`](1-Classic/PaymentInterfaces.cs), then [`1-Classic/CardFamily.cs`](1-Classic/CardFamily.cs).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | Two families (card, wallet), each a factory creating a charger, a refunder and a receipt formatter that belong together. |
| [`2-DotNet/`](2-DotNet/) | The families registered as keyed services: the key chooses the whole family. |

Niche patterns have no Problem level; the guide shows the "before" code as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- abstract-factory`

**Tests:** `dotnet test --project tests/Patterns.Creational.Tests --filter-class "*.AbstractFactoryTests"`

**Guide:** [§4.3 Abstract Factory](../../../docs/DESIGN_PATTERNS_GUIDE.md#43-abstract-factory)
