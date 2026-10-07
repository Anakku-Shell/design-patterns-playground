# Memento

**Relevance:** ⭐ Niche. Immutable records make the state its own memento; the full pattern is for editors and undo stacks over mutable objects.

**Intent:** without violating encapsulation, capture an object's internal state so that it can be restored later.

**Read first:** [`1-Classic/Cart.cs`](1-Classic/Cart.cs), then [`2-DotNet/CartState.cs`](2-DotNet/CartState.cs).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | The originator `Cart` creates and restores a `CartSnapshot` that shows nothing public (its state is `internal` and immutable); the caretaker `CartCaretaker` keeps them in a stack. |
| [`2-DotNet/`](2-DotNet/) | `CartState`, an immutable record over an `ImmutableDictionary`: every change returns a new state, so undo is a `Stack<CartState>`. |

Niche patterns have no Problem level; the guide shows the "before" code as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- memento`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.MementoTests"`

**Guide:** [§6.6 Memento](../../../docs/DESIGN_PATTERNS_GUIDE.md#66-memento)
