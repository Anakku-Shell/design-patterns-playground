# Singleton

**Relevance:** ⭐⭐ Useful. The idea of one shared instance is everywhere; the hand-written static version is an anti-pattern today.

**Intent:** ensure a class has only one instance and give a global point of access to it.

**Read first:** [`1-Classic/VatRateTable.cs`](1-Classic/VatRateTable.cs), then compare it with [`2-DotNet/`](2-DotNet/).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | A public static dictionary of VAT rates: global, mutable, and impossible to isolate in tests. |
| [`1-Classic/`](1-Classic/) | The GoF Singleton: private constructor, static `Lazy<T>` instance. |
| [`2-DotNet/`](2-DotNet/) | An ordinary class registered with `AddSingleton`: one instance per container. |

**Run it:** `dotnet run --project src/Patterns.Runner -- singleton`

**Tests:** `dotnet test --project tests/Patterns.Creational.Tests --filter-class "*.SingletonTests"`

**Guide:** [§4.1 Singleton](../../../docs/DESIGN_PATTERNS_GUIDE.md#41-singleton)
