# State

**Relevance:** ⭐⭐ Useful. Life cycles (orders, documents, connections) are everywhere; often a transition `switch` is enough, and classes win when each state has its own behaviour.

**Intent:** allow an object to alter its behaviour when its internal state changes; the object appears to change its class.

**Read first:** [`1-Classic/OrderStates.cs`](1-Classic/OrderStates.cs), then [`2-DotNet/OrderTransitions.cs`](2-DotNet/OrderTransitions.cs) and the theory `AllLevels_FollowTheSameDiagram` (all 20 status × action pairs).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `OrderWorkflow`: one method per action, each with an `if` on `Status`; the rules of one status are spread over four methods. |
| [`1-Classic/`](1-Classic/) | `OrderContext` delegates to an `OrderState`; `DraftState`, `PlacedState`, `PaidState`, `ShippedState` and `CancelledState` override only their legal moves. |
| [`2-DotNet/`](2-DotNet/) | `OrderTransitions.Next(status, action)`: the diagram as one `switch` expression on the tuple. |

**Run it:** `dotnet run --project src/Patterns.Runner -- state`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.StateTests"`

**Guide:** [§6.8 State](../../../docs/DESIGN_PATTERNS_GUIDE.md#68-state)
