# Template Method

**Relevance:** ⭐⭐ Useful. You meet it in every framework base class (`BackgroundService`, `Stream`, `DbContext`, `JsonConverter<T>`); in your own code, composition (Strategy) usually serves better.

**Intent:** define the skeleton of an algorithm in a base class and let subclasses fill in some of its steps without changing its structure.

**Read first:** [`1-Classic/OrderExporters.cs`](1-Classic/OrderExporters.cs), then [`2-DotNet/NightlyExportService.cs`](2-DotNet/NightlyExportService.cs).

| Level | What it shows |
|---|---|
| [`0-Problem/`](0-Problem/) | `CsvExporter` and `JsonExporter` each implement the whole loop; the filter and sort rules are duplicated. |
| [`1-Classic/`](1-Classic/) | `OrderExporter.Export` (the template, not virtual) with abstract `Header` and `Row` and a `Footer` hook; `CsvOrderExporter` and `JsonOrderExporter`. |
| [`2-DotNet/`](2-DotNet/) | `NightlyExportService : BackgroundService` fills in `ExecuteAsync`; the framework's `StartAsync`/`StopAsync` own the skeleton. |

**Run it:** `dotnet run --project src/Patterns.Runner -- template-method`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.TemplateMethodTests"`

**Guide:** [§6.10 Template Method](../../../docs/DESIGN_PATTERNS_GUIDE.md#610-template-method)
