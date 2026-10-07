# Bridge

**Relevance:** ⭐ Niche. You rarely design one, but you use them all the time: `ILogger` and its providers, ADO.NET drivers.

**Intent:** decouple an abstraction from its implementation so the two can vary independently.

**Read first:** [`1-Classic/Notifications.cs`](1-Classic/Notifications.cs), then the channels it holds, [`1-Classic/MessageChannels.cs`](1-Classic/MessageChannels.cs).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | Kinds of notification (`OrderShippedNotification`, `PaymentFailedNotification`) hold an `IMessageChannel` (email, SMS): any kind on any channel. |
| [`2-DotNet/`](2-DotNet/) | `ILogger` is the abstraction and `ListLoggerProvider` an implementor; `CheckoutLog` logs without knowing the providers. |

Niche patterns have no Problem level; the guide shows the "before" code (a class per combination) as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- bridge`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.BridgeTests"`

**Guide:** [§5.2 Bridge](../../../docs/DESIGN_PATTERNS_GUIDE.md#52-bridge)
