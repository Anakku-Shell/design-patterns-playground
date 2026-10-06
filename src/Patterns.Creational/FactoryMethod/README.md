# Factory Method

**Relevance:** ⭐⭐ Useful. Frameworks and older code use the subclass form; in applications, the DI container (keyed services, factory delegates) usually does the job.

**Intent:** define a method for creating an object, and let subclasses decide which class to create.

**Read first:** [`1-Classic/NotificationCampaign.cs`](1-Classic/NotificationCampaign.cs).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | A `switch` that `new`s the notifier inside the service that sends. |
| [`1-Classic/`](1-Classic/) | The GoF form: `CreateNotifier()` is overridden by `EmailCampaign` and `SmsCampaign`. |
| [`2-DotNet/`](2-DotNet/) | Keyed services: the container picks the class for each `NotificationChannel`. |

**Run it:** `dotnet run --project src/Patterns.Runner -- factory-method`

**Tests:** `dotnet test --project tests/Patterns.Creational.Tests --filter-class "*.FactoryMethodTests"`

**Guide:** [§4.2 Factory Method](../../../docs/DESIGN_PATTERNS_GUIDE.md#42-factory-method)
