# Command

**Relevance:** ⭐⭐ Useful. Delegates are one-method commands; request objects in web APIs and message handlers are commands by another name. Full undo is for editors and UI frameworks.

**Intent:** encapsulate a request as an object, so it can be stored, queued, logged or undone.

**Read first:** [`1-Classic/CartCommands.cs`](1-Classic/CartCommands.cs), then the test `Classic_UndoEverything_LeavesTheCartEmpty`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `UndoableCart` remembers the last action in a string and undoes it with a `switch`: one level of undo. |
| [`1-Classic/`](1-Classic/) | `ICartCommand` (`Execute`, `Undo`), `AddItemCommand` and `RemoveItemCommand` over the receiver `Cart`; the invoker `CartHistory` keeps a stack for unlimited undo. |
| [`2-DotNet/`](2-DotNet/) | `UndoableAction(Name, Do, Undo)`: a command as two lambdas, built by `CartActions`; `ActionHistory` over a `Stack`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- command`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.CommandTests"`

**Guide:** [§6.2 Command](../../../docs/DESIGN_PATTERNS_GUIDE.md#62-command)
