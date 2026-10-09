# Options

**Relevance:** ⭐⭐⭐ Essential. It is how configuration reaches code in every ASP.NET Core and worker application.

**Intent:** bind groups of related settings to typed classes, validate them, and inject them where needed.

**Read first:** [`1-Classic/ShippingOptions.cs`](1-Classic/ShippingOptions.cs), then [`2-DotNet/ShippingOptions.cs`](2-DotNet/ShippingOptions.cs) and the test `Monitor_SeesTheNewValue_OptionsDoesNot`.

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `ShippingSettings` reads a `Dictionary<string, string>` with string keys and `decimal.Parse` in every method; a typo fails only when the value is used. |
| [`1-Classic/`](1-Classic/) | A typed `ShippingOptions` filled and validated once by `ShippingOptionsReader.Read`; `ShippingCalculator` receives it. |
| [`2-DotNet/`](2-DotNet/) | `AddOptions<ShippingOptions>().Bind(...).Validate(...).ValidateOnStart()`; `IOptions<T>` against `IOptionsMonitor<T>` over a `SettableConfigurationProvider` that signals changes. |

**Run it:** `dotnet run --project src/Patterns.Runner -- options`

**Tests:** `dotnet test --project tests/Patterns.Modern.Tests --filter-class "*.OptionsTests"`

**Guide:** [§7.2 Options](../../../docs/DESIGN_PATTERNS_GUIDE.md#72-options)
