# CLAUDE.md

A learning playground about **design patterns**. 31 patterns (the 23 of the Gang of Four book plus 8 modern .NET ones) are implemented in .NET 10 on one small shop domain, each in up to three levels: the code without the pattern, the pattern by hand, and what .NET already provides. The owner knows .NET well and is learning patterns: the explanations matter as much as the code.

The explanations live in `docs/DESIGN_PATTERNS_GUIDE.md`, written before the code. **Keep it in sync with the code**: when the code has to differ from a guide section, fix the section in the same commit; every new term gets a glossary entry (chapter 10).

## Layout

- `DesignPatterns.slnx`: one solution.
- `src/Patterns.Shop/`: the shop domain (records `Product`, `Customer`, `Order`… and `SampleData`). Depends on nothing.
- `src/Patterns.Demo/`: the demo contract (`IDemo`, `Narrator`, `Relevance`, `PatternCategory`). Depends on nothing.
- `src/Patterns.Creational|Structural|Behavioral|Modern/`: one folder per pattern, plus `Demos.cs` (the category's `Demos.All`, in guide order). Behavioral also references the ASP.NET Core shared framework (middleware in memory, `BackgroundService`).
- `src/Patterns.Runner/`: the console app (`list`, `<key>`, `all`); `Catalog.All` concatenates the four `Demos.All`.
- `tests/Patterns.<Category>.Tests/`: one test class per pattern (`StrategyTests`). `tests/Patterns.Runner.Tests/`: shop, narrator, runner and catalog. `tests/Patterns.ArchitectureTests/`: the structural rules.
- `scripts/check-guide.sh`: checks the guide's table-of-contents links and writes `artifacts/mermaid-check.html` (open it in a browser; it must end with `All N diagrams parsed.`).
- Built so far: foundations (shop, demo contract, runner, architecture tests). No pattern yet. Next: creational patterns.

## Levels and folder rules

Each pattern folder (`src/Patterns.Behavioral/Strategy/`):

- `0-Problem/` (Essential and Useful patterns only), `1-Classic/`, `2-DotNet/`, `<Pattern>Demo.cs`, `README.md`.
- Namespaces drop the number: `Patterns.<Category>.<Pattern>.Problem|Classic|DotNet` (IDE0130 is off for this reason).
- **Levels never share the pattern's own types**: each level declares its own, so it reads on its own. The exception is "someone else's code" the pattern wraps (Adapter `External/`, Facade `Subsystems/`, Proxy's image store), which lives in its own sub-folder and is shared.
- The DotNet level codes the first .NET item of the pattern's catalog row; the rest appear only in the guide.
- The demo narrates each level with `Narrator`, uses `SampleData`, ends with a takeaway, and is added to its category's `Demos.All` and to `CatalogTests.Expected`.

## Architecture rules are tests

`Patterns.ArchitectureTests` (ArchUnitNET) encodes six rules, one test each, with a `// Why:` comment: `Shop_DependsOnNothing`, `Demo_DependsOnNothing`, `Pattern_DoesNotUseAnotherPattern`, `Classic_DoesNotUseProblem` / `DotNet_DoesNotUseProblem`, `Classic_DoesNotUseMicrosoftExtensions`, `PatternCode_DoesNotUseThirdPartyLibraries`. **Never weaken or delete an architecture test to make a change compile**: change the design instead, or stop and ask.

## Conventions

- Everything in English: code, comments, docs, commit messages.
- A class that plays a pattern role starts with `// Role: <GoF role> — <what it does here>.` and `// Guide: §N.M`. Problem code marks the pain with `// PAIN: …`. Comments explain *why*, never repeat the line below.
- `sealed` by default, values and DTOs are `record`s, constructor injection only, time from `TimeProvider`, money is `decimal` rounded with `decimal.Round(x, 2, MidpointRounding.AwayFromZero)`, culture-safe strings (`CultureInfo.InvariantCulture`, `ToUpperInvariant`), logging through `[LoggerMessage]`.
- Analyzers run at `latest-recommended` with warnings as errors. Watch for CA1716 (no Visual Basic keywords as type or virtual member names: `And`, `Next`, `Error`…) and CA1305 (culture).
- Package versions only in `Directory.Packages.props`. `src/` may use only the BCL and `Microsoft.*` packages. **Not allowed anywhere:** MediatR, AutoMapper, FluentAssertions; no third-party package in `src/` (the guide names them instead).
- Tests: xUnit v3 with plain `Assert`, written before the code (TDD). Equivalence tests show that all levels give the same result.

## Commands

```bash
dotnet build DesignPatterns.slnx -warnaserror                 # 0 warnings, 0 errors
dotnet test --solution DesignPatterns.slnx                    # all tests (Microsoft.Testing.Platform, see global.json)
dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.StrategyTests"   # one pattern
dotnet format DesignPatterns.slnx --verify-no-changes         # formatting
dotnet run --project src/Patterns.Runner                      # list the patterns
dotnet run --project src/Patterns.Runner -- strategy          # run one demo (or: all)
bash scripts/check-guide.sh                                   # guide links + Mermaid page
```

A category test project with no tests yet ignores exit code 8 ("zero tests ran") through `TestingPlatformCommandLineArguments` in its `.csproj`; the phase that adds its first tests removes that line.

## Workflow

- Work happens on `feature/design-patterns-playground`, one commit per phase (Conventional Commits), pushed to origin. Never merge; the owner merges into `develop`.
- Do not install software on the machine; ask the owner.
- Before declaring a change done: build with `-warnaserror`, all tests green, `dotnet format --verify-no-changes`, `dotnet run --project src/Patterns.Runner -- all`, guide and pattern README updated.
