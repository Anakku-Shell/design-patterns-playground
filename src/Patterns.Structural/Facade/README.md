# Facade

**Relevance:** ⭐⭐⭐ Essential. Application services, `File`, `WebApplication`: one simple call in front of many objects.

**Intent:** provide one simple interface to a set of interfaces in a subsystem.

**Read first:** [`1-Classic/CheckoutFacade.cs`](1-Classic/CheckoutFacade.cs), then the subsystems it calls in [`Subsystems/`](Subsystems/).

| Level | What it shows |
|---|---|
| [`Subsystems/`](Subsystems/) | `Inventory`, `PaymentGateway`, `Shipping`, `Mailer` (simulated), shared by every level. They do not know the facade exists. |
| [`0-Problem/`](0-Problem/) | `WebCheckout` and `MobileCheckout` each call the four subsystems in order: two copies of the sequence. |
| [`1-Classic/`](1-Classic/) | `CheckoutFacade.PlaceOrder`: reserve → charge → ship → mail, in one place; nothing is charged when stock is missing. |
| [`2-DotNet/`](2-DotNet/) | `File.WriteAllText` / `ReadAllText` next to the same work done with `FileStream`, `StreamWriter` and `StreamReader` (`InvoiceFile`). |

**Run it:** `dotnet run --project src/Patterns.Runner -- facade`

**Tests:** `dotnet test --project tests/Patterns.Structural.Tests --filter-class "*.FacadeTests"`

**Guide:** [§5.5 Facade](../../../docs/DESIGN_PATTERNS_GUIDE.md#55-facade)
