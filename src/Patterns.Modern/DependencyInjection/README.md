# Dependency Injection

**Relevance:** ⭐⭐⭐ Essential. Every ASP.NET Core and worker application is built around the DI container.

**Intent:** give an object the objects it depends on from outside, instead of letting it create or find them.

**Read first:** [`1-Classic/CheckoutService.cs`](1-Classic/CheckoutService.cs), then [`2-DotNet/CheckoutService.cs`](2-DotNet/CheckoutService.cs) and the tests `Lifetimes` and `ResolvingScopedFromRoot_Throws`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `CheckoutService` creates an `SmtpEmailSender` and reads `DateTime.Now` inside; `LocatorCheckoutService` asks a static `ServiceLocator` from inside `Confirm` (the anti-pattern). |
| [`1-Classic/`](1-Classic/) | Constructor injection: `CheckoutService(IEmailSender, TimeProvider)`, wired by a hand-written `CompositionRoot` ("pure DI"). |
| [`2-DotNet/`](2-DotNet/) | `ServiceCollection` with a singleton `TimeProvider`, a scoped `IEmailSender` and a transient `CheckoutService`; `ValidateScopes` and `ValidateOnBuild` on. |

**Run it:** `dotnet run --project src/Patterns.Runner -- dependency-injection`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.DependencyInjectionTests"`

**Guide:** [§7.1 Dependency Injection](../../../docs/DESIGN_PATTERNS_GUIDE.md#71-dependency-injection)
