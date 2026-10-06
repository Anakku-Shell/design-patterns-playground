# Design Patterns Playground — Design

Date: 2026-10-06 · Status: approved in conversation, pending written review

## 1. Purpose

A learning playground about **design patterns** in .NET. The owner knows .NET well and wants to understand each pattern: what problem it solves, how it works inside, how .NET already implements it, how relevant it is today, and when *not* to use it. **The explanations matter as much as the code.**

Success looks like this:
- Every pattern in the catalog (§3) can be **run** (`dotnet run -- <pattern>`) and shows, step by step, what happens.
- Every pattern has a section in **`docs/DESIGN_PATTERNS_GUIDE.md`** that can be read without opening the code, with diagrams.
- For every pattern the reader can answer: what it is for, how it is built by hand, what .NET gives you instead, how relevant it is today, when not to use it, and what it is confused with.

It is a sibling of `architecture-playground` (same stack and working style) but **standalone**: it never refers to that repo's code, and it explains its own domain from zero.

## 2. Principles

1. **Two or three levels per pattern.** *Problem* (the code without the pattern, and what hurts), *Classic* (the pattern by hand, plain C#), *DotNet* (the same with what .NET already provides). Essential and useful patterns get all three; niche and historical ones get Classic and DotNet only, and their "problem" is a short snippet in the guide.
2. **One business domain** (a small online shop, §4) for every example, so the reader focuses on the pattern, not on the domain. A pattern uses its own example only where the shop would be forced; the guide says why.
3. **No third-party libraries in pattern code.** Only the .NET base class library (BCL) and Microsoft packages (`Microsoft.Extensions.*`, ASP.NET Core). Third-party libraries that solve a pattern (MediatR, AutoMapper, FluentResults, ErrorOr, Scrutor, Polly…) are **named in the guide** with their licence situation (MediatR and AutoMapper became commercial in 2025), never referenced. Test libraries (xUnit, ArchUnitNET) are allowed.
4. **Well-chewed code.** Short comments that explain *why*, the pattern role of every class, a pointer to the guide section, and the exact spot that hurts in Problem code.
5. **Guide first.** The whole guide is written before the code, so it can be read while the code is built. Code phases follow the guide and fix it in the same commit when reality differs.

## 3. Catalog

Relevance scale (shown in the guide, in each pattern's README and by the runner):

| Mark | Name | Meaning |
|---|---|---|
| ⭐⭐⭐ | Essential | You will use or meet it almost daily. |
| ⭐⭐ | Useful | Has its place; you should recognise it. |
| ⭐ | Niche | Specific cases; knowing it exists is enough. |
| 🕰 | Historical | The language or framework already solves it; studied to understand older code. |

Levels: ⭐⭐⭐ and ⭐⭐ → Problem, Classic, DotNet. ⭐ and 🕰 → Classic, DotNet. In the "In .NET" column, **bold** items are coded in the DotNet level; the rest appear only in the guide. The implementation plan fixes the exact types, values and tests of each pattern.

| Pattern | Rel. | Shop example | In .NET (DotNet level) | Ecosystem (guide only) |
|---|---|---|---|---|
| **Creational** | | | | |
| Singleton | ⭐⭐ | VAT (value-added tax) rate table | **`AddSingleton`**, `Lazy<T>` (used by the Classic level); the hand-rolled static Singleton is presented as an anti-pattern | — |
| Factory Method | ⭐⭐ | creating a notifier per channel (email, SMS) | **keyed services in DI** (the container as the factory), `ILoggerFactory.CreateLogger`, `IHttpClientFactory` | — |
| Abstract Factory | ⭐ | payment provider families (charge + refund + receipt) | **choosing the family with keyed services**, `DbProviderFactory` | — |
| Builder | ⭐⭐⭐ | building orders; test data builders | **`StringBuilder`**, **`UriBuilder`**, `WebApplication.CreateBuilder` | — |
| Prototype | 🕰 | recurring order from a template | **`record` + `with`**; why `ICloneable` is discouraged | — |
| **Structural** | | | | |
| Adapter | ⭐⭐⭐ | external shipping carrier API with a different shape | **`StreamReader` over `Stream`** | AutoMapper (mapping, commercial) |
| Bridge | ⭐ | notifications: message kind × channel | **`ILogger` + a custom `ILoggerProvider`** | — |
| Composite | ⭐⭐ | product bundles (a bundle holds products and bundles) | **`IConfiguration` sections**, `CompositeFileProvider` | — |
| Decorator | ⭐⭐⭐ | price with discount and tax; caching repository | **`DelegatingHandler`**, `GZipStream`, decorating a registration in DI | Scrutor (`Decorate`) |
| Facade | ⭐⭐⭐ | `CheckoutService` over stock, payment and shipping | **`File.ReadAllText`/`WriteAllText`**, `WebApplication` | — |
| Flyweight | 🕰 | shared categories in a huge catalog | **`string.Intern`** | — |
| Proxy | ⭐⭐ | lazy-loaded product images; access control | **`Lazy<T>`**, **`DispatchProxy`**, EF Core lazy loading | Castle DynamicProxy |
| **Behavioral** | | | | |
| Chain of Responsibility | ⭐⭐⭐ | order validation pipeline | **ASP.NET Core middleware, run in memory** | — |
| Command | ⭐⭐ | cart operations with undo | **`Action`/`Func` delegates** | — |
| Interpreter | 🕰 | discount rules: `total > 100 AND category = 'books'` | **`System.Linq.Expressions`** (compiling the parsed rule), `Regex` | — |
| Iterator | ⭐⭐⭐ (understand it; never hand-write it) | paging through orders | **`IEnumerable<T>` + `yield return`**, **`IAsyncEnumerable<T>`** | — |
| Mediator | ⭐⭐ | in-process command dispatcher | nothing in the BCL: **a dispatcher over DI** | MediatR (commercial) |
| Memento | ⭐ | cart snapshots for undo | **immutable `record`s** | — |
| Observer | ⭐⭐⭐ | order placed → email, stock, analytics | **`event`/`EventHandler<T>`**, **`IObservable<T>`**, `IChangeToken` | System.Reactive (Rx) |
| State | ⭐⭐ | order life cycle | no native support; **a `switch` expression transition table** as the light alternative | Stateless |
| Strategy | ⭐⭐⭐ | shipping cost | **keyed services in DI**, **`Func<>`**, `IComparer<T>` | — |
| Template Method | ⭐⭐ | exporting orders (CSV, JSON) | **`BackgroundService.ExecuteAsync`** | — |
| Visitor | ⭐ | operations over bundles (VAT, export) | **pattern matching** (the modern alternative), **`ExpressionVisitor`** | — |
| **Modern / .NET** | | | | |
| Dependency Injection (Service Locator as anti-pattern) | ⭐⭐⭐ | wiring the checkout | **`Microsoft.Extensions.DependencyInjection`** | Autofac |
| Options | ⭐⭐⭐ | shipping settings | **`IOptions<T>`**, `IOptionsSnapshot<T>`, **`IOptionsMonitor<T>`** | — |
| Repository | ⭐⭐ (debated) | products, in memory | **`IQueryable<T>`** (the shape `DbSet<T>` gives you); `DbSet<T>` already is one | — |
| Unit of Work | ⭐⭐ | confirm order and stock together, in memory | **`System.Transactions` (`TransactionScope`)**; `DbContext.SaveChanges` | — |
| Specification | ⭐⭐ | "products on sale with stock" | **`Expression<Func<T, bool>>` with `IQueryable<T>`** (Classic uses plain predicates) | Ardalis.Specification |
| Result | ⭐⭐ | business errors without exceptions | none native beyond **the `TryXxx` pattern**; `TypedResults` | FluentResults, ErrorOr |
| Null Object | ⭐⭐ | no discount; guest customer | **`NullLogger<T>`** | — |
| Object Pool | ⭐ | buffers when generating invoices | **`ArrayPool<T>`**, **`ObjectPool<T>`** | — |

31 patterns: 23 from the Gang of Four (GoF) book and 8 modern ones. Repository and Unit of Work run **in memory**; the guide explains why EF Core's `DbSet`/`DbContext` already are those patterns, so no database or Docker is needed anywhere.

**Out of scope:** resilience (Retry, Circuit Breaker) and messaging patterns (outbox, saga), which belong to architecture. The guide mentions them in one line with where to learn more.

## 4. Domain: a small online shop

Kept minimal; explained from zero in guide chapter 3. Shared types in `Patterns.Shop`:

- `Product` (`Id`, `Name`, `Sku`, `Price`, `Category`, `Stock`)
- `Category` (`Name`)
- `Customer` (`Id`, `Name`, `Email`, `IsGuest`)
- `Address` (`Country`, `City`, `PostalCode`)
- `Order` (`Id`, `Customer`, `Lines`, `ShippingAddress`, `Status`, `Total`) and `OrderLine` (`Product`, `Quantity`, `LineTotal`)
- `OrderStatus` (`Draft`, `Placed`, `Paid`, `Shipped`, `Cancelled`)

Money is `decimal` (single currency, at most 2 decimals). Ids are `Guid`. Types a single pattern needs (carriers, notifiers, bundles…) live in that pattern's folder, not in `Patterns.Shop`. If a pattern needs to change a shared type's behaviour, it uses its own type instead.

## 5. Repository layout

```
design-patterns-playground/
  global.json                  SDK 10.0.401, rollForward latestFeature, test runner Microsoft.Testing.Platform
  Directory.Build.props        net10.0, nullable, implicit usings, warnings as errors, AnalysisLevel latest-recommended,
                               EnforceCodeStyleInBuild, InvariantGlobalization, central package management
  Directory.Packages.props     every package version
  .editorconfig
  DesignPatterns.slnx
  CLAUDE.md                    instructions for agents (layout, conventions, commands, workflow)
  README.md                    what this is, catalog table with relevance and links, quick start
  docs/
    DESIGN_PATTERNS_GUIDE.md
    superpowers/specs/         this spec
  src/
    Patterns.Shop/             shared domain; depends on nothing
    Patterns.Demo/             IDemo, Relevance, PatternCategory, narration helpers; depends on nothing
    Patterns.Creational/       one folder per pattern
    Patterns.Structural/
    Patterns.Behavioral/       also FrameworkReference Microsoft.AspNetCore.App (middleware in memory)
    Patterns.Modern/
    Patterns.Runner/           console app; references everything
  tests/
    Patterns.Creational.Tests/
    Patterns.Structural.Tests/
    Patterns.Behavioral.Tests/
    Patterns.Modern.Tests/
    Patterns.Runner.Tests/
    Patterns.ArchitectureTests/
```

`PLAN.md` (implementation plan) is a local, git-ignored file, as in the sibling repo.

### 5.1 Inside a pattern folder

Example `src/Patterns.Behavioral/Strategy/`:

```
README.md          relevance, intent in one sentence, which file to read first, levels, run command, guide link
0-Problem/         namespace Patterns.Behavioral.Strategy.Problem  (⭐⭐⭐ and ⭐⭐ only)
1-Classic/         namespace Patterns.Behavioral.Strategy.Classic
2-DotNet/          namespace Patterns.Behavioral.Strategy.DotNet
StrategyDemo.cs    namespace Patterns.Behavioral.Strategy — the IDemo the runner executes
```

The numbered folders give the reading order in any file explorer. Namespaces drop the number (`Problem`, `Classic`, `DotNet`); the `.editorconfig` relaxes the namespace-matches-folder rule (IDE0130) for this reason, with a comment.

### 5.2 Code conventions

- Everything in **English**: code, comments, docs, commit messages.
- Every class that plays a pattern role starts with a role comment and a guide pointer:
  ```csharp
  // Role: ConcreteStrategy — prices shipping by weight for the standard carrier.
  // Guide: §6.9
  ```
- Problem code marks what hurts: `// PAIN: every new carrier means another case here and in the tests.`
- Comments explain *why*, not *what*; no comment repeats the line below it.
- `sealed` classes by default, DTOs and values are `record`s, constructor injection only, time from `TimeProvider`, culture-safe string calls (`InvariantCulture`, `ToUpperInvariant`), logging (where used) through `[LoggerMessage]`.
- Package versions only in `Directory.Packages.props`. Allowed packages: `Microsoft.Extensions.DependencyInjection`, `.Options`, `.Logging`, `.ObjectPool`, `.Hosting`, ASP.NET Core via framework reference; tests: `xunit.v3`, `TngTech.ArchUnitNET.xUnitV3`. **Not allowed:** MediatR, AutoMapper, FluentAssertions, or any other third-party library in `src/`.

## 6. The runner

`Patterns.Runner` is a console app.

- `dotnet run --project src/Patterns.Runner` (or `-- list`): prints every pattern grouped by category, with relevance mark and the command to run it.
- `dotnet run --project src/Patterns.Runner -- <pattern>` (kebab-case, e.g. `strategy`, `chain-of-responsibility`): runs that demo. An unknown name prints the list and exits with code 1.
- `-- all`: runs every demo in catalog order.

Each pattern exposes one `IDemo`:

```csharp
public interface IDemo
{
    string Key { get; }               // "strategy"
    string Name { get; }              // "Strategy"
    PatternCategory Category { get; } // Creational, Structural, Behavioral, Modern
    Relevance Relevance { get; }      // Essential, Useful, Niche, Historical
    string GuideSection { get; }      // "§6.9"
    void Run(TextWriter output);
}
```

A demo narrates the levels in order (`── Level 0 · Problem ──`, `── Level 1 · Classic ──`, `── Level 2 · .NET ──`), says what it is about to do, does it with the shop example, and prints the result plus a one-line takeaway. It writes to the given `TextWriter` (not `Console`) so it can be tested. Each category project exposes its demos through a static `Demos.All` list; the runner concatenates the four lists (no reflection, so the wiring stays readable).

## 7. Testing

xUnit v3 with plain `Assert`, on Microsoft.Testing.Platform.

- **Pattern tests** (`Patterns.<Category>.Tests`, one class per pattern, e.g. `StrategyTests`):
  - **Equivalence:** with the same inputs, Problem, Classic and DotNet give the same result. The pattern changes structure, not behaviour.
  - **Pattern behaviour:** what makes the pattern the pattern (undo restores the cart in Command, a forbidden transition throws in State, the decorator order changes the price in Decorator, the singleton is one instance per container in Singleton…).
  - Written **before** the implementation (TDD).
- **Runner tests** (`Patterns.Runner.Tests`): every demo runs without throwing and writes output; the catalog has exactly the 31 patterns of §3 with their relevance; keys are unique; an unknown key returns exit code 1.
- **Architecture tests** (`Patterns.ArchitectureTests`, ArchUnitNET), one test per rule, named after it, with a comment on why:
  - `Shop_DependsOnNothing` (only BCL).
  - `Demo_DependsOnNothing`.
  - `Pattern_DoesNotUseAnotherPattern` (no namespace `Patterns.<Cat>.<A>` uses `Patterns.<Cat2>.<B>`).
  - `Classic_DoesNotUseProblem` and `DotNet_DoesNotUseProblem`.
  - `Classic_DoesNotUseMicrosoftExtensions` (by hand means by hand).
  - `PatternCode_DoesNotUseThirdPartyLibraries` (only `System.*`, `Microsoft.*` and our own namespaces).

## 8. The guide

`docs/DESIGN_PATTERNS_GUIDE.md`: table of contents at the top, Mermaid diagrams (`classDiagram`, `sequenceDiagram`, `flowchart`), every acronym spelled out on first use, every concept introduced in plain language before code, every term in the glossary.

### 8.1 Chapters

1. **Setup** — .NET SDK, VS Code and extensions, everyday commands (runner, tests, format).
2. **Project anatomy** — solution and projects, the folder of a pattern and its three levels, the runner, the kinds of tests, root build files.
3. **Foundations**
   - What a design pattern is and is not; pattern vs idiom vs architecture.
   - The GoF book (1994) and why some patterns aged: C# gained delegates, generics, lambdas, LINQ, records, pattern matching, and .NET gained DI.
   - How to read a class diagram: a Mermaid legend of every arrow used in the guide.
   - The principles patterns rest on: SOLID, composition over inheritance, program to an interface, encapsulate what varies, coupling and cohesion.
   - "Patternitis": using patterns for their own sake.
   - The shop domain, from zero, with a class diagram of `Patterns.Shop`.
4. **Creational patterns** — 4.1 Singleton … 4.5 Prototype.
5. **Structural patterns** — 5.1 Adapter … 5.7 Proxy.
6. **Behavioral patterns** — 6.1 Chain of Responsibility … 6.11 Visitor (order as in §3).
7. **Modern / .NET patterns** — 7.1 Dependency Injection … 7.8 Object Pool.
8. **Patterns that get confused** — one table per group with the difference in one sentence: Decorator / Proxy / Adapter / Facade; Strategy / State / Template Method; Factory Method / Abstract Factory / Builder; Observer / Mediator; Command / Strategy; Composite / Decorator.
9. **Combining and choosing** — real combinations (Composite + Visitor, Decorator + DI, middleware as Chain of Responsibility, Command + Memento for undo); a **symptom → pattern** table; a **when not to use** summary table; the out-of-scope patterns in one line each.
10. **Glossary**.

Section numbers in chapters 4–7 follow the order of §3 and are the `GuideSection` of each demo.

### 8.2 Template of a pattern section

1. **Card** — relevance, category, intent in one sentence, also known as, levels in the code, run command.
2. **The problem** — the situation in the shop in plain language, plus a real-life analogy.
3. **Without the pattern** — the code that hurts and exactly what hurts (link to `0-Problem/` when it exists; a snippet otherwise).
4. **Structure** — Mermaid class diagram with the GoF role names, then a table *role → our class → responsibility*.
5. **How it runs** — Mermaid sequence diagram of one call.
6. **By hand** — the key code, commented.
7. **In .NET** — what the framework gives you, with code, and how it differs from the hand-written version.
8. **In the ecosystem** — known libraries and their licence (only when relevant).
9. **When to use it** — concrete signals.
10. **When NOT to use it** — concrete signals that it is overkill, and the simpler alternative to use instead.
11. **Costs** — what you pay (more types, indirection, debugging…).
12. **Relevance today** — the mark and why.
13. **Relatives** — what it is confused with and what it combines with (links to chapter 8).
14. **Try it** — runner command and 2–3 experiments.
15. **Interview questions** — 2–3, answers inside `<details>`.

## 9. Phases

One commit per phase on `feature/design-patterns-playground` (created from `develop`), Conventional Commits, pushed to `origin`. The owner merges into `develop`; the agent never merges.

| # | Commit | Content |
|---|---|---|
| 0 | `docs(spec): add the design patterns playground design` | this spec |
| 1 | `docs(guide): add setup, project anatomy and foundations` | full table of contents + chapters 1–3 |
| 2 | `docs(guide): add creational and structural patterns` | chapters 4–5 |
| 3 | `docs(guide): add behavioral patterns` | chapter 6 |
| 4 | `docs(guide): add modern patterns, comparisons and glossary` | chapters 7–10 |
| 5 | `chore(foundations): add build files, solution, shop, runner and architecture tests` | root files, `CLAUDE.md`, `README.md`, `Patterns.Shop`, `Patterns.Demo`, `Patterns.Runner` (with an empty catalog), empty category projects, test projects, architecture tests |
| 6 | `feat(creational): …` | 5 patterns, tests, guide synced |
| 7 | `feat(structural): …` | 7 patterns |
| 8 | `feat(behavioral): …` | 11 patterns |
| 9 | `feat(modern): …` | 8 patterns |

The catalog test (§7) checks the patterns built so far until phase 9, then the full 31.

### 9.1 Done checks

- **Guide phases:** every term used is explained or in the glossary; every Mermaid block renders (checked in a Markdown preview); no contradiction with this spec; table of contents links work.
- **Code phases:**
  ```bash
  dotnet build DesignPatterns.slnx -warnaserror           # 0 warnings, 0 errors
  dotnet test DesignPatterns.slnx                         # all green
  dotnet format DesignPatterns.slnx --verify-no-changes
  dotnet run --project src/Patterns.Runner -- all         # every demo of the phase runs
  ```
  plus the pattern READMEs, the root README catalog and the guide synced with the code.
- **Every phase:** a fresh reviewer subagent reviews the uncommitted diff against this spec; Critical and Important findings are fixed, skipped Minor ones are listed in the phase report.

## 10. Workflow and constraints

- The agent does not install software; it asks the owner.
- Prerequisites: .NET SDK 10.0.401 (installed), VS Code. No Docker.
- `CLAUDE.md` (phase 5) records layout, conventions, commands and workflow for future sessions, and the rule **keep the guide in sync with the code**.
