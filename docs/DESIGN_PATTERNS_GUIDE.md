# Design Patterns Playground — Guide

This guide explains **31 design patterns** from zero: the 23 classic ones from the *Gang of Four* book and 8 that every modern .NET application uses. Each pattern is shown on the same small online shop and, in the code, at up to three levels:

| Level | Folder | What it shows |
|---|---|---|
| 0 · Problem | `0-Problem/` | The code **without** the pattern, and exactly what hurts in it. |
| 1 · Classic | `1-Classic/` | The pattern **by hand**, in plain C#, so you can see every moving part. |
| 2 · .NET | `2-DotNet/` | The same idea with **what .NET already gives you**: the version you would write at work. |

Every pattern also gets a **relevance mark**, because not all patterns deserve the same attention today:

| Mark | Name | Meaning |
|---|---|---|
| ⭐⭐⭐ | Essential | You will use it, or meet it in code you read, almost every day. |
| ⭐⭐ | Useful | It has its place; you should recognise it and know when it pays off. |
| ⭐ | Niche | It solves specific problems; knowing it exists is enough. |
| 🕰 | Historical | The language or the framework already solves it; you study it to understand older code and the vocabulary. |

Essential and useful patterns have all three levels. Niche and historical ones skip level 0: their "problem" is a short snippet in the guide.

**How to read it.** Chapters 1–3 come first: setup, how the repository is organised, and the foundations every pattern rests on (including the shop used in every example). After that, read the patterns in any order. Chapters 8 and 9 compare patterns that look alike and help you choose. Chapter 10 is the glossary: every term the guide uses is defined there.

## Contents

- [1. Setup](#1-setup)
  - [1.1 The .NET SDK](#11-the-net-sdk)
  - [1.2 VS Code and the recommended extensions](#12-vs-code-and-the-recommended-extensions)
  - [1.3 Everyday commands](#13-everyday-commands)
  - [1.4 How to study a pattern](#14-how-to-study-a-pattern)
- [2. Project anatomy](#2-project-anatomy)
  - [2.1 Solution and projects](#21-solution-and-projects)
  - [2.2 Root build files](#22-root-build-files)
  - [2.3 The folder of a pattern](#23-the-folder-of-a-pattern)
  - [2.4 The runner](#24-the-runner)
  - [2.5 Kinds of tests](#25-kinds-of-tests)
  - [2.6 Rules enforced by tests](#26-rules-enforced-by-tests)
- [3. Foundations](#3-foundations)
  - [3.1 What a design pattern is](#31-what-a-design-pattern-is)
  - [3.2 Pattern, idiom, architecture](#32-pattern-idiom-architecture)
  - [3.3 The GoF book and why some patterns aged](#33-the-gof-book-and-why-some-patterns-aged)
  - [3.4 How to read the diagrams](#34-how-to-read-the-diagrams)
  - [3.5 The principles under the patterns](#35-the-principles-under-the-patterns)
  - [3.6 Patternitis](#36-patternitis)
  - [3.7 How each pattern section is organised](#37-how-each-pattern-section-is-organised)
  - [3.8 The shop](#38-the-shop)
- [4. Creational patterns](#4-creational-patterns)
  - [4.1 Singleton](#41-singleton)
  - [4.2 Factory Method](#42-factory-method)
  - [4.3 Abstract Factory](#43-abstract-factory)
  - [4.4 Builder](#44-builder)
  - [4.5 Prototype](#45-prototype)
- [5. Structural patterns](#5-structural-patterns)
  - [5.1 Adapter](#51-adapter)
  - [5.2 Bridge](#52-bridge)
  - [5.3 Composite](#53-composite)
  - [5.4 Decorator](#54-decorator)
  - [5.5 Facade](#55-facade)
  - [5.6 Flyweight](#56-flyweight)
  - [5.7 Proxy](#57-proxy)
- [6. Behavioral patterns](#6-behavioral-patterns)
  - [6.1 Chain of Responsibility](#61-chain-of-responsibility)
  - [6.2 Command](#62-command)
  - [6.3 Interpreter](#63-interpreter)
  - [6.4 Iterator](#64-iterator)
  - [6.5 Mediator](#65-mediator)
  - [6.6 Memento](#66-memento)
  - [6.7 Observer](#67-observer)
  - [6.8 State](#68-state)
  - [6.9 Strategy](#69-strategy)
  - [6.10 Template Method](#610-template-method)
  - [6.11 Visitor](#611-visitor)
- [7. Modern .NET patterns](#7-modern-net-patterns)
  - [7.1 Dependency Injection](#71-dependency-injection)
  - [7.2 Options](#72-options)
  - [7.3 Repository](#73-repository)
  - [7.4 Unit of Work](#74-unit-of-work)
  - [7.5 Specification](#75-specification)
  - [7.6 Result](#76-result)
  - [7.7 Null Object](#77-null-object)
  - [7.8 Object Pool](#78-object-pool)
- [8. Patterns that get confused](#8-patterns-that-get-confused)
  - [8.1 Decorator, Proxy, Adapter, Facade](#81-decorator-proxy-adapter-facade)
  - [8.2 Strategy, State, Template Method](#82-strategy-state-template-method)
  - [8.3 Factory Method, Abstract Factory, Builder](#83-factory-method-abstract-factory-builder)
  - [8.4 Observer and Mediator](#84-observer-and-mediator)
  - [8.5 Command and Strategy](#85-command-and-strategy)
  - [8.6 Composite and Decorator](#86-composite-and-decorator)
- [9. Combining and choosing](#9-combining-and-choosing)
  - [9.1 Combinations you will meet in real code](#91-combinations-you-will-meet-in-real-code)
  - [9.2 From symptom to pattern](#92-from-symptom-to-pattern)
  - [9.3 When not to use each pattern](#93-when-not-to-use-each-pattern)
  - [9.4 Out of scope](#94-out-of-scope)
- [10. Glossary](#10-glossary)

---

## 1. Setup

What you need to build, run and test the playground, and the handful of commands you will use every day. No database and no Docker: every pattern runs in memory.

### 1.1 The .NET SDK

The **SDK** (Software Development Kit) is the package that contains everything to build .NET code: the C# compiler, the `dotnet` command-line tool, the runtime that executes the programs, and the base class library. The playground targets **.NET 10** with **C# 14**.

```bash
dotnet --list-sdks        # lists installed SDKs; you need a 10.0.4xx line
```

Install it with `winget install Microsoft.DotNet.SDK.10` (Windows) and open a **new** terminal afterwards: a terminal that was already open does not see the updated `PATH`.

The repository pins the SDK in [`global.json`](../global.json):

```json
{
  "sdk": { "version": "10.0.401", "rollForward": "latestFeature" },
  "test": { "runner": "Microsoft.Testing.Platform" }
}
```

- `version` is the SDK the playground was built with.
- `rollForward: latestFeature` means "10.0.401 or any newer **10.0** feature band" (10.0.5xx, 10.0.6xx…), but never 11. You get bug fixes without surprises from a new major version.
- `test.runner` switches `dotnet test` from the older VSTest runner to **Microsoft.Testing.Platform** (MTP), the newer test platform. In .NET 10 this is an opt-in, and this line is what turns it on; the `--solution`, `--project` and `--filter-class` options of section [1.3](#13-everyday-commands) only exist in MTP mode (section [2.5](#25-kinds-of-tests)).

### 1.2 VS Code and the recommended extensions

Any editor works (Visual Studio and Rider too). With **Visual Studio Code** (VS Code), open the repository folder and accept the recommended extensions it offers (listed in `.vscode/extensions.json`):

| Extension | Why |
|---|---|
| **C# Dev Kit** (`ms-dotnettools.csdevkit`) | IntelliSense, go-to-definition, the Solution Explorer and the Test Explorer for C#. |
| **Markdown Preview Mermaid Support** (`bierner.markdown-mermaid`) | Draws the diagrams of this guide in VS Code's Markdown preview (`Ctrl+Shift+V`). GitHub draws them on its own. |
| **EditorConfig** (`editorconfig.editorconfig`) | Applies the formatting rules of `.editorconfig` as you type (section [2.2](#22-root-build-files)). |

### 1.3 Everyday commands

Run them from the repository root.

```bash
dotnet run --project src/Patterns.Runner                   # list every pattern with its relevance
dotnet run --project src/Patterns.Runner -- strategy       # run one pattern's demo, narrated level by level
dotnet run --project src/Patterns.Runner -- all            # run every demo

dotnet build DesignPatterns.slnx -warnaserror              # build everything; any warning fails the build
dotnet test --solution DesignPatterns.slnx                 # run every test
dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.StrategyTests"   # one pattern's tests
dotnet format DesignPatterns.slnx --verify-no-changes      # check the formatting without changing files
```

- `--project` tells `dotnet run` which project to start; everything after `--` is passed to the program, not to `dotnet`.
- `-warnaserror` turns every compiler and analyzer warning into an error (the build files already do this; the flag makes it explicit).
- `--filter-class` keeps only the test classes whose full name (namespace + class) matches; `*` is a wildcard. xUnit v3 also offers `--filter-method` and `--filter-namespace`.
- `dotnet format` applies the `.editorconfig` rules; with `--verify-no-changes` it only reports what it *would* change and fails if anything is off.

### 1.4 How to study a pattern

The same five steps work for every pattern:

1. **Run its demo** (`dotnet run --project src/Patterns.Runner -- <key>`). The output narrates each level with the shop example and ends with a one-line takeaway.
2. **Read the guide section** up to "Structure": the problem, the analogy and the diagram.
3. **Read the code in order**: `0-Problem/` (look for the `// PAIN:` comments), then `1-Classic/` (each class says which role it plays), then `2-DotNet/`.
4. **Read the tests**. The equivalence tests prove the three levels behave the same; the other tests show what makes the pattern the pattern (undo in Command, forbidden transitions in State…).
5. **Do the "Try it" experiments** at the end of the section. Breaking something on purpose and watching a test fail teaches more than reading.

---

## 2. Project anatomy

How the repository is organised: which project holds what, how a pattern's folder is laid out, how the runner finds the demos, and which tests guard what.

### 2.1 Solution and projects

A **solution** (`DesignPatterns.slnx`) is the list of projects that are built together; `.slnx` is the XML (Extensible Markup Language) solution format introduced in .NET 9, much shorter than the old `.sln`. A **project** (`.csproj`) produces one assembly (a `.dll` or an `.exe`) and lists the packages and other projects it references. A **project reference** is a compile-time permission: a project can only use the types of the projects it references.

```mermaid
flowchart LR
    subgraph src
        Shop[Patterns.Shop<br/>the shop domain]
        Demo[Patterns.Demo<br/>IDemo, Narrator]
        Cre[Patterns.Creational]
        Str[Patterns.Structural]
        Beh[Patterns.Behavioral]
        Mod[Patterns.Modern]
        Run[Patterns.Runner<br/>console]
    end
    Cre --> Shop
    Cre --> Demo
    Str --> Shop
    Str --> Demo
    Beh --> Shop
    Beh --> Demo
    Mod --> Shop
    Mod --> Demo
    Run --> Cre
    Run --> Str
    Run --> Beh
    Run --> Mod
```

An arrow means "references". `Patterns.Shop` and `Patterns.Demo` reference nothing, so every pattern can use them and they can never depend on a pattern.

| Project | What it holds | References |
|---|---|---|
| `Patterns.Shop` | The shop domain shared by every example: `Product`, `Category`, `Customer`, `Address`, `Order`, `OrderLine`, `OrderStatus`, and `SampleData` (section [3.8](#38-the-shop)). | nothing |
| `Patterns.Demo` | The contract every demo implements (`IDemo`), the relevance marks, and `Narrator`, which prints the levels in a uniform format. | nothing |
| `Patterns.Creational` | Singleton, Factory Method, Abstract Factory, Builder, Prototype. | Shop, Demo, Microsoft DI (dependency injection) container |
| `Patterns.Structural` | Adapter, Bridge, Composite, Decorator, Facade, Flyweight, Proxy. | Shop, Demo, Microsoft Logging, Configuration |
| `Patterns.Behavioral` | Chain of Responsibility, Command, Interpreter, Iterator, Mediator, Memento, Observer, State, Strategy, Template Method, Visitor. | Shop, Demo, Microsoft DI, ASP.NET Core (to run real middleware in memory) |
| `Patterns.Modern` | Dependency Injection, Options, Repository, Unit of Work, Specification, Result, Null Object, Object Pool. | Shop, Demo, Microsoft DI, Options, Configuration, Logging, ObjectPool |
| `Patterns.Runner` | The console program that lists and runs the demos. | the four category projects |

The tests live in `tests/`: one project per category (`Patterns.Creational.Tests`…), `Patterns.Runner.Tests` and `Patterns.ArchitectureTests` (section [2.5](#25-kinds-of-tests)).

Why one project per **category** and not one per pattern? Thirty-one projects of two or three files each would be mostly ceremony. The price of sharing a project is that the compiler would let one pattern use another; an architecture test forbids it instead (section [2.6](#26-rules-enforced-by-tests)).

### 2.2 Root build files

Five files at the root apply to every project, so each is written once. MSBuild, the engine behind `dotnet build`, finds `Directory.Build.props` and `Directory.Packages.props` by walking up from each project's folder; the compiler and the editor find `.editorconfig` the same way; the `dotnet` command finds `global.json` walking up from the folder you run it in; Git reads `.gitattributes`.

**`Directory.Build.props`** — compiler settings for every project:

| Setting | Effect |
|---|---|
| `TargetFramework` `net10.0` | Every project targets .NET 10. |
| `Nullable` `enable` | Reference types are non-nullable unless marked `?`; the compiler warns when `null` can sneak in. |
| `ImplicitUsings` `enable` | Common namespaces (`System`, `System.Linq`, `System.Collections.Generic`…) are imported automatically. |
| `TreatWarningsAsErrors` `true` | A warning stops the build. Warnings that nobody fixes accumulate until nobody reads them. |
| `AnalysisLevel` `latest-recommended` | Turns on the recommended set of .NET code analyzers. An **analyzer** is a compiler plug-in that inspects your code and reports problems; these rules are named `CAxxxx` (code analysis), for example CA1305 "specify a culture when formatting". |
| `EnforceCodeStyleInBuild` `true` | Style rules from `.editorconfig` (rules named `IDExxxx`, after IDE: integrated development environment) are checked during the build, not only in the editor. |
| `InvariantGlobalization` `true` | The programs behave the same on every machine's language settings: `12.50` never prints as `12,50`. |
| `ManagePackageVersionsCentrally` `true` | Package versions live in one file (next row). |
| `GenerateDocumentationFile` `true`, `NoWarn` `CS1591` | Needed for the build to report unused `using` directives (IDE0005). XML documentation comments stay optional, so the "missing XML comment" warning (CS1591) is off. |

**`Directory.Packages.props`** — **central package management**: the only place where package versions are written. A `.csproj` lists `<PackageReference Include="xunit.v3" />` without a version. Every project therefore uses exactly the same version of each library.

**`.editorconfig`** — formatting and style rules (indentation, file-scoped namespaces, `_camelCase` private fields, braces always…), read by the editor, the build and `dotnet format`. One rule is relaxed on purpose: **IDE0130** normally asks the namespace to match the folder, but pattern folders are numbered (`1-Classic`) for reading order, and a namespace cannot contain `1-`. The namespace drops the number (`Patterns.Behavioral.Strategy.Classic`). A second relaxation applies only to `0-Problem` folders: **CA1822** ("this method uses no instance data, make it static") is off there, because Problem code imitates services as they are usually written (instance classes registered in DI), and the lesson is about another pain.

**`global.json`** — the SDK version (section [1.1](#11-the-net-sdk)).

**`.gitattributes`** — stores every text file with LF line endings, on every operating system. `.editorconfig` asks for LF and `dotnet format --verify-no-changes` checks it, so a Windows checkout that converted files to CRLF would otherwise fail the format check.

A test project with no tests yet would make `dotnet test` fail (Microsoft.Testing.Platform exits with code 8, "zero tests ran"). While the repository was being built, each category's test project ignored that code through `<TestingPlatformCommandLineArguments>--ignore-exit-code 8</TestingPlatformCommandLineArguments>` until its first tests arrived; every category has tests now, so no project carries the line. A new, still empty test project would need it.

### 2.3 The folder of a pattern

Every pattern has the same shape. Strategy, for example:

```
src/Patterns.Behavioral/Strategy/
  README.md          relevance, intent, what to read first, levels, run command, link to this guide
  0-Problem/         namespace Patterns.Behavioral.Strategy.Problem   (essential and useful patterns only)
  1-Classic/         namespace Patterns.Behavioral.Strategy.Classic
  2-DotNet/          namespace Patterns.Behavioral.Strategy.DotNet
  StrategyDemo.cs    namespace Patterns.Behavioral.Strategy           (what the runner executes)
```

**The levels never share the pattern's own types.** If Problem and Classic both need a "shipping calculator", each level declares its own. That duplication is deliberate: each level must be readable on its own, and comparing two files side by side is the point. The one exception is "someone else's code" that the pattern wraps or coordinates (the carrier SDK in Adapter, the subsystems in Facade, the image store in Proxy): it lives in its own sub-folder of the pattern and every level uses the same one, because in real life you could not change it either. The other exception is Interpreter's .NET level ([6.3](#63-interpreter)), which compiles the tree the Classic parser builds: that tree is its input. Types that a pattern needs and the shop does not have (a carrier client, a notifier, a bundle…) live in the pattern's folder, not in `Patterns.Shop`.

**Comments are part of the lesson.** Three kinds appear in pattern code:

```csharp
// Role: ConcreteStrategy — prices express shipping: a fixed fee plus one euro per unit.
// Guide: §6.9
public sealed class ExpressShipping : IShippingStrategy
{
    public decimal CostFor(Order order) => 9.99m + 1.00m * order.Units;
}
```

```csharp
public decimal CostFor(Order order, string method) => method switch
{
    "standard" => order.Total >= 50.00m ? 0.00m : 4.99m,
    "express" => 9.99m + 1.00m * order.Units,
    "pickup" => 0.00m,
    // PAIN: every new carrier means another case here, and this method's tests change with it.
    _ => throw new ArgumentException($"Unknown shipping method '{method}'."),
};
```

- `// Role:` names the role the class plays in the pattern (the names are the ones used in the pattern's class diagram) and says, in a few words, what it does in the shop.
- `// Guide: §N.M` points to the section of this guide that explains it.
- `// PAIN:` marks, in Problem code only, the exact spot that hurts and why.

Other comments explain **why** something is done, never *what* the next line does.

### 2.4 The runner

`Patterns.Runner` is a console program. Every pattern exposes one **demo**, a class implementing `IDemo`:

```csharp
public interface IDemo
{
    string Key { get; }               // "strategy": what you type after `--`
    string Name { get; }              // "Strategy"
    PatternCategory Category { get; } // Creational, Structural, Behavioral, Modern
    Relevance Relevance { get; }      // Essential, Useful, Niche, Historical
    string GuideSection { get; }      // "§6.9"
    void Run(TextWriter output);
}
```

`Run` writes to the `TextWriter` it receives, not to `Console`, so a test can capture and check the output. Each category project lists its demos in a static `Demos.All`, and the runner simply concatenates the four lists: there is no reflection or scanning, so following how a demo gets run takes one "go to definition".

| Command | What happens |
|---|---|
| `dotnet run --project src/Patterns.Runner` (or `-- list`) | Lists the patterns by category with their relevance mark and key (`(none yet)` under a category whose patterns are not built yet). |
| `-- <key>` | Runs that demo. The key is trimmed and case-insensitive: `Strategy`, `strategy` and `STRATEGY` are the same. Extra arguments are ignored. |
| `-- all` | Runs every demo, in the order of this guide. |
| anything else | Prints `Unknown pattern '<what you typed>'.` and the list, and exits with code 1 (so scripts can detect the mistake). |

A demo narrates through `Narrator`, so every demo looks the same. Strategy's output looks like this:

```
═══ Strategy (Behavioral · ⭐⭐⭐ Essential) · guide §6.9 ═══
── Level 0 · Problem ──
  → Shipping for 3 books (37.50) through one method with a switch on the method name
    = standard 4.99 · express 12.99 · pickup 0.00
── Level 1 · Classic ──
  → The same order, priced by three strategy objects behind IShippingStrategy
    = standard 4.99 · express 12.99 · pickup 0.00
── Level 2 · .NET ──
  → The strategies registered as keyed services and picked by name from the container
    = standard 4.99 · express 12.99 · pickup 0.00
  ✔ Same prices at every level: the pattern changes where the rules live, not what they compute.
```

`→` is a step the demo takes, `=` its result, `✔` the takeaway.

### 2.5 Kinds of tests

Tests use **xUnit v3** with its plain `Assert` class, run by **Microsoft.Testing.Platform** (`dotnet test`). Four kinds, each with a different job:

| Kind | Where | What it proves | Example |
|---|---|---|---|
| **Equivalence** | `Patterns.<Category>.Tests` | With the same input, Problem, Classic and .NET give the same result. A pattern changes the *structure* of the code, not its *behaviour*; these tests make that visible. | Shipping for 3 books is `4.99` / `12.99` / `0.00` in all three levels. |
| **Pattern behaviour** | `Patterns.<Category>.Tests` | What makes the pattern the pattern, including the edge cases. | Undo three times empties the cart (Command); shipping an order that is not paid throws (State); applying the coupon before or after VAT (value-added tax) gives different prices (Decorator). |
| **Runner** | `Patterns.Runner.Tests` | Every demo runs and writes output; the catalog holds exactly the expected patterns with their relevance; keys are unique; an unknown key exits with 1. | `UnknownKey_PrintsErrorAndList_Returns1` |
| **Architecture** | `Patterns.ArchitectureTests` | The structural rules of the repository (next section). | `Pattern_DoesNotUseAnotherPattern` |

Test names describe the behaviour, with underscores separating the parts: `Standard_AtExactly50_IsFree` reads as "standard shipping, at exactly 50, is free". The tests of a pattern are written **before** its code (TDD, Test-Driven Development: write a failing test, make it pass, clean up).

### 2.6 Rules enforced by tests

Some rules cannot be expressed with project references alone, so `Patterns.ArchitectureTests` checks them with **ArchUnitNET**, a library that loads the compiled assemblies and asserts who uses whom. One test per rule, named after the rule:

| Rule (test name) | Why it exists |
|---|---|
| `Shop_DependsOnNothing` | The shop is shared by every pattern. If it used a pattern, changing that pattern could break all the others. |
| `Demo_DependsOnNothing` | The demo contract must stay a neutral, tiny contract. |
| `Pattern_DoesNotUseAnotherPattern` | Each pattern must be understandable alone. Patterns share a project per category, so the compiler would allow it; this test does not. |
| `Classic_DoesNotUseProblem`, `DotNet_DoesNotUseProblem` | The Problem level is the "before" picture; the solutions must not lean on it. |
| `Classic_DoesNotUseMicrosoftExtensions` | "By hand" means by hand: the Classic level uses only the base class library, so every moving part of the pattern is visible. |
| `PatternCode_DoesNotUseThirdPartyLibraries` | The code shows the patterns and what .NET provides, nothing else. Third-party libraries (MediatR, AutoMapper, Scrutor…) are named in the guide, never referenced. |

If a change makes one of these tests fail, the design is what changes, never the test.

---

## 3. Foundations

Before the first pattern: what a pattern is (and is not), why some of the classic ones have aged, how to read the diagrams of this guide, the principles every pattern leans on, the risk of using patterns for their own sake, and the shop that every example uses.

### 3.1 What a design pattern is

A **design pattern** is a named, proven solution to a problem that keeps coming back when you design code, described in a way you can adapt to your own situation. It is a **description**, not a piece of code or a library: you cannot install Strategy, you recognise the situation and shape your classes after it.

The idea comes from building architecture. In the 1970s the architect Christopher Alexander catalogued recurring solutions for towns and houses ("a window place", "light on two sides of every room"), each with a name, the problem it solves and the forces that shape it. Programmers borrowed the format in the late 1980s.

Every pattern has four parts:

| Part | What it says | Strategy, for example |
|---|---|---|
| **Name** | A word to talk about the design. | "Strategy" |
| **Problem** | When to apply it: the situation and the forces in tension. | One task (pricing shipping) has several interchangeable ways of being done, and new ways keep arriving. |
| **Solution** | The classes, their roles and how they collaborate; a template, not finished code. | Each way becomes a class behind a common interface; the code that needs the price receives one of them. |
| **Consequences** | What you gain and what you pay. | Adding a way no longer edits existing code; you pay with more types and an extra indirection. |

The name is the most underrated part. "Wrap the repository in a caching decorator" is one sentence that carries a whole design: everyone who knows the vocabulary knows which interface, which classes and which call order you mean. That shared vocabulary, more than any line of code, is what you take from this playground.

A pattern is **not**:
- a **goal**: nobody is paid to "use patterns"; they are paid to make code that is easy to change;
- a **recipe to copy**: the structure adapts to the language and the problem (most patterns look different in C# 14 than in 1994's C++);
- **always an improvement**: every pattern adds types and indirection. Section [3.6](#36-patternitis) is about that cost.

### 3.2 Pattern, idiom, architecture

Patterns exist at different scales. This playground is about the middle one.

| Scale | What it organises | Examples | Where you learn it |
|---|---|---|---|
| **Idiom** | A few lines, in one language. | `using` to dispose resources, `TryParse` instead of exceptions, `is not null`, `?.` and `??`. | The language's documentation and code reviews. |
| **Design pattern** | A few classes and how they collaborate, in any object-oriented language. | Strategy, Decorator, Observer. | This guide. |
| **Architecture pattern** | The whole application: its big parts and the rules between them. | Layered, Clean/Hexagonal, Vertical slices, Microservices. | Architecture books (the sibling repository `architecture-playground`, if you have it). |

The boundaries are blurry: Dependency Injection is a design pattern that ends up shaping the architecture; middleware is a design pattern (Chain of Responsibility) that the ASP.NET Core architecture is built around. The useful question is not "which scale is this?" but "how many classes does this decision touch?".

### 3.3 The GoF book and why some patterns aged

In 1994 Erich Gamma, Richard Helm, Ralph Johnson and John Vlissides published *Design Patterns: Elements of Reusable Object-Oriented Software*. The four authors became known as the **Gang of Four** (**GoF**), and their 23 patterns, with examples in C++ and Smalltalk, are still the common vocabulary. They grouped them in three families, which are chapters 4–6 of this guide:

| Family | The question it answers | Chapter |
|---|---|---|
| **Creational** | Who creates an object, and how, so that the code using it does not depend on the concrete class? | [4](#4-creational-patterns) |
| **Structural** | How do we wrap and combine objects into larger structures? | [5](#5-structural-patterns) |
| **Behavioral** | Who does what, and how do objects talk to each other? | [6](#6-behavioral-patterns) |

The C++ of 1994 had no garbage collector, no lambdas, no generics in the modern sense, no interfaces as a separate concept, no reflection worth the name and no standard collection library. Many GoF patterns are partly **workarounds for missing language features**. In 1996 Peter Norvig showed that 16 of the 23 have simpler or invisible implementations in dynamic languages such as Lisp and Dylan, thanks to first-class functions and types, macros and multimethods (at least for some uses). C# and .NET have since absorbed many of them:

| Language or framework feature | Pattern it absorbed | What is left to learn |
|---|---|---|
| Delegates and lambdas (`Func<>`, `Action`) | Strategy, Command, Template Method (partly) | When a whole class still beats a lambda. |
| `IEnumerable<T>`, `foreach`, `yield return` | Iterator | How lazy enumeration works and when it bites. |
| `event`, `IObservable<T>` | Observer | Unsubscribing, and what happens when a subscriber throws. |
| Records and `with` | Prototype, Memento | Shallow vs deep copies. |
| Pattern matching (`switch` expressions) | Visitor (often), State (simple cases) | When a class hierarchy still wins. |
| The dependency injection container | Singleton, Factory Method, Abstract Factory (partly) | Lifetimes, and why a static singleton is now a smell. |
| The garbage collector and string interning | Flyweight (mostly) | Why it rarely matters in .NET. |

That table is why relevance marks exist (see the introduction). A 🕰 historical pattern is not useless: you still meet it in older code and in interviews, and knowing what the language does for you is part of knowing the language.

The GoF book did not cover patterns that became everyday .NET practice later: Dependency Injection, Options, Repository, Unit of Work, Specification, Result, Null Object, Object Pool. They are chapter [7](#7-modern-net-patterns).

### 3.4 How to read the diagrams

Each pattern has a **class diagram** (which types exist and how they relate) and a **sequence diagram** (what happens, in order, during one call). Both are drawn with **Mermaid**, a text format that GitHub and VS Code (with the extension of section [1.2](#12-vs-code-and-the-recommended-extensions)) turn into pictures.

**Class diagrams.** Every relation used in this guide, with the C# that produces it:

```mermaid
classDiagram
    class IShippingStrategy {
        <<interface>>
        +CostFor(Order order) decimal
    }
    class ExpressShipping {
        +CostFor(Order order) decimal
    }
    class OrderExporter {
        <<abstract>>
        +Export(orders) string
        #Row(Order order, bool isLast)* string
    }
    class CsvOrderExporter {
        #Row(Order order, bool isLast) string
    }
    class ShippingCalculator {
        -IShippingStrategy _strategy
    }
    IShippingStrategy <|.. ExpressShipping : realization
    OrderExporter <|-- CsvOrderExporter : inheritance
    ShippingCalculator --> IShippingStrategy : association
    Bundle o-- ICatalogItem : aggregation
    Order *-- OrderLine : composition
    ReceiptPrinter ..> Order : dependency
```

| Relation | Arrow | Meaning | In C# |
|---|---|---|---|
| **Realization** | dashed line, hollow triangle | A class implements an interface. | `class ExpressShipping : IShippingStrategy` |
| **Inheritance** | solid line, hollow triangle | A class extends another class. | `class CsvOrderExporter : OrderExporter` |
| **Association** | solid line, open arrow | A class keeps a reference to another and uses it over time. | a field: `private readonly IShippingStrategy _strategy;` |
| **Aggregation** | solid line, hollow diamond on the owner's side | A "has" relation where the parts can live without the whole. | `Bundle` holds items that also exist outside it. |
| **Composition** | solid line, filled diamond on the owner's side | The parts belong to the whole and live and die with it. | An `Order`'s lines mean nothing without the order. |
| **Dependency** | dashed line, open arrow | A class uses another only briefly: a parameter, a local variable, a return type. | `string Print(Order order)` |

Inside a class box: `+` public, `-` private, `#` protected; `Name(parameters) ReturnType`; a trailing `*` marks an abstract member. `<<interface>>` and `<<abstract>>` above the name say what kind of type it is.

In pattern sections, the label above the name is the **role** the class plays in the pattern, using the GoF names (`<<Strategy>>`, `<<ConcreteStrategy>>`, `<<Context>>`). The same role appears in the class's `// Role:` comment, so you can go from the diagram to the code and back. Interfaces are recognisable by their `I` prefix.

**Sequence diagrams.** Time runs downwards. Each column is an object (a *participant*); a solid arrow is a call, a dashed arrow is the value returned; a note explains a step.

```mermaid
sequenceDiagram
    participant Client
    participant Calculator as ShippingCalculator
    participant Strategy as ExpressShipping
    Client->>Calculator: CostFor(order)
    Calculator->>Strategy: CostFor(order)
    Note right of Strategy: 9.99 + 1.00 per unit
    Strategy-->>Calculator: 12.99
    Calculator-->>Client: 12.99
```

Some sections also use a **flowchart** (boxes and arrows for a process) or a **state diagram** (the states something can be in and the events that move it between them; see the order life cycle in [3.8](#38-the-shop)).

### 3.5 The principles under the patterns

Patterns are concrete applications of a few general principles. When you understand the principle, the pattern stops being something to memorise.

#### SOLID

Five principles of object-oriented design gathered by Robert C. Martin; the initials spell SOLID.

| Principle | In plain words | In the shop | Patterns that lean on it |
|---|---|---|---|
| **S**ingle Responsibility | A class should have one reason to change. | Pricing shipping and sending the confirmation email change for different reasons (a new carrier, a new email template), so they belong in different classes. | Facade, Command, Observer |
| **O**pen/Closed | Open for extension, closed for modification: add behaviour by adding code, not by editing working code. | A new shipping method is a new class, not a new `case` in an existing `switch`. | Strategy, Decorator, Chain of Responsibility, Visitor |
| **L**iskov Substitution | Any implementation of an abstraction must work wherever the abstraction is expected, without surprises. | Every `IShippingStrategy` returns a non-negative price; one that threw for some countries would break code that trusts the interface. | Every pattern built on an interface |
| **I**nterface Segregation | Many small interfaces are better than one large one; no class should implement methods it does not need. | A notifier that only sends does not have to implement "read inbox". | Adapter, Bridge, Repository (specific vs generic) |
| **D**ependency Inversion | High-level code should depend on abstractions, not on concrete low-level classes; the details depend on the abstraction. | The checkout depends on `IEmailSender`, not on an SMTP (Simple Mail Transfer Protocol) client. | Dependency Injection, Factory Method, Abstract Factory, Adapter |

#### Composition over inheritance

**Inheritance** (`class B : A`) reuses code by becoming a kind of the parent: it is fixed at compile time, exposes the parent's protected internals to the child, and a class can only have one parent. **Composition** reuses code by *holding* another object and delegating to it: the held object can be swapped at run time and only its public contract is visible.

The GoF advice is "favour object composition over class inheritance". In the shop: an `ExpressCsvExporter : CsvExporter : OrderExporter` hierarchy explodes as soon as two independent things vary (format and filter); an exporter that *holds* a format object and a filter object does not. Decorator, Strategy, Bridge and Composite are all forms of composition. Template Method is the GoF pattern built on inheritance, and its section explains when that is still fine.

#### Program to an interface, not an implementation

Code that needs a service should know **what** it does (the interface), not **which class** does it. Then the class can change, be replaced in a test, or be wrapped, without touching the code that uses it. In C# this usually means depending on an interface (`IShippingStrategy`) or an abstract class, and receiving the concrete object from outside (section [7.1](#71-dependency-injection)).

"Interface" here means the contract, not necessarily the `interface` keyword: a `Func<Order, decimal>` is also a contract.

#### Encapsulate what varies

Find the part of the code that changes often or in different ways, and put it behind its own boundary so that the rest does not change with it. Almost every behavioral pattern is this principle applied to one kind of variation: *how* something is computed (Strategy), *what happens next* depending on the state (State), *who reacts* to an event (Observer), *which steps* run in a request (Chain of Responsibility).

#### Coupling and cohesion

- **Coupling** is how much one piece of code depends on another. High coupling means a change in one place forces changes in others. Patterns lower it by putting an abstraction between the two sides.
- **Cohesion** is how much the things inside one piece of code belong together. High cohesion means a class does one job and everything in it serves that job.

The goal is **low coupling and high cohesion**: small, focused pieces that know little about each other.

```mermaid
flowchart LR
    subgraph before [High coupling]
        OS1[OrderService] --> E1[SmtpEmailSender]
        OS1 --> S1[StockUpdater]
        OS1 --> A1[Analytics]
    end
    subgraph after [Low coupling, Observer]
        OS2[OrderService] --> P[IOrderObserver]
        E2[EmailObserver] -.-> P
        S2[StockObserver] -.-> P
        A2[AnalyticsObserver] -.-> P
    end
```

On the left, `OrderService` knows three concrete classes and changes whenever a new reaction to an order is added. On the right it knows one interface; reactions are added without touching it.

### 3.6 Patternitis

**Patternitis** is the habit of applying patterns because they exist, not because the code needs them. It is the most common way patterns make code worse. Symptoms:

- an interface with exactly one implementation, and no test or plan that needs a second one;
- a factory whose only job is to call `new`;
- a strategy for something that has had one behaviour for years;
- three classes and two interfaces where a `switch` of four lines was clear;
- a reader has to open five files to follow one call.

Every pattern trades **directness** for **flexibility**. Indirection is a cost paid every time someone reads, debugs or navigates the code; flexibility only pays when the change it prepares for actually happens.

Two rules of thumb keep the balance:

- **YAGNI** (*You Aren't Gonna Need It*): do not build for a change you only imagine. Write the simple version.
- **The rule of three**: the first time, just write it; the second time, notice the duplication; the third time, refactor. A pattern usually earns its place when the third variation arrives, not before.

The healthy direction is **refactoring towards a pattern** when the code starts to hurt, not designing with patterns up front. That is exactly the order of the levels in this playground: Problem first, then the pattern that removes the pain. Every pattern section has a "When NOT to use it" part with the simpler alternative.

### 3.7 How each pattern section is organised

Every pattern in chapters 4–7 follows the same template, so you always know where to look:

| # | Part | What it contains |
|---|---|---|
| 1 | **Card** | Relevance, family, intent in one sentence, other names, the levels in the code, the command to run its demo. |
| 2 | **The problem** | The situation in the shop, in plain language, and an analogy from everyday life. |
| 3 | **Without the pattern** | The code that hurts and what exactly hurts (from `0-Problem/`, or a snippet for niche and historical patterns). |
| 4 | **Structure** | The class diagram with the role names, and a table: role → our class → responsibility. |
| 5 | **How it runs** | A sequence diagram of one call. |
| 6 | **By hand** | The key code of `1-Classic/`, commented. |
| 7 | **In .NET** | What the framework gives you (`2-DotNet/`), and how it differs from the hand-written version. |
| 8 | **In the ecosystem** | Well-known libraries for the pattern and their licence (only when there are some). |
| 9 | **When to use it** | Concrete signals in your code. |
| 10 | **When NOT to use it** | Concrete signals that it is overkill, and the simpler alternative. |
| 11 | **Costs** | What you pay: more types, indirection, harder debugging… |
| 12 | **Relevance today** | The mark and why. |
| 13 | **Relatives** | What it is confused with and what it combines with (chapter 8 goes deeper). |
| 14 | **Try it** | The demo command and two or three experiments. |
| 15 | **Interview questions** | Two or three questions; click to reveal the answers. |

### 3.8 The shop

Every example happens in the same small **online shop**, so that you only have to learn the domain once. It is deliberately tiny: just enough for the patterns to have something real to work on. The types live in `src/Patterns.Shop` and depend on nothing.

| Type | What it is |
|---|---|
| `Category` | A product family: Books, Electronics, Home. |
| `Product` | Something the shop sells: an id, a name, a **SKU** (Stock Keeping Unit: the shop's own product code, like `BOOK-001`), a price, its category and the units in stock. |
| `Customer` | Who buys: a name, an email and whether they are a **guest** (bought without an account). |
| `Address` | Where an order is shipped. `Country` uses two-letter ISO (International Organization for Standardization) codes: `ES` Spain, `PT` Portugal, `FR` France. |
| `OrderLine` | One product in an order and how many units; `LineTotal` = price × quantity. |
| `Order` | A customer's purchase: its lines, the shipping address and its status. `Total` is the sum of the line totals; `Units` the number of items. |
| `OrderStatus` | Where the order is in its life: `Draft`, `Placed`, `Paid`, `Shipped`, `Cancelled`. |

All of them except the `OrderStatus` enum are C# **records**: types whose equality compares values instead of references, and which can be copied with changes using `with` (`order with { Status = OrderStatus.Paid }`). Money is `decimal` (exact for amounts, unlike `double`), in a single currency (euros), with at most two decimals. Ids are `Guid`s.

```mermaid
classDiagram
    class Category {
        +string Name
    }
    class Product {
        +Guid Id
        +string Name
        +string Sku
        +decimal Price
        +Category Category
        +int Stock
    }
    class Customer {
        +Guid Id
        +string Name
        +string Email
        +bool IsGuest
    }
    class Address {
        +string Country
        +string City
        +string PostalCode
    }
    class OrderLine {
        +Product Product
        +int Quantity
        +decimal LineTotal
    }
    class Order {
        +Guid Id
        +Customer Customer
        +IReadOnlyList~OrderLine~ Lines
        +Address ShippingAddress
        +OrderStatus Status
        +decimal Total
        +int Units
    }
    class OrderStatus {
        <<enumeration>>
        Draft
        Placed
        Paid
        Shipped
        Cancelled
    }
    Product --> Category
    OrderLine --> Product
    Order *-- OrderLine
    Order --> Customer
    Order --> Address
    Order --> OrderStatus
```

**Sample data.** `SampleData` gives every demo and test the same products, customers and addresses, with fixed product and customer ids so the output is stable (orders built with `OrderOf` get a new id each time):

| Name | Value |
|---|---|
| `SampleData.Book` | "Clean Code", SKU `BOOK-001`, **12.50**, Books, 20 in stock |
| `SampleData.Headphones` | "Wireless Headphones", SKU `ELEC-001`, **59.90**, Electronics, 5 in stock |
| `SampleData.Mug` | "Coffee Mug", SKU `HOME-001`, **8.00**, Home, **0 in stock** (the out-of-stock case, on purpose) |
| `SampleData.Ana` | A registered customer, `ana@example.com` |
| `SampleData.Guest` | A guest customer (no account, no email) |
| `SampleData.Madrid` | `ES`, Madrid, 28001 |
| `SampleData.Lisbon` | `PT`, Lisbon, 1100-148 |
| `SampleData.OrderOf((Book, 2), (Headphones, 1))` | A draft order for Ana, shipped to Madrid: total **84.90**, 3 units |

With those prices the numbers in the examples are easy to check by hand: 4 books are exactly 50.00, 8 books exactly 100.00.

**The order life cycle.** An order starts as a draft, is placed, paid and shipped; it can be cancelled until it has been shipped. Section [6.8](#68-state) (State) implements exactly this diagram three times.

```mermaid
stateDiagram-v2
    [*] --> Draft
    Draft --> Placed : Place
    Placed --> Paid : Pay
    Paid --> Shipped : Ship
    Draft --> Cancelled : Cancel
    Placed --> Cancelled : Cancel
    Paid --> Cancelled : Cancel
    Shipped --> [*]
    Cancelled --> [*]
```

Any other move (paying a draft, shipping an order that is not paid, cancelling a shipped order) is an error.

---

## 4. Creational patterns

**Creational** patterns answer one question: **who decides which object gets created, and how**. Writing `new EmailNotifier()` in the middle of business code ties that code to one concrete class, one way of building it and one moment in time. Creational patterns move that decision somewhere else: to a subclass, to a factory object, to a step-by-step builder, or to a copy of an existing object.

In modern .NET most of these decisions belong to the **dependency injection (DI) container** (section [7.1](#71-dependency-injection)): you register *what* to create and *for how long it lives*, and the container builds the objects. That is why several patterns in this chapter look smaller in their .NET level than in their hand-written one.

| Pattern | Relevance | In one line |
|---|---|---|
| [4.1 Singleton](#41-singleton) | ⭐⭐ Useful | Exactly one instance of a class, reachable by everyone. Today: one instance **per container**, not a static field. |
| [4.2 Factory Method](#42-factory-method) | ⭐⭐ Useful | A method that subclasses (or a registration) override to decide which class to create. |
| [4.3 Abstract Factory](#43-abstract-factory) | ⭐ Niche | One object that creates a whole family of related objects that must match. |
| [4.4 Builder](#44-builder) | ⭐⭐⭐ Essential | Build a complex object step by step, and only hand it out when it is valid. |
| [4.5 Prototype](#45-prototype) | 🕰 Historical | Create an object by copying an existing one. Records and `with` absorbed it. |

### 4.1 Singleton

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful — the *idea* (one shared instance) is everywhere; the hand-written *static* version is an anti-pattern today |
| Family | Creational |
| Intent | Ensure a class has only one instance and give a global point of access to it. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- singleton` |
| Code | [`src/Patterns.Creational/Singleton/`](../src/Patterns.Creational/Singleton/) |

#### The problem

The shop charges VAT (value-added tax), and the rate depends on the country: 21 % in Spain (`ES`), 23 % in Portugal (`PT`), 20 % in France (`FR`). The checkout, the receipt and the invoice all need the same table. Loading it once and sharing it is sensible: there is one truth, and building it again on every call is waste.

*Analogy:* an office has **one** official calendar of public holidays. Everybody reads the same one; nobody keeps a private copy that drifts out of date.

#### Without the pattern

The quickest way to share data is a public static field ([`0-Problem/`](../src/Patterns.Creational/Singleton/0-Problem/)):

```csharp
public static class VatRates
{
    // PAIN: public and mutable. Any code (and any test) can change a rate for everybody, and
    // tests that run in parallel see each other's changes.
    public static Dictionary<string, decimal> Rates { get; } = new()
    {
        ["ES"] = 0.21m, ["PT"] = 0.23m, ["FR"] = 0.20m,
    };
}
```

What hurts:

- **Shared mutable state.** `VatRates.Rates["ES"] = 0.99m` anywhere changes every price in the application. Nothing in the type stops it.
- **Hidden dependency.** A class that reads `VatRates.Rates` does not say so in its constructor; you find out by reading its body.
- **Untestable in isolation.** A test cannot give one class a different table; it has to change the global one and remember to put it back. The test `Problem_AnyoneCanChangeTheRates` does exactly that, with a `finally` to restore the value, and that `finally` *is* the pain.

#### Structure

```mermaid
classDiagram
    class VatRateTable {
        <<Singleton>>
        -Lazy~VatRateTable~ LazyInstance$
        +VatRateTable Instance$
        -VatRateTable()
        +RateFor(string countryCode) decimal
    }
    class Checkout {
        <<Client>>
    }
    Checkout ..> VatRateTable : VatRateTable.Instance
```

| Role | Our class | Responsibility |
|---|---|---|
| Singleton | `VatRateTable` | Holds the rates, prevents other instances (private constructor) and exposes the only one through `Instance`. |
| Client | whoever needs a rate | Reads `VatRateTable.Instance.RateFor("ES")`. |

`$` marks a static member.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Type as VatRateTable (static)
    participant Lazy as Lazy of VatRateTable
    Client->>Type: Instance
    Type->>Lazy: Value
    Note right of Lazy: first access only: runs new VatRateTable(), thread-safe
    Lazy-->>Type: the instance
    Type-->>Client: the instance
    Client->>Type: Instance (again)
    Type->>Lazy: Value
    Lazy-->>Type: the same instance
    Type-->>Client: the same instance
```

#### By hand

```csharp
// Role: Singleton — the one VAT rate table of the application.
// Guide: §4.1
public sealed class VatRateTable
{
    // Lazy<T> creates the instance on first use and is thread-safe by default
    // (LazyThreadSafetyMode.ExecutionAndPublication): two threads never build two tables.
    private static readonly Lazy<VatRateTable> LazyInstance = new(() => new VatRateTable());

    public static VatRateTable Instance => LazyInstance.Value;

    private readonly Dictionary<string, decimal> _rates = new()
    {
        ["ES"] = 0.21m, ["PT"] = 0.23m, ["FR"] = 0.20m,
    };

    private VatRateTable() { } // private: nobody else can call new VatRateTable()

    public decimal RateFor(string countryCode)
    {
        var code = countryCode.Trim().ToUpperInvariant();
        return _rates.TryGetValue(code, out var rate)
            ? rate
            : throw new ArgumentException($"No VAT rate for country '{code}'."); // no paramName: it would append " (Parameter '…')" to the message
    }
}
```

Three things make it a Singleton: the **private constructor**, the **static field** that holds the one instance, and the **static accessor**. The rates are now private and read-only from outside, which fixes the first pain. The other two remain: `VatRateTable.Instance` is still a hidden, global dependency, and a test still cannot replace it.

Before `Lazy<T>` existed, people wrote "double-checked locking" by hand (`if (instance == null) lock (...) if (instance == null) ...`), which is easy to get subtly wrong. If laziness does not matter, `public static VatRateTable Instance { get; } = new();` is also thread-safe: the runtime runs static initializers once.

#### In .NET

In .NET you rarely write the pattern; you **register** a class as a singleton and the DI container enforces "only one" ([`2-DotNet/`](../src/Patterns.Creational/Singleton/2-DotNet/)):

```csharp
// The class is ordinary: public constructor, no static members.
public sealed class VatRateTable
{
    public decimal RateFor(string countryCode) { /* same as above */ }
}

public static class VatRatesServiceCollectionExtensions
{
    public static IServiceCollection AddVatRates(this IServiceCollection services) =>
        services.AddSingleton<VatRateTable>(); // one instance for the lifetime of this container
}

// Consumers ask for it in their constructor: the dependency is visible.
public sealed class Checkout(VatRateTable vatRates) { /* ... */ }
```

The important change of meaning: a DI singleton is **one instance per container**, not one per process. The same container always returns the same object; two containers return two different objects. That is exactly what tests need: each test builds its own container (or just calls `new VatRateTable()`) and nobody shares state. The tests `DotNet_OneContainer_ResolvesTheSameObject` and `DotNet_TwoContainers_HaveDifferentObjects` show both halves.

| | Static Singleton (Classic) | DI singleton (.NET) |
|---|---|---|
| "Only one" enforced by | the class itself (private constructor) | the container's registration |
| Scope | the whole process | one container |
| The dependency is | hidden (`VatRateTable.Instance` inside a method) | visible (a constructor parameter) |
| Replaceable in a test | no | yes: register another instance or pass one by hand |
| Lifetime can change later | rewrite the class | change `AddSingleton` to `AddScoped` |

`Lazy<T>` itself is worth knowing on its own: it is the standard way to create something expensive once, on first use, safely across threads.

#### When to use it

- The object is **stateless or read-only** (a rate table, a configuration snapshot, a `JsonSerializerOptions` instance, an `HttpClient` handler) and building it more than once is waste.
- It wraps something that must be **unique**: a cache shared by the whole application, a connection multiplexer.
- Use it through DI: `AddSingleton`, and receive it in constructors.

#### When NOT to use it

- **As a global variable.** If the reason is "so I can reach it from anywhere", that is the anti-pattern. Pass it in the constructor.
- **When it has mutable state per user or per request** (a cart, a current user): that is `AddScoped`, not a singleton. A singleton that holds per-request data mixes users' data.
- **Never hand-write the static version in new code** that has a DI container. Simpler alternative: an ordinary class registered with `AddSingleton`, or a `static readonly` field for a true constant.
- **When it depends on a scoped service:** a singleton that receives a scoped `DbContext` keeps it forever (a *captive dependency*, section [7.1](#71-dependency-injection)).

#### Costs

- The static version: global state, hidden coupling, tests that interfere with each other, and a class that controls its own lifetime (two responsibilities).
- The DI version: almost none, but its state is shared by every thread, so it must be thread-safe or immutable.

#### Relevance today

⭐⭐ **Useful.** "One shared instance" is one of the three lifetimes of every .NET application, so you meet the idea daily. The GoF implementation (private constructor + static accessor) is an **anti-pattern** in code with a DI container: you study it to recognise it in older code and to understand what `AddSingleton` replaced.

#### Relatives

- **Flyweight** ([5.6](#56-flyweight)) also shares instances, but *many* of them, one per distinct value; Singleton has exactly one.
- **Object Pool** ([7.8](#78-object-pool)) keeps several reusable instances and lends them out; a singleton is never "returned".
- **Abstract Factory** and **Facade** objects are often singletons.
- **Dependency Injection** ([7.1](#71-dependency-injection)) is what made the static Singleton unnecessary.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- singleton
```

1. In `SingletonTests`, remove the `finally` from `Problem_AnyoneCanChangeTheRates` and run the whole test class: other tests start failing or passing depending on the order the tests run in. That is shared mutable state.
2. In the DotNet level, change `AddSingleton` to `AddTransient` and run `DotNet_OneContainer_ResolvesTheSameObject`: it fails, because every resolution now builds a new table.
3. Add `["DE"] = 0.19m` to the Classic table and ask for `" de "`.

#### Interview questions

<details>
<summary>Why is the classic Singleton considered an anti-pattern today?</summary>

It is global state behind a static accessor: dependencies are hidden, the class controls its own lifetime, and tests cannot replace it or isolate from each other. A DI container gives the same "one instance" with a visible constructor dependency and one instance per container.
</details>

<details>
<summary>What is the difference between <code>AddSingleton</code> and the GoF Singleton?</summary>

`AddSingleton` means one instance *per container*, enforced by the registration; the class is ordinary and can be created by hand. The GoF Singleton means one instance *per process*, enforced by the class itself through a private constructor.
</details>

<details>
<summary>How do you make a lazy singleton thread-safe in C#?</summary>

Use `Lazy<T>` (thread-safe by default) or a static read-only field, which the runtime initializes exactly once. Avoid hand-written double-checked locking.
</details>

### 4.2 Factory Method

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Creational |
| Intent | Define a method for creating an object, and let subclasses decide which class to create. |
| Also known as | Virtual Constructor |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- factory-method` |
| Code | [`src/Patterns.Creational/FactoryMethod/`](../src/Patterns.Creational/FactoryMethod/) |

#### The problem

When an order ships, the shop tells the customer. Some customers want an email, some an SMS (Short Message Service, a text message). The code that sends the notification is the same in both cases: build the message, send it, record the result. Only **which notifier** is created changes.

Every level uses the same small shape, declared again inside each level:

```csharp
public enum NotificationChannel { Email, Sms }

public interface INotifier
{
    string Send(Customer customer, string message);
}
// EmailNotifier → "EMAIL to ana@example.com: Your order has shipped"
// SmsNotifier   → "SMS to Ana: Your order has shipped"
```

*Analogy:* a restaurant franchise has one procedure for serving a dish (plate it, check it, take it to the table). Each restaurant decides **which** dish its kitchen produces. The procedure is fixed; the "create the dish" step is left to each restaurant.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Creational/FactoryMethod/0-Problem/):

```csharp
public sealed class NotificationService
{
    public string Notify(NotificationChannel channel, Customer customer, string message)
    {
        // PAIN: the service knows every concrete notifier, and every new channel edits this method.
        INotifier notifier = channel switch
        {
            NotificationChannel.Email => new EmailNotifier(),
            NotificationChannel.Sms => new SmsNotifier(),
            _ => throw new ArgumentOutOfRangeException(nameof(channel)),
        };
        return notifier.Send(customer, message);
    }
}
```

What hurts: the "send" logic is mixed with the "which class" decision. Adding WhatsApp means editing a class whose real job is sending, and every class that needs a notifier repeats this `switch`.

A `switch` like this is **not always wrong**: with two channels that rarely change, it is the simplest thing that works (see "When NOT to use it").

#### Structure

```mermaid
classDiagram
    class NotificationCampaign {
        <<Creator>>
        +Notify(Customer customer, string message) string
        #CreateNotifier()* INotifier
    }
    class EmailCampaign {
        <<ConcreteCreator>>
        #CreateNotifier() INotifier
    }
    class SmsCampaign {
        <<ConcreteCreator>>
        #CreateNotifier() INotifier
    }
    class INotifier {
        <<Product>>
        +Send(Customer customer, string message) string
    }
    class EmailNotifier {
        <<ConcreteProduct>>
    }
    class SmsNotifier {
        <<ConcreteProduct>>
    }
    NotificationCampaign <|-- EmailCampaign
    NotificationCampaign <|-- SmsCampaign
    INotifier <|.. EmailNotifier
    INotifier <|.. SmsNotifier
    NotificationCampaign ..> INotifier : uses
    EmailCampaign ..> EmailNotifier : creates
    SmsCampaign ..> SmsNotifier : creates
```

| Role | Our class | Responsibility |
|---|---|---|
| Creator | `NotificationCampaign` | Contains the logic that *uses* a notifier, and declares the factory method `CreateNotifier()`. |
| ConcreteCreator | `EmailCampaign`, `SmsCampaign` | Override the factory method to return their notifier. |
| Product | `INotifier` | What the factory method returns. |
| ConcreteProduct | `EmailNotifier`, `SmsNotifier` | The real notifiers. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Campaign as EmailCampaign
    participant Notifier as EmailNotifier
    Client->>Campaign: Notify(ana, "Your order has shipped")
    Campaign->>Campaign: CreateNotifier()
    Note right of Campaign: the subclass decides the class
    Campaign->>Notifier: Send(ana, message)
    Notifier-->>Campaign: "EMAIL to ana@example.com: ..."
    Campaign-->>Client: "EMAIL to ana@example.com: ..."
```

#### By hand

```csharp
// Role: Creator — the notification logic, with the "which notifier" step left open.
// Guide: §4.2
public abstract class NotificationCampaign
{
    public string Notify(Customer customer, string message)
    {
        var notifier = CreateNotifier(); // the factory method: subclasses decide the class
        return notifier.Send(customer, message);
    }

    protected abstract INotifier CreateNotifier();
}

// Role: ConcreteCreator — campaigns that reach the customer by email.
public sealed class EmailCampaign : NotificationCampaign
{
    protected override INotifier CreateNotifier() => new EmailNotifier();
}

// Role: ConcreteCreator — campaigns that reach the customer by SMS.
public sealed class SmsCampaign : NotificationCampaign
{
    protected override INotifier CreateNotifier() => new SmsNotifier();
}
```

`Notify` no longer knows any concrete notifier; a new channel is a new subclass, and `NotificationCampaign` does not change. The pattern is Template Method ([6.10](#610-template-method)) applied to creation: the base class fixes the algorithm and one step of it is "create the object".

**Three things are called "factory"**, and only the first is the GoF pattern:

| Name | What it is | Example |
|---|---|---|
| **Factory Method** (GoF) | An overridable method; a *subclass* decides the class. | `CreateNotifier()` above. |
| **"A factory"** (everyday) | Any method, class or delegate whose job is to create objects. No inheritance needed. | `static INotifier Create(NotificationChannel c)`, a `Func<INotifier>`. |
| **Static factory method** | A static method used instead of a constructor, usually for a clearer name or caching. | `TimeSpan.FromMinutes(5)`, `Guid.NewGuid()`. |

#### In .NET

In modern .NET the decision "which class for this channel" usually moves into the **DI container**, using **keyed services** (.NET 8 and later): several implementations of the same interface, each registered under a key ([`2-DotNet/`](../src/Patterns.Creational/FactoryMethod/2-DotNet/)):

```csharp
services.AddKeyedSingleton<INotifier, EmailNotifier>(NotificationChannel.Email);
services.AddKeyedSingleton<INotifier, SmsNotifier>(NotificationChannel.Sms);

public sealed class NotificationService(IServiceProvider services)
{
    public string Notify(NotificationChannel channel, Customer customer, string message)
    {
        // The container is the factory: the registrations decide the class for each key.
        var notifier = services.GetRequiredKeyedService<INotifier>(channel);
        return notifier.Send(customer, message);
    }
}
```

A new channel is a new class plus one registration line; `NotificationService` does not change. An unregistered key throws `InvalidOperationException`. When the key is known at compile time, a constructor parameter is cleaner than asking the provider: `NotificationService([FromKeyedServices(NotificationChannel.Email)] INotifier notifier)`.

Other factories in the framework you already use:

- `ILoggerFactory.CreateLogger("Checkout")` creates a logger without you knowing which providers stand behind it.
- `IHttpClientFactory.CreateClient("carrier")` creates a configured `HttpClient` and manages the lifetime of its handlers.
- Every `services.AddSingleton<IX>(sp => new X(...))` registration is a factory delegate.

#### When to use it

- A class contains logic that is the same for every variant, and only **which object it creates** changes.
- You see the same `switch` that `new`s different classes in more than one place.
- You write a framework or library and want users to plug in their own class (the base class calls `CreateX()`, the user overrides it).
- In application code with a container, prefer keyed services or a factory delegate (a `Func<T>` you register yourself; the Microsoft container does not inject `Func<T>` automatically) over subclassing.

#### When NOT to use it

- **Two or three variants that rarely change**, created in one place: a `switch` expression is clearer.
- **Only to wrap `new`**: a factory whose method is `return new X();` with no decision in it is ceremony (section [3.6](#36-patternitis)). Let the container create it.
- **When subclassing the creator just to change the product** creates parallel hierarchies (one campaign per notifier). If that grows, inject the notifier (or a `Func<INotifier>`) instead.

#### Costs

- One subclass per product, and the two hierarchies grow together.
- Keyed services: a missing registration fails at run time, not at compile time; the tests have to cover each key.

#### Relevance today

⭐⭐ **Useful.** The subclass-based version shows up in frameworks and in older code; in applications, the container (keyed services, factory delegates, `ILoggerFactory`, `IHttpClientFactory`) does the job. Knowing the idea helps you read all of them.

#### Relatives

- **Abstract Factory** ([4.3](#43-abstract-factory)) is often a set of factory methods, one per product of a family. Comparison in [8.3](#83-factory-method-abstract-factory-builder).
- **Template Method** ([6.10](#610-template-method)): Factory Method is a template method whose step is "create".
- **Strategy** ([6.9](#69-strategy)) with keyed services looks the same in DI: there the key picks *a behaviour*, here *an object to create*.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- factory-method
```

1. Add a `Push` channel at each level. Count the files you touch: Problem edits the `switch`; Classic adds a subclass and a notifier; .NET adds a notifier and one registration.
2. Call the .NET `NotificationService` with `(NotificationChannel)99` and read the exception message.
3. Replace the Classic subclasses with a `NotificationCampaign(Func<INotifier> createNotifier)` constructor. What did you gain, and what did you lose?

#### Interview questions

<details>
<summary>What is the difference between Factory Method and a "simple factory"?</summary>

Factory Method is an overridable method in a class hierarchy: subclasses decide which product to create, and the base class uses it inside its own logic. A simple factory is just a method or class with a `switch` that creates objects; it involves no inheritance.
</details>

<details>
<summary>How do keyed services replace Factory Method in .NET?</summary>

Each implementation is registered under a key (`AddKeyedSingleton<INotifier, EmailNotifier>(NotificationChannel.Email)`) and the consumer resolves by key (`GetRequiredKeyedService` or `[FromKeyedServices]`). The registration decides the class, so the consumer and the base class no longer need subclasses.
</details>

### 4.3 Abstract Factory

#### Card

| | |
|---|---|
| Relevance | ⭐ Niche |
| Family | Creational |
| Intent | Provide an interface for creating families of related objects without naming their concrete classes. |
| Also known as | Kit |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- abstract-factory` |
| Code | [`src/Patterns.Creational/AbstractFactory/`](../src/Patterns.Creational/AbstractFactory/) |

#### The problem

The shop takes payments through two providers: **card** and **wallet** (a digital wallet). Each provider needs three cooperating objects: a **charger** that takes the money and returns a transaction id, a **refunder** that gives it back, and a **receipt formatter**. The three must come from the **same** provider: refunding a card transaction through the wallet refunder is a bug.

*Analogy:* furniture sold in collections. If you pick the "oak" collection, the table, the chairs and the shelf all come in oak. You choose the collection once; you do not choose the wood piece by piece.

#### Without the pattern

```csharp
// The checkout picks every piece itself, by provider name.
IPaymentCharger charger = provider == "card" ? new CardCharger() : new WalletCharger();
IReceiptFormatter formatter = provider == "card" ? new CardReceiptFormatter() : new WalletReceiptFormatter();
// Nothing stops a mixed family: a card charger with a wallet formatter, after a typo in one line.
```

What hurts: the "which provider" decision is repeated once per object, the client knows six concrete classes, and nothing guarantees the pieces match.

#### Structure

```mermaid
classDiagram
    class IPaymentProviderFactory {
        <<AbstractFactory>>
        +CreateCharger() IPaymentCharger
        +CreateRefunder() IRefunder
        +CreateReceiptFormatter() IReceiptFormatter
    }
    class CardProviderFactory {
        <<ConcreteFactory>>
    }
    class WalletProviderFactory {
        <<ConcreteFactory>>
    }
    class IPaymentCharger {
        <<AbstractProduct>>
        +Charge(decimal amount) string
    }
    class IReceiptFormatter {
        <<AbstractProduct>>
        +Format(string transactionId, decimal amount) string
    }
    class CheckoutPayment {
        <<Client>>
        +Pay(Order order) string
    }
    IPaymentProviderFactory <|.. CardProviderFactory
    IPaymentProviderFactory <|.. WalletProviderFactory
    CheckoutPayment --> IPaymentProviderFactory
    CheckoutPayment ..> IPaymentCharger
    CheckoutPayment ..> IReceiptFormatter
```

`IRefunder` (`Refund(string transactionId) : string`) is the third abstract product; it is left out of the diagram to keep it readable.

| Role | Our class | Responsibility |
|---|---|---|
| AbstractFactory | `IPaymentProviderFactory` | One creation method per product of the family. |
| ConcreteFactory | `CardProviderFactory`, `WalletProviderFactory` | Create the products of one provider; transaction ids start with `CARD` or `WALLET`. |
| AbstractProduct | `IPaymentCharger`, `IRefunder`, `IReceiptFormatter` | What each product can do. |
| ConcreteProduct | `CardCharger`, `WalletRefunder`, … | One per product per family. |
| Client | `CheckoutPayment` | Receives one factory and uses only the interfaces. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client as CheckoutPayment
    participant Factory as CardProviderFactory
    participant Charger as CardCharger
    participant Formatter as CardReceiptFormatter
    Client->>Factory: CreateCharger()
    Factory-->>Client: charger
    Client->>Charger: Charge(25.00)
    Charger-->>Client: "CARD-0001"
    Client->>Factory: CreateReceiptFormatter()
    Factory-->>Client: formatter
    Client->>Formatter: Format("CARD-0001", 25.00)
    Formatter-->>Client: "Card payment CARD-0001: 25.00"
```

#### By hand

```csharp
// Role: AbstractFactory — creates the three objects of one payment provider.
// Guide: §4.3
public interface IPaymentProviderFactory
{
    IPaymentCharger CreateCharger();
    IRefunder CreateRefunder();
    IReceiptFormatter CreateReceiptFormatter();
}

// Role: ConcreteFactory — the card family. Everything it creates speaks "CARD".
public sealed class CardProviderFactory : IPaymentProviderFactory
{
    public IPaymentCharger CreateCharger() => new CardCharger();       // ids CARD-0001, CARD-0002…
    public IRefunder CreateRefunder() => new CardRefunder();           // rejects ids that are not CARD-…
    public IReceiptFormatter CreateReceiptFormatter() => new CardReceiptFormatter();
}

// Role: Client — knows the interfaces only; the family is decided by whoever passes the factory.
public sealed class CheckoutPayment(IPaymentProviderFactory provider)
{
    // Created once: the charger numbers its transactions (CARD-0001, CARD-0002…).
    private readonly IPaymentCharger _charger = provider.CreateCharger();
    private readonly IReceiptFormatter _formatter = provider.CreateReceiptFormatter();

    public string Pay(Order order)
    {
        var transactionId = _charger.Charge(order.Total);
        return _formatter.Format(transactionId, order.Total);
    }
}
```

Paying for two books: card gives `"Card payment CARD-0001: 25.00"`, wallet gives `"Wallet payment WALLET-0001: 25.00"`. `CardRefunder.Refund("WALLET-0001")` throws `ArgumentException`: the families protect themselves against mixing. Amounts are written with `CultureInfo.InvariantCulture`, so `25.00` never becomes `25,00` in an application running with a Spanish culture. (This repository runs with invariant globalization, section [2.2](#22-root-build-files), but code you copy elsewhere will not.)

The choice of family happens **once**, where the factory is created. Adding a "bank transfer" provider is a new factory and three products; `CheckoutPayment` does not change. Adding a fourth *product* to every family (say, a fraud checker) is the expensive direction: the interface and every factory change.

#### In .NET

The family is picked in DI, again with keyed services ([`2-DotNet/`](../src/Patterns.Creational/AbstractFactory/2-DotNet/)):

```csharp
services.AddKeyedSingleton<IPaymentProviderFactory, CardProviderFactory>("card");
services.AddKeyedSingleton<IPaymentProviderFactory, WalletProviderFactory>("wallet");

var factory = provider.GetRequiredKeyedService<IPaymentProviderFactory>("card");
var receipt = new CheckoutPayment(factory).Pay(order);
```

The framework's own Abstract Factory is **`DbProviderFactory`** from ADO.NET (the low-level database API under Entity Framework and Dapper). One factory per database engine creates the matching connection, command and parameter objects:

```csharp
DbProviderFactory factory = SqlClientFactory.Instance;   // or NpgsqlFactory.Instance, SqliteFactory.Instance…
using DbConnection connection = factory.CreateConnection()!;
using DbCommand command = factory.CreateCommand()!;
DbParameter parameter = factory.CreateParameter()!;
// Code written against DbConnection/DbCommand works with any engine; the factory keeps the family consistent.
```

That snippet is not coded in the repository (it needs a database driver), but it is the textbook example of the pattern in .NET.

#### When to use it

- Several objects must be **used together** and **change together** (one provider, one database engine, one UI, user interface, theme).
- The client must not know the concrete classes, and mixing families would be a bug.
- The set of products is stable, while new families appear.

#### When NOT to use it

- **Only one family exists** and no second one is planned: inject the objects directly.
- **The products are independent** of each other: inject each one separately (each with its own key if needed); grouping them in a factory adds nothing.
- **The set of products keeps changing:** every new product edits every factory. Simpler alternative: one keyed registration per product, with a shared key per family.

#### Costs

- Many types: one factory per family plus one class per product per family.
- Adding a product touches the interface and every concrete factory.

#### Relevance today

⭐ **Niche.** DI containers and keyed services cover most "pick an implementation" needs. The pattern still earns its place when a family *must* stay consistent, as `DbProviderFactory` shows; recognising it is enough for most developers.

#### Relatives

- **Factory Method** ([4.2](#42-factory-method)): an abstract factory is usually a set of factory methods. Comparison in [8.3](#83-factory-method-abstract-factory-builder).
- **Singleton** ([4.1](#41-singleton)): concrete factories hold no state, so they are registered as singletons.
- **Bridge** ([5.2](#52-bridge)): a factory can create the right implementor for an abstraction.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- abstract-factory
```

1. Add a `TransferProviderFactory` (prefix `TRANSFER`). How many files did you add, and how many existing ones changed?
2. Now add a fourth product, `IFraudChecker`, to every family. Compare the count with experiment 1.
3. Refund a wallet transaction with the card refunder and read the message.

#### Interview questions

<details>
<summary>What is the difference between Abstract Factory and Factory Method?</summary>

Factory Method creates one product and relies on a subclass to choose the class. Abstract Factory is an object that creates a whole family of related products and is passed to the client; switching the factory object switches the whole family at once.
</details>

<details>
<summary>Give an example of Abstract Factory in .NET.</summary>

`DbProviderFactory`: each database provider (`SqlClientFactory`, `NpgsqlFactory`, …) creates matching `DbConnection`, `DbCommand` and `DbParameter` objects.
</details>

### 4.4 Builder

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Creational |
| Intent | Separate the construction of a complex object from its representation, so it can be built step by step and only handed out when complete. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- builder` |
| Code | [`src/Patterns.Creational/Builder/`](../src/Patterns.Creational/Builder/) |

#### The problem

An order has a customer, a shipping address, one or more lines, and optionally a gift note. Some combinations are invalid: an order without lines, or without an address. The shop builds orders in many places (the web checkout, the mobile app, imports, tests), and each place should not have to remember all the rules.

*Analogy:* ordering a custom sandwich. You say "wholemeal bread, add ham, add cheese, no tomato" one step at a time, and the sandwich is only handed over at the end, once it is complete. Nobody hands you half a sandwich.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Creational/Builder/0-Problem/):

```csharp
public sealed class MutableOrder
{
    public Customer? Customer { get; set; }
    public Address? ShippingAddress { get; set; }
    public IList<OrderLine> Lines { get; } = new List<OrderLine>();
    public decimal Total => Lines.Sum(line => line.LineTotal);

    // PAIN: an invalid order exists until someone remembers to call IsValid(), and every caller
    // must remember to.
    public bool IsValid() => Customer is not null && ShippingAddress is not null && Lines.Count > 0;
}
```

What hurts: the object is **invalid while it is being filled in**, and it can travel through the code in that state. Validation is a method someone has to remember to call, and the rules (what happens when the same product is added twice?) are scattered over every caller. The other classic way to avoid this, a constructor with every parameter, becomes unreadable once there are several optional values (the "telescoping constructor").

#### Structure

```mermaid
classDiagram
    class OrderBuilder {
        <<Builder>>
        +For(Customer customer)$ OrderBuilder
        +ShipTo(Address address) OrderBuilder
        +Add(Product product, int quantity) OrderBuilder
        +WithGiftNote(string note) OrderBuilder
        +Build() BuiltOrder
    }
    class BuiltOrder {
        <<Product>>
        +Order Order
        +string? GiftNote
    }
    class Checkout {
        <<Director>>
    }
    Checkout ..> OrderBuilder : drives
    OrderBuilder ..> BuiltOrder : creates
```

| Role | Our class | Responsibility |
|---|---|---|
| Builder | `OrderBuilder` | Collects the parts step by step, applies the rules, and creates the result only when it is valid. |
| Product | `BuiltOrder` (an `Order` plus the optional gift note) | The finished, valid, immutable object. |
| Director | whoever calls the steps (the checkout, a test) | Knows *which* steps to call, not how they work. |

The GoF version has an abstract `Builder` with several concrete builders producing different representations from the same steps. In C# the common form is a single **fluent builder**: each step returns the builder itself, so calls chain.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Builder as OrderBuilder
    Client->>Builder: For(ana)
    Client->>Builder: ShipTo(madrid)
    Client->>Builder: Add(book, 2)
    Client->>Builder: Add(headphones)
    Client->>Builder: Build()
    Note right of Builder: checks address and lines, then creates the Order
    Builder-->>Client: BuiltOrder (total 84.90)
```

#### By hand

```csharp
// Role: Builder — assembles an order step by step; an invalid order never leaves this class.
// Guide: §4.4
public sealed class OrderBuilder
{
    private readonly Customer _customer;
    private readonly List<OrderLine> _lines = [];
    private Address? _address;
    private string? _giftNote;

    private OrderBuilder(Customer customer) => _customer = customer;

    public static OrderBuilder For(Customer customer) => new(customer);

    public OrderBuilder ShipTo(Address address) { _address = address; return this; }

    public OrderBuilder Add(Product product, int quantity = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1);
        var index = _lines.FindIndex(line => line.Product == product);
        if (index >= 0)
            _lines[index] = _lines[index] with { Quantity = _lines[index].Quantity + quantity }; // same product: add up
        else
            _lines.Add(new OrderLine(product, quantity));
        return this;
    }

    public OrderBuilder WithGiftNote(string note) { _giftNote = note; return this; }

    public BuiltOrder Build()
    {
        if (_address is null) throw new InvalidOperationException("An order needs a shipping address.");
        if (_lines.Count == 0) throw new InvalidOperationException("An order needs at least one line.");

        // A copy of the list: later calls on this builder cannot change the order already built.
        return new BuiltOrder(new Order(Guid.NewGuid(), _customer, [.. _lines], _address), _giftNote);
    }
}

// Usage: reads like the sentence it builds.
var built = OrderBuilder.For(SampleData.Ana)
    .ShipTo(SampleData.Madrid)
    .Add(SampleData.Book, 2)
    .Add(SampleData.Headphones)
    .Build(); // built.Order.Total == 84.90
```

The rules live in **one** place: adding a book twice merges the quantities, a quantity below 1 throws `ArgumentOutOfRangeException`, and `Build()` refuses an order without an address or lines with an exact message. The resulting `Order` is an immutable record, so it stays valid after `Build`.

**Builder's second face: the test data builder.** Tests need many orders that differ in one detail. Writing every field in every test hides the detail that matters. A *test data builder* (in the test project, not in production code) supplies sensible defaults and lets each test say only what is special:

```csharp
// Defaults: customer Ana, shipped to Madrid. The test states only what it is about.
var order = AnOrder.WithLines((SampleData.Book, 4)).ShippedTo(SampleData.Lisbon).Build();
```

When a new required field is added to `Order`, only `AnOrder` changes, not a hundred tests.

#### In .NET

The framework is full of builders. The DotNet level codes two of them ([`2-DotNet/`](../src/Patterns.Creational/Builder/2-DotNet/)):

**`StringBuilder`** builds a string from many pieces without creating a new string at every `+` (strings are immutable, so `a + b + c` in a loop copies everything again and again):

```csharp
public static string Print(Order order)
{
    var receipt = new StringBuilder();
    receipt.AppendLine(CultureInfo.InvariantCulture, $"Order for {order.Customer.Name}");
    foreach (var line in order.Lines)
    {
        receipt.AppendLine(CultureInfo.InvariantCulture,
            $"{line.Quantity} x {line.Product.Name} @ {line.Product.Price:0.00} = {line.LineTotal:0.00}");
    }
    receipt.Append(CultureInfo.InvariantCulture, $"Total: {order.Total:0.00}");
    return receipt.ToString(); // the one moment the final string is created
}
```

For two books and the headphones it produces exactly the text below. (`AppendLine` ends each line with `Environment.NewLine`, `\r\n` on Windows, so the test compares both sides after `ReplaceLineEndings("\n")`.)

```
Order for Ana
2 x Clean Code @ 12.50 = 25.00
1 x Wireless Headphones @ 59.90 = 59.90
Total: 84.90
```

**`UriBuilder`** builds a valid URI (Uniform Resource Identifier, the general form of a web address) from parts, handling the separators for you:

```csharp
public static Uri For(Guid orderId) =>
    new UriBuilder(Uri.UriSchemeHttps, "track.example.com")
    {
        Path = $"orders/{orderId}",
        Query = "lang=en",
    }.Uri; // https://track.example.com/orders/<id>?lang=en
```

**`WebApplication.CreateBuilder(args)`** in ASP.NET Core is a builder on a large scale: you configure services, configuration and logging step by step, and `builder.Build()` produces the `WebApplication`. After `Build()`, the service registrations become read-only (you still add middleware and endpoints to the app), which is exactly the "only hand it out when complete" idea. `Host.CreateApplicationBuilder`, `ConfigurationBuilder` and `ImmutableArray.CreateBuilder<T>()` follow the same shape.

#### When to use it

- The object has **many optional parts**, or the constructor would need many parameters.
- The object must be **valid and immutable** once created, but is assembled over several steps.
- Construction has **rules** (merge duplicates, check required parts) that should live in one place.
- Tests build many variations of the same object: a test data builder.

#### When NOT to use it

- **Two or three required values:** a constructor or a record with required properties is enough.
- **Optional values without rules:** named arguments (`new Order(Id: …, Customer: ana, …)`) or object initializers with `required` and `init` properties already give named, readable construction without a builder class.
- **A builder that only copies values into a constructor**, with no rules and no steps, is ceremony.

#### Costs

- One more class per built type, which must be kept in sync with the product.
- A builder is mutable and usually not thread-safe; it is meant to be used once and thrown away.

#### Relevance today

⭐⭐⭐ **Essential.** You use `StringBuilder` and `WebApplication.CreateBuilder` in almost every project, and test data builders are one of the most useful tools for readable tests.

#### Relatives

- **Factory Method** and **Abstract Factory** create an object in one call; Builder assembles one over several. Comparison in [8.3](#83-factory-method-abstract-factory-builder).
- **Composite** ([5.3](#53-composite)): builders are a handy way to assemble trees.
- **Fluent interface**: Builder is the most common use of method chaining, but chaining alone (LINQ, for example) is not a builder.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- builder
```

1. Call `Build()` without `ShipTo` and read the message. Then try the same mistake with `MutableOrder`: nothing stops you.
2. Add the same book twice with quantities 1 and 3, and check that the order has one line with 4.
3. Remove `CultureInfo.InvariantCulture` from one `AppendLine` and build: nothing warns you (analyzer CA1305 does not check `StringBuilder`'s interpolated overloads), and with this repository's invariant globalization (section [2.2](#22-root-build-files)) the output does not even change. In an ordinary project on a Spanish machine that line would print `12,50`. That silent risk is why the test pins the exact string.

#### Interview questions

<details>
<summary>What problem does Builder solve that a constructor does not?</summary>

It assembles an object over several steps with rules between them (merge duplicates, required parts) and returns it only when it is valid, keeping the product immutable. It also avoids constructors with many optional parameters.
</details>

<details>
<summary>What is a test data builder?</summary>

A builder in the test project that gives every field a sensible default, so each test sets only the values it is about. Tests become shorter, state their intent, and survive changes to the built type.
</details>

<details>
<summary>Why use <code>StringBuilder</code> instead of string concatenation?</summary>

Strings are immutable: concatenating in a loop creates and copies a new string every time. `StringBuilder` appends into a growing buffer and creates the final string once, in `ToString()`.
</details>

### 4.5 Prototype

#### Card

| | |
|---|---|
| Relevance | 🕰 Historical — records and `with` do it for you |
| Family | Creational |
| Intent | Create new objects by copying an existing one (the prototype). |
| Also known as | Clone |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- prototype` |
| Code | [`src/Patterns.Creational/Prototype/`](../src/Patterns.Creational/Prototype/) |

#### The problem

Some customers order the same things every month: coffee and a book, say. The shop keeps that as a **template** and creates each month's order from it. Building the order again from scratch every month repeats work; copying the template and changing what differs (the id, the delivery date) is simpler.

*Analogy:* a document template. You do not rewrite the company letterhead each time; you copy the template and edit the copy. Editing the copy must never change the template.

#### Without the pattern

```csharp
// A plain class with settable properties, copied by hand, field by field, in the caller:
public sealed class PlainTemplate { public string Name { get; set; } = ""; public List<OrderLine> Lines { get; set; } = []; }

var next = new PlainTemplate { Name = template.Name, Lines = template.Lines };
next.Lines.Add(new OrderLine(SampleData.Mug, 1));
// Oops: Lines is the same List object, so the template now has the mug too.
```

What hurts: the caller must know every field of the class (and update every copy site when a field is added), and it is easy to copy a **reference** when you meant to copy the **thing**.

#### Structure

```mermaid
classDiagram
    class IPrototype~T~ {
        <<Prototype>>
        +Clone() T
    }
    class OrderTemplate {
        <<ConcretePrototype>>
        +string Name
        +IList~OrderLine~ Lines
        +Clone() OrderTemplate
        +ShallowClone() OrderTemplate
    }
    class Client
    IPrototype~T~ <|.. OrderTemplate
    Client ..> OrderTemplate : Clone()
```

| Role | Our class | Responsibility |
|---|---|---|
| Prototype | `IPrototype<T>` | Declares `Clone()`. |
| ConcretePrototype | `OrderTemplate` | Knows how to copy itself correctly, including which parts must be copied and which can be shared. |
| Client | whoever needs a new order | Asks the prototype for a copy instead of calling a constructor. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Template as OrderTemplate (prototype)
    participant Copy as OrderTemplate (copy)
    Client->>Template: Clone()
    Template->>Copy: new, with a new list of the same lines
    Template-->>Client: copy
    Client->>Copy: Lines.Add(mug)
    Note over Template,Copy: the template still has its original lines
```

#### By hand

```csharp
// Role: ConcretePrototype — a monthly order that knows how to copy itself.
// Guide: §4.5
public sealed class OrderTemplate(string name, IEnumerable<OrderLine> lines) : IPrototype<OrderTemplate>
{
    public string Name { get; set; } = name;

    // Mutable on purpose: it is what makes the difference between a shallow and a deep copy visible.
    public IList<OrderLine> Lines { get; } = new List<OrderLine>(lines);

    // Deep enough: a new list. The OrderLine records inside are immutable, so sharing them is safe.
    public OrderTemplate Clone() => new(Name, Lines);

    // MemberwiseClone copies the fields one by one: the copy gets the SAME list object.
    public OrderTemplate ShallowClone() => (OrderTemplate)MemberwiseClone();
}
```

**Shallow vs deep copy.** A **shallow copy** copies the object's fields as they are: value types (numbers, dates) are copied, but reference-type fields (a `List<T>`, another object) are copied as **references**, so the copy and the original point to the same list. A **deep copy** also copies what those references point to. The test `ShallowClone_SharesTheLines` adds a line to a shallow copy and finds it in the original too.

```mermaid
flowchart LR
    subgraph shallow [Shallow copy: MemberwiseClone]
        T1[template] --> L1[(List of lines)]
        C1[copy] --> L1
    end
    subgraph deep [Deep copy: Clone]
        T2[template] --> L2[(List of lines)]
        C2[copy] --> L3[(new List, same lines)]
    end
```

"Deep enough" is a judgment call: our `Clone` copies the list but shares the `OrderLine` records, because they are immutable and can never change under anyone's feet. Immutable parts never need copying.

#### In .NET

C# records have a built-in copy operation, `with`, that creates a new record with some properties changed ([`2-DotNet/`](../src/Patterns.Creational/Prototype/2-DotNet/)):

```csharp
// Guide: §4.5
public sealed record RecurringOrder(Guid Id, string Name, ImmutableArray<OrderLine> Lines, DateOnly NextDelivery);

var march = new RecurringOrder(Guid.NewGuid(), "Monthly coffee",
    [new OrderLine(SampleData.Mug, 1)], new DateOnly(2026, 3, 1));

// The prototype pattern in one expression: copy, then change what differs.
var april = march with { Id = Guid.NewGuid(), NextDelivery = march.NextDelivery.AddMonths(1) };
// In the code this expression lives in the record itself: march.ForNextMonth().
```

Careful: **`with` is a shallow copy** too. It works safely here because `ImmutableArray<OrderLine>` cannot be modified: sharing it between `march` and `april` is harmless. With a `List<OrderLine>` property, `with` would share the list and bring back the bug from "Without the pattern". Records plus immutable collections are what make Prototype disappear.

**Why `ICloneable` is discouraged.** .NET has had `ICloneable` since version 1.0, and Microsoft's design guidelines say not to implement it in public APIs:

- its contract does not say whether `Clone()` is **deep or shallow**, so callers cannot rely on either;
- it returns `object`, so every caller casts.

If you need a copy method, write one with a clear name and type (`Clone()` returning `OrderTemplate`, or `DeepCopy()`), or use a record.

#### When to use it

- Creating an object is expensive or complex, and you already have one that is almost what you need.
- You need many objects that differ in a few values from a template.
- In C#: use a **record with `with`** and immutable members; write a `Clone()` by hand only for mutable classes you cannot turn into records.

#### When NOT to use it

- **The object is cheap to create** from its constructor: just create it.
- **The type is a record with immutable members:** `with` already is the pattern. Do not write an `IPrototype<T>`.
- **The object graph is deep, mutable and has cycles:** hand-written deep copy is fragile. Rethink the design towards immutability, or serialize and deserialize as a last resort.

#### Costs

- Every class must implement copying correctly, and keep it correct when fields are added.
- The shallow-vs-deep decision is easy to get wrong and the bug is silent (two objects quietly share state).

#### Relevance today

🕰 **Historical.** In 1994 copying objects needed a pattern; in C# 9 and later, records and `with` give you the copy for free, and immutable collections make it safe. You study Prototype to understand shallow vs deep copies, which matters every day, and to read older code with `Clone()` methods.

#### Relatives

- **Memento** ([6.6](#66-memento)) also copies state, but to restore it later, not to create a new object.
- **Abstract Factory** ([4.3](#43-abstract-factory)) can keep a set of prototypes and clone them instead of calling constructors.
- **Flyweight** ([5.6](#56-flyweight)) shares one object on purpose; Prototype makes independent copies.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- prototype
```

1. Replace `ImmutableArray<OrderLine>` with `List<OrderLine>` in `RecurringOrder`, add a line to `april`, and look at `march`.
2. Records compare by value: `march == (march with { })` is `true`, and `march == april` is `false` because the id and date differ. (It works for `Lines` only because `with` shares the same `ImmutableArray`, which compares by array reference, not by content.)
3. Add a `DiscountPercent` property to `OrderTemplate` and forget to copy it in `Clone()`. Which test would have caught it?

#### Interview questions

<details>
<summary>What is the difference between a shallow and a deep copy?</summary>

A shallow copy duplicates the object but keeps the same references for its reference-type fields, so the copy and the original share those inner objects. A deep copy also duplicates the inner objects, so the two are independent.
</details>

<details>
<summary>Is the <code>with</code> expression a deep copy?</summary>

No, it is shallow: properties are copied as they are, so a mutable list would be shared. It is safe when the record's members are immutable.
</details>

<details>
<summary>Why should you not implement <code>ICloneable</code>?</summary>

Its contract does not say whether the copy is deep or shallow, and it returns `object`. Callers cannot rely on its behaviour, so Microsoft's guidelines recommend a clearly named, strongly typed copy method instead.
</details>

---

## 5. Structural patterns

**Structural** patterns are about **how objects are wrapped and combined** into larger structures: putting one object inside another to change its interface, add behaviour, control access, or treat a group like a single thing. Almost all of them are forms of composition (section [3.5](#35-the-principles-under-the-patterns)): an object holds another and forwards calls to it.

| Pattern | Relevance | In one line |
|---|---|---|
| [5.1 Adapter](#51-adapter) | ⭐⭐⭐ Essential | Make a class you cannot change fit the interface your code expects. |
| [5.2 Bridge](#52-bridge) | ⭐ Niche | Split two dimensions that vary independently into two hierarchies joined by a reference. |
| [5.3 Composite](#53-composite) | ⭐⭐ Useful | Treat a single item and a group of items through the same interface (trees). |
| [5.4 Decorator](#54-decorator) | ⭐⭐⭐ Essential | Wrap an object to add behaviour, keeping its interface; wrappers stack. |
| [5.5 Facade](#55-facade) | ⭐⭐⭐ Essential | One simple entry point in front of a set of subsystems. |
| [5.6 Flyweight](#56-flyweight) | 🕰 Historical | Share one object among many users instead of keeping thousands of identical copies. |
| [5.7 Proxy](#57-proxy) | ⭐⭐ Useful | A stand-in with the same interface that controls access to the real object. |

**The wrapper family.** Four of these patterns look almost identical in a class diagram: a class holds a reference to another and forwards calls. What differs is the **intent**:

```mermaid
flowchart LR
    C[Your code] --> A[Adapter] --> X[Object with<br/>another interface]
    C --> D[Decorator] --> Y[Object with<br/>the same interface]
    C --> P[Proxy] --> Z[Object with the same<br/>interface, guarded]
    C --> F[Facade] --> S1[Subsystem 1]
    F --> S2[Subsystem 2]
    F --> S3[Subsystem 3]
```

- **Adapter** changes the interface (theirs → ours).
- **Decorator** keeps the interface and *adds* behaviour; you can stack several.
- **Proxy** keeps the interface and *controls access* (later, only for some users, somewhere else).
- **Facade** offers a *new, simpler* interface over *several* objects.

Section [8.1](#81-decorator-proxy-adapter-facade) compares them side by side.

### 5.1 Adapter

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Structural |
| Intent | Convert the interface of a class into the interface its clients expect, so classes that could not work together can. |
| Also known as | Wrapper |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- adapter` |
| Code | [`src/Patterns.Structural/Adapter/`](../src/Patterns.Structural/Adapter/) |

#### The problem

The shop ships through an external carrier. The carrier gives us an old SDK (Software Development Kit, the library they publish to talk to their service) with one method, and a peculiar format: you send a text payload `"<country>;<units>;<totalCents>"` and receive `"PRICE=4.00;DAYS=2"`. We cannot change their code. Our code wants to ask a simple question in its own words: *"what does shipping this order cost, and how long does it take?"*

The carrier's rule (simulated in [`Adapter/External/`](../src/Patterns.Structural/Adapter/External/), which every level shares because it is "their" code): price `3.00 + 0.50 × units`; `2` days to Spain, `4` anywhere else. A malformed payload throws `FormatException`. The simulated client also keeps the `LastPayload` it received, so the tests and the demo can show exactly what was sent.

*Analogy:* a travel plug adapter. Your laptop's plug and the hotel's socket do not fit; the adapter sits between them and changes neither.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Structural/Adapter/0-Problem/):

```csharp
public sealed class CheckoutSummary(LegacyCarrierClient carrier)
{
    public string Describe(Order order)
    {
        // PAIN: our code speaks the carrier's private format. The same build-and-parse code lives in
        // OrderTracking too, and both break when the carrier changes its format.
        var cents = (int)(order.Total * 100);
        var answer = carrier.RequestQuote(
            string.Create(CultureInfo.InvariantCulture, $"{order.ShippingAddress.Country};{order.Units};{cents}"));
        var parts = answer.Split(';');
        var price = decimal.Parse(parts[0]["PRICE=".Length..], CultureInfo.InvariantCulture);
        var days = int.Parse(parts[1]["DAYS=".Length..], CultureInfo.InvariantCulture);
        return string.Create(CultureInfo.InvariantCulture, $"Shipping {price:0.00} in {days} days");
    }
}
```

What hurts: the carrier's format **leaks** into our classes, it is duplicated (`CheckoutSummary` and `OrderTracking.DeliveryDays` each have a copy, written slightly differently), and a test of the checkout needs the carrier's SDK.

#### Structure

```mermaid
classDiagram
    class IShippingQuoteProvider {
        <<Target>>
        +Quote(Order order) ShippingQuote
    }
    class LegacyCarrierAdapter {
        <<Adapter>>
        -LegacyCarrierClient _client
        +Quote(Order order) ShippingQuote
    }
    class LegacyCarrierClient {
        <<Adaptee>>
        +RequestQuote(string payload) string
    }
    class CheckoutSummary {
        <<Client>>
    }
    CheckoutSummary --> IShippingQuoteProvider
    IShippingQuoteProvider <|.. LegacyCarrierAdapter
    LegacyCarrierAdapter --> LegacyCarrierClient
```

| Role | Our class | Responsibility |
|---|---|---|
| Target | `IShippingQuoteProvider` | The interface *our* code wants, in our words: `ShippingQuote Quote(Order)`. |
| Adaptee | `LegacyCarrierClient` | The existing class with the incompatible interface. We do not change it. |
| Adapter | `LegacyCarrierAdapter` | Implements the target by translating to and from the adaptee's format. |
| Client | `CheckoutSummary` | Uses the target only. |

`IShippingQuoteProvider` is also called a **port**: an interface owned by our code that describes what we need from the outside world. The adapter plugs the outside world into it.

#### How it runs

```mermaid
sequenceDiagram
    participant Client as CheckoutSummary
    participant Adapter as LegacyCarrierAdapter
    participant SDK as LegacyCarrierClient
    Client->>Adapter: Quote(order: 2 books to Madrid)
    Note right of Adapter: Order to payload "ES#59;2#59;2500"
    Adapter->>SDK: RequestQuote("ES#59;2#59;2500")
    SDK-->>Adapter: "PRICE=4.00#59;DAYS=2"
    Note right of Adapter: text to ShippingQuote, invariant culture
    Adapter-->>Client: ShippingQuote(4.00, 2)
```

#### By hand

```csharp
public sealed record ShippingQuote(decimal Price, int Days);

// Role: Target — what our code needs from a carrier, in our own terms.
public interface IShippingQuoteProvider
{
    ShippingQuote Quote(Order order);
}

// Role: Adapter — speaks the legacy carrier's text protocol so nobody else has to.
// Guide: §5.1
public sealed class LegacyCarrierAdapter(LegacyCarrierClient client) : IShippingQuoteProvider
{
    public ShippingQuote Quote(Order order)
    {
        var cents = (int)(order.Total * 100);
        var payload = string.Create(CultureInfo.InvariantCulture,
            $"{order.ShippingAddress.Country};{order.Units};{cents}");

        var fields = client.RequestQuote(payload)
            .Split(';')
            .Select(part => part.Split('='))
            .ToDictionary(pair => pair[0], pair => pair[1]);

        // InvariantCulture: the carrier always writes "4.00". Under a Spanish culture a plain
        // decimal.Parse would read "." as the thousands separator and return 400.
        return new ShippingQuote(
            decimal.Parse(fields["PRICE"], CultureInfo.InvariantCulture),
            int.Parse(fields["DAYS"], CultureInfo.InvariantCulture));
    }
}
```

Two books to Madrid give `ShippingQuote(4.00, 2)`; to Lisbon, `ShippingQuote(4.00, 4)`. The carrier's format now lives in **one** class. `CheckoutSummary` depends on `IShippingQuoteProvider`, so its tests use a fake provider, and switching carriers means writing a new adapter, not editing the checkout.

**Object adapter vs class adapter.** The GoF book describes two forms:

| | Object adapter | Class adapter |
|---|---|---|
| How | The adapter **holds** an adaptee and forwards to it (composition). | The adapter **inherits** from the adaptee and implements the target. |
| In C# | The normal form. | Rarely possible: C# has single class inheritance, and SDK classes are often `sealed`. |
| Can adapt | The adaptee and any subclass of it; the adaptee can be injected. | Only that one class. |

This repository, like almost all C#, uses object adapters.

#### In .NET

The framework uses adapters wherever two interfaces meet. The DotNet level codes **`StreamReader`** ([`2-DotNet/`](../src/Patterns.Structural/Adapter/2-DotNet/)): a `Stream` only knows bytes (`Read(byte[] buffer, …)`), while our code wants lines of text. `StreamReader` adapts the `Stream` interface to the `TextReader` one, decoding bytes into characters on the way:

```csharp
// Guide: §5.1
public static class CarrierRateFile
{
    // The carrier's rate file has one "<country>;<days>" per line, for example "ES;2\nPT;4".
    public static IReadOnlyDictionary<string, int> ReadDeliveryDays(Stream file)
    {
        // Adapter: bytes → text lines. leaveOpen: the caller owns the stream and disposes it.
        using var reader = new StreamReader(file, Encoding.UTF8, leaveOpen: true);
        var days = new Dictionary<string, int>();
        while (reader.ReadLine() is { } line)
        {
            var parts = line.Split(';');
            days[parts[0]] = int.Parse(parts[1], CultureInfo.InvariantCulture);
        }
        return days;
    }
}
```

Other adapters you meet in .NET:

- `StringReader` / `StringWriter`: a `string` seen as a `TextReader` / `TextWriter`.
- `PipeReader.AsStream()` / `PipeWriter.AsStream()` (`System.IO.Pipelines`): a pipe seen as a `Stream`, for APIs that only accept streams.
- Every *typed client* you write around an `HttpClient` to call an external API is an adapter, as is every class that wraps a vendor SDK behind one of your interfaces.

#### In the ecosystem

**AutoMapper** copies data between objects of different shapes (for example from a domain object to a DTO, a data transfer object, sent as JSON, JavaScript Object Notation). People sometimes call that "adapting", but it is a different thing: Adapter converts an **interface** (calls and behaviour) so two pieces of code can work together; mapping converts **data** from one shape to another. AutoMapper moved to a **commercial licence in 2025** (with a free tier for some users; check the current terms). Simpler alternatives: a hand-written mapping method (`ToDto()`), which is explicit and found by "go to definition", or a source generator such as **Mapperly** (Apache-2.0), which writes that method for you at compile time.

#### When to use it

- You must use a class you cannot change (an SDK, legacy code, a generated client) and its interface does not fit yours.
- You want your code to depend on **your** interface (a port) so the outside service can be replaced or faked in tests.
- Two existing interfaces must work together and neither can be changed.

#### When NOT to use it

- **You own both sides:** change one of them instead of adding a translator between them.
- **The external interface already fits** what you need: wrapping it "just in case" is an interface with one implementation that only forwards (section [3.6](#36-patternitis)).
- **You only need to copy data between shapes:** write a mapping method; you do not need an adapter class.

#### Costs

- One more class and interface per external dependency.
- The adapter must be kept in sync with the external format, and its translation can hide information (errors, extra fields) if done carelessly.

#### Relevance today

⭐⭐⭐ **Essential.** Every application that talks to an external service, a vendor SDK or legacy code needs adapters, and hexagonal ("ports and adapters") architectures are named after them.

#### Relatives

- **Facade** ([5.5](#55-facade)) also wraps, but it simplifies *several* objects behind a *new* interface; an adapter makes *one* object fit an *existing* interface. **Decorator** ([5.4](#54-decorator)) and **Proxy** ([5.7](#57-proxy)) keep the interface. Comparison in [8.1](#81-decorator-proxy-adapter-facade).
- **Bridge** ([5.2](#52-bridge)) separates abstraction and implementation *by design*; Adapter fixes a mismatch *after the fact*.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- adapter
```

1. Change the simulated carrier to answer `"PRICE=4,00;DAYS=2"`. Nothing throws: with the invariant culture `,` is the thousands separator, so the price silently becomes **400**. Find where you would have to fix it in each level (in Problem, count the copies).
2. Write a `FakeQuoteProvider` that always returns `ShippingQuote(0, 1)` and test `CheckoutSummary` with it. Try the same against the Problem version.
3. Feed `CarrierRateFile` a file with Windows line endings (`"ES;2\r\nPT;4"`). Does `ReadLine` handle it?

#### Interview questions

<details>
<summary>What is the difference between an object adapter and a class adapter?</summary>

An object adapter holds an instance of the adaptee and forwards to it (composition). A class adapter inherits from the adaptee and implements the target interface. C# code almost always uses object adapters because of single inheritance and sealed classes.
</details>

<details>
<summary>How is Adapter different from Facade?</summary>

Adapter makes one existing class fit an interface that already exists (yours). Facade designs a new, simpler interface over several classes. One converts, the other simplifies.
</details>

<details>
<summary>Is mapping a DTO to a domain object the Adapter pattern?</summary>

Not really. Adapter converts an interface, so code can call another object through it. Mapping converts data from one shape to another; it is a function, not a wrapper.
</details>

### 5.2 Bridge

#### Card

| | |
|---|---|
| Relevance | ⭐ Niche |
| Family | Structural |
| Intent | Decouple an abstraction from its implementation so the two can vary independently. |
| Also known as | Handle/Body |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- bridge` |
| Code | [`src/Patterns.Structural/Bridge/`](../src/Patterns.Structural/Bridge/) |

#### The problem

The shop sends **kinds** of notification (order shipped, payment failed) through **channels** (email, SMS). These are two independent dimensions: a new kind should work on every channel, and a new channel should carry every kind.

*Analogy:* a TV remote and a TV. Remotes (basic, with voice control) and TVs (brand A, brand B) evolve separately; any remote works with any TV through a common set of signals.

#### Without the pattern

With inheritance alone, every combination becomes a class:

```csharp
class OrderShippedEmail { … }   class OrderShippedSms { … }
class PaymentFailedEmail { … }  class PaymentFailedSms { … }
// A third channel (push) adds 2 classes; a third kind (refund issued) adds 3 more.
// Kinds × channels classes, and the text of each kind is repeated once per channel.
```

What hurts: the number of classes is the **product** of the two dimensions, and the same logic is copied across the combinations.

#### Structure

```mermaid
classDiagram
    class Notification {
        <<Abstraction>>
        #IMessageChannel Channel
        +Send(Customer customer)* string
    }
    class OrderShippedNotification {
        <<RefinedAbstraction>>
    }
    class PaymentFailedNotification {
        <<RefinedAbstraction>>
    }
    class IMessageChannel {
        <<Implementor>>
        +Deliver(Customer customer, string title, string body) string
    }
    class EmailChannel {
        <<ConcreteImplementor>>
    }
    class SmsChannel {
        <<ConcreteImplementor>>
    }
    Notification <|-- OrderShippedNotification
    Notification <|-- PaymentFailedNotification
    Notification o-- IMessageChannel : bridge
    IMessageChannel <|.. EmailChannel
    IMessageChannel <|.. SmsChannel
```

| Role | Our class | Responsibility |
|---|---|---|
| Abstraction | `Notification` | What the shop wants to say; holds a channel (the "bridge"). |
| RefinedAbstraction | `OrderShippedNotification`, `PaymentFailedNotification` | Each kind builds its title and body. |
| Implementor | `IMessageChannel` | How a message physically reaches a customer. |
| ConcreteImplementor | `EmailChannel`, `SmsChannel` | One per channel. |

Two hierarchies of 2 classes each give the 4 combinations; a new channel or kind adds **one** class.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Kind as OrderShippedNotification
    participant Channel as SmsChannel
    Client->>Kind: Send(ana)
    Note right of Kind: title "Order shipped", body from the order id
    Kind->>Channel: Deliver(ana, title, body)
    Channel-->>Kind: "[sms] to Ana | Order shipped: Order 1a2b3c4d is on its way."
    Kind-->>Client: the delivered text
```

#### By hand

```csharp
// Role: Implementor — how a message reaches a customer.
// Guide: §5.2
public interface IMessageChannel
{
    string Deliver(Customer customer, string title, string body);
}

// Role: ConcreteImplementor — email: addressed to the customer's email.
public sealed class EmailChannel : IMessageChannel
{
    public string Deliver(Customer customer, string title, string body) =>
        $"[email] to {customer.Email} | {title} | {body}";
}

// Role: ConcreteImplementor — SMS: short, addressed by name (a real one would use a phone number).
public sealed class SmsChannel : IMessageChannel
{
    public string Deliver(Customer customer, string title, string body) =>
        $"[sms] to {customer.Name} | {title}: {body}";
}

// Role: Abstraction — what the shop wants to say, independent of how it travels.
public abstract class Notification(IMessageChannel channel)
{
    protected IMessageChannel Channel { get; } = channel; // the bridge
    public abstract string Send(Customer customer);
}

// Role: RefinedAbstraction — "your order has shipped".
public sealed class OrderShippedNotification(IMessageChannel channel, Guid orderId) : Notification(channel)
{
    public override string Send(Customer customer) =>
        Channel.Deliver(customer, "Order shipped", $"Order {orderId.ToString()[..8]} is on its way.");
}
```

`PaymentFailedNotification(channel, 25.00m)` sends title `"Payment failed"` and body `"We could not charge 25.00."`. Any kind runs on any channel: `new PaymentFailedNotification(new EmailChannel(), 25.00m).Send(ana)` gives `"[email] to ana@example.com | Payment failed | We could not charge 25.00."`.

The word "implementation" in the GoF intent does not mean "the class that implements the interface". It means **the lower-level side the abstraction is built on** (here, the delivery mechanism).

#### In .NET

**Logging** in .NET is a bridge ([`2-DotNet/`](../src/Patterns.Structural/Bridge/2-DotNet/)). Your code writes to `ILogger` (the abstraction, with refinements like the `LogInformation` and `LogError` extension methods and `ILogger<T>`); **logging providers** (console, debug, Application Insights, a file…) are the implementors. The two sides vary independently: add a provider and every log call in the application uses it; add log calls and every provider receives them.

```csharp
// Role: ConcreteImplementor — a logging provider that keeps lines in a list (handy in tests).
// Guide: §5.2
public sealed class ListLoggerProvider : ILoggerProvider
{
    private readonly List<string> _lines = [];

    public IReadOnlyList<string> Lines => _lines;

    public ILogger CreateLogger(string categoryName) => new ListLogger(categoryName, _lines);

    public void Dispose()
    {
        // Nothing to release: the lines stay readable after the logger factory is disposed.
    }

    private sealed class ListLogger(string category, List<string> lines) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            lines.Add($"{category}: {formatter(state, exception)}");
    }
}

// What the checkout logs, through [LoggerMessage] (the repository's logging convention; section 7.7 explains it).
public static partial class CheckoutLog
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Order {OrderNumber} placed")]
    public static partial void OrderPlaced(ILogger logger, string orderNumber);
}

var provider = new ListLoggerProvider();
using var factory = LoggerFactory.Create(logging => logging.AddProvider(provider));
CheckoutLog.OrderPlaced(factory.CreateLogger("Checkout"), "1a2b3c4d");
// provider.Lines: ["Checkout: Order 1a2b3c4d placed"]
```

The code that logs never learns which providers exist; `LoggerFactory` builds a logger that forwards each call to one logger per provider.

#### When to use it

- Two dimensions vary independently and you see (or foresee) a class per combination.
- You want to choose or change the implementation at run time (the channel per customer preference).
- A library must run on several platforms or back-ends behind one API (drivers, logging, storage).

#### When NOT to use it

- **Only one dimension really varies:** a plain interface with implementations (Strategy, [6.9](#69-strategy)) is enough.
- **The combinations are few and fixed:** two or three small classes are simpler than two hierarchies.
- Do not design a bridge up front "in case" a second dimension appears; refactor to it when the class count starts to multiply.

#### Costs

- Two hierarchies instead of one; the split must be chosen well, or every change crosses the bridge.
- One more level of indirection on every call.

#### Relevance today

⭐ **Niche.** You rarely design a bridge in application code, but you use them constantly (logging, ADO.NET drivers, `Stream` over different storages). Recognising the shape explains why those APIs are built as they are.

#### Relatives

- **Adapter** ([5.1](#51-adapter)) makes mismatched interfaces fit after the fact; Bridge separates the two sides by design, up front.
- **Strategy** ([6.9](#69-strategy)) has the same shape (an object holding an interface); Bridge is about the *structure* of two hierarchies, Strategy about swapping *one algorithm*.
- **Abstract Factory** ([4.3](#43-abstract-factory)) can create the right implementor for an abstraction.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- bridge
```

1. Add a `PushChannel`. How many notification classes did you change?
2. Add a `RefundIssuedNotification`. How many channel classes did you change?
3. Register two providers in the `LoggerFactory` (yours and `AddConsole()` if you add the package to a scratch project) and log once: both receive it.

#### Interview questions

<details>
<summary>What problem does Bridge solve?</summary>

The "class explosion" when two independent dimensions are combined through inheritance (kinds × channels). It splits them into two hierarchies connected by a reference, so each grows on its own.
</details>

<details>
<summary>Where is Bridge in .NET?</summary>

In logging: `ILogger` is the abstraction your code uses, and `ILoggerProvider` implementations (console, debug, third-party sinks) are the implementors; they vary independently.
</details>

### 5.3 Composite

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Structural |
| Intent | Compose objects into tree structures and let clients treat individual objects and groups of objects the same way. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- composite` |
| Code | [`src/Patterns.Structural/Composite/`](../src/Patterns.Structural/Composite/) |

#### The problem

The shop sells **bundles**: a "Starter kit" contains a book, a mug and an "Audio" bundle, which contains the headphones. A bundle can hold products and other bundles, to any depth. The shop needs the price of anything (a product or a bundle) and how many products it contains, without caring which one it is holding.

The example: `Starter kit` = [Book 12.50, Mug 8.00, `Audio` = [Headphones 59.90]] → price **80.40**, **3** products.

*Analogy:* folders and files. A folder's size is the sum of what it contains, files and folders alike, and you ask a folder for its size exactly as you ask a file.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Structural/Composite/0-Problem/):

```csharp
public sealed class CatalogEntry
{
    public Product? Product { get; init; }                                    // set for a single product…
    public IList<CatalogEntry> Children { get; } = new List<CatalogEntry>(); // …or children, for a bundle

    public static decimal PriceOf(CatalogEntry entry)
    {
        // PAIN: every operation (price, count, export…) repeats this "is it one or many?" check.
        if (entry.Product is not null) { return entry.Product.Price; }
        decimal total = 0;
        foreach (var child in entry.Children) { total += PriceOf(child); }
        return total;
    }

    // ProductCountOf(entry) repeats the same check: if (entry.Product is not null) return 1; …
}
```

What hurts: the caller must know whether it holds one thing or many, and that `if` is copied into every operation. Nothing prevents an entry with both a product and children.

#### Structure

```mermaid
classDiagram
    class ICatalogItem {
        <<Component>>
        +string Name
        +decimal Price
        +int ProductCount
    }
    class ProductItem {
        <<Leaf>>
        +ProductItem(Product product)
    }
    class Bundle {
        <<Composite>>
        +Bundle(string name)
        +Add(ICatalogItem item)
        +IReadOnlyList~ICatalogItem~ Items
    }
    ICatalogItem <|.. ProductItem
    ICatalogItem <|.. Bundle
    Bundle o-- ICatalogItem : children
```

| Role | Our class | Responsibility |
|---|---|---|
| Component | `ICatalogItem` | What every catalog item can tell you: name, price, product count. |
| Leaf | `ProductItem` | A single product; answers from its own data. |
| Composite | `Bundle` | Holds children (leaves or composites) and answers by asking them. |

The key is the arrow from `Bundle` back to `ICatalogItem`: a bundle holds *components*, so it can hold other bundles, and the tree can be as deep as needed.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Kit as Bundle "Starter kit"
    participant Book as ProductItem Book
    participant Mug as ProductItem Mug
    participant Audio as Bundle "Audio"
    participant Phones as ProductItem Headphones
    Client->>Kit: Price
    Kit->>Book: Price
    Book-->>Kit: 12.50
    Kit->>Mug: Price
    Mug-->>Kit: 8.00
    Kit->>Audio: Price
    Audio->>Phones: Price
    Phones-->>Audio: 59.90
    Audio-->>Kit: 59.90
    Kit-->>Client: 80.40
```

#### By hand

```csharp
// Role: Component — anything in the catalog: a single product or a bundle of items.
// Guide: §5.3
public interface ICatalogItem
{
    string Name { get; }
    decimal Price { get; }
    int ProductCount { get; }
}

// Role: Leaf — one product.
public sealed class ProductItem(Product product) : ICatalogItem
{
    public string Name => product.Name;
    public decimal Price => product.Price;
    public int ProductCount => 1;
}

// Role: Composite — a bundle; answers every question by asking its children.
public sealed class Bundle(string name) : ICatalogItem
{
    private readonly List<ICatalogItem> _items = [];

    public string Name => name;
    public IReadOnlyList<ICatalogItem> Items => _items;
    public decimal Price => _items.Sum(item => item.Price);
    public int ProductCount => _items.Sum(item => item.ProductCount);

    public void Add(ICatalogItem item)
    {
        // A cycle would make Price recurse forever and crash the process (see below).
        if (ReferenceEquals(item, this) || (item is Bundle bundle && bundle.Contains(this)))
        {
            throw new InvalidOperationException("A bundle cannot contain itself.");
        }
        _items.Add(item);
    }

    private bool Contains(ICatalogItem target) =>
        _items.Any(item => ReferenceEquals(item, target) || (item is Bundle inner && inner.Contains(target)));
}
```

The client asks `kit.Price` and never checks what it holds. An empty bundle costs `0.00`, because the sum of nothing is zero.

**The cycle check.** A tree must not contain itself. If `kit.Add(kit)` were allowed (or `audio.Add(kit)` when `kit` already contains `audio`), `kit.Price` would ask `audio.Price`, which would ask `kit.Price`, and so on forever. In .NET that ends in a `StackOverflowException`, which **cannot be caught**: the whole process dies. So `Add` refuses it up front with a clear exception. The tests `Bundle_CannotContainItself` and `Bundle_CannotContainItsParent` pin both cases.

**Where does `Add` live?** GoF discusses two options. Putting `Add` on the component interface (*transparency*) lets the client treat everything identically, but then a leaf must do something with `Add` (throw?). Putting it only on the composite (*safety*), as here, means the compiler stops you from adding children to a product. In C#, the safe version is the usual choice.

#### In .NET

**Configuration** in .NET is a composite ([`2-DotNet/`](../src/Patterns.Structural/Composite/2-DotNet/)). Every `IConfigurationSection` is itself an `IConfiguration`: a section either has a value (a leaf) or has children (a composite), and you navigate both the same way.

```csharp
// appsettings-like data, in memory: Shipping → Carriers → Fast/Slow → Days
var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Shipping:Carriers:Fast:Days"] = "1",
        ["Shipping:Carriers:Slow:Days"] = "5",
    })
    .Build();

// Guide: §5.3
public static IReadOnlyDictionary<string, int> Read(IConfiguration configuration) =>
    configuration.GetSection("Shipping:Carriers")
        .GetChildren() // each child section is again an IConfiguration
        .ToDictionary(carrier => carrier.Key,
                      carrier => int.Parse(carrier["Days"]!, CultureInfo.InvariantCulture));
// { Fast: 1, Slow: 5 }
```

Other composites in .NET: `CompositeFileProvider` combines several `IFileProvider`s into one; `CompositeChangeToken` combines several change tokens; the ASP.NET Core and Windows Forms/WPF control trees; `Expression` trees (section [6.3](#63-interpreter)).

#### When to use it

- Your data is naturally a **tree** (bundles, menus, folders, organisation charts, expressions) and clients should not care whether they hold a leaf or a branch.
- Operations are "do it to me and to all my children" (sum, count, render, validate).

#### When NOT to use it

- **The structure is flat** (a list of products): a `List<T>` and LINQ are enough.
- **Leaves and groups need very different operations:** forcing them under one interface leads to methods that throw `NotSupportedException` on half the types.
- **Only one operation walks the tree, once:** a recursive function over a simple record may be clearer than an interface and two classes.

#### Costs

- The common interface can become too general: it is hard to restrict what a composite may contain (only books, at most two levels…) without type checks.
- Recursion: deep trees cost stack depth, and cycles must be prevented explicitly.

#### Relevance today

⭐⭐ **Useful.** Trees are everywhere (UI, configuration, expressions, file systems, menus) and Composite is how you model them without `if (isGroup)` everywhere.

#### Relatives

- **Decorator** ([5.4](#54-decorator)) has a similar diagram, but wraps **one** object to add behaviour, while a composite holds **many** to treat them as one. Comparison in [8.6](#86-composite-and-decorator).
- **Visitor** ([6.11](#611-visitor)) adds new operations to a composite tree without changing its classes.
- **Iterator** ([6.4](#64-iterator)) walks a composite tree.
- **Builder** ([4.4](#44-builder)) is a convenient way to assemble a tree.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- composite
```

1. Add a `Discount` property to `ICatalogItem` and implement it in both classes. Then try the same in the Problem version: how many `if`s did you write?
2. Try `audio.Add(kit)` after `kit.Add(audio)` and read the message.
3. Add a third carrier to the in-memory configuration under `Shipping:Carriers:Pickup:Days` and run `CarrierSettings.Read` again: no code changes.

#### Interview questions

<details>
<summary>What does Composite let a client do?</summary>

Treat a single object and a group of objects through the same interface, so code that works on one item also works on a whole tree of them.
</details>

<details>
<summary>Why must a composite prevent cycles?</summary>

Operations recurse through the children; a cycle makes the recursion infinite, which in .NET ends in an uncatchable `StackOverflowException` that kills the process.
</details>

<details>
<summary>Should <code>Add</code> be on the component interface or on the composite?</summary>

On the component it is more uniform (transparency) but leaves must reject it at run time; on the composite only it is type-safe (safety). C# code usually chooses safety.
</details>

### 5.4 Decorator

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Structural |
| Intent | Attach additional responsibilities to an object dynamically, by wrapping it in objects with the same interface. |
| Also known as | Wrapper |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- decorator` |
| Code | [`src/Patterns.Structural/Decorator/`](../src/Patterns.Structural/Decorator/) |

#### The problem

The price of an order starts as the sum of its lines, then rules apply on top: VAT, a coupon, maybe a loyalty discount or rounding rules later. Different checkouts combine different rules, in different orders.

The example order is 8 books: **100.00**. With VAT (21 %) → **121.00**. With a 5.00 coupon → **95.00**. Both, VAT first → **116.00**; coupon first → **114.95**. The order of the rules changes the price.

*Analogy:* a coffee. The base is an espresso; milk, extra shot and syrup are added on top, in any combination, and each one adds its cost. You do not need a menu item for every combination.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Structural/Decorator/0-Problem/):

```csharp
public sealed class PriceCalculator
{
    // PAIN: each new rule adds a parameter and another branch; the order of the steps is hidden in
    // here, and there is no way to choose a different order.
    public decimal Price(Order order, bool applyVat, decimal? coupon)
    {
        var price = order.Total;
        if (applyVat) { price = decimal.Round(price * 1.21m, 2, MidpointRounding.AwayFromZero); }
        if (coupon is { } amount) { price = Math.Max(0, price - amount); }
        return price;
    }
}
```

What hurts: rules are welded into one method. A loyalty discount means another parameter and every caller changes; "coupon before VAT" for one market means another flag.

#### Structure

```mermaid
classDiagram
    class IPriceCalculator {
        <<Component>>
        +PriceOf(Order order) decimal
    }
    class BasePrice {
        <<ConcreteComponent>>
    }
    class VatDecorator {
        <<ConcreteDecorator>>
        -IPriceCalculator _inner
        -decimal _rate
    }
    class CouponDecorator {
        <<ConcreteDecorator>>
        -IPriceCalculator _inner
        -decimal _amount
    }
    IPriceCalculator <|.. BasePrice
    IPriceCalculator <|.. VatDecorator
    IPriceCalculator <|.. CouponDecorator
    VatDecorator --> IPriceCalculator : wraps
    CouponDecorator --> IPriceCalculator : wraps
```

| Role | Our class | Responsibility |
|---|---|---|
| Component | `IPriceCalculator` | The interface everyone shares. |
| ConcreteComponent | `BasePrice` | The real work at the centre: `order.Total`. |
| ConcreteDecorator | `VatDecorator`, `CouponDecorator` | Hold an inner `IPriceCalculator`, call it, and change its result. |

The trick is that a decorator **is** an `IPriceCalculator` and **holds** one. So decorators can wrap the base or each other, in any number and order. GoF also has an abstract `Decorator` base class holding the inner component; in C# with small decorators it is usually skipped.

#### How it runs

VAT first, then the coupon: `new CouponDecorator(new VatDecorator(new BasePrice(), 0.21m), 5m)`. The call goes **in** through each wrapper to the centre, and the result comes back **out** through each one, which changes it on the way:

```mermaid
sequenceDiagram
    participant Client
    participant Coupon as CouponDecorator (5.00)
    participant Vat as VatDecorator (21 %)
    participant Base as BasePrice
    Client->>Coupon: PriceOf(order)
    Coupon->>Vat: PriceOf(order)
    Vat->>Base: PriceOf(order)
    Base-->>Vat: 100.00
    Note right of Vat: 100.00 × 1.21
    Vat-->>Coupon: 121.00
    Note right of Coupon: 121.00 − 5.00
    Coupon-->>Client: 116.00
```

Swap the wrappers (`new VatDecorator(new CouponDecorator(new BasePrice(), 5m), 0.21m)`) and the coupon applies first: `(100.00 − 5.00) × 1.21 = 114.95`. The **outermost** decorator acts **last** on the result.

#### By hand

```csharp
// Role: Component — anything that can price an order.
// Guide: §5.4
public interface IPriceCalculator
{
    decimal PriceOf(Order order);
}

// Role: ConcreteComponent — the undecorated price.
public sealed class BasePrice : IPriceCalculator
{
    public decimal PriceOf(Order order) => order.Total;
}

// Role: ConcreteDecorator — adds VAT to whatever the inner calculator says.
public sealed class VatDecorator(IPriceCalculator inner, decimal rate) : IPriceCalculator
{
    public decimal PriceOf(Order order) =>
        decimal.Round(inner.PriceOf(order) * (1 + rate), 2, MidpointRounding.AwayFromZero);
}

// Role: ConcreteDecorator — subtracts a coupon, never going below zero.
public sealed class CouponDecorator(IPriceCalculator inner, decimal amount) : IPriceCalculator
{
    public decimal PriceOf(Order order) =>
        decimal.Round(Math.Max(0, inner.PriceOf(order) - amount), 2, MidpointRounding.AwayFromZero);
}
```

Each rule is a small class with one job, testable on its own; new rules are new classes; the order is decided where the chain is **assembled**, visible in one line. A coupon of 500 on a 100.00 order gives `0.00`, not a negative price.

#### In .NET

**`DelegatingHandler`** is the decorator of `HttpClient` ([`2-DotNet/`](../src/Patterns.Structural/Decorator/2-DotNet/)). Every `HttpClient` sends requests through a chain of `HttpMessageHandler`s; a `DelegatingHandler` holds an `InnerHandler`, can change the request on the way in and the response on the way out, and calls `base.SendAsync` to pass the request on.

```csharp
// Role: ConcreteDecorator — stamps every outgoing request with a correlation id.
// Guide: §5.4
public sealed class CorrelationIdHandler(Func<string> newId) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Correlation-Id", newId());
        return base.SendAsync(request, cancellationToken); // on to the inner handler
    }
}

// Built by hand (CarrierHttpClient.Create): the outermost handler runs first on the way in.
// StubCarrierHandler is the "network": it answers 200 "ok" and writes the correlation header it received
// into the same log, so the log shows the request passed the log handler and reached the carrier with the
// header. (CorrelationIdHandler writes nothing itself: Try it 3 asks what changes if you swap the two.)
var log = new List<string>();
using var client = new HttpClient(new CorrelationIdHandler(() => "abc-123")
{
    InnerHandler = new RequestLogHandler(log) { InnerHandler = new StubCarrierHandler(log) },
});
await client.GetAsync(new Uri("https://carrier.example.com/quote"));
// log: ["GET https://carrier.example.com/quote", "carrier saw X-Correlation-Id: abc-123"]
```

`RequestLogHandler` and `StubCarrierHandler` take an `ICollection<string>`, not a `List<string>`: public members should not expose `List<T>` (analyzer CA1002), and the handlers only need to add.

In a real application you do not nest them by hand: **`IHttpClientFactory`** builds the chain from the registrations, in the order you add them (the first one added is the outermost):

```csharp
var requestLog = new List<string>();
// Handlers are registered as transient services; constructor arguments that are not services need a factory.
services.AddTransient(_ => new CorrelationIdHandler(() => Guid.NewGuid().ToString()));
services.AddTransient(_ => new RequestLogHandler(requestLog));
services.AddHttpClient("carrier")
    .AddHttpMessageHandler<CorrelationIdHandler>() // outermost
    .AddHttpMessageHandler<RequestLogHandler>();   // then this, then the real network handler
```

Resilience (retries, timeouts, circuit breakers from `Microsoft.Extensions.Http.Resilience`) is added to the same chain as more handlers.

**Decorating a registration in DI.** The Microsoft container has no `Decorate` method, but a factory registration does the same by hand:

```csharp
services.AddSingleton<IPriceCalculator>(_ =>
    new CouponDecorator(new VatDecorator(new BasePrice(), 0.21m), 5m));
```

**Streams** are decorators too: `new GZipStream(new BufferedStream(file), CompressionMode.Compress)` compresses the data, buffers it and then writes it to a `FileStream`; every layer is still a `Stream`. `CryptoStream` adds encryption the same way.

#### In the ecosystem

**Scrutor** (MIT licence) adds `Decorate` to the Microsoft container: it replaces an existing registration with the decorator, which receives the original as its inner object. `services.Decorate<IPriceCalculator, LoggingPriceCalculator>()` works when every other constructor parameter is a service; with plain values like a rate, use the factory overload: `services.Decorate<IPriceCalculator>((inner, _) => new VatDecorator(inner, 0.21m))`. For simple cases the factory lambda above, without any library, is enough.

#### When to use it

- You need to add behaviour (logging, caching, retries, validation, metrics, tax…) to an object **without changing it**, and to combine those behaviours freely.
- Different call sites need different combinations or orders.
- Cross-cutting concerns around an interface: a `CachingProductRepository` wrapping the real repository is the classic case.

#### When NOT to use it

- **One fixed extra step** that always applies: put it in the class, or in a single method that calls the next.
- **The behaviour needs the inside of the object**, not just its interface: a decorator only sees what the interface exposes.
- **Long chains that nobody can follow:** if a reader cannot tell what runs and in which order, list the steps explicitly (a pipeline or a plain method).

#### Costs

- Many small objects; a stack trace shows every layer.
- The order of decorators matters and is decided far from the decorators themselves: document it where the chain is assembled and test it, as `Order_Matters` does.
- Identity: the outer object is not the inner one, so code that checks types (`is BasePrice`) breaks.

#### Relevance today

⭐⭐⭐ **Essential.** `DelegatingHandler`, streams, middleware (which is a chain of decorators around the next delegate, see [6.1](#61-chain-of-responsibility)), and decorated services in DI are everyday .NET.

#### Relatives

- **Proxy** ([5.7](#57-proxy)) has the same structure; a proxy *controls access* to its subject, a decorator *adds behaviour*, and decorators are meant to stack. **Adapter** ([5.1](#51-adapter)) changes the interface; a decorator keeps it. Comparison in [8.1](#81-decorator-proxy-adapter-facade).
- **Composite** ([5.3](#53-composite)): a decorator is like a composite with exactly one child. Comparison in [8.6](#86-composite-and-decorator).
- **Chain of Responsibility** ([6.1](#61-chain-of-responsibility)): middleware and handler chains are decorators that may also stop the call.
- **Strategy** ([6.9](#69-strategy)) changes the *inside* of an object (its algorithm); Decorator changes the *outside* (what happens around it).

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- decorator
```

1. Write a `LoyaltyDecorator` (10 % off) and put it in three different positions in the chain. Print the price each time.
2. Add the same rule to the Problem version and count the callers you had to change.
3. Swap the two handlers in the `HttpClient` chain. Does the carrier still receive the correlation header? Does anything in the log change? Why?

#### Interview questions

<details>
<summary>Decorator or inheritance: why wrap instead of subclass?</summary>

Subclasses fix the combination at compile time and need one class per combination; decorators are combined at run time, in any order and number, and each one can be reused around any implementation of the interface.
</details>

<details>
<summary>How does <code>IHttpClientFactory</code> use Decorator?</summary>

Each `AddHttpMessageHandler` registers a `DelegatingHandler`; the factory chains them by setting each one's `InnerHandler` to the next, ending in the real network handler. The first registered is the outermost.
</details>

<details>
<summary>What is the difference between Decorator and Proxy?</summary>

Same structure, different intent: a decorator adds behaviour and is stacked by the client; a proxy controls access to its subject (lazily, securely, remotely) and usually manages the subject itself.
</details>

### 5.5 Facade

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Structural |
| Intent | Provide one simple interface to a set of interfaces in a subsystem. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- facade` |
| Code | [`src/Patterns.Structural/Facade/`](../src/Patterns.Structural/Facade/) |

#### The problem

Placing an order involves four **subsystems** (parts of the system with their own job), simulated in [`Facade/Subsystems/`](../src/Patterns.Structural/Facade/Subsystems/) and shared by every level:

| Subsystem | What it does |
|---|---|
| `Inventory` | Holds the stock. `Reserve(lines)` reserves all lines or none, and throws `"Not enough stock for <name>."`. |
| `PaymentGateway` | `Charge(customer, amount)` takes the money and returns `PAY-0001`, `PAY-0002`… |
| `Shipping` | `Schedule(order)` books the carrier and returns `TRK-0001`, … |
| `Mailer` | `Send(customer, text)` sends the confirmation; `Sent` lists what was sent. |

They must be called in the right order: reserve the stock, charge, schedule shipping, send the email. Both the website and the mobile app place orders.

*Analogy:* a hotel reception. You ask the receptionist for a taxi, a restaurant table and a wake-up call; you do not phone the taxi company, the restaurant and the switchboard yourself, and you do not need to know they exist.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Structural/Facade/0-Problem/):

```csharp
public sealed class WebCheckout(Inventory inventory, PaymentGateway payments, Shipping shipping, Mailer mailer)
{
    public (string PaymentId, string TrackingNumber) Place(Order order)
    {
        // PAIN: the whole sequence is copied in MobileCheckout. A fix here (say, reserving stock
        // before charging) is easy to forget there.
        inventory.Reserve(order.Lines);
        var paymentId = payments.Charge(order.Customer, order.Total);
        var tracking = shipping.Schedule(order);
        mailer.Send(order.Customer, $"Order confirmed. Tracking number: {tracking}.");
        return (paymentId, tracking);
    }
}
```

What hurts: every client knows four subsystems and the right order to call them; the knowledge is duplicated and each copy can drift.

#### Structure

```mermaid
classDiagram
    class CheckoutFacade {
        <<Facade>>
        +PlaceOrder(Order order) CheckoutReceipt
    }
    class Inventory {
        <<Subsystem>>
    }
    class PaymentGateway {
        <<Subsystem>>
    }
    class Shipping {
        <<Subsystem>>
    }
    class Mailer {
        <<Subsystem>>
    }
    class Client {
        <<Client>>
    }
    Client --> CheckoutFacade
    CheckoutFacade --> Inventory
    CheckoutFacade --> PaymentGateway
    CheckoutFacade --> Shipping
    CheckoutFacade --> Mailer
```

| Role | Our class | Responsibility |
|---|---|---|
| Facade | `CheckoutFacade` | Knows which subsystems to call and in what order; offers one method. |
| Subsystem | `Inventory`, `PaymentGateway`, `Shipping`, `Mailer` | Do the real work; they do not know the facade exists. |
| Client | any checkout (web, mobile, the demo) | Calls the facade only. |

The subsystems stay public: a client that needs something special can still use them directly. A facade **simplifies**; it does not hide by force.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Facade as CheckoutFacade
    participant Inventory
    participant Payments as PaymentGateway
    participant Shipping
    participant Mailer
    Client->>Facade: PlaceOrder(order)
    Facade->>Inventory: Reserve(lines)
    Note right of Inventory: first, so nothing is charged if stock is missing
    Facade->>Payments: Charge(ana, 25.00)
    Payments-->>Facade: "PAY-0001"
    Facade->>Shipping: Schedule(order)
    Shipping-->>Facade: "TRK-0001"
    Facade->>Mailer: Send(ana, confirmation)
    Facade-->>Client: CheckoutReceipt(id, PAY-0001, TRK-0001)
```

#### By hand

```csharp
public sealed record CheckoutReceipt(Guid OrderId, string PaymentId, string TrackingNumber);

// Role: Facade — one call to place an order; the order of the steps lives only here.
// Guide: §5.5
public sealed class CheckoutFacade(Inventory inventory, PaymentGateway payments, Shipping shipping, Mailer mailer)
{
    public CheckoutReceipt PlaceOrder(Order order)
    {
        inventory.Reserve(order.Lines); // all-or-nothing, and before charging: no money taken without stock
        var paymentId = payments.Charge(order.Customer, order.Total);
        var tracking = shipping.Schedule(order);
        mailer.Send(order.Customer, $"Order confirmed. Tracking number: {tracking}.");
        return new CheckoutReceipt(order.Id, paymentId, tracking);
    }
}
```

Two books give the receipt `PAY-0001` / `TRK-0001` and the book stock goes from 20 to 18. Ordering six headphones (only five in stock) throws `"Not enough stock for Wireless Headphones."`, and nothing is charged, reserved or mailed: the test `NotEnoughStock_ChargesNothing` checks all four. That guarantee now lives in one place, so every client gets it.

A real facade would also decide what happens when a *later* step fails (release the stock if the payment is declined). That is a compensation, outside this pattern; the facade is the natural place to put it.

#### In .NET

The framework is full of facades: one call that coordinates several lower-level objects. The DotNet level codes the simplest one, **`File`** ([`2-DotNet/`](../src/Patterns.Structural/Facade/2-DotNet/)):

```csharp
// Guide: §5.5
public static class InvoiceFile
{
    // The facade: one line opens the file, encodes the text, writes it and closes everything.
    public static string SaveSimple(string path, string invoice)
    {
        File.WriteAllText(path, invoice);
        return path;
    }

    // What the facade does for you, step by step: a stream, an encoding, a writer, and their disposal.
    public static string SaveTheLongWay(string path, string invoice)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream, Utf8NoBom); // UTF-8 without a byte order mark (BOM), like File.WriteAllText
        writer.Write(invoice);
        return path;
    }

    // ReadSimple (File.ReadAllText) and ReadTheLongWay (FileStream + StreamReader) do the same for reading.
}
```

Both write the same bytes (the test compares them, and reads each file back the other way); `File` just hides the stream, the encoder and the disposal. The long way is still available when you need control (append, share, buffer size).

**`WebApplication`** is a facade on a large scale: one object in front of the host, the server, the configuration, the logging, the DI container and the middleware pipeline. `app.MapGet("/", …)` and `app.Run()` hide dozens of objects.

#### When to use it

- Clients repeat the same sequence of calls to several objects.
- You want one place for the rules of a process (order of steps, what to check first).
- You want to give a simple entry point to a complex library or legacy subsystem, and keep the rest of the code unaware of its details.

#### When NOT to use it

- **There is only one subsystem call behind it:** a facade that forwards one call to one object is an empty layer.
- **The facade grows into a "god object"** that every feature adds a method to: split it by use case (one facade or handler per process).
- **Clients need fine control** of the subsystems most of the time: they will bypass the facade anyway.

#### Costs

- One more class; it can become a dumping ground if not kept to one process.
- It can hide useful options of the subsystems; leave them reachable.

#### Relevance today

⭐⭐⭐ **Essential.** Application services ("place order", "register customer") are facades over the domain and infrastructure, and the framework uses facades everywhere to keep simple things simple (`File`, `WebApplication`, `HttpClient.GetStringAsync`).

#### Relatives

- **Adapter** ([5.1](#51-adapter)) converts one interface into another existing one; a facade invents a new, simpler one over several objects. Comparison in [8.1](#81-decorator-proxy-adapter-facade).
- **Mediator** ([6.5](#65-mediator)) also sits among several objects, but the colleagues *know* the mediator and talk through it; subsystems do not know their facade. Comparison in [8.4](#84-observer-and-mediator).
- **Singleton** ([4.1](#41-singleton)): a stateless facade is often registered as one.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- facade
```

1. In the Problem version, swap the charge and the reserve in `MobileCheckout` only, then order six headphones from mobile: money is charged and the order fails. The facade version cannot drift that way.
2. Add a fifth subsystem (`Analytics.Track(order)`) and count the classes you change in each level.
3. Open `File.WriteAllText` with "go to definition" (or the .NET source browser) and follow what it calls.

#### Interview questions

<details>
<summary>What is a Facade, in one sentence?</summary>

A class that offers one simple interface over several subsystems, so clients do not need to know them or the order to call them.
</details>

<details>
<summary>Does a Facade prevent clients from using the subsystems?</summary>

No. It simplifies the common case; the subsystems stay available for clients that need more control.
</details>

<details>
<summary>How is Facade different from Mediator?</summary>

A facade is called by clients and calls the subsystems, which do not know it exists (one direction). A mediator is known by its colleagues, which talk to each other only through it (both directions).
</details>

### 5.6 Flyweight

#### Card

| | |
|---|---|
| Relevance | 🕰 Historical — the runtime and cheap memory solve it in most cases |
| Family | Structural |
| Intent | Use sharing to support large numbers of fine-grained objects efficiently. |
| Also known as | — |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- flyweight` |
| Code | [`src/Patterns.Structural/Flyweight/`](../src/Patterns.Structural/Flyweight/) |

#### The problem

Imagine the catalog grows to 10,000 entries. Each one shows its category's display name, an icon and the category's VAT rate. If every entry keeps its own copy of that category data, the same three values are stored ten thousand times. Only three categories exist.

*Analogy:* a printed book does not contain a separate metal type for every letter "e" on every page; the printer reuses the same few letter shapes everywhere and only the **position** of each letter changes.

#### Without the pattern

```csharp
// Every entry creates its own category object: 10,000 entries, 10,000 identical copies.
var entries = skus.Select(sku => new CatalogEntry(sku, 12.50m, new CategoryInfo("Books", "📚", 0.04m)));
```

What hurts: memory proportional to the number of entries for data that only has three distinct values.

#### Structure

The pattern splits the state in two:

- **Intrinsic state**: what is the same for many objects and can be shared (the category's name, icon and VAT rate). It lives in the **flyweight**, which must be immutable because many owners share it.
- **Extrinsic state**: what differs per use (the SKU and the price of each entry). It stays outside the flyweight, in the object that uses it.

```mermaid
classDiagram
    class CategoryInfoFactory {
        <<FlyweightFactory>>
        -Dictionary~string, CategoryInfo~ _cache
        +Get(string name) CategoryInfo
        +int Created
    }
    class CategoryInfo {
        <<Flyweight>>
        +string Name
        +string Icon
        +decimal VatRate
    }
    class CatalogEntry {
        <<Client>>
        +string Sku
        +decimal Price
        +CategoryInfo Category
    }
    CategoryInfoFactory o-- CategoryInfo : caches
    CatalogEntry --> CategoryInfo : shares
```

| Role | Our class | Responsibility |
|---|---|---|
| Flyweight | `CategoryInfo` | The shared, immutable intrinsic state. |
| FlyweightFactory | `CategoryInfoFactory` | Hands out flyweights, creating each distinct one only once. |
| Client | `CatalogEntry` | Keeps the extrinsic state and a reference to a shared flyweight. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Factory as CategoryInfoFactory
    Client->>Factory: Get("Books")
    Note right of Factory: not cached: create it (Created = 1)
    Factory-->>Client: books
    Client->>Factory: Get("books")
    Note right of Factory: cached (the name is case-insensitive)
    Factory-->>Client: the same books object
```

#### By hand

```csharp
// Role: Flyweight — category data shared by every entry of that category. Immutable on purpose.
// Guide: §5.6
public sealed record CategoryInfo(string Name, string Icon, decimal VatRate);

// Role: FlyweightFactory — creates each category once and hands out the shared instance.
public sealed class CategoryInfoFactory
{
    private static readonly Dictionary<string, (string Icon, decimal VatRate)> Known =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Books"] = ("📚", 0.04m), ["Electronics"] = ("🔌", 0.21m), ["Home"] = ("🏠", 0.21m),
        };

    // "books" → "Books": the flyweight always carries the canonical name, whatever the caller typed.
    private static readonly Dictionary<string, string> CanonicalNames =
        Known.Keys.ToDictionary(key => key, StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, CategoryInfo> _cache = new(StringComparer.OrdinalIgnoreCase);

    public int Created { get; private set; }

    public CategoryInfo Get(string name)
    {
        if (_cache.TryGetValue(name, out var shared)) { return shared; }
        if (!Known.TryGetValue(name, out var data))
        {
            throw new ArgumentException($"Unknown category '{name}'."); // "Unknown category 'Toys'."
        }

        Created++;
        return _cache[name] = new CategoryInfo(CanonicalNames[name], data.Icon, data.VatRate);
    }
}

// Role: Client — the extrinsic state (SKU, price) plus a reference to the shared category.
public sealed record CatalogEntry(string Sku, decimal Price, CategoryInfo Category);
```

10,000 entries built through the factory create **3** `CategoryInfo` objects (`Created == 3`), and two Books entries hold the very same object (`Assert.Same`). (The VAT rates here are illustrative per-category values for this example; the shop's real rates per country are in [4.1](#41-singleton).)

#### In .NET

**String interning** ([`2-DotNet/`](../src/Patterns.Structural/Flyweight/2-DotNet/)) is the runtime's own flyweight. The runtime keeps an **intern pool**: a table with one shared instance per distinct string value. Every string *literal* in your code is interned automatically, so `"BOOK-001"` written twice is one object. Strings built at run time are not:

The code wraps both calls in `SkuText.Build` and `SkuText.BuildInterned`:

```csharp
// Guide: §5.6
var a = string.Concat("BOOK-", "001");
var b = string.Concat("BOOK-", "001");
// ReferenceEquals(a, b) == false: equal values, two objects.

var internedA = string.Intern(a);
var internedB = string.Intern(b);
// ReferenceEquals(internedA, internedB) == true: both are now the pool's single instance.
```

Interned strings are never freed until the process ends, so interning everything is a memory leak, not an optimisation.

**Why Flyweight is 🕰 in .NET.** The pattern was designed for 1990s machines, where thousands of small objects were a real cost. Today:

- memory is cheap, and the **garbage collector** (GC, the part of the runtime that frees unused objects) handles many small, short-lived objects very efficiently;
- **value types** (`struct`, `record struct`, enums) avoid separate objects altogether for small data;
- the runtime and the BCL (Base Class Library, the standard library of .NET) already share immutable instances for you: string literals, `Array.Empty<T>()`, `Task.CompletedTask`, `StringComparer.Ordinal`, `Encoding.UTF8`, `EqualityComparer<T>.Default`;
- sharing by reference is natural once data is immutable (records), without a special pattern.

It still matters in specific places: game engines (thousands of sprites sharing one texture), text editors (glyphs), parsers that intern identifiers, and caches of immutable lookup data. Measure before you reach for it.

#### When to use it

- A **profiler** shows many objects that hold the same immutable data, and memory is a real problem.
- The shared part is truly immutable and the per-use part is small.

#### When NOT to use it

- **Without a measurement** showing the memory problem: it is premature optimisation.
- **When the shared data is mutable:** one owner's change would appear in all the others.
- **A few hundred objects:** the factory and the split of state cost more in complexity than they save. Simpler alternative: immutable records, shared by reference where it is natural, or a `static readonly` instance.

#### Costs

- The state is split in two, which makes the code harder to follow.
- The factory's cache needs its own care: thread safety, and never freeing entries (like interned strings).

#### Relevance today

🕰 **Historical.** The runtime, value types and cheap memory solve the problem in almost all application code. You study it to understand string interning and the intrinsic/extrinsic split, and to recognise it in performance-critical code.

#### Relatives

- **Singleton** ([4.1](#41-singleton)) shares *one* instance of a class; Flyweight shares *one instance per distinct value*.
- **Object Pool** ([7.8](#78-object-pool)) *lends* mutable objects to one user at a time and takes them back; flyweights are immutable and shared by everyone at once.
- **Composite** ([5.3](#53-composite)): leaves of a large tree are often flyweights.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- flyweight
```

1. Build 10,000 entries without the factory and compare memory with `GC.GetTotalAllocatedBytes()` before and after each version.
2. Ask the factory for `"BOOKS"` and `"books"`: same object?
3. Check `ReferenceEquals("BOOK-001", "BOOK-001")` with two literals, and explain the result.

#### Interview questions

<details>
<summary>What are intrinsic and extrinsic state?</summary>

Intrinsic state is shared and immutable, and lives in the flyweight (the category data). Extrinsic state differs per use and is kept outside, by the client (the SKU and price of each entry).
</details>

<details>
<summary>Where does .NET use the Flyweight idea?</summary>

String interning: literals and `string.Intern` return one shared instance per distinct value. Also shared immutable instances like `Array.Empty<T>()` and `Task.CompletedTask`.
</details>

### 5.7 Proxy

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Structural |
| Intent | Provide a stand-in for another object to control access to it. |
| Also known as | Surrogate |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- proxy` |
| Code | [`src/Patterns.Structural/Proxy/`](../src/Patterns.Structural/Proxy/) |

#### The problem

Two situations in the shop, both about controlling access to an object:

1. **Product images are expensive to load** (from disk or a remote store). A product page lists many products, but the visitor looks at only a few images. Loading all of them up front is waste.
2. **Only administrators can change prices.** The price editor itself should not be full of permission checks.

Every level uses the same shared pieces, in [`Proxy/Common/`](../src/Patterns.Structural/Proxy/Common/): an `ImageStore` whose `Read(fileName)` returns 1024 bytes and counts its `Reads`, and a `User(Name, IsAdmin)`.

*Analogy:* a building receptionist. Visitors talk to the receptionist, who looks like the way in; the receptionist checks your badge (protection) or calls the person down only when you actually arrive (lazy). The person you visit does not do the checking.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Structural/Proxy/0-Problem/):

```csharp
public sealed class ProductPage(ImageStore store, IEnumerable<string> imageFiles)
{
    // PAIN: every image is read when the page is built, even those nobody looks at.
    private readonly Dictionary<string, byte[]> _images = imageFiles.ToDictionary(f => f, store.Read);
}

public sealed class PriceAdminPage(User user)
{
    private readonly Dictionary<Guid, decimal> _prices = [];

    public void ChangePrice(Product product, decimal newPrice)
    {
        // PAIN: the same check is copied into every method that changes something; one forgotten
        // copy is a security hole.
        if (!user.IsAdmin) { throw new UnauthorizedAccessException("Only administrators can change prices."); }
        _prices[product.Id] = newPrice;
    }

    // ResetPrice(product) starts with the same if: the second copy.
}
```

#### Structure

```mermaid
classDiagram
    class IProductImage {
        <<Subject>>
        +string FileName
        +byte[] Content
    }
    class StoredImage {
        <<RealSubject>>
    }
    class LazyImageProxy {
        <<Proxy>>
        -byte[]? _content
    }
    class Client
    Client --> IProductImage
    IProductImage <|.. StoredImage
    IProductImage <|.. LazyImageProxy
    LazyImageProxy ..> ImageStore : reads on first use
```

| Role | Our class | Responsibility |
|---|---|---|
| Subject | `IProductImage`, `IPriceEditor` | The interface the client uses; real object and proxy share it. |
| RealSubject | `StoredImage`, `PriceEditor` | Does the real work (reads the image, stores the price). |
| Proxy | `LazyImageProxy`, `AdminOnlyPriceEditor` | Looks like the real object and decides *when* or *whether* to let the call through. |

Three classic kinds of proxy:

| Kind | Controls | Example |
|---|---|---|
| **Virtual** | *When* the real object is created or loaded: only on first use. | `LazyImageProxy`, EF Core lazy loading. |
| **Protection** | *Who* may call it. | `AdminOnlyPriceEditor`. |
| **Remote** | *Where* it is: a local object that stands for one in another process or machine. | A generated gRPC (Google's remote procedure call framework) client, a typed client for a web API (application programming interface): you call a method, it sends a network request. |

Logging, caching and counting proxies ("smart references") are also common.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Proxy as LazyImageProxy
    participant Store as ImageStore
    Client->>Proxy: new LazyImageProxy(store, "book.jpg")
    Note right of Proxy: nothing read yet (Reads = 0)
    Client->>Proxy: Content
    Proxy->>Store: Read("book.jpg")
    Store-->>Proxy: 1024 bytes
    Proxy-->>Client: 1024 bytes
    Client->>Proxy: Content (again)
    Note right of Proxy: cached: Reads stays 1
    Proxy-->>Client: the same bytes
```

#### By hand

```csharp
// Role: Proxy (virtual) — looks like an image, but reads it from the store only on first use.
// Guide: §5.7
public sealed class LazyImageProxy(ImageStore store, string fileName) : IProductImage
{
    private byte[]? _content;

    public string FileName => fileName;
    public byte[] Content => _content ??= store.Read(fileName); // read once, then reuse
}

// Role: Proxy (protection) — lets only administrators through to the real editor.
public sealed class AdminOnlyPriceEditor(IPriceEditor inner, User user) : IPriceEditor
{
    public void ChangePrice(Product product, decimal newPrice)
    {
        if (!user.IsAdmin) { throw new UnauthorizedAccessException("Only administrators can change prices."); }
        inner.ChangePrice(product, newPrice);
    }
}
```

The client receives an `IProductImage` or an `IPriceEditor` and cannot tell a proxy from the real thing. The real `PriceEditor` contains no security code at all; the check lives in one class, wrapped around it where the editor is created.

This hand-written lazy proxy is **not thread-safe**: two threads could both read the image. That is one reason to use `Lazy<T>` below.

#### In .NET

**`Lazy<T>`** is a ready-made virtual proxy for a value ([`2-DotNet/`](../src/Patterns.Structural/Proxy/2-DotNet/)): it runs the factory on first `.Value`, once, thread-safely.

In the code, `ProductImage` holds one:

```csharp
var image = new Lazy<byte[]>(() => store.Read("book.jpg")); // nothing read yet
var bytes = image.Value;                                    // read now…
var again = image.Value;                                    // …and not again: store.Reads == 1
```

**`DispatchProxy`** creates a proxy for **any interface** at run time: you write one `Invoke` method that receives every call, and the runtime generates a class implementing the interface that forwards to it. That makes a logging proxy for any service in a few lines:

```csharp
// Guide: §5.7
// On a non-generic class, so callers write LoggingProxy.Create<IPriceEditor>(…) (analyzer CA1000: no static
// members on generic types).
public static class LoggingProxy
{
    public static T Create<T>(T target, ICollection<string> log) where T : class
    {
        var proxy = DispatchProxy.Create<T, LoggingDispatchProxy<T>>(); // the runtime builds a class implementing T
        var self = (LoggingDispatchProxy<T>)(object)proxy;
        self.Target = target;
        self.Log = log;
        return proxy;
    }
}

// Role: Proxy (generated) — logs every call to any interface, then forwards it to the real object.
// Not sealed: DispatchProxy generates a class at run time that derives from this one.
public class LoggingDispatchProxy<T> : DispatchProxy where T : class
{
    internal T Target { get; set; } = default!;
    internal ICollection<string> Log { get; set; } = default!;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        Log.Add($"{targetMethod!.Name} called");
        // Reflection: slower than a direct call. DoNotWrapExceptions lets the target's own exception
        // (say, UnauthorizedAccessException) reach the caller instead of a TargetInvocationException.
        return targetMethod.Invoke(Target, BindingFlags.DoNotWrapExceptions, binder: null, args, culture: null);
    }
}

var editor = LoggingProxy.Create<IPriceEditor>(new PriceEditor(), log);
editor.ChangePrice(SampleData.Book, 11.00m); // log: ["ChangePrice called"]
```

`T` must be an interface, and calls go through **reflection** (inspecting and invoking code at run time through metadata), which is slower than a direct call.

**EF Core lazy loading** is a virtual proxy you may already use: with `UseLazyLoadingProxies()` (package `Microsoft.EntityFrameworkCore.Proxies`), EF Core generates subclasses of your entities whose `virtual` navigation properties load the related data from the database on first access. It is convenient and also the classic source of the "N+1 queries" problem (one query per item in a loop).

#### In the ecosystem

**Castle DynamicProxy** (part of Castle.Core, Apache-2.0) generates proxies for interfaces *and* for classes with virtual members, with interceptors. It is what Moq and NSubstitute use to create mocks, what EF Core's lazy-loading proxies are built on, and what Autofac uses for interception.

#### When to use it

- **Virtual:** creating or loading the real object is expensive and often not needed.
- **Protection:** access rules should be enforced in one place, around the object, not inside it.
- **Remote:** callers should use a remote service as if it were a local object (generated clients).
- **Smart:** add logging, caching or counting around an interface without touching it.

#### When NOT to use it

- **The real object is cheap:** load it eagerly; laziness adds state and surprises (when does the slow call happen?).
- **Lazy loading in loops:** a virtual proxy that queries the database per item hides an N+1 problem. Load what you need up front.
- **Authorization that belongs at the boundary:** in ASP.NET Core, `[Authorize]` and policies on the endpoint are usually simpler than a protection proxy per service.
- **A remote proxy that hides that the call is remote:** network calls fail and are slow; the API should make that visible (async, errors), not pretend to be local.

#### Costs

- One more layer, invisible to the caller, which can surprise (a property access that suddenly hits the disk or the network).
- Generated proxies (`DispatchProxy`, Castle) use reflection: slower, and harder to debug.

#### Relevance today

⭐⭐ **Useful.** You use proxies constantly (`Lazy<T>`, EF Core navigation loading, mocking libraries, generated API clients) and write a few by hand. Knowing the kinds tells you what a proxy is doing behind an innocent-looking property.

#### Relatives

- **Decorator** ([5.4](#54-decorator)): the same structure; a decorator adds behaviour and stacks, a proxy controls access and often manages its subject. **Adapter** ([5.1](#51-adapter)) changes the interface; a proxy keeps it. Comparison in [8.1](#81-decorator-proxy-adapter-facade).
- **Flyweight** ([5.6](#56-flyweight)) and **Singleton** ([4.1](#41-singleton)) share instances; a virtual proxy *postpones* one.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- proxy
```

1. Build a page with ten `LazyImageProxy` images, show only two, and check `store.Reads`.
2. Wrap `AdminOnlyPriceEditor` in a `LoggingProxy.Create<IPriceEditor>(…)`, call it as a non-admin, and look at the log: is the failed call logged? Swap the order and try again.
3. Try `LoggingProxy.Create<PriceEditor>(...)` with the class instead of the interface and read the exception.

#### Interview questions

<details>
<summary>Name three kinds of proxy.</summary>

Virtual (creates or loads the real object on first use), protection (checks who may call it) and remote (a local stand-in for an object in another process or machine).
</details>

<details>
<summary>How does EF Core lazy loading work, and what is its risk?</summary>

EF Core generates proxy subclasses of the entities whose virtual navigation properties query the database on first access. The risk is N+1 queries: accessing a navigation inside a loop runs one query per item.
</details>

<details>
<summary>What is <code>DispatchProxy</code>?</summary>

A BCL class that generates, at run time, an implementation of an interface that routes every call to your `Invoke` method, so you can write logging, caching or interception proxies for any interface.
</details>

---

## 6. Behavioral patterns

**Behavioral** patterns are about **who does what, and how objects talk to each other**: which object handles a request, how a request is passed along, how an object reacts when another changes, how an algorithm is swapped. They mostly apply the principle "encapsulate what varies" (section [3.5](#35-the-principles-under-the-patterns)) to one kind of variation each.

This is the chapter where C# changed the most since 1994. Delegates, lambdas, `event`, `yield return`, records and pattern matching absorbed several of these patterns into the language; the sections say so where it happens.

| Pattern | Relevance | In one line |
|---|---|---|
| [6.1 Chain of Responsibility](#61-chain-of-responsibility) | ⭐⭐⭐ Essential | Pass a request along a chain of handlers; each one handles it, passes it on, or stops it. |
| [6.2 Command](#62-command) | ⭐⭐ Useful | Turn a request into an object, so it can be stored, queued or undone. |
| [6.3 Interpreter](#63-interpreter) | 🕰 Historical | Represent a small language as a tree of objects and evaluate it. |
| [6.4 Iterator](#64-iterator) | ⭐⭐⭐ Essential | Walk through a collection without knowing how it is stored. Understand it; never hand-write it. |
| [6.5 Mediator](#65-mediator) | ⭐⭐ Useful | Objects talk through one central object instead of to each other. |
| [6.6 Memento](#66-memento) | ⭐ Niche | Capture an object's state so it can be restored later, without exposing it. |
| [6.7 Observer](#67-observer) | ⭐⭐⭐ Essential | When one object changes, everyone interested is notified. |
| [6.8 State](#68-state) | ⭐⭐ Useful | An object changes its behaviour when its state changes. |
| [6.9 Strategy](#69-strategy) | ⭐⭐⭐ Essential | Swap one algorithm for another behind the same interface. |
| [6.10 Template Method](#610-template-method) | ⭐⭐ Useful | A base class fixes the steps of an algorithm; subclasses fill in some of them. |
| [6.11 Visitor](#611-visitor) | ⭐ Niche | Add operations to a fixed set of classes without changing them. |

### 6.1 Chain of Responsibility

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Behavioral |
| Intent | Avoid coupling the sender of a request to its receiver by giving several objects a chance to handle it; chain them and pass the request along. |
| Also known as | Pipeline (in its modern form) |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- chain-of-responsibility` |
| Code | [`src/Patterns.Behavioral/ChainOfResponsibility/`](../src/Patterns.Behavioral/ChainOfResponsibility/) |

#### The problem

Before an order is accepted, the shop checks it against several rules, in this order, and reports the **first** one that fails:

| # | Rule | Message when it fails |
|---|---|---|
| 1 | The order has lines. | `Order has no lines.` |
| 2 | No line has more than 10 units. | `At most 10 units per product.` |
| 3 | There is stock for every line. | `Not enough stock for <name>.` |
| 4 | The shop ships to the country (`ES`, `PT`, `FR`). | `We do not ship to <country>.` |

Each check returns an `OrderCheck(bool IsValid, string? Error)`, with `OrderCheck.Ok` and `OrderCheck.Fail(message)`. Rules will be added (fraud, minimum amount), removed and reordered.

*Analogy:* a support hotline. The first-level agent solves what they can and passes the rest to the second level, who passes what they cannot solve to an engineer. You do not choose who answers; you call once and the request travels until someone handles it.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/ChainOfResponsibility/0-Problem/):

```csharp
public sealed class OrderValidator
{
    // PAIN: one method knows every rule and their order. Adding, removing or reordering a rule
    // means editing (and re-testing) this method.
    public OrderCheck Validate(Order order)
    {
        if (order.Lines.Count == 0) { return OrderCheck.Fail("Order has no lines."); }
        if (order.Lines.Any(l => l.Quantity > 10)) { return OrderCheck.Fail("At most 10 units per product."); }
        var missing = order.Lines.FirstOrDefault(l => l.Product.Stock < l.Quantity);
        if (missing is not null) { return OrderCheck.Fail($"Not enough stock for {missing.Product.Name}."); }
        if (order.ShippingAddress.Country is not ("ES" or "PT" or "FR"))
        {
            return OrderCheck.Fail($"We do not ship to {order.ShippingAddress.Country}.");
        }
        return OrderCheck.Ok;
    }
}
```

For four rules this is honestly fine. It starts to hurt when rules multiply, need their own dependencies (a fraud service, the stock database), or different checkouts need different sets of rules.

#### Structure

```mermaid
classDiagram
    class OrderRule {
        <<Handler>>
        -OrderRule? _next
        +SetNext(OrderRule next) OrderRule
        +Check(Order order) OrderCheck
        #Passes(Order order)* OrderCheck
    }
    class NotEmptyRule {
        <<ConcreteHandler>>
    }
    class MaxQuantityRule {
        <<ConcreteHandler>>
    }
    class StockRule {
        <<ConcreteHandler>>
    }
    class ShippingCountryRule {
        <<ConcreteHandler>>
    }
    OrderRule <|-- NotEmptyRule
    OrderRule <|-- MaxQuantityRule
    OrderRule <|-- StockRule
    OrderRule <|-- ShippingCountryRule
    OrderRule --> OrderRule : next
```

| Role | Our class | Responsibility |
|---|---|---|
| Handler | `OrderRule` | Holds the next handler; `Check` runs this rule and, if it passes, asks the next one. |
| ConcreteHandler | `NotEmptyRule`, `MaxQuantityRule`, `StockRule`, `ShippingCountryRule` | One rule each. |
| Client | the checkout | Builds the chain once and calls `Check` on the first link. |

The self-reference (`OrderRule --> OrderRule : next`) is the chain.

#### How it runs

An empty order to the United States: the first rule fails, and the others never run.

```mermaid
sequenceDiagram
    participant Client
    participant R1 as NotEmptyRule
    participant R2 as MaxQuantityRule
    participant R3 as StockRule
    Client->>R1: Check(order)
    Note right of R1: no lines: fail here
    R1-->>Client: Fail("Order has no lines.")
    Note over R2,R3: never called
```

For a valid order (two books to Madrid), each rule passes and calls the next; the last one returns `OrderCheck.Ok`, which travels back to the client.

#### By hand

```csharp
// Role: Handler — one rule in the chain; knows only the next link.
// Guide: §6.1
public abstract class OrderRule
{
    private OrderRule? _next;

    // Returns the next rule, so a chain reads as a sentence: a.SetNext(b).SetNext(c).
    public OrderRule SetNext(OrderRule next) => _next = next;

    public OrderCheck Check(Order order)
    {
        var result = Passes(order);
        if (!result.IsValid) { return result; }       // stop: first failure wins
        return _next?.Check(order) ?? OrderCheck.Ok;  // pass it on (or we were the last)
    }

    protected abstract OrderCheck Passes(Order order);
}

// Role: ConcreteHandler — refuses orders without lines.
public sealed class NotEmptyRule : OrderRule
{
    protected override OrderCheck Passes(Order order) =>
        order.Lines.Count == 0 ? OrderCheck.Fail("Order has no lines.") : OrderCheck.Ok;
}

// Building the chain: the order of the rules is visible in one place.
var rules = new NotEmptyRule();
rules.SetNext(new MaxQuantityRule()).SetNext(new StockRule()).SetNext(new ShippingCountryRule());
var check = rules.Check(order);
```

Each rule is a small class with one reason to change. Adding a rule is a new class and one more `SetNext`; reordering is moving it in that line. The GoF form lets a handler *either* handle the request *or* pass it; here every handler does its part and passes on unless it fails, which is the common modern variant (a **pipeline**).

#### In .NET

**ASP.NET Core middleware** is Chain of Responsibility, and it is the most important instance of the pattern in .NET ([`2-DotNet/`](../src/Patterns.Behavioral/ChainOfResponsibility/2-DotNet/)). Every HTTP request goes through a chain of middleware; each one can do work before and after the next one, or **short-circuit** (answer and not call the next one at all). The DotNet level builds a pipeline in memory, without a web server, and calls it with a fake request:

```csharp
// Guide: §6.1
public static class OrderPipeline
{
    // ICollection, not List: public APIs do not expose List<T> (analyzer CA1002).
    public static RequestDelegate Build(ICollection<string> trace)
    {
        var app = new ApplicationBuilder(new ServiceCollection().BuildServiceProvider());

        app.Use(async (context, next) =>
        {
            trace.Add("correlation");
            context.Response.Headers["X-Correlation-Id"] = "abc-123";
            await next(context);
        });
        app.Use(async (context, next) =>
        {
            trace.Add("auth");
            if (!context.Request.Headers.ContainsKey("X-Customer-Id"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return; // short-circuit: next is never called, the rest of the chain never runs
            }
            await next(context);
        });
        app.Use(async (context, next) =>
        {
            trace.Add("log");
            await next(context);
        });
        app.Run(async context => // terminal: no next
        {
            trace.Add("endpoint");
            await context.Response.WriteAsync("order accepted");
        });

        return app.Build(); // one RequestDelegate: the whole chain, nested
    }
}

var trace = new List<string>();
var context = new DefaultHttpContext();
context.Request.Headers["X-Customer-Id"] = "ana";
await OrderPipeline.Build(trace)(context);
// trace: ["correlation", "auth", "log", "endpoint"]; without the header: ["correlation", "auth"] and 401
```

**How the chain is built.** `Use` does not run anything; it records a function that, given the *next* delegate, returns a new delegate. `Build()` composes them **from the last to the first**: it starts from a built-in delegate that answers 404 (Not Found), wraps it with the terminal component that `Run` added (which never calls its next, so the 404 is never reached), wraps that with the "log" middleware, then "auth", then "correlation". A pipeline without `Run` therefore answers 404 to everything that reaches its end. The result is one `RequestDelegate` made of nested delegates:

```mermaid
flowchart LR
    R([request]) --> C
    subgraph C [correlation]
        direction LR
        subgraph A [auth]
            direction LR
            subgraph L [log]
                direction LR
                E[endpoint]
            end
        end
    end
    A -. no X-Customer-Id: 401, stop .-> X([response])
    E --> X
```

This is why the **order of `app.Use…` calls matters** in `Program.cs`: authentication must come before authorization, exception handling before everything it should catch. Each middleware is also a decorator ([5.4](#54-decorator)) around the rest of the pipeline: it can act before *and* after `await next(context)`.

Other chains in .NET: `DelegatingHandler` chains in `HttpClient` (section [5.4](#54-decorator)), and MVC filters (which can short-circuit an action).

#### When to use it

- A request goes through **several independent steps** (validation rules, middleware, approval levels) whose number and order change.
- Any step may need to **stop** the processing.
- Different entry points need different combinations of the same steps.

#### When NOT to use it

- **A handful of fixed checks:** a method with a few `if`s, as in "Without the pattern", is clearer.
- **Every step must always run and none can stop the others:** that is a list of steps (`foreach (var rule in rules)`), not a chain.
- **You need all the errors, not the first one** (a form showing every invalid field): collect results from a list of validators instead.
- **The order is hard to see:** if the chain is assembled in many places, nobody knows what runs. Build it in one place.

#### Costs

- A request may reach the end without being handled; decide what the end of the chain does.
- Debugging means stepping through several objects; stack traces are deep (very visible in ASP.NET Core).
- The order is a hidden contract between handlers.

#### Relevance today

⭐⭐⭐ **Essential.** Every ASP.NET Core application is a chain of middleware, and every `HttpClient` a chain of handlers. You configure one in every web project, and understanding it explains why order in `Program.cs` matters.

#### Relatives

- **Decorator** ([5.4](#54-decorator)): same nesting; a decorator always calls the inner object, a chain link may stop.
- **Command** ([6.2](#62-command)): the request passed along a chain is often a command object.
- **Composite** ([5.3](#53-composite)): a request can travel up a tree from a child to its parents, which is the original GoF example (help in a UI).
- Section [9.1](#91-combinations-you-will-meet-in-real-code) shows middleware as Chain of Responsibility plus Decorator.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- chain-of-responsibility
```

1. Move `ShippingCountryRule` to the front and validate an empty order to `"US"`: which message wins now?
2. Write a `MinimumTotalRule` (orders under 10.00 are refused) and add it to the chain without touching the other rules.
3. Swap the "auth" and "log" middleware and send a request without the header: what does the trace say?

#### Interview questions

<details>
<summary>How is ASP.NET Core middleware an example of Chain of Responsibility?</summary>

Each middleware receives the request and a `next` delegate; it can do work and call `next` to pass the request on, or answer directly and short-circuit the rest. `Build()` nests them into a single `RequestDelegate`.
</details>

<details>
<summary>Why does the order of <code>app.Use</code> calls matter?</summary>

Middleware runs in registration order on the way in (and reverse order on the way out). A middleware can only affect what comes after it, so, for example, authentication must be registered before authorization and exception handling before the code whose exceptions it should catch.
</details>

<details>
<summary>When would you use a list of validators instead of a chain?</summary>

When every rule must run and you want all the errors at once (form validation), or when no rule needs to stop the others.
</details>

### 6.2 Command

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Behavioral |
| Intent | Encapsulate a request as an object, so it can be stored, queued, logged or undone. |
| Also known as | Action, Transaction |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- command` |
| Code | [`src/Patterns.Behavioral/Command/`](../src/Patterns.Behavioral/Command/) |

#### The problem

The shopping cart has **undo**: add two books, add a mug, remove the books, then undo three times and the cart is empty again. For that, the cart's history must remember not only *what* was done, but *how to reverse it*.

Every level uses a `Cart` (the **receiver**: the object that does the real work) with `Items` (product id → quantity), `Add(product, quantity)` and `Remove(productId)` (removing a product that is not there does nothing).

*Analogy:* a restaurant order slip. The waiter does not cook; they write the order on a slip and hand it to the kitchen. The slip can wait in a queue, be cancelled, or be kept to know what was served. The request has become a **thing**.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/Command/0-Problem/):

```csharp
public sealed class UndoableCart(Cart cart)
{
    private string? _lastAction;
    private Product? _lastProduct;
    private int _lastQuantity;

    public void Add(Product product, int quantity)
    {
        var before = cart.Items.GetValueOrDefault(product.Id); // what undo must restore
        cart.Add(product, quantity);
        (_lastAction, _lastProduct, _lastQuantity) = ("add", product, before);
    }

    // Remove(product) is the same: it records ("remove", product, the quantity it had).

    public void Undo()
    {
        // PAIN: only the last action is remembered (one level of undo), and every new action
        // ("apply coupon", "change quantity") adds a case to this switch.
        switch (_lastAction)
        {
            case "add":
                cart.Remove(_lastProduct!.Id);
                if (_lastQuantity > 0) { cart.Add(_lastProduct, _lastQuantity); }
                break;
            case "remove" when _lastQuantity > 0: cart.Add(_lastProduct!, _lastQuantity); break;
        }
        _lastAction = null;
    }
}
```

#### Structure

```mermaid
classDiagram
    class ICartCommand {
        <<Command>>
        +Execute()
        +Undo()
    }
    class AddItemCommand {
        <<ConcreteCommand>>
    }
    class RemoveItemCommand {
        <<ConcreteCommand>>
        -int _removedQuantity
    }
    class Cart {
        <<Receiver>>
        +Add(Product product, int quantity)
        +Remove(Guid productId)
    }
    class CartHistory {
        <<Invoker>>
        -Stack~ICartCommand~ _done
        +Run(ICartCommand command)
        +Undo() bool
    }
    ICartCommand <|.. AddItemCommand
    ICartCommand <|.. RemoveItemCommand
    AddItemCommand --> Cart
    RemoveItemCommand --> Cart
    CartHistory o-- ICartCommand
```

| Role | Our class | Responsibility |
|---|---|---|
| Command | `ICartCommand` | `Execute()` and `Undo()`. |
| ConcreteCommand | `AddItemCommand`, `RemoveItemCommand` | Hold the receiver and the parameters, and remember what they need to undo themselves. |
| Receiver | `Cart` | Does the real work. Knows nothing about commands. |
| Invoker | `CartHistory` | Runs commands and keeps them on a stack to undo them later. |
| Client | the cart UI | Creates the commands and gives them to the invoker. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant History as CartHistory
    participant Remove as RemoveItemCommand
    participant Cart
    Client->>History: Run(remove book)
    History->>Remove: Execute()
    Remove->>Cart: Remove(book.Id)
    Note right of Remove: remembers that 2 books were removed
    History->>History: push(command)
    Client->>History: Undo()
    History->>History: pop() the command
    History->>Remove: Undo()
    Remove->>Cart: Add(book, 2)
    History-->>Client: true
```

#### By hand

```csharp
// Role: Command — a cart operation that can be done and undone.
// Guide: §6.2
public interface ICartCommand
{
    void Execute();
    void Undo();
}

// Role: ConcreteCommand — removes a product, remembering how many there were.
public sealed class RemoveItemCommand(Cart cart, Product product) : ICartCommand
{
    private int _removedQuantity;

    public void Execute()
    {
        _removedQuantity = cart.Items.GetValueOrDefault(product.Id); // 0 if it was not there
        cart.Remove(product.Id);
    }

    public void Undo()
    {
        if (_removedQuantity > 0) { cart.Add(product, _removedQuantity); } // nothing removed, nothing to restore
    }
}

// Role: Invoker — runs commands and keeps them for undo; it never knows what a command does.
public sealed class CartHistory
{
    private readonly Stack<ICartCommand> _done = new();

    public void Run(ICartCommand command)
    {
        command.Execute();
        _done.Push(command);
    }

    public bool Undo()
    {
        if (!_done.TryPop(out var command)) { return false; }
        command.Undo();
        return true;
    }
}
```

`AddItemCommand` remembers the quantity the product had before, so its `Undo` puts the cart back exactly as it was. Undo now has **unlimited levels** (a stack), and a new operation is a new command class; `CartHistory` never changes. `RemoveItemCommand` takes the `Product`, not just its id, so `Undo` can add it back.

Because a request is now an object, you can also: **queue** it (run later, in a background worker), **log** it (an audit trail of what users did), **retry** it, or send it over the wire (the request objects of a web API are commands).

#### In .NET

In C#, a command with one method is just a **delegate**. `Action` (no return value) and `Func<T>` are commands as values. With undo, a command is a pair of delegates ([`2-DotNet/`](../src/Patterns.Behavioral/Command/2-DotNet/)):

```csharp
// Guide: §6.2
public sealed record UndoableAction(string Name, Action Do, Action Undo);

public sealed class ActionHistory
{
    private readonly Stack<UndoableAction> _done = new();

    public void Run(UndoableAction action) { action.Do(); _done.Push(action); }

    public bool Undo()
    {
        if (!_done.TryPop(out var action)) { return false; }
        action.Undo();
        return true;
    }
}

// The cart's commands, as two lambdas each (CartActions.Remove is the same idea).
public static class CartActions
{
    public static UndoableAction Add(Cart cart, Product product, int quantity)
    {
        var before = 0; // what undo must restore
        return new UndoableAction($"add {quantity} x {product.Name}",
            Do: () =>
            {
                before = cart.Items.GetValueOrDefault(product.Id); // read when it runs, not when it is created
                cart.Add(product, quantity);
            },
            Undo: () =>
            {
                cart.Remove(product.Id);
                if (before > 0) { cart.Add(product, before); }
            });
    }
}

history.Run(CartActions.Add(cart, SampleData.Book, 2)); // Name: "add 2 x Clean Code"
```

The lambdas **capture** the cart and the product (a *closure*: a function that keeps references to the variables around it), which is exactly what a ConcreteCommand's fields did; `before` is a captured variable the two lambdas share, set when `Do` runs (reading it when the action is created would restore the wrong quantity if the cart changes in between). No command classes, same pattern. The `Name` is there for an "undo add 2 x Clean Code" menu item or a log.

Other commands you meet in .NET: `ICommand` in WPF and MAUI (buttons bound to commands, with `CanExecute`); the request objects of the Mediator section ([6.5](#65-mediator)); work items queued to a `Channel<T>` or a background queue; `Task.Run(() => …)` takes a command.

#### When to use it

- **Undo/redo**, history, or replay.
- Requests must be **queued, scheduled, retried or logged** before or instead of being run now.
- The code that *decides* what to do is separate from the code that *does* it (a button and an action; a web request and its handler).

#### When NOT to use it

- **The action runs immediately and is never stored, undone or queued:** call the method.
- **A command class per method "for consistency":** if nothing needs to treat the request as data, the class is ceremony. Simpler alternative: a delegate.
- **Undo of things you cannot undo** (an email already sent, a payment captured): a command with an `Undo` that pretends is worse than none. Use compensating actions with their own rules.

#### Costs

- One class per operation in the class-based form.
- Commands that remember state for undo must capture it correctly at `Execute` time; bugs here are subtle (undoing the wrong quantity).

#### Relevance today

⭐⭐ **Useful.** Delegates made one-method commands part of the language, and request objects in web APIs and message handlers are commands by another name. The full pattern with undo is mostly found in editors, design tools and UI frameworks.

#### Relatives

- **Memento** ([6.6](#66-memento)) is the other way to undo: save the whole state instead of reversing each operation. Section [9.1](#91-combinations-you-will-meet-in-real-code) combines them.
- **Strategy** ([6.9](#69-strategy)) also wraps behaviour in an object; a strategy is *how* to do something, a command is *a request to do* something. Comparison in [8.5](#85-command-and-strategy).
- **Chain of Responsibility** ([6.1](#61-chain-of-responsibility)) passes commands along; **Mediator** ([6.5](#65-mediator)) dispatches them to handlers.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- command
```

1. Add a **redo** stack to `CartHistory`: undone commands go there, `Redo()` runs them again, and a new `Run` clears it.
2. Call `Undo()` twice on the Problem version and look at the cart.
3. Write a `ClearCartCommand` that removes everything and restores everything on undo.

#### Interview questions

<details>
<summary>What does turning a request into an object give you?</summary>

You can store it, queue it, log it, retry it, send it elsewhere, and undo it, because the request now exists as data separate from the moment it is executed.
</details>

<details>
<summary>How do delegates relate to the Command pattern?</summary>

A delegate (`Action`, `Func<T>`) is a command with one method; lambdas capture the receiver and parameters, which a ConcreteCommand would hold in fields. For undo you pair two delegates.
</details>

<details>
<summary>Command or Memento for undo?</summary>

Command reverses each operation and stores only what each needs; it is efficient but every command must know how to undo itself. Memento stores snapshots of the whole state; it is simple and always correct but costs memory per snapshot.
</details>

### 6.3 Interpreter

#### Card

| | |
|---|---|
| Relevance | 🕰 Historical — you rarely write one; you use the ones .NET has |
| Family | Behavioral |
| Intent | Given a language, define a representation for its grammar and an interpreter that uses it to evaluate sentences. |
| Also known as | — |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- interpreter` |
| Code | [`src/Patterns.Behavioral/Interpreter/`](../src/Patterns.Behavioral/Interpreter/) |

#### The problem

The marketing team wants to write discount rules themselves, as text, without a deployment:

```
total > 100 AND category = 'books'
```

means "orders over 100.00 that contain at least one book". The shop must **understand** such sentences and evaluate them against an order.

The language is tiny. Its **grammar** (the rules that say which sentences are valid) is:

| Rule | Means | Example |
|---|---|---|
| `rule := condition ("AND" condition)*` | One or more conditions joined by `AND`. | `total > 100 AND category = 'books'` |
| `condition := "total" (">" \| ">=") number` | The order total compared with a number. | `total >= 100` |
| `condition := "category" "=" "'" text "'"` | Some line of the order is in that category. | `category = 'books'` |

Keywords are case-insensitive (`TOTAL`, `and`), categories are compared ignoring case, numbers use a dot (`99.90`).

*Analogy:* musical notation. A score is a sentence in a small language; a musician *interprets* it, symbol by symbol, to produce music.

#### Without the pattern

```csharp
// Ad-hoc string handling: works for this exact sentence, breaks for the next one.
var parts = rule.Split(" AND ");
var limit = decimal.Parse(parts[0].Replace("total > ", ""), CultureInfo.InvariantCulture);
var category = parts[1].Replace("category = '", "").TrimEnd('\'');
// ">=" breaks it, a third condition breaks it, "Total" breaks it, and nobody can tell why.
```

What hurts: no structure. Every new feature of the language is another `Replace`, and invalid input produces nonsense instead of a clear error.

#### Structure

The Interpreter pattern turns each grammar rule into a class. A parsed sentence becomes a **tree** of those objects (an *abstract syntax tree*, AST), and evaluating the sentence means asking the root to interpret itself.

```mermaid
classDiagram
    class IRuleExpression {
        <<AbstractExpression>>
        +Interpret(Order order) bool
    }
    class TotalGreaterThan {
        <<TerminalExpression>>
        +decimal Amount
        +bool OrEqual
    }
    class HasCategory {
        <<TerminalExpression>>
        +string Name
    }
    class AndExpression {
        <<NonterminalExpression>>
        +IRuleExpression Left
        +IRuleExpression Right
    }
    class DiscountRuleParser {
        +Parse(string rule)$ IRuleExpression
    }
    IRuleExpression <|.. TotalGreaterThan
    IRuleExpression <|.. HasCategory
    IRuleExpression <|.. AndExpression
    AndExpression o-- IRuleExpression
    DiscountRuleParser ..> IRuleExpression : builds
```

| Role | Our class | Responsibility |
|---|---|---|
| AbstractExpression | `IRuleExpression` | `Interpret(order)`. |
| TerminalExpression | `TotalGreaterThan`, `HasCategory` | Leaves of the tree: a single condition. |
| NonterminalExpression | `AndExpression` | Combines other expressions. |
| Context | the `Order` | What the sentence is evaluated against. |
| (Parser) | `DiscountRuleParser` | Turns text into the tree. GoF leaves parsing out of the pattern; you always need one. |

The sample rule becomes this tree:

```mermaid
flowchart TD
    A["AndExpression"] --> T["TotalGreaterThan(100, orEqual: false)"]
    A --> C["HasCategory('books')"]
```

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Root as AndExpression
    participant Total as TotalGreaterThan(100)
    participant Cat as HasCategory("books")
    Client->>Root: Interpret(9 books, 112.50)
    Root->>Total: Interpret(order)
    Total-->>Root: true (112.50 > 100)
    Root->>Cat: Interpret(order)
    Cat-->>Root: true
    Root-->>Client: true
```

#### By hand

```csharp
// Role: AbstractExpression — any piece of a discount rule.
// Guide: §6.3
public interface IRuleExpression
{
    bool Interpret(Order order);
}

// Role: TerminalExpression — "total > n" or "total >= n".
public sealed record TotalGreaterThan(decimal Amount, bool OrEqual) : IRuleExpression
{
    public bool Interpret(Order order) => OrEqual ? order.Total >= Amount : order.Total > Amount;
}

// Role: TerminalExpression — "category = 'x'": some line is in that category.
public sealed record HasCategory(string Name) : IRuleExpression
{
    public bool Interpret(Order order) =>
        order.Lines.Any(l => string.Equals(l.Product.Category.Name, Name, StringComparison.OrdinalIgnoreCase));
}

// Role: NonterminalExpression — both sides must hold.
// Named AndExpression, not And: "And" is a Visual Basic keyword and analyzer CA1716 rejects it as a type name.
public sealed record AndExpression(IRuleExpression Left, IRuleExpression Right) : IRuleExpression
{
    public bool Interpret(Order order) => Left.Interpret(order) && Right.Interpret(order);
}
```

`DiscountRuleParser.Parse(text)` splits the text into **tokens** (words, numbers, `>`, `>=`, `=`, quoted text) and follows the grammar table: read a condition, then while the next token is `AND`, read another and combine them with `AndExpression`. That style, one method per grammar rule, is called a **recursive descent parser**. Invalid input fails with a position: `"total >> 5"` → `FormatException("Unexpected '>' at position 7.")` (positions count characters from 0), `"total >"` → `"Unexpected end of rule."`, `"price > 5"` → `"Unexpected 'price' at position 0."`.

With the rule `total > 100 AND category = 'books'`: 9 books (112.50) → true; 7 books (87.50) → false; 2 headphones (119.80) → false, no book.

#### In .NET

**Expression trees** (`System.Linq.Expressions`) are .NET's built-in representation of code as a tree of objects, and they can be **compiled** to real, fast code. The DotNet level parses the rule with the Classic parser (the one place where a level uses another level's types: the tree is its input) and then translates the tree into an expression tree ([`2-DotNet/`](../src/Patterns.Behavioral/Interpreter/2-DotNet/)):

```csharp
// Guide: §6.3
public static class RuleCompiler
{
    public static Func<Order, bool> Compile(string rule)
    {
        var order = Expression.Parameter(typeof(Order), "order");
        var body = Translate(DiscountRuleParser.Parse(rule), order);
        return Expression.Lambda<Func<Order, bool>>(body, order).Compile(); // IL (Intermediate Language, what C# compiles to), as if hand-written
    }

    private static Expression Translate(IRuleExpression node, ParameterExpression order) => node switch
    {
        TotalGreaterThan { OrEqual: true } t => Expression.GreaterThanOrEqual(Total(order), Expression.Constant(t.Amount)),
        TotalGreaterThan t => Expression.GreaterThan(Total(order), Expression.Constant(t.Amount)),
        HasCategory c => AnyLineIn(order, c.Name),          // a call to Enumerable.Any over order.Lines
        AndExpression a => Expression.AndAlso(Translate(a.Left, order), Translate(a.Right, order)),
        _ => throw new NotSupportedException(node.GetType().Name),
    };
    // Total(order) = Expression.Property(order, nameof(Order.Total)); AnyLineIn builds
    // Expression.Call(typeof(Enumerable), nameof(Enumerable.Any), [typeof(OrderLine)], lines, predicate).
}
```

The difference from the Classic level: Classic **walks the tree every time** it evaluates a rule; the compiled version walks it **once**, produces a delegate, and evaluating is then a normal method call. Expression trees are also how Entity Framework translates `Where(p => p.Price > 100)` into SQL (for properties mapped to columns) (section [7.5](#75-specification)): instead of compiling the tree, it *reads* it.

**`Regex`** is the other interpreter in the BCL: the pattern string is a sentence in the regular-expression language; `Regex` parses it into a tree and interprets it against the input (or compiles it with `RegexOptions.Compiled`, or generates C# for it at build time with `[GeneratedRegex]`).

#### When to use it

- A **small, stable language** that non-developers or configuration must express (rules, filters, search queries), and the grammar fits on one screen.
- You need to evaluate the same sentence many times, or translate it into something else (SQL, an expression tree).

#### When NOT to use it

- **The language is large or will grow:** use a parser generator (ANTLR), or an existing language (C# scripting, JSON-based rules, a SQL `WHERE`). Hand-written interpreters do not scale.
- **The "language" is a fixed set of options:** a form with fields (minimum total, category) is simpler and safer than free text.
- **Performance matters and you walk the tree every time:** compile it (expression trees) or cache the result.

#### Costs

- One class per grammar rule; complex grammars become many classes.
- You must write and maintain a parser, error messages and tests for every edge of the grammar.
- A text language users can write is also a source of support questions and security concerns (validate and limit it).

#### Relevance today

🕰 **Historical.** As a hand-written pattern it is rare: you use the interpreters .NET already has (expression trees, `Regex`, LINQ providers) or a parser generator. Studying it explains how those work, and expression trees are everyday in EF Core.

#### Relatives

- **Composite** ([5.3](#53-composite)): the syntax tree *is* a composite (`AndExpression` holds expressions).
- **Visitor** ([6.11](#611-visitor)) adds operations over the tree (print it, translate it) without changing the node classes; `ExpressionVisitor` is exactly that for expression trees.
- **Specification** ([7.5](#75-specification)) is a small interpreter of business rules built from code instead of text.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- interpreter
```

1. Add `OR` to the grammar. Where does it go, and does `AND` bind tighter than `OR`?
2. Feed the parser `"total >> 5"` and other broken rules; are the messages useful to a marketing user?
3. Time 1,000,000 evaluations with `Interpret` and with the compiled delegate.

#### Interview questions

<details>
<summary>What is an abstract syntax tree?</summary>

The tree of objects a parser builds from a sentence: each node is a grammar construct (a condition, an AND), and leaves are the simplest pieces. Interpreting, translating or printing the sentence means walking the tree.
</details>

<details>
<summary>Where does .NET use the Interpreter idea?</summary>

Expression trees (`System.Linq.Expressions`), which LINQ providers like EF Core read and translate to SQL, and `Regex`, which parses a pattern into a tree and evaluates it.
</details>

### 6.4 Iterator

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential — understand it; never hand-write it |
| Family | Behavioral |
| Intent | Access the elements of a collection one by one without exposing how it is stored. |
| Also known as | Cursor, Enumerator |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- iterator` |
| Code | [`src/Patterns.Behavioral/Iterator/`](../src/Patterns.Behavioral/Iterator/) |

#### The problem

The order history page shows orders in **pages** of a fixed size: 7 orders with a page size of 3 give pages of 3, 3 and 1. The caller should not care whether the history is a list, a database or a remote API; it wants "the next page" until there are no more. A page size below 1 is an error (`ArgumentOutOfRangeException`).

*Analogy:* a TV remote's "next channel" button. You go through the channels one by one without knowing how the TV stores them.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/Iterator/0-Problem/):

```csharp
public sealed class OrderHistory
{
    // PAIN: the storage is public; callers depend on it being a List (they call GetRange on it).
    [SuppressMessage("Design", "CA1002:Do not expose generic lists",
        Justification = "The pain this level shows: the analyzer flags exactly this leak.")]
    public List<Order> Orders { get; } = [];
}

// Every caller (here HistoryScreen, the order history page) repeats the paging arithmetic:
for (var start = 0; start < history.Orders.Count; start += pageSize)
{
    // PAIN: Math.Min and the bounds are easy to get wrong (an off-by-one here loses the last page).
    var page = history.Orders.GetRange(start, Math.Min(pageSize, history.Orders.Count - start));
    Show(page);
}
```

What hurts: the history cannot change its storage (to a database, to pages from an API) without breaking every caller, and the paging logic is copied everywhere with its bugs.

#### Structure

```mermaid
classDiagram
    class IIterator~T~ {
        <<Iterator>>
        +bool HasNext
        +GetNext() T
    }
    class PageIterator {
        <<ConcreteIterator>>
        -int _position
    }
    class OrderHistory {
        <<ConcreteAggregate>>
        -List~Order~ _orders
        +Pages(int pageSize) IIterator
    }
    IIterator~T~ <|.. PageIterator
    OrderHistory ..> PageIterator : creates
    PageIterator --> OrderHistory : reads
```

| Role | Our class | Responsibility |
|---|---|---|
| Iterator | `IIterator<T>` | `HasNext` and `GetNext()`. |
| ConcreteIterator | `PageIterator` | Remembers where it is and builds the next page. |
| Aggregate | `OrderHistory` | Holds the orders privately and creates iterators over them. |

In .NET the same roles have standard names: `IEnumerable<T>` is the Aggregate (it has `GetEnumerator()`), `IEnumerator<T>` is the Iterator (`MoveNext()`, `Current`).

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant History as OrderHistory
    participant It as PageIterator
    Client->>History: Pages(3)
    History-->>Client: iterator at position 0
    loop while HasNext
        Client->>It: HasNext
        It-->>Client: true
        Client->>It: GetNext()
        It-->>Client: page of 3, 3, then 1 orders
    end
    Client->>It: HasNext
    It-->>Client: false
```

#### By hand

```csharp
// Role: Iterator — walks a sequence one element at a time.
// Guide: §6.4
public interface IIterator<out T>
{
    bool HasNext { get; }
    T GetNext(); // GoF calls it Next(); that is a Visual Basic keyword, which analyzer CA1716 rejects
}

// Role: ConcreteAggregate — keeps its storage private and hands out iterators.
public sealed class OrderHistory(IEnumerable<Order> orders)
{
    private readonly List<Order> _orders = [.. orders];

    public IIterator<IReadOnlyList<Order>> Pages(int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        return new PageIterator(_orders, pageSize);
    }

    // Role: ConcreteIterator — remembers where it is; the caller never sees an index.
    private sealed class PageIterator(List<Order> orders, int pageSize) : IIterator<IReadOnlyList<Order>>
    {
        private int _position;

        public bool HasNext => _position < orders.Count;

        public IReadOnlyList<Order> GetNext()
        {
            if (!HasNext) { throw new InvalidOperationException("No more pages."); }
            var page = orders.GetRange(_position, Math.Min(pageSize, orders.Count - _position));
            _position += page.Count;
            return page;
        }
    }
}
```

The paging arithmetic exists once, the list is private, and the iterator is a nested class with access to it. An empty history has no pages: `HasNext` is `false` from the start.

#### In .NET

You should **never write that class**: C# writes it for you. A method that returns `IEnumerable<T>` and uses `yield return` is turned by the compiler into an iterator ([`2-DotNet/`](../src/Patterns.Behavioral/Iterator/2-DotNet/)):

```csharp
// Guide: §6.4
// The source stays an IEnumerable<Order> (it could be a database read): pages are built only when asked for.
public sealed class OrderHistory(IEnumerable<Order> orders)
{
    public IEnumerable<IReadOnlyList<Order>> Pages(int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1); // see the note on laziness below
        return PagesCore(pageSize);
    }

    private IEnumerable<IReadOnlyList<Order>> PagesCore(int pageSize)
    {
        var page = new List<Order>(pageSize);
        foreach (var order in orders)
        {
            page.Add(order);
            if (page.Count == pageSize)
            {
                yield return page; // pauses here until the caller asks for the next page
                page = new List<Order>(pageSize);
            }
        }
        if (page.Count > 0) { yield return page; }
    }
}
```

**What `foreach` really does.** `foreach (var page in history.Pages(3)) Show(page);` is compiled into roughly:

```csharp
using (IEnumerator<IReadOnlyList<Order>> e = history.Pages(3).GetEnumerator())
{
    while (e.MoveNext())      // "HasNext + advance" in one call
    {
        var page = e.Current; // "the element where we are"
        Show(page);
    }
}                             // Dispose: lets the iterator clean up (close a file, a connection)
```

**What `yield return` really does.** The compiler rewrites the method into a hidden class that implements `IEnumerator<T>`, with a field holding a **state** number (where the method was paused) and fields for its local variables (`page`, the enumerator of `orders`). Each `MoveNext()` runs the method's code from where it stopped until the next `yield return`, stores the value in `Current`, remembers the position and returns `true`; when the method ends, `MoveNext()` returns `false`. It is a **state machine** generated from ordinary-looking code, the same idea the compiler uses for `async`/`await`.

**Laziness.** Nothing in an iterator method runs until someone starts enumerating, and then only as far as they go. `history.Pages(3).First()` reads three orders from the source and builds **one** page, never the rest; the test `Yield_IsLazy` uses a counting source to prove it. Two consequences:

- Argument checks inside an iterator method would also be delayed until the first `MoveNext()`. That is why `Pages` checks `pageSize` and then calls a separate `PagesCore` that holds the `yield`.
- Enumerating twice runs the code twice. If the source is expensive (a database query), materialise it once with `ToList()`.

**`IAsyncEnumerable<T>`** is the same pattern when getting each element needs `await` (a page from an API, rows from a database):

```csharp
// RemoteOrders.StreamAsync; the real one also takes a CancellationToken marked [EnumeratorCancellation].
public static async IAsyncEnumerable<Order> StreamAsync(FakeOrderApi api)
{
    for (var page = 1; ; page++)
    {
        var orders = await api.GetPageAsync(page); // a call only when the consumer asks for more
        if (orders.Count == 0) { yield break; }
        foreach (var order in orders) { yield return order; }
    }
}

await foreach (var order in RemoteOrders.StreamAsync(api)) { /* api.Calls grows as you consume */ }
```

For in-memory paging, LINQ already has it: `orders.Chunk(3)` (.NET 6+) returns the pages as arrays.

#### When to use it

- Always, through `IEnumerable<T>` / `yield return`: expose sequences as `IEnumerable<T>` (or `IReadOnlyList<T>`) instead of the concrete collection.
- `yield return` when the elements are computed, read from somewhere, or potentially infinite, and the caller may stop early.
- `IAsyncEnumerable<T>` when producing each element needs I/O.

#### When NOT to use it

- **Never hand-write an iterator class** like `PageIterator` in C#: `yield return` does it correctly (including `Dispose`).
- **Lazy sequences over things that change or close:** an `IEnumerable<T>` over a `DbContext` returned from a method that disposes the context fails when enumerated later. Return a materialised list.
- **When the caller needs the count or random access:** return `IReadOnlyList<T>`, not a lazy sequence they will enumerate several times.

#### Costs

- Laziness surprises: delayed exceptions, multiple enumeration, sequences that are read after their source is gone.
- A small allocation per enumeration (the generated enumerator object).

#### Relevance today

⭐⭐⭐ **Essential.** `foreach`, LINQ, `yield return` and `await foreach` are the Iterator pattern, used in every line of collection code. You must understand how they work (laziness, the generated state machine); you should never implement the pattern by hand.

#### Relatives

- **Composite** ([5.3](#53-composite)): iterators walk trees; a recursive `yield return` flattens one.
- **Visitor** ([6.11](#611-visitor)) also traverses a structure, but runs an *operation* per element type; an iterator only hands out the elements.
- **Factory Method** ([4.2](#42-factory-method)): `GetEnumerator()` is a factory method for iterators.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- iterator
```

1. Put the `pageSize` check inside the `yield` method, call `Pages(0)` without enumerating, and see that nothing throws. Then call `.ToList()`.
2. Enumerate `StreamAsync` with `.Take(5)` (LINQ for `IAsyncEnumerable<T>` is built into .NET 10) and check `api.Calls`.
3. Compare `history.Pages(3)` with `orders.Chunk(3)` on the same data.

#### Interview questions

<details>
<summary>What does the compiler generate for a method with <code>yield return</code>?</summary>

A hidden class implementing `IEnumerable<T>` and `IEnumerator<T>`, with a state field and fields for the local variables. Each `MoveNext()` resumes the method from the last `yield return` and runs to the next one: a state machine.
</details>

<details>
<summary>Why are argument checks in iterator methods delayed, and how do you fix it?</summary>

Because none of the method's body runs until the first `MoveNext()`. Split it: a public non-iterator method that checks the arguments and returns a call to a private iterator method.
</details>

<details>
<summary>What is the danger of returning a lazy <code>IEnumerable&lt;T&gt;</code> from a repository?</summary>

It may be enumerated after the context or connection behind it is disposed, or enumerated several times, running the query again each time. Return a materialised list unless laziness is the point.
</details>

### 6.5 Mediator

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Behavioral |
| Intent | Define an object that encapsulates how a set of objects interact, so they do not refer to each other directly. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- mediator` |
| Code | [`src/Patterns.Behavioral/Mediator/`](../src/Patterns.Behavioral/Mediator/) |

#### The problem

The shop's web API has endpoints such as "place an order" and "get an order's total". Each endpoint needs several collaborators: the order store, a price check, an audit log, and more every month. If each endpoint receives and calls all of them, the endpoint classes grow huge constructors and know everything.

The examples use two **requests**: `PlaceOrder(Order)` returns the new order's id, and `GetOrderTotal(Guid OrderId)` returns its total (`25.00` for two books), over an in-memory `OrderStore`.

*Analogy:* an airport control tower. Planes do not talk to each other to decide who lands; each talks only to the tower, which coordinates them. Add a plane and nobody else changes.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/Mediator/0-Problem/):

```csharp
// PAIN: the endpoint knows every collaborator, and its constructor grows with every feature.
public sealed class OrdersEndpoint(OrderStore store, PriceCheck priceCheck, AuditLog audit)
{
    public Guid Place(Order order)
    {
        priceCheck.Verify(order);
        store.Save(order);
        audit.Record($"placed {order.Id}");
        return order.Id;
    }

    public decimal TotalOf(Guid orderId) => store.Get(orderId).Total;
}
```

#### Structure

There are **two forms** of Mediator.

**The GoF form: colleagues talk through a mediator.** The book's example is a dialog box: when the user picks a value in a list, a text box and a button must update. Instead of every widget knowing the others, each tells the mediator "I changed", and the mediator decides what the others do. In the shop, a checkout form where choosing a shipping method updates the total and enabling the coupon field changes the button:

```csharp
public sealed class CheckoutFormMediator
{
    public required ShippingSelector Shipping { get; init; }
    public required CouponBox Coupon { get; init; }
    public required PayButton Pay { get; init; }

    // Colleagues call this; only the mediator knows how they affect each other.
    public void Changed(object colleague)
    {
        if (colleague == Shipping || colleague == Coupon)
            Pay.ShowTotal(Shipping.Cost + Coupon.Discount);
    }
}
```

**The request/handler form** (what .NET developers usually mean today): callers send a **request** object to a **dispatcher**, which finds the one **handler** for that request type. The caller knows only the dispatcher; handlers know only what they need.

```mermaid
classDiagram
    class Dispatcher {
        <<Mediator>>
        +Send~TResponse~(IRequest~TResponse~ request) TResponse
    }
    class IRequest~TResponse~ {
        <<Request>>
    }
    class IRequestHandler~TRequest, TResponse~ {
        <<Handler>>
        +Handle(TRequest request) TResponse
    }
    class PlaceOrderHandler {
        <<ConcreteHandler>>
    }
    class OrdersEndpoint {
        <<Colleague>>
    }
    OrdersEndpoint --> Dispatcher
    Dispatcher ..> IRequestHandler~TRequest, TResponse~ : finds and calls
    IRequestHandler~TRequest, TResponse~ <|.. PlaceOrderHandler
```

| Role | Our class | Responsibility |
|---|---|---|
| Mediator | `Dispatcher` | Finds the handler for a request type and calls it. |
| Request | `PlaceOrder`, `GetOrderTotal` (records implementing `IRequest<TResponse>`) | The message, with the type of its answer. |
| Handler | `PlaceOrderHandler`, `GetOrderTotalHandler` | The code for one request, with only the dependencies it needs. |
| Colleague | the endpoints | Send requests; know nothing about handlers. |

#### How it runs

```mermaid
sequenceDiagram
    participant Endpoint
    participant Dispatcher
    participant Handler as GetOrderTotalHandler
    participant Store as OrderStore
    Endpoint->>Dispatcher: Send(new GetOrderTotal(id))
    Note right of Dispatcher: look up the handler by request type
    Dispatcher->>Handler: Handle(request)
    Handler->>Store: Get(id)
    Store-->>Handler: order
    Handler-->>Dispatcher: 25.00
    Dispatcher-->>Endpoint: 25.00
```

#### By hand

```csharp
// Role: Request — a message that declares the type of its answer.
// Guide: §6.5
public interface IRequest<TResponse>;

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    TResponse Handle(TRequest request);
}

public sealed record GetOrderTotal(Guid OrderId) : IRequest<decimal>;

// Role: Mediator — routes each request to the one handler registered for its type.
public sealed class Dispatcher
{
    // Request type → a function that calls its handler. Storing a function hides the generic types,
    // so Send only needs to know TResponse.
    private readonly Dictionary<Type, Func<object, object?>> _handlers = [];

    public void Register<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse> =>
        _handlers[typeof(TRequest)] = request => handler.Handle((TRequest)request);

    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        if (!_handlers.TryGetValue(request.GetType(), out var handle))
        {
            throw new InvalidOperationException($"No handler registered for {request.GetType().Name}.");
        }
        return (TResponse)handle(request)!;
    }
}

var total = dispatcher.Send(new GetOrderTotal(orderId)); // TResponse inferred: decimal
```

The endpoint now depends on one thing, the dispatcher. Each handler is small, focused and testable on its own, and a new feature is a new request plus its handler.

#### In .NET

The BCL has no mediator; the DotNet level builds one over the DI container in about 40 lines ([`2-DotNet/`](../src/Patterns.Behavioral/Mediator/2-DotNet/)). Handlers are ordinary registered services, so they receive their own dependencies by constructor injection:

```csharp
// AddOrderRequests(this IServiceCollection services) in the DotNet level:
services.AddSingleton<OrderStore>();
services.AddSingleton<PriceCheck>();
services.AddSingleton<AuditLog>();
services.AddTransient<IRequestHandler<PlaceOrder, Guid>, PlaceOrderHandler>();
services.AddTransient<IRequestHandler<GetOrderTotal, decimal>, GetOrderTotalHandler>();
services.AddTransient<Dispatcher>();

// Guide: §6.5
public sealed class Dispatcher(IServiceProvider provider)
{
    public TResponse Send<TResponse>(IRequest<TResponse> request)
    {
        // Build the closed generic type IRequestHandler<GetOrderTotal, decimal> at run time.
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No handler registered for {request.GetType().Name}.");

        // Reflection; DoNotWrapExceptions keeps the handler's own exception type for the caller.
        return (TResponse)handlerType.GetMethod(nameof(IRequestHandler<PlaceOrder, Guid>.Handle))!
            .Invoke(handler, BindingFlags.DoNotWrapExceptions, binder: null, [request], culture: null)!;
    }
}
```

That is the core of what MediatR does (MediatR caches the handler lookups and adds async, notifications and pipeline behaviours on top). The dispatcher is registered as **transient**, like the handlers: a singleton would resolve every handler from the root provider, keeping disposable handlers alive until the application stops and failing for handlers with scoped dependencies.

#### In the ecosystem

**MediatR** is the best-known implementation of the request/handler mediator in .NET, and its **pipeline behaviours** (logging, validation, transactions around every handler) made it popular. It moved to a **commercial licence in 2025** (with a free tier for some users; check the current terms). Alternatives: a hand-written dispatcher like the one above; **Mediator** by Martin Othamar (MIT), which uses a source generator instead of reflection; or no mediator at all (see below).

#### When to use it

- **GoF form:** several objects (UI widgets, game entities) influence each other in many-to-many ways.
- **Request/handler form:** many endpoints or message consumers, each with its own dependencies, and you want one handler per use case plus a single place to add cross-cutting behaviour (logging, validation, transactions) around all of them.

#### When NOT to use it

- **A small API:** an endpoint that calls a service directly is clearer, and "go to definition" works. A mediator turns a direct call into a lookup by type.
- **Just to make constructors smaller:** that hides dependencies instead of reducing them. Split the class instead.
- **Handlers that call other handlers through the mediator:** the flow becomes invisible. Call the shared code directly.
- In minimal APIs, an endpoint can receive exactly the service it needs as a parameter; that is often all you need.

#### Costs

- Indirection: you cannot "go to definition" from `Send(new GetOrderTotal(…))` to its handler.
- A missing handler fails at run time, not at compile time.
- The mediator can become a god object if it starts containing logic.

#### Relevance today

⭐⭐ **Useful.** The request/handler mediator is very common in .NET codebases (often through MediatR, often in "vertical slice" code, where each feature is one request, its handler and its tests, or in CQRS, Command Query Responsibility Segregation, which uses separate models for writing and reading); the GoF form appears in UI code. Both are easy to overuse.

#### Relatives

- **Observer** ([6.7](#67-observer)): a mediator *coordinates* (it decides what happens); an observer subject only *announces* (subscribers decide). Comparison in [8.4](#84-observer-and-mediator).
- **Facade** ([5.5](#55-facade)): subsystems do not know their facade; colleagues do know their mediator.
- **Command** ([6.2](#62-command)): the requests sent to a dispatcher are commands. **Chain of Responsibility** ([6.1](#61-chain-of-responsibility)): MediatR's pipeline behaviours are a chain around each handler.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- mediator
```

1. Send a request with no registered handler and read the message.
2. Add a `CancelOrder` request and handler. How many existing files changed?
3. Add a "pipeline behaviour" to the DotNet dispatcher: log `"handling GetOrderTotal"` before every handler, in one place.

#### Interview questions

<details>
<summary>What problem does Mediator solve?</summary>

Many objects that would otherwise reference each other (many-to-many) talk through one mediator instead, so each depends only on the mediator and changes in how they interact live in one place.
</details>

<details>
<summary>What is the downside of the request/handler mediator?</summary>

It replaces direct calls with a lookup by type: navigation and compile-time checks are lost, a missing handler fails at run time, and simple flows become harder to follow.
</details>

<details>
<summary>Why do some teams drop MediatR?</summary>

Its licence became commercial in 2025, and the core (dispatching a request to a handler from the container) is a few dozen lines; many teams also prefer direct calls for small APIs.
</details>

### 6.6 Memento

#### Card

| | |
|---|---|
| Relevance | ⭐ Niche |
| Family | Behavioral |
| Intent | Without breaking encapsulation, capture an object's internal state so it can be restored later. |
| Also known as | Token, Snapshot |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- memento` |
| Code | [`src/Patterns.Behavioral/Memento/`](../src/Patterns.Behavioral/Memento/) |

#### The problem

Another way to undo in the cart (compare with Command in [6.2](#62-command)): before each change, **save a snapshot** of the cart; to undo, **restore** the last snapshot. Simple and always correct, whatever the change was. The difficulty: something outside the cart (the history) must keep those snapshots, yet the cart's internals should stay private.

*Analogy:* a save point in a video game. The game stores your progress; you can go back to it. You cannot open the save file and edit your health points: only the game can read it.

#### Without the pattern

```csharp
// Saving state by reading the cart's internals from outside:
var saved = new Dictionary<Guid, int>(cart.Items); // the history now depends on how Cart stores items
// …and restoring needs a public setter that anyone can call:
cart.ReplaceItems(saved); // breaks encapsulation: any code can overwrite the cart
```

What hurts: either the cart exposes its internals to whoever keeps the history, or it offers a public "overwrite everything" method that anyone can misuse.

#### Structure

```mermaid
classDiagram
    class Cart {
        <<Originator>>
        +Add(Product product, int quantity)
        +Remove(Guid productId)
        +CreateSnapshot() CartSnapshot
        +Restore(CartSnapshot snapshot)
    }
    class CartSnapshot {
        <<Memento>>
        ~ImmutableDictionary Items
    }
    class CartCaretaker {
        <<Caretaker>>
        -Stack~CartSnapshot~ _history
        +Save()
        +Undo() bool
    }
    Cart ..> CartSnapshot : creates and reads
    CartCaretaker o-- CartSnapshot : keeps, never reads
    CartCaretaker --> Cart
```

`~` marks an `internal` member.

| Role | Our class | Responsibility |
|---|---|---|
| Originator | `Cart` | Creates snapshots of its own state and restores from them. |
| Memento | `CartSnapshot` | Holds the saved state; offers **nothing public**. |
| Caretaker | `CartCaretaker` | Keeps snapshots in a stack and hands them back for undo; cannot look inside. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Caretaker as CartCaretaker
    participant Cart
    Client->>Caretaker: Save()
    Caretaker->>Cart: CreateSnapshot()
    Cart-->>Caretaker: snapshot (opaque)
    Client->>Cart: Add(mug, 1)
    Client->>Caretaker: Undo()
    Caretaker->>Cart: Restore(snapshot)
    Note right of Cart: items are back to what they were
    Caretaker-->>Client: true
```

#### By hand

```csharp
// Role: Memento — an opaque snapshot of a cart. Nothing outside this assembly can read it.
// Guide: §6.6
public sealed class CartSnapshot
{
    internal CartSnapshot(ImmutableDictionary<Guid, int> items) => Items = items;
    internal ImmutableDictionary<Guid, int> Items { get; }
}

// Role: Originator — the only one who knows how to save and restore itself.
public sealed class Cart
{
    private ImmutableDictionary<Guid, int> _items = ImmutableDictionary<Guid, int>.Empty;

    public IReadOnlyDictionary<Guid, int> Items => _items;
    public void Add(Product product, int quantity) =>
        _items = _items.SetItem(product.Id, _items.GetValueOrDefault(product.Id) + quantity);
    public void Remove(Guid productId) => _items = _items.Remove(productId);

    public CartSnapshot CreateSnapshot() => new(_items); // immutable: later edits cannot change it
    public void Restore(CartSnapshot snapshot) => _items = snapshot.Items;
}

// Role: Caretaker — keeps the snapshots, in order, without knowing what is inside.
public sealed class CartCaretaker(Cart cart)
{
    private readonly Stack<CartSnapshot> _history = new();

    public void Save() => _history.Push(cart.CreateSnapshot());

    public bool Undo()
    {
        if (!_history.TryPop(out var snapshot)) { return false; }
        cart.Restore(snapshot);
        return true;
    }
}
```

**Encapsulation of the snapshot.** GoF asks for two views of the memento: a *wide* one for the originator (it can read the state) and a *narrow* one for everybody else (they can only hold it). C++ did this with `friend` classes. In C#:

- `internal` members, as here: only code in the same assembly can read the snapshot. Good enough when the caretaker lives in another project; within one assembly it is a convention.
- A nested class inside `Cart` with private members gives the strictest version: only `Cart` can read it.

**Why records make it trivial.** The snapshot must not change after it is taken. If the cart kept a mutable `Dictionary` and the snapshot shared it, the next `Add` would change the "saved" state too (the shallow-copy trap of [4.5](#45-prototype)). Immutable data removes the problem: an `ImmutableDictionary` can be shared safely, so taking a snapshot costs nothing. Note that `with` alone is not enough: it is a shallow copy, so a record holding a mutable `Dictionary` would still share it. What matters is that the *contents* are immutable.

#### In .NET

When the state is an **immutable record**, the state is its own memento ([`2-DotNet/`](../src/Patterns.Behavioral/Memento/2-DotNet/)): every change returns a new state, and the old one is untouched, forever.

```csharp
// Guide: §6.6
public sealed record CartState(ImmutableDictionary<Guid, int> Items)
{
    public static CartState Empty { get; } = new(ImmutableDictionary<Guid, int>.Empty);

    public CartState Add(Product product, int quantity) =>
        this with { Items = Items.SetItem(product.Id, Items.GetValueOrDefault(product.Id) + quantity) };

    public CartState Remove(Guid productId) => this with { Items = Items.Remove(productId) };
}

var history = new Stack<CartState>();
var cart = CartState.Empty;
history.Push(cart); cart = cart.Add(SampleData.Book, 2);
history.Push(cart); cart = cart.Add(SampleData.Mug, 1);
cart = history.Pop(); // undo: the state with just the books, exactly as it was
```

No originator, no caretaker class, no encapsulation problem: there is nothing to protect because nothing can change. This is how many UI state libraries (Redux and its imitators: the whole UI state is one immutable value, replaced on every change) and many functional designs implement undo and "time travel" debugging.

#### When to use it

- You need undo, rollback or "cancel editing" and the state is **small or cheap to share** (immutable).
- Reversing each operation (Command) would be complex or error-prone, and a snapshot is simple.
- A transaction-like "try this, and go back if it fails".

#### When NOT to use it

- **The state is large and mutable:** copying it on every change costs memory and time. Use Command (store only the change) or immutable data structures.
- **The state includes external resources** (open files, database rows, sent emails): a snapshot cannot restore them.
- **The type is already an immutable record:** just keep the old values in a stack; do not add a `Snapshot` class.

#### Costs

- Memory per snapshot; long histories need a limit.
- Keeping the snapshot opaque in C# takes care (`internal` or nested types).

#### Relevance today

⭐ **Niche.** The class-based pattern is rare in application code; immutable records made the idea everyday without a pattern name. Knowing it explains why immutability makes undo, caching and concurrency easier.

#### Relatives

- **Command** ([6.2](#62-command)) is the other way to undo; they combine well (each command keeps a memento of what it changed). See [9.1](#91-combinations-you-will-meet-in-real-code).
- **Prototype** ([4.5](#45-prototype)) also copies state, to create a new object rather than to restore an old one.
- **State** ([6.8](#68-state)): a memento can capture which state an object was in.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- memento
```

1. Change `Cart` to use a mutable `Dictionary` that the snapshot shares, and run `Snapshot_IsNotChangedByLaterEdits`.
2. Limit `CartCaretaker` to the last 10 snapshots.
3. Rewrite the Command demo's undo with `CartState` and a stack. Which is shorter?

#### Interview questions

<details>
<summary>How does Memento keep encapsulation?</summary>

Only the originator can read the snapshot's contents; the caretaker stores and returns it without seeing inside. In C# that is done with `internal` or nested types.
</details>

<details>
<summary>Why do immutable records make Memento almost free?</summary>

An immutable state cannot change after it is created, so the old state can be kept as is: there is nothing to copy and nothing to protect.
</details>

### 6.7 Observer

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Behavioral |
| Intent | Define a one-to-many dependency so that when one object changes state, all its dependents are notified automatically. |
| Also known as | Publish-Subscribe, Dependents, Listener |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- observer` |
| Code | [`src/Patterns.Behavioral/Observer/`](../src/Patterns.Behavioral/Observer/) |

#### The problem

When an order is placed, three things must happen: a confirmation **email**, a **stock** reservation and an **analytics** event. Next month there will be a fourth (loyalty points), then a fifth. The code that places orders should not have to change each time someone new wants to react.

In the examples each reaction writes one line to a shared log, so the tests can check who ran and in which order. For two books:

```
email: order 1a2b3c4d confirmed
stock: reserve 2 units
analytics: order total 25.00
```

*Analogy:* a newspaper subscription. The publisher does not know who reads the paper or why; it prints and delivers to whoever subscribed. Subscribing and cancelling do not change the newspaper.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/Observer/0-Problem/):

```csharp
public sealed class OrderService(EmailSender email, StockUpdater stock, Analytics analytics)
{
    public void Place(Order order)
    {
        // PAIN: OrderService depends on every class that reacts to an order. A new reaction means
        // editing this class, its constructor and its tests.
        email.SendConfirmation(order);
        stock.Reserve(order);
        analytics.Track(order);
    }
}
```

This is the "high coupling" picture of section [3.5](#35-the-principles-under-the-patterns).

#### Structure

```mermaid
classDiagram
    class OrderPublisher {
        <<Subject>>
        -List~IOrderObserver~ _observers
        +Attach(IOrderObserver observer)
        +Detach(IOrderObserver observer)
        +Publish(Order order)
    }
    class IOrderObserver {
        <<Observer>>
        +OnOrderPlaced(Order order)
    }
    class EmailObserver {
        <<ConcreteObserver>>
    }
    class StockObserver {
        <<ConcreteObserver>>
    }
    class AnalyticsObserver {
        <<ConcreteObserver>>
    }
    OrderPublisher o-- IOrderObserver : notifies
    IOrderObserver <|.. EmailObserver
    IOrderObserver <|.. StockObserver
    IOrderObserver <|.. AnalyticsObserver
```

| Role | Our class | Responsibility |
|---|---|---|
| Subject | `OrderPublisher` | Keeps the list of observers and notifies them all. Knows only the interface. |
| Observer | `IOrderObserver` | What an observer must offer: `OnOrderPlaced(order)`. |
| ConcreteObserver | `EmailObserver`, `StockObserver`, `AnalyticsObserver` | One reaction each. |

#### How it runs

```mermaid
sequenceDiagram
    participant Service as Order placing code
    participant Subject as OrderPublisher
    participant Email as EmailObserver
    participant Stock as StockObserver
    participant Analytics as AnalyticsObserver
    Service->>Subject: Publish(order)
    Subject->>Email: OnOrderPlaced(order)
    Subject->>Stock: OnOrderPlaced(order)
    Subject->>Analytics: OnOrderPlaced(order)
    Note over Subject: in subscription order
```

#### By hand

```csharp
// Role: Observer — anything that wants to know when an order is placed.
// Guide: §6.7
public interface IOrderObserver
{
    void OnOrderPlaced(Order order);
}

// Role: Subject — announces placed orders to whoever subscribed.
public sealed class OrderPublisher
{
    private readonly List<IOrderObserver> _observers = [];

    public void Attach(IOrderObserver observer) => _observers.Add(observer);
    public void Detach(IOrderObserver observer) => _observers.Remove(observer);

    public void Publish(Order order)
    {
        // One failing observer must not silence the others: notify everybody, then report.
        var failures = new List<Exception>();
        foreach (var observer in _observers.ToList()) // a copy: an observer may detach while notified
        {
            try { observer.OnOrderPlaced(order); }
            catch (Exception ex) { failures.Add(ex); }
        }
        if (failures.Count > 0) { throw new AggregateException(failures); }
    }
}
```

`OrderPublisher` knows only `IOrderObserver`. Loyalty points are a new observer and one `Attach` where the application is wired; no existing class changes.

**Push vs pull.** There are two ways to tell observers what changed:

| | Push | Pull |
|---|---|---|
| The notification carries | the data (`OnOrderPlaced(order)`) | only "something changed" |
| The observer then | uses the data it received | asks the subject for what it needs |
| Good when | observers need the same data | observers need different parts, or the data is large |
| In .NET | `event EventHandler<OrderPlacedEventArgs>`, `IObservable<T>` | `IChangeToken` (configuration reload: "it changed, read it again") |

This repository uses push.

**A failing observer.** If the stock observer throws, should the email still go out? In the Classic subject, yes: `Publish` notifies everyone, collects the failures and throws one `AggregateException` at the end (test `Classic_FailingObserver_DoesNotStopTheOthers`). The C# `event` below behaves differently, and that difference is worth knowing.

#### In .NET

C# has the pattern built in as **events** ([`2-DotNet/`](../src/Patterns.Behavioral/Observer/2-DotNet/)):

```csharp
// Guide: §6.7
public sealed class OrderPlacedEventArgs(Order order) : EventArgs
{
    public Order Order { get; } = order;
}

public sealed class OrderService
{
    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced; // the subject's subscriber list

    public void Place(Order order) => OrderPlaced?.Invoke(this, new OrderPlacedEventArgs(order));
}

EventHandler<OrderPlacedEventArgs> handler = (_, e) => log.Add($"stock: reserve {e.Order.Units} units");
service.OrderPlaced += handler; // Attach
service.OrderPlaced -= handler; // Detach: needs the same delegate instance, so keep a reference to the lambda
```

An `event` is a **multicast delegate**: one delegate that holds a list of methods and calls them in order. The `event` keyword restricts outsiders to `+=` and `-=`; only the declaring class can raise it. Two differences from the hand-written subject:

- **A failing subscriber stops the rest.** `Invoke` calls the handlers one after another; if the second throws, the exception goes straight to `Place`, and the third handler **never runs**. The test `Event_StopsAtFirstFailingSubscriber` documents it. If you need "notify everyone anyway", loop over `OrderPlaced.GetInvocationList()` with a `try`/`catch`, as the Classic level does.
- **Memory leaks.** The subject's delegate holds a reference to every subscriber (to the object whose method was subscribed, or to what a lambda captures). A long-lived subject (a singleton service) keeps short-lived subscribers (a page, a view model) **alive forever** if they never unsubscribe: the garbage collector cannot free them. Rule: whoever subscribes must unsubscribe when it is done (`-=`, or `Dispose` of the subscription), or the subject must live no longer than its subscribers.

**`IObservable<T>` / `IObserver<T>`** are the BCL's interfaces for a *stream* of notifications: `OnNext(value)` for each item, `OnError(exception)` and `OnCompleted()` at the end. `Subscribe` returns an `IDisposable`, which makes unsubscribing explicit and leak-proof with `using`:

```csharp
// Named OrderFeed, not OrderStream: analyzer CA1711 keeps the "Stream" suffix for System.IO.Stream types.
public sealed class OrderFeed : IObservable<Order>
{
    private readonly List<IObserver<Order>> _observers = [];

    public IDisposable Subscribe(IObserver<Order> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(() => _observers.Remove(observer)); // Dispose = unsubscribe
    }

    public void Publish(Order order)
    {
        foreach (var observer in _observers.ToList()) { observer.OnNext(order); }
    }

    private sealed class Unsubscriber(Action unsubscribe) : IDisposable
    {
        public void Dispose() => unsubscribe();
    }
}
```

The DotNet level subscribes a `LoggingObserver(log, describe)`, an `IObserver<Order>` that writes one line per order; the three lines come from `Reactions.Email`, `Reactions.Stock` and `Reactions.Analytics`, which the event handlers use too.

**`IChangeToken`** is the pull-style observer used by configuration and file providers: `ChangeToken.OnChange(() => configuration.GetReloadToken(), () => …)` runs your callback when `appsettings.json` changes, and you read the new values yourself.

#### In the ecosystem

**System.Reactive** (Rx.NET, MIT, a .NET Foundation project) builds a whole query language on `IObservable<T>`: filter, combine, throttle and buffer streams of events with LINQ-like operators (`Where`, `Throttle`, `Buffer`). It shines for UI events and real-time data; for "notify three services that an order was placed" it is far more than you need.

#### When to use it

- One object's change must trigger **several independent reactions**, and new reactions keep appearing.
- The object that changes should not know who reacts (a domain object raising "order placed"; a UI control raising "clicked").
- A stream of values over time (sensor readings, price ticks): `IObservable<T>`.

#### When NOT to use it

- **There is exactly one reaction that always happens:** call it directly; an event hides a simple call.
- **The reactions must happen in a guaranteed order, or all succeed together:** in-process events are not transactions. If stock reservation must not happen without the email, coordinate them explicitly (a facade, [5.5](#55-facade)).
- **The reaction is slow or can fail and must be retried** (send an email via an external service): an in-memory event lost on a crash is not enough; use a message queue (out of scope, see [9.4](#94-out-of-scope)).
- **Subscribers with short lives on a long-lived subject**, if you cannot guarantee unsubscription.

#### Costs

- The flow is implicit: reading `Place` you cannot see what happens next; you must find the subscribers.
- Ordering, error handling and threading of notifications are decisions to make (and document).
- Memory leaks from forgotten subscriptions.

#### Relevance today

⭐⭐⭐ **Essential.** C# events, UI frameworks, `IObservable<T>`, `IChangeToken`, domain events and message buses are all Observer. You use it, and its pitfalls (leaks, exceptions), constantly.

#### Relatives

- **Mediator** ([6.5](#65-mediator)): a mediator decides what each colleague does; an observer subject only announces. They combine: a mediator can be an observer of its colleagues. Comparison in [8.4](#84-observer-and-mediator).
- **Chain of Responsibility** ([6.1](#61-chain-of-responsibility)) passes a request until one handles it; Observer sends it to everybody.
- **Command** ([6.2](#62-command)): what gets published is often a command or event object.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- observer
```

1. Add a `LoyaltyObserver` (`"loyalty: 25 points"`) without touching `OrderPublisher`. Then add the same reaction to the Problem version.
2. Make the stock observer throw and compare the log in the Classic and `event` versions.
3. Subscribe a lambda that captures a large `byte[]` to a static event, drop every other reference, call `GC.Collect()`, and check with `GC.GetTotalMemory` whether the memory is freed.

#### Interview questions

<details>
<summary>What happens if a C# event handler throws?</summary>

The exception propagates from the `Invoke` call and the remaining handlers are not called. To notify everyone regardless, iterate `GetInvocationList()` and catch per handler.
</details>

<details>
<summary>How can events cause memory leaks?</summary>

The event's delegate references every subscriber. If the publisher lives longer than a subscriber that never unsubscribes, the subscriber stays reachable and is never collected.
</details>

<details>
<summary>What is the difference between push and pull observers?</summary>

In push, the notification carries the data observers need. In pull, it only says something changed, and each observer reads what it needs from the subject. `IChangeToken` is pull; events with `EventArgs` are push.
</details>

### 6.8 State

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Behavioral |
| Intent | Allow an object to change its behaviour when its internal state changes; it appears to change its class. |
| Also known as | Objects for States |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- state` |
| Code | [`src/Patterns.Behavioral/State/`](../src/Patterns.Behavioral/State/) |

#### The problem

An order moves through its **life cycle**, described in section [3.8](#38-the-shop):

```mermaid
stateDiagram-v2
    [*] --> Draft
    Draft --> Placed : Place
    Placed --> Paid : Pay
    Paid --> Shipped : Ship
    Draft --> Cancelled : Cancel
    Placed --> Cancelled : Cancel
    Paid --> Cancelled : Cancel
    Shipped --> [*]
    Cancelled --> [*]
```

Four actions (`Place`, `Pay`, `Ship`, `Cancel`), five statuses. What an action does **depends on the status**: `Ship` on a paid order ships it; on a placed order it is an error. Every illegal move throws `InvalidOperationException` with the message `"Cannot <action> an order that is <Status>."`, for example `"Cannot ship an order that is Placed."`.

*Analogy:* a traffic light. Pressing the pedestrian button does different things depending on whether the light is green, amber or red. The button is the same; the behaviour comes from the current state.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/State/0-Problem/):

```csharp
public sealed class OrderWorkflow
{
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public void Pay()
    {
        // PAIN: the rules of each status are scattered across four methods. To know everything a
        // Paid order can do, you read all of them.
        if (Status != OrderStatus.Placed) { throw Illegal("pay"); }
        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Cancelled) { throw Illegal("cancel"); }
        Status = OrderStatus.Cancelled;
    }
    // Place() and Ship() repeat the same shape…

    private InvalidOperationException Illegal(string action) =>
        new($"Cannot {action} an order that is {Status}.");
}
```

With 5 statuses and 4 actions this is still readable. It hurts when each status also has its own data and behaviour (a paid order has a payment id, a shipped one a tracking number, each with its own rules for refunds…), and every method grows a `switch` on `Status`.

#### Structure

```mermaid
classDiagram
    class OrderContext {
        <<Context>>
        -OrderState _state
        +OrderStatus Status
        +Place()
        +Pay()
        +Ship()
        +Cancel()
    }
    class OrderState {
        <<State>>
        +OrderStatus Status*
        +Place(OrderContext order)
        +Pay(OrderContext order)
        +Ship(OrderContext order)
        +Cancel(OrderContext order)
    }
    class DraftState {
        <<ConcreteState>>
    }
    class PlacedState {
        <<ConcreteState>>
    }
    class PaidState {
        <<ConcreteState>>
    }
    class ShippedState {
        <<ConcreteState>>
    }
    class CancelledState {
        <<ConcreteState>>
    }
    OrderContext --> OrderState : current
    OrderState <|-- DraftState
    OrderState <|-- PlacedState
    OrderState <|-- PaidState
    OrderState <|-- ShippedState
    OrderState <|-- CancelledState
```

| Role | Our class | Responsibility |
|---|---|---|
| Context | `OrderContext` | What clients use. Holds the current state object and delegates every action to it. |
| State | `OrderState` | One method per action; by default every action is illegal (throws). |
| ConcreteState | `DraftState`, `PlacedState`, `PaidState`, `ShippedState`, `CancelledState` | Override only the actions that are legal in that status, and move the context to the next state. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Order as OrderContext
    participant Placed as PlacedState
    participant Paid as PaidState
    Client->>Order: Pay()
    Order->>Placed: Pay(order)
    Placed->>Order: TransitionTo(PaidState)
    Client->>Order: Pay()
    Order->>Paid: Pay(order)
    Note right of Paid: not overridden: base throws
    Paid-->>Client: "Cannot pay an order that is Paid."
```

#### By hand

```csharp
// Role: State — what an order can do in one status. By default, nothing is allowed.
// Guide: §6.8
public abstract class OrderState
{
    public abstract OrderStatus Status { get; }

    public virtual void Place(OrderContext order) => throw Illegal("place");
    public virtual void Pay(OrderContext order) => throw Illegal("pay");
    public virtual void Ship(OrderContext order) => throw Illegal("ship");
    public virtual void Cancel(OrderContext order) => throw Illegal("cancel");

    private InvalidOperationException Illegal(string action) =>
        new($"Cannot {action} an order that is {Status}.");
}

// Role: ConcreteState — a placed order can be paid or cancelled, nothing else.
public sealed class PlacedState : OrderState
{
    public override OrderStatus Status => OrderStatus.Placed;
    public override void Pay(OrderContext order) => order.TransitionTo(new PaidState());
    public override void Cancel(OrderContext order) => order.TransitionTo(new CancelledState());
}

// Role: Context — the order clients use; its behaviour comes from the current state object.
public sealed class OrderContext
{
    private OrderState _state = new DraftState();

    public OrderStatus Status => _state.Status;
    public void Place() => _state.Place(this);
    public void Pay() => _state.Pay(this);
    public void Ship() => _state.Ship(this);
    public void Cancel() => _state.Cancel(this);

    internal void TransitionTo(OrderState next) => _state = next;
}
```

Everything a placed order can do is in `PlacedState`, in five lines. Adding a status (`Refunded`) is a new class plus the transitions that lead to it; the existing statuses do not change unless they gain a new move. States without data can be shared single instances (they are stateless), which is a small Flyweight ([5.6](#56-flyweight)).

#### In .NET

.NET has no State pattern built in. For a life cycle like this one, the light alternative is a **transition table** written as a single `switch` expression on the pair (status, action) ([`2-DotNet/`](../src/Patterns.Behavioral/State/2-DotNet/)):

```csharp
public enum OrderAction { Place, Pay, Ship, Cancel }

// Guide: §6.8
public static class OrderTransitions
{
    public static OrderStatus Next(OrderStatus from, OrderAction action) => (from, action) switch
    {
        (OrderStatus.Draft, OrderAction.Place) => OrderStatus.Placed,
        (OrderStatus.Placed, OrderAction.Pay) => OrderStatus.Paid,
        (OrderStatus.Paid, OrderAction.Ship) => OrderStatus.Shipped,
        (OrderStatus.Draft or OrderStatus.Placed or OrderStatus.Paid, OrderAction.Cancel) => OrderStatus.Cancelled,
        _ => throw new InvalidOperationException(
            $"Cannot {action.ToString().ToLowerInvariant()} an order that is {from}."),
    };
}
```

The whole state diagram fits on one screen and reads like it. Combined with records (`order with { Status = OrderTransitions.Next(order.Status, OrderAction.Pay) }`), it is often all you need.

**Switch or classes?**

| Use a transition `switch` when… | Use State classes when… |
|---|---|
| States differ only in **which moves are allowed**. | States differ in **behaviour and data** (a paid order computes refunds, a shipped one tracks the parcel). |
| The diagram fits in one table and changes rarely. | Each state has a lot of logic, and you want it together. |
| You want to see the whole machine at a glance. | Several people change different states independently. |

Many real workflows start as a `switch` and are refactored to classes when one state grows its own logic (section [3.6](#36-patternitis)).

#### In the ecosystem

**Stateless** (Apache-2.0) lets you declare a state machine fluently (`machine.Configure(Placed).Permit(Pay, Paid)`), with guards, entry/exit actions and diagram export. Useful for large workflows with many guards; overkill for five states.

#### When to use it

- An object's behaviour depends on its state, and **many methods** would otherwise `switch` on the same state field.
- States have their own data and rules that belong together.
- The life cycle must be enforced: illegal transitions must be impossible, not just discouraged.

#### When NOT to use it

- **Few states, rules that only say which moves are legal:** a transition `switch` (the .NET level) is shorter and shows the whole diagram at once.
- **Two states:** a `bool` and an `if`.
- **The states come from configuration** or change per customer: a table-driven machine (or a library) is better than one class per state.

#### Costs

- One class per state; the overall diagram is no longer visible in one place, only by reading every class.
- The states and the context are tightly coupled (states call `TransitionTo`).

#### Relevance today

⭐⭐ **Useful.** Every order, payment, ticket or document has a life cycle. Most are well served by a `switch` expression; the class-based State pattern earns its place when states carry real behaviour.

#### Relatives

- **Strategy** ([6.9](#69-strategy)) has the same structure (a context delegating to an interchangeable object). A strategy is chosen **from outside** and usually stays; a state **changes itself** as the object moves through its life. Comparison in [8.2](#82-strategy-state-template-method).
- **Flyweight** ([5.6](#56-flyweight)): stateless state objects can be shared.
- **Memento** ([6.6](#66-memento)) can capture the state to restore it.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- state
```

1. Add a `Refund` action: only a `Paid` or `Shipped` order can be refunded, and it ends `Cancelled`. Do it in all three levels and compare.
2. Try to ship a placed order and cancel a shipped one; read the messages.
3. Run the `[Theory]` over all 20 (status, action) pairs and count how many are legal.

#### Interview questions

<details>
<summary>What is the difference between State and Strategy?</summary>

Same structure, different intent. A strategy is chosen by the client and represents *how* to do something; a state represents *where* the object is in its life and replaces itself as the object changes, so the transitions live inside the states.
</details>

<details>
<summary>When is a <code>switch</code> expression better than the State pattern?</summary>

When states only differ in which transitions are allowed: a `switch` on `(status, action)` is a readable transition table that shows the whole state machine at once.
</details>

### 6.9 Strategy

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Behavioral |
| Intent | Define a family of algorithms, encapsulate each one, and make them interchangeable. |
| Also known as | Policy |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- strategy` |
| Code | [`src/Patterns.Behavioral/Strategy/`](../src/Patterns.Behavioral/Strategy/) |

#### The problem

The shop offers three shipping methods, each priced differently:

| Method | Price |
|---|---|
| `standard` | 4.99, **free** when the order total is 50.00 or more |
| `express` | 9.99 + 1.00 per unit |
| `pickup` | 0.00 (collect it in the store) |

For three books (37.50): standard 4.99, express 12.99, pickup 0.00. For four books (exactly 50.00), standard is free: the rule is `>=`, not `>`. More methods (a locker network, same-day delivery) are coming.

*Analogy:* getting to the airport. Bus, taxi or train: the goal is the same, the way and the cost differ, and you pick one depending on the situation. The trip does not change because a new tram line opens.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/Strategy/0-Problem/) (also shown in section [2.3](#23-the-folder-of-a-pattern)):

```csharp
public sealed class ShippingCalculator
{
    public decimal CostFor(Order order, string method) => method switch
    {
        "standard" => order.Total >= 50.00m ? 0.00m : 4.99m,
        "express" => 9.99m + 1.00m * order.Units,
        "pickup" => 0.00m,
        // PAIN: every new carrier means another case here, and this method's tests change with it.
        _ => throw new ArgumentException($"Unknown shipping method '{method}'."),
    };
}
```

For three methods that never change, this `switch` is perfectly fine. It hurts when methods are added often, have their own dependencies (a carrier API) or their own tests, or are chosen by configuration.

#### Structure

```mermaid
classDiagram
    class ShippingCalculator {
        <<Context>>
        -IShippingStrategy _strategy
        +CostFor(Order order) decimal
    }
    class IShippingStrategy {
        <<Strategy>>
        +CostFor(Order order) decimal
    }
    class StandardShipping {
        <<ConcreteStrategy>>
    }
    class ExpressShipping {
        <<ConcreteStrategy>>
    }
    class StorePickup {
        <<ConcreteStrategy>>
    }
    ShippingCalculator --> IShippingStrategy
    IShippingStrategy <|.. StandardShipping
    IShippingStrategy <|.. ExpressShipping
    IShippingStrategy <|.. StorePickup
```

| Role | Our class | Responsibility |
|---|---|---|
| Strategy | `IShippingStrategy` | The common interface of the algorithms. |
| ConcreteStrategy | `StandardShipping`, `ExpressShipping`, `StorePickup` | One pricing rule each. |
| Context | `ShippingCalculator` | Holds a strategy and uses it; does not know which one. |

#### How it runs

The sequence diagram of section [3.4](#34-how-to-read-the-diagrams) is exactly this pattern: the calculator forwards `CostFor(order)` to the express strategy, which returns 12.99.

#### By hand

```csharp
// Role: Strategy — one way of pricing shipping.
// Guide: §6.9
public interface IShippingStrategy
{
    decimal CostFor(Order order);
}

// Role: ConcreteStrategy — standard shipping, free from 50.00 (inclusive).
public sealed class StandardShipping : IShippingStrategy
{
    public decimal CostFor(Order order) => order.Total >= 50.00m ? 0.00m : 4.99m;
}

// Role: ConcreteStrategy — express: a fixed fee plus one euro per unit.
public sealed class ExpressShipping : IShippingStrategy
{
    public decimal CostFor(Order order) => 9.99m + 1.00m * order.Units;
}

// Role: Context — prices shipping with whatever strategy it was given.
public sealed class ShippingCalculator(IShippingStrategy strategy)
{
    public decimal CostFor(Order order) => strategy.CostFor(order);
}
```

Each rule is its own class with its own tests (`Standard_AtExactly50_IsFree`). A new method is a new class; neither the calculator nor the other strategies change (the Open/Closed principle, [3.5](#35-the-principles-under-the-patterns)).

#### In .NET

**Keyed services** let the container hold all the strategies and pick one by name at run time ([`2-DotNet/`](../src/Patterns.Behavioral/Strategy/2-DotNet/)):

```csharp
// AddShippingStrategies(this IServiceCollection services) in the DotNet level:
services.AddKeyedSingleton<IShippingStrategy, StandardShipping>("standard");
services.AddKeyedSingleton<IShippingStrategy, ExpressShipping>("express");
services.AddKeyedSingleton<IShippingStrategy, StorePickup>("pickup");

// Guide: §6.9
public sealed class ShippingCalculator(IServiceProvider services)
{
    public decimal CostFor(Order order, string method) =>
        services.GetRequiredKeyedService<IShippingStrategy>(method).CostFor(order); // unknown key: InvalidOperationException
}
```

Compare with the Problem level: the same `CostFor(order, method)` signature, but the `switch` is now the container's registrations, and a new method is a new class plus one line of registration.

**A strategy is often just a function.** When the algorithm is one method with no dependencies, a `Func<Order, decimal>` *is* the strategy interface, and a lambda is a concrete strategy:

```csharp
public static class ShippingRules
{
    public static Func<Order, decimal> Standard { get; } = order => order.Total >= 50.00m ? 0.00m : 4.99m;
    public static Func<Order, decimal> Express { get; } = order => 9.99m + 1.00m * order.Units;
    public static Func<Order, decimal> Pickup { get; } = _ => 0.00m;

    // Takes any strategy: PriceWith(threeBooks, ShippingRules.Express) is 37.50 + 12.99 = 50.49.
    public static decimal PriceWith(Order order, Func<Order, decimal> shipping) => order.Total + shipping(order);
}
```

**Strategy vs `Func<>` vs keyed services:**

| Choose | When |
|---|---|
| `Func<T, TResult>` | The algorithm is one small function, with no dependencies of its own, chosen in code. |
| An interface + classes | The algorithm has several methods, its own dependencies (a carrier client), its own tests, or a name you want to see in the code. |
| Keyed services | The strategy is chosen **at run time by a key** (a user's choice, a configuration value) and the strategies come from the container. |

The BCL uses strategies everywhere: `IComparer<T>` decides how `List<T>.Sort` orders elements, `IEqualityComparer<T>` how a `Dictionary` compares keys (`StringComparer.OrdinalIgnoreCase` is a strategy), and every LINQ method that takes a lambda takes a strategy:

```csharp
orders.Sort(Comparer<Order>.Create((a, b) => b.Total.CompareTo(a.Total))); // most expensive first
```

#### When to use it

- Several ways of doing the same thing, chosen at run time (by the user, by configuration, by data).
- A `switch` on a "type" or "mode" string that keeps growing, especially if each branch is long or has its own dependencies.
- You want to test each algorithm in isolation.

#### When NOT to use it

- **Two or three stable variants:** a `switch` expression is clearer, and everything is in one place.
- **The variants never change at run time** and only one is used: there is nothing to swap.
- **Only to pass a little behaviour:** use a `Func<>` parameter, not an interface and a class.

#### Costs

- More types; the reader must find which strategy is used where.
- Clients (or the composition root) must know the strategies to choose one.

#### Relevance today

⭐⭐⭐ **Essential.** Every lambda passed to LINQ, every comparer and every service resolved by key is a strategy. It is the pattern behind "depend on an interface and inject the implementation".

#### Relatives

- **State** ([6.8](#68-state)): same structure; a state replaces itself, a strategy is chosen from outside. **Template Method** ([6.10](#610-template-method)) varies part of an algorithm by inheritance; Strategy varies all of it by composition. Comparison in [8.2](#82-strategy-state-template-method).
- **Command** ([6.2](#62-command)): both wrap behaviour in an object; comparison in [8.5](#85-command-and-strategy).
- **Factory Method** ([4.2](#42-factory-method)) and keyed services: a factory often chooses the strategy. See [9.1](#91-combinations-you-will-meet-in-real-code).
- **Decorator** ([5.4](#54-decorator)) changes the outside of an object; Strategy its inside.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- strategy
```

1. Add a `locker` method (2.50 flat) at each level and count the files you touch.
2. Change `>=` to `>` in `StandardShipping` and run the tests: which one catches it?
3. Ask the .NET calculator for `"drone"` and read the exception.

#### Interview questions

<details>
<summary>How do lambdas relate to Strategy?</summary>

A delegate type like `Func<Order, decimal>` is a one-method strategy interface, and each lambda is a concrete strategy. When the algorithm needs no state or dependencies, a lambda replaces the classes.
</details>

<details>
<summary>Give three examples of Strategy in the BCL.</summary>

`IComparer<T>` for sorting, `IEqualityComparer<T>` (for example `StringComparer.OrdinalIgnoreCase`) for dictionaries and sets, and the predicate and selector lambdas of LINQ.
</details>

<details>
<summary>When is a <code>switch</code> better than Strategy?</summary>

When there are few variants, they rarely change, and they have no dependencies of their own: the switch keeps all the rules visible in one place.
</details>

### 6.10 Template Method

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Behavioral |
| Intent | Define the skeleton of an algorithm in a method, deferring some steps to subclasses. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- template-method` |
| Code | [`src/Patterns.Behavioral/TemplateMethod/`](../src/Patterns.Behavioral/TemplateMethod/) |

#### The problem

The shop exports orders as **CSV** (comma-separated values, for spreadsheets) and as **JSON** (for other systems). Both exports follow the same rules: skip draft orders, sort by total (highest first, then by id), write a header, one row per order, a footer. Only the **format** of the header, rows and footer differs.

The tests use three fixed orders: A (two books, 25.00, `Placed`), B (headphones, 59.90, `Paid`) and C (a mug, `Draft`). The CSV export is exactly:

```
id,customer,status,total
00000000-0000-0000-0000-000000000002,Ana,Paid,59.90
00000000-0000-0000-0000-000000000001,Ana,Placed,25.00
```

and the JSON export `[{"id":"…0002","total":59.90},{"id":"…0001","total":25.00}]` (full ids, no spaces). Order C is a draft, so it appears in neither.

*Analogy:* a recipe card for "a sandwich": take bread, add the filling, close, cut. Every sandwich follows those steps; only "add the filling" changes between a ham sandwich and a cheese one.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Behavioral/TemplateMethod/0-Problem/):

```csharp
public sealed class CsvExporter
{
    public string Export(IEnumerable<Order> orders)
    {
        // PAIN: the filter and sort rules are copied in JsonExporter. Changing them (say, also skip
        // cancelled orders) in one exporter and forgetting the other is a matter of time.
        var selected = orders.Where(o => o.Status != OrderStatus.Draft)
                             .OrderByDescending(o => o.Total).ThenBy(o => o.Id);
        var csv = new StringBuilder("id,customer,status,total\n");
        foreach (var order in selected)
        {
            csv.Append(CultureInfo.InvariantCulture, $"{order.Id},{order.Customer.Name},{order.Status},{order.Total:0.00}\n");
        }
        return csv.ToString();
    }
}
```

#### Structure

```mermaid
classDiagram
    class OrderExporter {
        <<AbstractClass>>
        +Export(IEnumerable~Order~ orders) string
        #Header()* string
        #Row(Order order, bool isLast)* string
        #Footer() string
    }
    class CsvOrderExporter {
        <<ConcreteClass>>
        #Header() string
        #Row(Order order, bool isLast) string
    }
    class JsonOrderExporter {
        <<ConcreteClass>>
        #Header() string
        #Row(Order order, bool isLast) string
        #Footer() string
    }
    OrderExporter <|-- CsvOrderExporter
    OrderExporter <|-- JsonOrderExporter
```

| Role | Our class | Responsibility |
|---|---|---|
| AbstractClass | `OrderExporter` | `Export` is the **template method**: the fixed steps. It calls abstract steps and hooks. |
| ConcreteClass | `CsvOrderExporter`, `JsonOrderExporter` | Fill in the steps that vary: header, row, and optionally footer. |

Three kinds of method appear:

- the **template method** (`Export`): public and *not* virtual, so subclasses cannot change the skeleton;
- **abstract steps** (`Header`, `Row`): every subclass must provide them;
- **hooks** (`Footer`): virtual with a default (here, empty), so subclasses *may* override them. JSON needs a closing `]`; CSV does not.

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Base as OrderExporter.Export
    participant Json as JsonOrderExporter
    Client->>Base: Export(orders)
    Note right of Base: skip drafts, sort by total desc, then id
    Base->>Json: Header()
    Json-->>Base: "["
    Base->>Json: Row(B, isLast: false)
    Base->>Json: Row(A, isLast: true)
    Base->>Json: Footer()
    Json-->>Base: "]"
    Base-->>Client: the JSON text
```

The base class calls the subclass, not the other way round.

#### By hand

```csharp
// Role: AbstractClass — the export algorithm, with the format left to subclasses.
// Guide: §6.10
public abstract class OrderExporter
{
    // The template method: not virtual, so every exporter follows the same rules.
    public string Export(IEnumerable<Order> orders)
    {
        var selected = orders.Where(o => o.Status != OrderStatus.Draft)
                             .OrderByDescending(o => o.Total).ThenBy(o => o.Id)
                             .ToList();
        var text = new StringBuilder(Header());
        for (var i = 0; i < selected.Count; i++)
        {
            text.Append(Row(selected[i], isLast: i == selected.Count - 1));
        }
        return text.Append(Footer()).ToString();
    }

    protected abstract string Header();
    protected abstract string Row(Order order, bool isLast);
    protected virtual string Footer() => ""; // a hook: optional, empty by default
}

// Role: ConcreteClass — the CSV format.
public sealed class CsvOrderExporter : OrderExporter
{
    protected override string Header() => "id,customer,status,total\n";
    protected override string Row(Order order, bool isLast) => string.Create(CultureInfo.InvariantCulture,
        $"{order.Id},{order.Customer.Name},{order.Status},{order.Total:0.00}\n");
}

// Role: ConcreteClass — the JSON format (written by hand to keep the example small).
public sealed class JsonOrderExporter : OrderExporter
{
    protected override string Header() => "[";
    protected override string Row(Order order, bool isLast) => string.Create(CultureInfo.InvariantCulture,
        $"{{\"id\":\"{order.Id}\",\"total\":{order.Total:0.00}}}{(isLast ? "" : ",")}");
    protected override string Footer() => "]";
}
```

The selection rules live in one place; a new format is a subclass with two or three small methods.

**The Hollywood principle: "don't call us, we'll call you".** In ordinary code, your code calls the library. With Template Method the roles reverse: the base class (often a framework) owns the flow and **calls your code** at the right moments. That inversion of control is what makes frameworks frameworks.

**Why to prefer composition.** Template Method is the GoF pattern built on inheritance, and it inherits its limits (section [3.5](#35-the-principles-under-the-patterns)): a class has one base class, so you cannot combine "CSV format" with "only paid orders" and "gzip" without a subclass per combination; and subclasses depend on the base class's internals. If the varying steps were passed in as objects or functions (`new OrderExporter(new CsvFormat())`), you would have Strategy ([6.9](#69-strategy)) instead, with free combination. Template Method is still fine when the steps are few, belong together, and the base class is designed for it, as in the framework classes below.

#### In .NET

**`BackgroundService`** is a template method you use in every worker service ([`2-DotNet/`](../src/Patterns.Behavioral/TemplateMethod/2-DotNet/)). The framework's `StartAsync` and `StopAsync` own the skeleton (start the work on a background task, cancel it on shutdown, wait for it); you fill in one abstract step, `ExecuteAsync`:

```csharp
// Role: ConcreteClass — fills in the one step BackgroundService leaves open.
// Guide: §6.10
public sealed class NightlyExportService(IEnumerable<Order> orders, TextWriter output) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // OrderCsv is this level's own CSV writer, with the same rules (levels never share the pattern's types).
        output.Write(OrderCsv.Export(orders)); // export once, then finish
        return Task.CompletedTask;
    }
}
```

The host calls `StartAsync`, which calls your `ExecuteAsync`: the Hollywood principle in action. Since .NET 10, `StartAsync` runs the whole `ExecuteAsync` on a background task and returns at once, so a test must `await service.ExecuteTask` (or call `StopAsync`) before checking the result.

**`Stream`** is another: methods like `CopyTo`, `ReadExactly` and the `Span`-based `Read` are written once in the `Stream` base class on top of the abstract `Read(byte[], int, int)` and `Write`, which each concrete stream (`FileStream`, `MemoryStream`, `NetworkStream`) implements. MVC's `Controller.OnActionExecuting` / `OnActionExecuted`, EF Core's `DbContext.OnConfiguring` / `OnModelCreating` and `JsonConverter<T>.Read` / `Write` are steps and hooks of the same kind.

#### When to use it

- Several classes share the **same algorithm** with a few varying steps, and the steps belong together.
- You write a **framework or base class** that must control the order of steps (and guarantee the common ones run) while letting users customise some of them.

#### When NOT to use it

- **The varying parts are independent of each other** (format, filter, compression): pass them as objects or delegates (Strategy) so they combine freely.
- **Only one implementation exists:** write the method directly.
- **Deep hierarchies** (a template method calling a template method in the parent): the flow becomes very hard to follow. Prefer composition.

#### Costs

- Inheritance: one base class, coupling to its internals, and subclasses that break when the base changes (the "fragile base class" problem).
- The flow jumps between base and subclass, which makes reading and debugging harder.

#### Relevance today

⭐⭐ **Useful.** You *use* framework template methods (`BackgroundService`, `Stream`, `DbContext`) all the time; you *write* new ones less often, because composition (Strategy, delegates) is usually the better choice in application code.

#### Relatives

- **Strategy** ([6.9](#69-strategy)) varies the whole algorithm by composition; Template Method varies steps by inheritance. Comparison in [8.2](#82-strategy-state-template-method).
- **Factory Method** ([4.2](#42-factory-method)) is a template method whose varying step is "create an object".
- **Chain of Responsibility** ([6.1](#61-chain-of-responsibility)): the `Check` method of `OrderRule` is itself a small template method.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- template-method
```

1. Also skip `Cancelled` orders: change one line in Classic, two in Problem, and one in the DotNet level's own `OrderCsv` (each level keeps its own copy).
2. Write a `MarkdownOrderExporter` that produces a table, overriding the footer hook if needed.
3. Rewrite the Classic level with composition: `OrderExporter(IOrderFormat format)`. What does each version make easy?

#### Interview questions

<details>
<summary>What is a hook in Template Method?</summary>

A virtual step with a default implementation (often empty) that subclasses may override but do not have to, unlike abstract steps, which they must provide.
</details>

<details>
<summary>What is the Hollywood principle?</summary>

"Don't call us, we'll call you": the base class or framework controls the flow and calls the user's code at defined points, instead of the user's code calling the library.
</details>

<details>
<summary>Why is Template Method often replaced by Strategy?</summary>

Template Method relies on inheritance: one base class, combinations need more subclasses, and subclasses depend on base internals. Passing the varying steps as objects or delegates lets them combine and be tested independently.
</details>

### 6.11 Visitor

#### Card

| | |
|---|---|
| Relevance | ⭐ Niche — pattern matching is the modern alternative |
| Family | Behavioral |
| Intent | Represent an operation to be performed on the elements of an object structure, so new operations can be added without changing the elements' classes. |
| Also known as | — |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- visitor` |
| Code | [`src/Patterns.Behavioral/Visitor/`](../src/Patterns.Behavioral/Visitor/) |

#### The problem

The catalog tree of Composite ([5.3](#53-composite)) needs more and more **operations**: the VAT of a bundle, an indented outline for printing, an export to the supplier's format, a weight for shipping… Adding a method to every node class for every new operation turns the catalog classes into a dumping ground. This pattern uses its own small tree (`CatalogNode` with `ProductNode` and `BundleNode`), the same Starter kit as in Composite.

Two operations are coded:

- **VAT**: books at 4 %, everything else at 21 %, summed without rounding and rounded once at the end. Starter kit: `0.50 + 1.68 + 12.579 = 14.759` → **14.76**.
- **Outline**: an indented list, two spaces per level, prices with a dot:

```
Starter kit
  Clean Code 12.50
  Coffee Mug 8.00
  Audio
    Wireless Headphones 59.90
```

*Analogy:* a tax inspector visiting businesses. The shops and restaurants do not learn tax law; the inspector brings it, and applies a different rule to each kind of business. A health inspector visiting the same places brings a different set of rules. New inspections need no change to the businesses.

#### Without the pattern

```csharp
// Each operation is a type switch over the node classes, repeated in every operation:
decimal VatOf(CatalogNode node)
{
    if (node is ProductNode p) return p.Product.Price * (p.Product.Category.Name == "Books" ? 0.04m : 0.21m);
    if (node is BundleNode b) return b.Children.Sum(VatOf);
    throw new NotSupportedException(); // a new node type compiles fine and fails here, at run time
}
```

In 1994 C++ this was considered fragile: the type checks were casts, and forgetting a node type was silent. That is the problem Visitor solved, and the reason C# pattern matching now competes with it (see "In .NET").

#### Structure

```mermaid
classDiagram
    class ICatalogVisitor {
        <<Visitor>>
        +Visit(ProductNode node)
        +Visit(BundleNode node)
    }
    class VatVisitor {
        <<ConcreteVisitor>>
        +decimal Total
    }
    class OutlineVisitor {
        <<ConcreteVisitor>>
        +string Text
    }
    class CatalogNode {
        <<Element>>
        +Accept(ICatalogVisitor visitor)*
    }
    class ProductNode {
        <<ConcreteElement>>
    }
    class BundleNode {
        <<ConcreteElement>>
    }
    ICatalogVisitor <|.. VatVisitor
    ICatalogVisitor <|.. OutlineVisitor
    CatalogNode <|-- ProductNode
    CatalogNode <|-- BundleNode
    CatalogNode ..> ICatalogVisitor : Accept
```

| Role | Our class | Responsibility |
|---|---|---|
| Visitor | `ICatalogVisitor` | One `Visit` overload per element class. |
| ConcreteVisitor | `VatVisitor`, `OutlineVisitor` | One operation, with a method per element type. |
| Element | `CatalogNode` | Declares `Accept(visitor)`. |
| ConcreteElement | `ProductNode`, `BundleNode` | Implement `Accept` by calling `visitor.Visit(this)`. |

#### How it runs

**Double dispatch, step by step.** The operation to run depends on **two** types at once: which visitor (VAT or outline) and which node (product or bundle). A normal virtual call chooses a method by **one** type at run time (the object it is called on): that is *single dispatch*. Visitor gets the second choice with a second call:

1. The client calls `node.Accept(visitor)`. `Accept` is virtual, so the runtime picks the **node's** override: first dispatch, on the node type.
2. Inside `ProductNode.Accept`, the code is `visitor.Visit(this)`. There, `this` has the static type `ProductNode`, so the **compiler** already chose the `Visit(ProductNode)` overload.
3. `Visit` is an interface method, so the runtime picks the **visitor's** implementation: second dispatch, on the visitor type.

```mermaid
sequenceDiagram
    participant Client
    participant Node as ProductNode (Clean Code)
    participant Visitor as VatVisitor
    Client->>Node: Accept(visitor)
    Note right of Node: 1st dispatch: ProductNode.Accept runs
    Node->>Visitor: Visit(this as ProductNode)
    Note right of Visitor: 2nd dispatch: VatVisitor.Visit(ProductNode) runs
    Visitor->>Visitor: Total += 12.50 × 0.04
```

For a bundle, `BundleNode.Accept` calls `visitor.Visit(this)`, and `VatVisitor.Visit(BundleNode)` calls `child.Accept(this)` on each child, so the visit walks the whole tree.

#### By hand

```csharp
// Role: Visitor — one method per kind of node: the operation, split by node type.
// Guide: §6.11
public interface ICatalogVisitor
{
    void Visit(ProductNode node);
    void Visit(BundleNode node);
}

// Role: ConcreteElement — a product; its only job in the pattern is to say "I am a product".
public sealed class ProductNode(Product product) : CatalogNode
{
    public Product Product => product;
    public override void Accept(ICatalogVisitor visitor) => visitor.Visit(this); // this: ProductNode
}

// Role: ConcreteVisitor — computes VAT over a whole tree.
public sealed class VatVisitor : ICatalogVisitor
{
    private decimal _raw; // summed unrounded; rounded once, at the end

    public decimal Total => decimal.Round(_raw, 2, MidpointRounding.AwayFromZero);

    public void Visit(ProductNode node) =>
        _raw += node.Product.Price * (node.Product.Category.Name == "Books" ? 0.04m : 0.21m);

    public void Visit(BundleNode node)
    {
        foreach (var child in node.Children)
        {
            child.Accept(this); // walk into the bundle
        }
    }
}
```

A new operation (weight, supplier export) is a new visitor class; the node classes do not change. The price: a new **node type** (a gift card) means a new `Visit` method in the interface and in **every** visitor. Visitor makes adding operations easy and adding types hard; plain classes do the opposite. Choose it when the set of types is stable and the operations keep coming.

#### In .NET

**Pattern matching** is the modern alternative ([`2-DotNet/`](../src/Patterns.Behavioral/Visitor/2-DotNet/)). With a closed hierarchy of records, an operation is a function with a `switch` expression; the "dispatch on the node type" is the `switch`:

```csharp
public abstract record CatalogItem;
public sealed record ProductItem(Product Product) : CatalogItem;
public sealed record BundleItem(string Name, IReadOnlyList<CatalogItem> Children) : CatalogItem;

// Guide: §6.11
public static class VatCalculator
{
    public static decimal VatOf(CatalogItem item) =>
        decimal.Round(RawVat(item), 2, MidpointRounding.AwayFromZero);

    private static decimal RawVat(CatalogItem item) => item switch
    {
        ProductItem { Product.Category.Name: "Books" } p => p.Product.Price * 0.04m,
        ProductItem p => p.Product.Price * 0.21m,
        BundleItem b => b.Children.Sum(RawVat),
        _ => throw new NotSupportedException(item.GetType().Name),
    };
}
```

Same result (14.76), no `Accept`, no visitor interface, and the elements know nothing about the operations. What it loses: C# does not yet check that a `switch` over a class hierarchy covers every subtype, so a new record type reaches the `_` arm at run time. That is the trade-off "Without the pattern" described, accepted today because the code is short and the `switch` is all in one place.

**`ExpressionVisitor`** is the BCL's Visitor for expression trees (section [6.3](#63-interpreter)): it has one `Visit…` method per node kind (`VisitBinary`, `VisitConstant`, `VisitMember`…), and you override the ones you care about. EF Core uses visitors like this to translate LINQ into SQL. A small one that collects the constants of a condition:

```csharp
public sealed class ConstantCollector : ExpressionVisitor
{
    private readonly List<object?> _constants = [];

    public IReadOnlyList<object?> Constants => _constants; // not List<T>: analyzer CA1002

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _constants.Add(node.Value);
        return base.VisitConstant(node);
    }
}

Expression<Func<Order, bool>> rule = o => o.Total > 100m && o.Units < 5;
var collector = new ConstantCollector();
collector.Visit(rule); // collector.Constants: [100, 5]
```

The Roslyn compiler APIs (`CSharpSyntaxVisitor`, `CSharpSyntaxWalker`) are visitors too, used to write analyzers and source generators.

#### When to use it

- A **stable set of element types** (a syntax tree, a document model) and a **growing set of operations** over them.
- Operations that need different code per element type and want to keep their state together (a running total, an indentation level).
- Libraries that expose a tree to users: a visitor base class (like `ExpressionVisitor`) lets users write operations without touching the library.

#### When NOT to use it

- **Your own small hierarchy in C#:** a `switch` expression over records does the same with far less code.
- **The element types change often:** every new type edits every visitor.
- **One or two operations:** put them on the classes, or write a function.

#### Costs

- Double dispatch is hard to explain and to follow in a debugger.
- Elements must expose enough state for visitors to work, which weakens encapsulation.
- New element types are expensive.

#### Relevance today

⭐ **Niche.** You write a visitor class when a library hands you one (`ExpressionVisitor`, Roslyn's syntax visitors). For your own types, pattern matching made the hand-written pattern unnecessary in most cases.

#### Relatives

- **Composite** ([5.3](#53-composite)): visitors usually walk composite trees. Section [9.1](#91-combinations-you-will-meet-in-real-code) combines them.
- **Interpreter** ([6.3](#63-interpreter)): the syntax tree of an interpreter is the typical structure a visitor works on.
- **Iterator** ([6.4](#64-iterator)) hands you elements; a visitor runs type-specific code on each one.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- visitor
```

1. Write a `ProductCountVisitor` and the same operation as a `switch` function. Compare their length.
2. Add a `GiftCardNode`: how many files must change in Classic? And in the .NET level, what happens if you forget the new case?
3. Extend `ConstantCollector` to also collect member names (`Total`, `Units`) by overriding `VisitMember`.

#### Interview questions

<details>
<summary>What is double dispatch?</summary>

Choosing the code to run by the run-time types of two objects. Visitor does it with two virtual calls: `element.Accept(visitor)` picks the element's method, which calls `visitor.Visit(this)` and picks the visitor's method for that element type.
</details>

<details>
<summary>What does Visitor make easy, and what does it make hard?</summary>

It makes adding new operations easy (a new visitor class) and adding new element types hard (every visitor needs a new method).
</details>

<details>
<summary>How does C# pattern matching replace Visitor?</summary>

A `switch` expression on the element's type (with property patterns for details) dispatches per type in one function, with no `Accept` methods. It trades the compiler's completeness check for much less code.
</details>

---

## 7. Modern .NET patterns

The GoF book is from 1994. Since then, other patterns have become just as important, and some of them run inside **every** .NET application: the DI container builds your objects, configuration reaches your code as options, and Entity Framework is a repository and a unit of work. They are not in the GoF catalog, but they use the same ideas (program to an interface, encapsulate what varies) and you meet them daily.

| Pattern | Relevance | In one line |
|---|---|---|
| [7.1 Dependency Injection](#71-dependency-injection) | ⭐⭐⭐ Essential | Objects receive their collaborators from outside instead of creating or looking them up. |
| [7.2 Options](#72-options) | ⭐⭐⭐ Essential | Settings arrive as typed, validated objects instead of strings read everywhere. |
| [7.3 Repository](#73-repository) | ⭐⭐ Useful (debated) | A collection-like interface over storage, so business code does not know how data is kept. |
| [7.4 Unit of Work](#74-unit-of-work) | ⭐⭐ Useful | Collect changes and commit them together, all or nothing. |
| [7.5 Specification](#75-specification) | ⭐⭐ Useful | A business rule ("in stock and cheap") as an object you can combine and reuse. |
| [7.6 Result](#76-result) | ⭐⭐ Useful | Return expected failures as values instead of throwing exceptions. |
| [7.7 Null Object](#77-null-object) | ⭐⭐ Useful | An object that does nothing, used instead of `null`. |
| [7.8 Object Pool](#78-object-pool) | ⭐ Niche | Reuse expensive objects instead of creating and discarding them. |

### 7.1 Dependency Injection

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Modern (creational in spirit) |
| Intent | Give an object the objects it depends on from outside, instead of letting it create or find them. |
| Also known as | Inversion of Control (IoC, the broader idea); constructor injection |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- dependency-injection` |
| Code | [`src/Patterns.Modern/DependencyInjection/`](../src/Patterns.Modern/DependencyInjection/) |

#### The problem

Confirming an order sends an email and stamps the time: `"Order 1a2b3c4d confirmed at 10:30"`. The checkout needs two **dependencies** (objects it uses to do its job): something that sends emails (`IEmailSender`) and something that tells the time (`TimeProvider`, the .NET 8+ abstraction over the clock). In tests, we want a fake email sender that just records the message, and a clock fixed at 10:30 (in UTC, Coordinated Universal Time).

*Analogy:* a chef does not grow the vegetables or forge the knives. The restaurant supplies them, and can switch supplier without retraining the chef. A chef who insists on growing their own tomatoes cannot cook anywhere else.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/DependencyInjection/0-Problem/), two common shapes:

```csharp
// (a) Creating the dependencies inside.
public sealed class CheckoutService
{
    public string Confirm(Order order)
    {
        // PAIN: a real SMTP sender and the real clock, hard-wired. A test sends a real email and gets
        // a different time on every run; nothing can be replaced.
        var sender = new SmtpEmailSender();
        var text = string.Create(CultureInfo.InvariantCulture,
            $"Order {order.Id.ToString()[..8]} confirmed at {DateTime.Now:HH:mm}");
        sender.Send(order.Customer.Email, text);
        return text;
    }
}

// (b) A service locator: a global registry asked from inside methods.
var sender = ServiceLocator.Get<IEmailSender>(); // PAIN: the dependency is invisible from outside
```

What hurts: the dependencies are **hidden** (the constructor says the class needs nothing, which is false), the class decides *which* implementation to use, and tests cannot substitute anything. The test `Problem_HidesItsDependencies` checks that the only constructor has no parameters: there is nothing to inject. The `SmtpEmailSender` of the Problem level is simulated (it sends nothing), but the shape is the real one.

The **Service Locator** (b) is often presented as a fix, and it is an **anti-pattern**: the class can be constructed, but it fails at run time if the locator is not configured (the test `Problem_Locator_FailsOnlyWhenUsed`: `"No service registered for IEmailSender."`), and you only learn what it needs by reading every method.

#### Structure

```mermaid
classDiagram
    class CheckoutService {
        <<Client>>
        +CheckoutService(IEmailSender sender, TimeProvider clock)
        +Confirm(Order order) string
    }
    class IEmailSender {
        <<Service>>
        +Send(string recipient, string text)
    }
    class FakeEmailSender {
        <<ConcreteService>>
    }
    class TimeProvider {
        <<Service>>
    }
    class CompositionRoot {
        <<Injector>>
        +CreateCheckout(IEmailSender sender, TimeProvider clock)$ CheckoutService
    }
    CheckoutService --> IEmailSender
    CheckoutService --> TimeProvider
    IEmailSender <|.. FakeEmailSender
    CompositionRoot ..> CheckoutService : creates and injects
```

| Role | Our class | Responsibility |
|---|---|---|
| Client | `CheckoutService` | Declares what it needs in its constructor and uses it. Creates nothing. |
| Service | `IEmailSender`, `TimeProvider` | The abstractions the client depends on. |
| ConcreteService | `FakeEmailSender` (tests and demo), an SMTP sender (production) | The implementations. |
| Injector | the composition root or the DI container | Creates the objects and passes each its dependencies. |

The **composition root** is the single place, near the program's entry point (`Program.cs`), where the whole object graph is assembled. Everywhere else, classes only receive.

#### How it runs

```mermaid
sequenceDiagram
    participant Main as Program (composition root)
    participant Container as ServiceProvider
    participant Checkout as CheckoutService
    Main->>Container: GetRequiredService of CheckoutService
    Note right of Container: sees the constructor needs IEmailSender and TimeProvider
    Container->>Container: resolve IEmailSender (FakeEmailSender)
    Container->>Container: resolve TimeProvider
    Container->>Checkout: new CheckoutService(sender, clock)
    Container-->>Main: checkout
    Main->>Checkout: Confirm(order)
    Checkout-->>Main: "Order 1a2b3c4d confirmed at 10:30"
```

#### By hand

```csharp
// Role: Client — says what it needs; never creates or looks up a dependency.
// Guide: §7.1
public sealed class CheckoutService(IEmailSender sender, TimeProvider clock)
{
    public string Confirm(Order order)
    {
        var text = string.Create(CultureInfo.InvariantCulture,
            $"Order {order.Id.ToString()[..8]} confirmed at {clock.GetUtcNow():HH:mm}");
        sender.Send(order.Customer.Email, text);
        return text;
    }
}

// Role: Injector — the composition root, written by hand ("pure DI").
public static class CompositionRoot
{
    public static CheckoutService CreateCheckout(IEmailSender sender, TimeProvider clock) => new(sender, clock);
}

// Production: CompositionRoot.CreateCheckout(new SmtpEmailSender(…), TimeProvider.System)
// Test:       CompositionRoot.CreateCheckout(new FakeEmailSender(), new FixedTime(10, 30))
```

(The Problem level reads local time, `DateTime.Now`; the injected levels print UTC, `clock.GetUtcNow()`, on purpose: a server's local time depends on where it runs. `TimeProvider.GetLocalNow()` is the exact replacement for `DateTime.Now` when local time is what you want.)

That is the whole pattern: **constructor parameters**. A container is optional. Wiring by hand (*pure DI*) is perfectly valid for small programs and makes every dependency visible to the compiler.

#### In .NET

`Microsoft.Extensions.DependencyInjection` is the container every ASP.NET Core and worker application uses ([`2-DotNet/`](../src/Patterns.Modern/DependencyInjection/2-DotNet/)). You **register** what implements what and for how long it lives; the container reads constructors and builds the graph. The registrations live in an extension method, `AddCheckout`, the usual way a feature offers its registrations to `Program.cs`:

```csharp
public static IServiceCollection AddCheckout(this IServiceCollection services, TimeProvider clock)
{
    services.AddSingleton(clock);                        // TimeProvider.System in production
    services.AddScoped<IEmailSender, FakeEmailSender>();
    services.AddTransient<CheckoutService>();
    return services;
}

using var provider = new ServiceCollection().AddCheckout(clock).BuildServiceProvider(new ServiceProviderOptions
{
    ValidateScopes = true,  // catch scoped services resolved from the root (see captive dependencies)
    ValidateOnBuild = true, // check every registration can be built, now instead of at first use
});

using var scope = provider.CreateScope();
var checkout = scope.ServiceProvider.GetRequiredService<CheckoutService>();
```

**Lifetimes.** Every registration says how long an instance lives:

| Lifetime | One instance per… | Typical for | Watch out |
|---|---|---|---|
| `AddSingleton` | container (the whole application) | stateless services, caches, `TimeProvider`, `HttpClient` factories | must be thread-safe; must not depend on scoped services |
| `AddScoped` | scope (in ASP.NET Core: one HTTP request) | `DbContext`, unit of work, per-request state | resolving it from the root container is a bug |
| `AddTransient` | resolution (every time it is asked for) | lightweight stateless classes | a disposable transient lives until its scope ends |

The tests `Lifetimes` check the three rows: the same singleton across scopes, the same scoped instance within a scope and a different one in another scope, a new transient every time.

**Captive dependency.** A service must not depend on a service with a **shorter** lifetime. A singleton that receives a scoped `DbContext` keeps that one context forever, shared by every request and every thread: data from one user leaks to another and the context breaks under concurrency. `ValidateScopes = true` (on by default in Development in ASP.NET Core) makes the container throw `InvalidOperationException` when a scoped service is resolved from the root, as the test `ResolvingScopedFromRoot_Throws` shows. With `ValidateOnBuild` as well, a singleton `CheckoutService` that needs the scoped `IEmailSender` is refused when the provider is built, before any request runs (the test `CaptiveDependency_FailsOnBuild`).

**Service Locator in disguise.** Injecting `IServiceProvider` and calling `GetService` inside methods is the Service Locator again. It is acceptable only inside infrastructure that genuinely resolves by run-time information (the mediator of [6.5](#65-mediator), keyed lookups by a user's choice in [6.9](#69-strategy)), never as a shortcut in business classes.

#### In the ecosystem

**Autofac** (MIT) is the best-known alternative container: modules, assembly scanning, decorators, interception and property injection. It plugs into the Microsoft abstractions. The built-in container covers most applications; reach for another one only for a feature you actually need.

#### When to use it

- Always, for classes with dependencies that do I/O (database, network, files, email), depend on time or randomness, or have alternative implementations.
- When you want to test a class in isolation: constructor injection is what lets a test pass fakes.

#### When NOT to use it

- **Values and simple objects:** records, DTOs, `Order`, a `StringBuilder` — just `new` them. Not everything is a service.
- **Interfaces for everything:** inject a concrete class when there is no second implementation and no I/O to fake (section [3.6](#36-patternitis)).
- **Long constructors** (eight dependencies) are not a reason for a locator or property injection: they are a sign the class does too much. Split it.

#### Costs

- Indirection: "who creates this, and which implementation is it?" is answered in `Program.cs`, not at the call site.
- Configuration errors (missing registration, captive dependency) appear at start-up or run time unless validated.

#### Relevance today

⭐⭐⭐ **Essential.** Every modern .NET application is built around a DI container, and every pattern in this guide that says "inject" relies on it.

#### Relatives

- **Singleton** ([4.1](#41-singleton)) and **Factory Method** ([4.2](#42-factory-method)): the container's lifetimes and keyed services replaced their hand-written forms.
- **Strategy** ([6.9](#69-strategy)) and **Decorator** ([5.4](#54-decorator)) are mostly wired through DI. See [9.1](#91-combinations-you-will-meet-in-real-code).
- **Null Object** ([7.7](#77-null-object)) is a common default for optional dependencies.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- dependency-injection
```

1. Register `CheckoutService` as a singleton while `IEmailSender` stays scoped, with `ValidateScopes` and `ValidateOnBuild` both on: read the error.
2. Write a test for the Problem `CheckoutService` that checks the confirmation text. What stops you?
3. Replace the container with the hand-written composition root in the demo. What did you lose?

#### Interview questions

<details>
<summary>What is a captive dependency?</summary>

A longer-lived service holding a shorter-lived one, typically a singleton with a scoped `DbContext`: the scoped object is captured for the lifetime of the singleton and shared across requests and threads.
</details>

<details>
<summary>Why is Service Locator an anti-pattern?</summary>

The dependencies are hidden inside methods instead of declared in the constructor, so you cannot see what a class needs, the compiler cannot help, and missing registrations fail only at run time.
</details>

<details>
<summary>What is the composition root?</summary>

The single place, at the application's entry point, where the object graph is assembled (registrations in `Program.cs`). All other code only receives its dependencies.
</details>

### 7.2 Options

#### Card

| | |
|---|---|
| Relevance | ⭐⭐⭐ Essential |
| Family | Modern |
| Intent | Bind groups of related settings to typed classes, validate them, and inject them where needed. |
| Also known as | Options pattern |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- options` |
| Code | [`src/Patterns.Modern/Options/`](../src/Patterns.Modern/Options/) |

#### The problem

Shipping has settings that operations staff change without a new release: the standard cost (`4.99`), the free-shipping threshold (`50.00`) and the countries the shop ships to (`ES`, `PT`, `FR`). In `appsettings.json` they look like this:

```json
{ "Shipping": { "StandardCost": 4.99, "FreeShippingThreshold": 50.00, "Countries": [ "ES", "PT", "FR" ] } }
```

*Analogy:* a car's dashboard settings. You do not open the engine to change the language or the units; you change a setting, and the car checks the value makes sense before using it.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/Options/0-Problem/):

```csharp
public sealed class ShippingSettings(Dictionary<string, string> raw)
{
    // PAIN: string keys and parsing in every method. A typo ("Shiping:StandardCost") or a bad value
    // fails only when this line runs, maybe in production, maybe weeks after the deployment.
    public decimal StandardCost() => decimal.Parse(raw["Shipping:StandardCost"], CultureInfo.InvariantCulture);
    public decimal Threshold() => decimal.Parse(raw["Shipping:FreeShippingThreshold"], CultureInfo.InvariantCulture);
}
```

#### Structure

```mermaid
classDiagram
    class ShippingOptions {
        <<Options>>
        +decimal StandardCost
        +decimal FreeShippingThreshold
        +IReadOnlyList~string~ Countries
    }
    class IOptions~T~ {
        <<Accessor>>
        +T Value
    }
    class IOptionsMonitor~T~ {
        <<Accessor>>
        +T CurrentValue
        +OnChange(listener)
    }
    class ShippingCalculator {
        <<Consumer>>
    }
    class IConfiguration {
        <<Source>>
    }
    ShippingCalculator --> IOptions~T~
    IOptions~T~ ..> ShippingOptions
    IConfiguration ..> ShippingOptions : bound into
```

| Role | Our class | Responsibility |
|---|---|---|
| Options | `ShippingOptions` | A plain class with one property per setting. |
| Source | `IConfiguration` | Where values come from (JSON files, environment variables, command line, a vault). |
| Accessor | `IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>` | How consumers receive the options, and when they see changes. |
| Consumer | any service | Depends on the accessor, never on `IConfiguration`. |

#### How it runs

```mermaid
sequenceDiagram
    participant Consumer
    participant Options as IOptions of ShippingOptions
    participant Factory as OptionsFactory
    participant Config as IConfiguration
    Consumer->>Options: Value (first time)
    Options->>Factory: Create()
    Factory->>Config: bind section "Shipping"
    Factory->>Factory: run validations
    Note right of Factory: invalid: OptionsValidationException
    Factory-->>Options: ShippingOptions
    Options-->>Consumer: cached instance (same one every time)
```

#### By hand

```csharp
// Role: Options — the shipping settings, typed.
// Guide: §7.2
public sealed class ShippingOptions
{
    public decimal StandardCost { get; set; }
    public decimal FreeShippingThreshold { get; set; }
    public IReadOnlyList<string> Countries { get; set; } = []; // the binder fills read-only list interfaces too
}

// Reads and validates once, at start-up; after that, everybody gets a typed object.
public static class ShippingOptionsReader
{
    public static ShippingOptions Read(IDictionary<string, string> raw)
    {
        var options = new ShippingOptions
        {
            StandardCost = decimal.Parse(raw["Shipping:StandardCost"], CultureInfo.InvariantCulture),
            FreeShippingThreshold = decimal.Parse(raw["Shipping:FreeShippingThreshold"], CultureInfo.InvariantCulture),
            // In index order, by number: as text, "10" sorts before "2".
            Countries = [.. raw.Where(kv => kv.Key.StartsWith("Shipping:Countries:", StringComparison.Ordinal))
                               .OrderBy(kv => int.Parse(kv.Key["Shipping:Countries:".Length..], CultureInfo.InvariantCulture))
                               .Select(kv => kv.Value)],
        };
        if (options.FreeShippingThreshold <= 0) throw new ArgumentException("FreeShippingThreshold must be positive.");
        if (options.Countries.Count == 0) throw new ArgumentException("At least one shipping country is required.");
        return options;
    }
}
```

Parsing happens **once**, errors appear **at start-up**, and the rest of the code receives `ShippingOptions` with real `decimal`s. (Arrays in configuration are keys with an index: `Shipping:Countries:0 = ES`, `…:1 = PT`.)

#### In .NET

`Microsoft.Extensions.Options` does the binding, validation and change tracking ([`2-DotNet/`](../src/Patterns.Modern/Options/2-DotNet/)):

```csharp
// Inside AddShippingOptions(this IServiceCollection services, IConfiguration configuration):
services.AddOptions<ShippingOptions>()
    .Bind(configuration.GetSection("Shipping"))
    .Validate(o => o.FreeShippingThreshold > 0, "FreeShippingThreshold must be positive.")
    .Validate(o => o.Countries.Count > 0, "At least one shipping country is required.")
    .ValidateOnStart(); // with a host: fail at start-up, not at first use

public sealed class ShippingCalculator(IOptions<ShippingOptions> options)
{
    public decimal CostFor(Order order) =>
        order.Total >= options.Value.FreeShippingThreshold ? 0.00m : options.Value.StandardCost;
}
```

`ValidateOnStart` registers the options with an `IStartupValidator` (.NET 8+), which the generic host calls in `IHost.StartAsync`: a bad setting stops the application before the first request. A bare `ServiceProvider` does not call it by itself, so there validation runs when the options are first read, and an invalid value throws `OptionsValidationException` with your message (test `Invalid_FailsWhenRead`); the test `ValidateOnStart_FailsWithoutReadingTheOptions` calls `IStartupValidator.Validate()` the way the host does. For attribute-based rules, `ValidateDataAnnotations()` (package `Microsoft.Extensions.Options.DataAnnotations`) or the `[OptionsValidator]` source generator.

**Which accessor?**

| Accessor | Lifetime | Sees changes in the source? | Use it when |
|---|---|---|---|
| `IOptions<T>` | singleton | **No**: read once, cached forever. | Settings that never change while the app runs (most of them). |
| `IOptionsSnapshot<T>` | scoped | Yes, **per scope**: recomputed once per request, stable inside it. | A request must see one consistent value even if the file changes meanwhile. Cannot be injected into singletons. |
| `IOptionsMonitor<T>` | singleton | Yes, **immediately**: `CurrentValue` is always the latest; `OnChange` notifies you. | Singletons that must react to changes (a background service, a cache size). |

The test `Monitor_SeesTheNewValue_OptionsDoesNot` changes `StandardCost` from 4.99 to 5.99 at run time: `IOptions<T>.Value` still says 4.99, `IOptionsMonitor<T>.CurrentValue` says 5.99. Reloading needs a configuration provider that raises a change token; the demo uses a tiny `SettableConfigurationProvider` whose `Set(key, value)` calls `OnReload()`, because the in-memory provider does not signal changes. `IOptionsMonitor` is an Observer ([6.7](#67-observer)) built on `IChangeToken`.

#### When to use it

- Any group of related settings: bind a section to a class, inject `IOptions<T>`.
- When bad settings must stop the application at start-up, not fail later (`ValidateOnStart`).

#### When NOT to use it

- **A single constant that never changes per environment:** a `const` is simpler.
- **Do not inject `IConfiguration` into business classes** "to read a value": that is the Problem level with extra steps.
- **Do not use `IOptionsSnapshot` or `IOptionsMonitor` "just in case":** most settings are fixed at start-up; `IOptions<T>` is the simplest and fastest.

#### Costs

- One class per settings group, and binding is by name: a renamed property silently stops binding unless validated.
- Three accessors to choose from, with lifetime rules (`IOptionsSnapshot` in a singleton is a captive dependency).

#### Relevance today

⭐⭐⭐ **Essential.** It is how configuration reaches code in every ASP.NET Core and worker application.

#### Relatives

- **Dependency Injection** ([7.1](#71-dependency-injection)): options are injected like any service.
- **Observer** ([6.7](#67-observer)): `IOptionsMonitor<T>.OnChange` and `IChangeToken`.
- **Composite** ([5.3](#53-composite)): the configuration tree the options are bound from.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- options
```

1. Set `FreeShippingThreshold` to `0` and read the options: where does it fail at each level?
2. Rename the property `StandardCost` to `StandardPrice` without changing the configuration. What value do you get? Which validation would catch it?
3. Subscribe to `IOptionsMonitor<ShippingOptions>.OnChange` and change the value twice.

#### Interview questions

<details>
<summary>What is the difference between <code>IOptions</code>, <code>IOptionsSnapshot</code> and <code>IOptionsMonitor</code>?</summary>

`IOptions<T>` is a singleton read once. `IOptionsSnapshot<T>` is scoped and recomputed per scope (per request). `IOptionsMonitor<T>` is a singleton that always returns the current value and notifies changes.
</details>

<details>
<summary>Why validate options at start-up?</summary>

So a wrong setting stops the deployment immediately with a clear message, instead of failing at the first request that happens to read it.
</details>

### 7.3 Repository

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful — and debated (see "When NOT to use it") |
| Family | Modern (from *Patterns of Enterprise Application Architecture*, Martin Fowler, 2002, and Domain-Driven Design) |
| Intent | Mediate between the business code and the data storage through a collection-like interface. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- repository` |
| Code | [`src/Patterns.Modern/Repository/`](../src/Patterns.Modern/Repository/) |

#### The problem

The catalog service needs products: by id, by category, and to add new ones. Where they are stored (a database, an API, memory) is a detail the business code should not care about. In this repository everything runs **in memory**; a `List<Product>` stands in for a database table.

*Analogy:* a library desk. You ask the librarian for "books by this author"; you do not walk into the archive and search the shelves yourself, and the library can reorganise the archive without you noticing.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/Repository/0-Problem/):

```csharp
public sealed class CatalogService(IList<Product> table) // the list stands in for a database table
{
    // PAIN: storage details and queries are repeated inside business methods. Moving to a database
    // means rewriting every method that touches the list.
    public IReadOnlyList<Product> Books() =>
        [.. table.Where(p => p.Category.Name.Equals("Books", StringComparison.OrdinalIgnoreCase))];

    public IReadOnlyList<Product> InCategory(string category) =>
        [.. table.Where(p => p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase))];

    public decimal PriceOf(Guid id) => table.First(p => p.Id == id).Price;

    // PAIN: nothing stops a second product with the same id.
    public void AddProduct(Product product) => table.Add(product);
}
```

#### Structure

```mermaid
classDiagram
    class IProductRepository {
        <<Repository>>
        +GetById(Guid id) Product?
        +ListByCategory(string category) IReadOnlyList~Product~
        +Add(Product product)
    }
    class InMemoryProductRepository {
        <<ConcreteRepository>>
    }
    class CatalogService {
        <<Client>>
    }
    CatalogService --> IProductRepository
    IProductRepository <|.. InMemoryProductRepository
```

| Role | Our class | Responsibility |
|---|---|---|
| Repository | `IProductRepository` | Speaks the language of the domain: "products in this category", not SQL. |
| ConcreteRepository | `InMemoryProductRepository` (an `EfProductRepository` in a real app) | Knows the storage. |
| Client | `CatalogService` | Uses the interface only. |

#### How it runs

```mermaid
sequenceDiagram
    participant Service as CatalogService
    participant Repo as InMemoryProductRepository
    Service->>Repo: ListByCategory("books")
    Note right of Repo: filters its private storage, ignoring case
    Repo-->>Service: [Clean Code]
```

#### By hand

```csharp
// Role: Repository — products, as the business code thinks of them.
// Guide: §7.3
public interface IProductRepository
{
    Product? GetById(Guid id);
    IReadOnlyList<Product> ListByCategory(string category);
    void Add(Product product);
}

// Role: ConcreteRepository — keeps products in memory (a database in a real application).
public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = [];

    public Product? GetById(Guid id) => _products.GetValueOrDefault(id);

    public IReadOnlyList<Product> ListByCategory(string category) =>
        [.. _products.Values.Where(p => p.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase))];

    public void Add(Product product)
    {
        if (!_products.TryAdd(product.Id, product))
            throw new InvalidOperationException($"Product {product.Id} already exists.");
    }
}
```

The repository returns **materialised** lists (`IReadOnlyList<T>`), not lazy queries, so callers cannot accidentally run more queries later (section [6.4](#64-iterator)); the test `ListByCategory_ReturnsAMaterialisedList` adds a book after the call, and the list the caller holds does not change. The client, `CatalogService(IProductRepository)`, offers `InCategory` and `PriceOf` and never sees the dictionary.

#### In .NET

With Entity Framework Core you already have a repository: **`DbSet<T>` is a repository** (a collection-like set of entities, with `Find`, `Add`, `Remove` and LINQ queries) and **`DbContext` is a unit of work** (section [7.4](#74-unit-of-work)). What `DbSet<T>` gives queries is the **`IQueryable<T>`** shape: a query that is *described* in C# and *executed* by the provider. The DotNet level uses exactly that shape over a list, with no database ([`2-DotNet/`](../src/Patterns.Modern/Repository/2-DotNet/)):

```csharp
// Guide: §7.3
public sealed class ProductQueries(IQueryable<Product> products) // a DbSet<Product> in a real app
{
    [SuppressMessage("Performance", "CA1862", Justification = "An IQueryable provider translates a case conversion to SQL, not a StringComparison overload.")]
    public IReadOnlyList<Product> InCategory(string category)
    {
        var wanted = category.ToUpperInvariant();
        return [.. products.Where(p => p.Category.Name.ToUpperInvariant() == wanted)];
    }
}

var queries = new ProductQueries(SampleData.Products.AsQueryable());
```

(A case conversion instead of `StringComparison`: query providers can translate a conversion to SQL, but not a `StringComparison` overload. Over this in-memory list the code uses `ToUpperInvariant()`, the culture-safe form the analyzers require, and silences CA1862, the analyzer that suggests the `StringComparison` overload, with that reason. Against EF Core you would write `ToUpper()`, which it translates to SQL `UPPER`, with a justified suppression of the culture analyzer, or better, rely on the database collation for case-insensitive comparison.)

**The debate: a repository over EF Core?**

| For a repository on top of EF Core | Against |
|---|---|
| Business code speaks domain language (`ListOverdueOrders()`), and the queries live in one place. | `DbSet<T>` already is a repository; another layer often just forwards calls. |
| Easy to replace with an in-memory fake in unit tests. | Fakes behave differently from the database (translation, case, transactions); integration tests against a real database are more honest. |
| Hides EF Core from the domain (useful in Clean or hexagonal architectures). | Generic `IRepository<T>` with `GetAll`/`Find(predicate)` leaks `IQueryable` or loses EF features (includes, projections, tracking). |
| Enforces aggregate boundaries (DDD, Domain-Driven Design; an *aggregate* is a cluster of objects, like an order and its lines, that is loaded and saved as one unit): only load and save whole aggregates. | More code to maintain for every new query. |

A reasonable position: avoid the **generic** repository over EF Core; use **specific** repositories (one per aggregate, with named query methods) when the domain is rich or you need to hide the storage; use `DbContext` directly in simple CRUD (create, read, update, delete) applications.

#### When to use it

- The domain logic is rich and should not know about storage.
- The storage may change, or is not a database (an external API, files).
- You want named queries in one place, per aggregate.

#### When NOT to use it

- **Simple CRUD with EF Core:** use `DbContext` and `DbSet<T>` directly.
- **A generic `IRepository<T>`** that only wraps `DbSet<T>`: it adds a layer and removes features.
- **Repositories returning `IQueryable<T>`:** the storage leaks back out through the query; it is a repository in name only.

#### Costs

- One interface and one class per aggregate, kept in sync with the storage.
- Easy to end up with dozens of `GetByXAndY` methods; a Specification ([7.5](#75-specification)) can help.

#### Relevance today

⭐⭐ **Useful, debated.** You will meet repositories in most enterprise .NET code and in every DDD discussion. Whether to write one over EF Core is a team decision; knowing both sides of the debate is what matters.

#### Relatives

- **Unit of Work** ([7.4](#74-unit-of-work)): repositories collect changes; the unit of work commits them.
- **Specification** ([7.5](#75-specification)): queries passed to a repository as objects.
- **Facade** ([5.5](#55-facade)) and **Adapter** ([5.1](#51-adapter)): a repository is a facade over the storage and often an adapter to it.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- repository
```

1. Add `ListInStock()` to the repository and use it from `CatalogService`; then do the same in the Problem level.
2. Add the same product twice and read the message.
3. Write `ProductQueries.InCategory` with `string.Equals(…, StringComparison.OrdinalIgnoreCase)` and think about what EF Core would do with it.

#### Interview questions

<details>
<summary>Is <code>DbSet&lt;T&gt;</code> a repository?</summary>

Yes, in shape: a collection-like set of entities with add, remove, find and queries, while `DbContext` is the unit of work. That is the main argument against wrapping it in another generic repository.
</details>

<details>
<summary>Why is returning <code>IQueryable&lt;T&gt;</code> from a repository discouraged?</summary>

The caller can compose any query, so the storage details and the query logic leak out of the repository, and the repository no longer controls what runs against the database.
</details>

### 7.4 Unit of Work

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Modern (Fowler, 2002) |
| Intent | Keep track of the changes made during a business operation and commit them together, all or nothing. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- unit-of-work` |
| Code | [`src/Patterns.Modern/UnitOfWork/`](../src/Patterns.Modern/UnitOfWork/) |

#### The problem

Confirming an order means two changes: **save the order** and **decrease the stock**. Both must happen, or neither. If the order is saved and the stock update fails, the shop has sold something it does not have. The examples use an in-memory `InMemoryShop` with a stock table and an order list.

*Analogy:* a supermarket checkout. The cashier scans everything first; you pay once at the end. If your card is declined, nothing leaves the shop: you do not take home half of the shopping.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/UnitOfWork/0-Problem/):

```csharp
// PAIN: each repository writes immediately. The order is saved and the books are taken, then the mug
// is out of stock, and the shop is left with half-done work: an order and stock movements for a sale that failed.
orderRepository.Save(order);                     // order: 2 books and 1 mug
foreach (var line in order.Lines)
    stockRepository.Decrease(line.Product.Id, line.Quantity); // books: 20 → 18; mug: throws
```

The test `Problem_LeavesHalfDoneWork` shows the order in the store, and 18 books, after the failure.

#### Structure

```mermaid
classDiagram
    class UnitOfWork {
        <<UnitOfWork>>
        -List~Order~ _newOrders
        -List~(Guid, int)~ _stockChanges
        +RegisterOrder(Order order)
        +DecreaseStock(Guid productId, int units)
        +Commit()
    }
    class InMemoryShop {
        <<Store>>
        +Dictionary Stock
        +IReadOnlyList~Order~ Orders
        +AddOrder(Order order)
    }
    class CheckoutService {
        <<Client>>
    }
    CheckoutService --> UnitOfWork
    UnitOfWork --> InMemoryShop : applies on Commit
```

| Role | Our class | Responsibility |
|---|---|---|
| UnitOfWork | `UnitOfWork` | Records the intended changes; `Commit` validates all of them, then applies all of them. |
| Store | `InMemoryShop` | The data; changed only by `Commit`. |
| Client | the checkout | Registers changes during the operation, commits once at the end. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant UoW as UnitOfWork
    participant Shop as InMemoryShop
    Client->>UoW: RegisterOrder(order)
    Client->>UoW: DecreaseStock(book, 2)
    Note right of UoW: nothing written yet
    Client->>UoW: Commit()
    UoW->>Shop: check every change (stock is enough?)
    alt all valid
        UoW->>Shop: add order, decrease stock
    else any invalid
        UoW-->>Client: InvalidOperationException, nothing applied
    end
```

#### By hand

```csharp
// Role: UnitOfWork — collects changes and applies them together, or not at all.
// Guide: §7.4
public sealed class UnitOfWork(InMemoryShop shop)
{
    private readonly List<Order> _newOrders = [];
    private readonly List<(Guid ProductId, int Units)> _stockChanges = [];

    public void RegisterOrder(Order order) => _newOrders.Add(order);
    public void DecreaseStock(Guid productId, int units)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(units); // a negative decrease would add stock
        _stockChanges.Add((productId, units));
    }

    public void Commit()
    {
        // 1. Validate everything first, per product: two changes that each fit may not fit together…
        foreach (var change in _stockChanges.GroupBy(c => c.ProductId))
            if (!shop.Stock.TryGetValue(change.Key, out var inStock) || inStock < change.Sum(c => c.Units))
                throw new InvalidOperationException($"Not enough stock for {change.Key}.");

        // 2. …then apply everything. Nothing above wrote anything, so a failure leaves the shop untouched.
        foreach (var (productId, units) in _stockChanges) shop.Stock[productId] -= units;
        foreach (var order in _newOrders) shop.AddOrder(order);
        _newOrders.Clear();
        _stockChanges.Clear();
    }
}
```

"Nothing above wrote anything" holds only if validation checks exactly what applying needs: an unknown product fails validation (`TryGetValue`, not a default of 0 that the indexer would then reject half-way through applying), and zero or negative units are refused when registered (tests `Commit_WithUnknownProduct_AppliesNothing` and `DecreaseStock_RejectsZeroOrNegativeUnits`).

The client, `CheckoutService(InMemoryShop)`, creates one `UnitOfWork` per `Confirm`, registers the order and one stock decrease per line, and commits once. The test `Classic_ChecksTheSumOfChangesToOneProduct` decreases the books by 15 and by 10: each fits the 20 in stock, together they do not, and nothing is applied.

In memory, "validate then apply" is enough. Against a database, the unit of work wraps the writes in a **transaction**: a group of operations the database guarantees to apply completely or not at all.

#### In .NET

**`DbContext` is the everyday unit of work.** Its *change tracker* records every entity you add, modify or remove; nothing is written until `SaveChanges()`, which sends all the changes in **one transaction** by default:

```csharp
db.Orders.Add(order);                      // tracked, not written
db.StockItems.Find(bookId)!.Units -= 2;    // tracked, not written (an EF entity is usually a mutable class, unlike our Product record)
await db.SaveChangesAsync();               // one transaction: both or neither
```

That is why `DbContext` is registered as **scoped** (one per request: one unit of work per operation).

**`System.Transactions`** is the BCL's general mechanism ([`2-DotNet/`](../src/Patterns.Modern/UnitOfWork/2-DotNet/)). A `TransactionScope` creates an *ambient* transaction (available to any code that runs inside it, through `Transaction.Current`); resources **enlist** in it and are told to commit or roll back at the end. The DotNet level writes a small in-memory resource that does so:

```csharp
// Guide: §7.4
public sealed class TransactionalShop(InMemoryShop shop) : IEnlistmentNotification
{
    private readonly List<Order> _newOrders = [];
    private readonly List<(Guid ProductId, int Units)> _stockChanges = [];
    private Transaction? _enlistedIn; // the one transaction whose changes this object holds

    public void RegisterOrder(Order order) { EnlistOnce(); _newOrders.Add(order); }
    public void DecreaseStock(Guid productId, int units) { /* units > 0, as in Classic */ EnlistOnce(); _stockChanges.Add((productId, units)); }

    public void Prepare(PreparingEnlistment enlistment) // phase 1: can we commit?
    {
        var enough = _stockChanges.GroupBy(c => c.ProductId)
            .All(change => shop.Stock.TryGetValue(change.Key, out var inStock) && inStock >= change.Sum(c => c.Units));
        if (enough) enlistment.Prepared();
        else { Discard(); enlistment.ForceRollback(); }
    }

    public void Commit(Enlistment enlistment)            // phase 2: apply
    {
        foreach (var (id, units) in _stockChanges) shop.Stock[id] -= units;
        foreach (var order in _newOrders) shop.AddOrder(order);
        Discard();
        enlistment.Done();
    }

    public void Rollback(Enlistment enlistment) { Discard(); enlistment.Done(); }
    public void InDoubt(Enlistment enlistment) { Discard(); enlistment.Done(); }

    private void EnlistOnce()
    {
        var transaction = Transaction.Current
            ?? throw new InvalidOperationException("TransactionalShop must be used inside a TransactionScope.");
        if (_enlistedIn is null) { transaction.EnlistVolatile(this, EnlistmentOptions.None); _enlistedIn = transaction; }
        else if (!_enlistedIn.Equals(transaction))
            throw new InvalidOperationException("TransactionalShop already holds changes of another transaction.");
    }

    private void Discard() { _newOrders.Clear(); _stockChanges.Clear(); _enlistedIn = null; }
}

var txShop = new TransactionalShop(shop);
using (var scope = new TransactionScope())
{
    txShop.RegisterOrder(order);
    txShop.DecreaseStock(SampleData.Book.Id, 2);
    scope.Complete(); // without this line, disposing the scope rolls everything back
}
// If Prepare calls ForceRollback (not enough stock), disposing the scope throws TransactionAbortedException.
```

The object holds the changes of **one transaction at a time**: a nested `TransactionScope(TransactionScopeOption.RequiresNew)` that used it while the outer one had pending changes would otherwise lose its own, so it throws (test `DotNet_ChangesInAnotherTransaction_Throw`). `Rollback` and `InDoubt` discard the pending changes, so the same object works in the next scope (`DotNet_ScopeWithoutComplete_RollsBack` reuses it).

The two methods `Prepare` and `Commit` are the **two-phase commit** protocol: every resource first says whether it *can* commit, and only if all agree does anyone commit. Notes: with `async` code, create the scope with `TransactionScopeAsyncFlowOption.Enabled` or the ambient transaction does not flow across `await`; a scope that spans two databases needs a distributed transaction, which on modern .NET is supported only on Windows and must be enabled explicitly.

#### When to use it

- A business operation changes several things that must stay consistent.
- You want to collect changes during an operation and decide at the end whether to keep them.
- With EF Core: you already do, every time you call `SaveChanges` once per operation.

#### When NOT to use it

- **A single change:** one write is already atomic.
- **A hand-written `IUnitOfWork` over EF Core** that only calls `SaveChanges`: `DbContext` is the unit of work already.
- **Changes across services or a message broker:** a local transaction cannot cover them; that needs patterns like the outbox or a saga (out of scope, [9.4](#94-out-of-scope)).

#### Costs

- Changes are deferred, so errors appear at commit time, far from the code that registered them.
- Long units of work hold locks (in a database) and memory.

#### Relevance today

⭐⭐ **Useful.** You use it in every EF Core application through `DbContext`; writing one by hand is rare. Understanding it explains `SaveChanges`, scoped lifetimes and transaction boundaries.

#### Relatives

- **Repository** ([7.3](#73-repository)): repositories register changes; the unit of work commits them.
- **Command** ([6.2](#62-command)) and **Memento** ([6.6](#66-memento)): the other ways to make an operation reversible.
- **Facade** ([5.5](#55-facade)): an application service often owns one unit of work per operation.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- unit-of-work
```

1. Register an order and a stock decrease for the out-of-stock mug, commit, and check that the order list is still empty.
2. Remove `scope.Complete()` and check the stock after the `using` block.
3. Add `await Task.Yield()` inside the scope without `TransactionScopeAsyncFlowOption.Enabled` and see what `Transaction.Current` is after it.

#### Interview questions

<details>
<summary>How is <code>DbContext</code> a unit of work?</summary>

Its change tracker records added, modified and removed entities, and `SaveChanges` writes all of them in one database transaction. That is why it is scoped to one operation or request.
</details>

<details>
<summary>What happens if a <code>TransactionScope</code> is disposed without <code>Complete()</code>?</summary>

The transaction rolls back: every enlisted resource is told to discard its changes.
</details>

### 7.5 Specification

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Modern (Eric Evans and Martin Fowler; Domain-Driven Design) |
| Intent | Encapsulate a business rule as an object that answers "does this candidate satisfy me?", and combine rules with and, or and not. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- specification` |
| Code | [`src/Patterns.Modern/Specification/`](../src/Patterns.Modern/Specification/) |

#### The problem

Marketing asks for product lists: "in stock and cheaper than 20", "out of stock", "books or home products", and new combinations every week. The rules are always built from the same pieces: in stock (`Stock > 0`), in a category (ignoring case), cheaper than an amount.

With the sample data: in stock and cheaper than 20 → `[Clean Code]`; not in stock → `[Coffee Mug]`; books or home → `[Clean Code, Coffee Mug]`.

*Analogy:* the filters of an online shop's search page. Each checkbox is one rule; you tick several and they combine. Nobody writes a new page for each combination.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/Specification/0-Problem/):

```csharp
public sealed class ProductFinder(IReadOnlyList<Product> products)
{
    // PAIN: one method per combination. Three rules already allow dozens of combinations, and each method
    // repeats the definition of "in stock" or "in a category"; next week marketing asks for another one.
    public IReadOnlyList<Product> FindInStockBooks() =>
        [.. products.Where(p => p.Stock > 0 && p.Category.Name.Equals("Books", StringComparison.OrdinalIgnoreCase))];

    public IReadOnlyList<Product> FindCheapInStock(decimal limit) =>
        [.. products.Where(p => p.Stock > 0 && p.Price < limit)];

    // …and FindOutOfStock(), FindBooksOrHome(), with the next combination already on its way.
}
```

#### Structure

```mermaid
classDiagram
    class Specification~T~ {
        <<Specification>>
        +IsSatisfiedBy(T candidate)* bool
        +And(Specification~T~ other) Specification~T~
        +Or(Specification~T~ other) Specification~T~
        +Not() Specification~T~
    }
    class InStock {
        <<LeafSpecification>>
    }
    class CheaperThan {
        <<LeafSpecification>>
    }
    class AndSpecification {
        <<CompositeSpecification>>
    }
    Specification~T~ <|-- InStock
    Specification~T~ <|-- CheaperThan
    Specification~T~ <|-- AndSpecification
    AndSpecification o-- Specification~T~ : left, right
```

| Role | Our class | Responsibility |
|---|---|---|
| Specification | `Specification<T>` | `IsSatisfiedBy(candidate)` plus the combinators. |
| Leaf specification | `InStock`, `InCategory`, `CheaperThan` | One business rule each, with a name. |
| Composite specification | `AndSpecification`, `OrSpecification`, `NotSpecification` | Combine other specifications (a Composite, [5.3](#53-composite), and a small Interpreter, [6.3](#63-interpreter)). |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant AndSpec as AndSpecification
    participant InStock
    participant Cheap as CheaperThan(20)
    Client->>AndSpec: IsSatisfiedBy(Clean Code)
    AndSpec->>InStock: IsSatisfiedBy(Clean Code)
    InStock-->>AndSpec: true (stock 20)
    AndSpec->>Cheap: IsSatisfiedBy(Clean Code)
    Cheap-->>AndSpec: true (12.50)
    AndSpec-->>Client: true
```

#### By hand

```csharp
// Role: Specification — a business rule about T, combinable with others.
// Guide: §7.5
public abstract class Specification<T>
{
    public abstract bool IsSatisfiedBy(T candidate);

    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);
    public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);
    public Specification<T> Not() => new NotSpecification<T>(this);
}

// Role: Leaf specification — "there is at least one unit".
public sealed class InStock : Specification<Product>
{
    public override bool IsSatisfiedBy(Product candidate) => candidate.Stock > 0;
}

// Role: Composite specification — both rules hold.
public sealed class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override bool IsSatisfiedBy(T candidate) => left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate);
}

var cheapInStock = new InStock().And(new CheaperThan(20m));
var result = SampleData.Products.Where(cheapInStock.IsSatisfiedBy).ToList(); // [Clean Code]
```

Each rule is defined **once**, has a name in the domain language, and can be tested alone. New lists are new combinations, not new methods. The same specification can also *validate* ("can this product be put on sale?") and *select* (filter a list).

#### In .NET

The Classic version only works **in memory**: `IsSatisfiedBy` is compiled C#, so a database cannot run it. To filter in the database, the rule must be an **expression tree** (section [6.3](#63-interpreter)) that a LINQ provider like EF Core can translate to SQL ([`2-DotNet/`](../src/Patterns.Modern/Specification/2-DotNet/)):

```csharp
// Guide: §7.5
public abstract class ExpressionSpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public ExpressionSpecification<T> And(ExpressionSpecification<T> other) =>
        new CombinedSpecification<T>(this, other, Expression.AndAlso);

    public ExpressionSpecification<T> Or(ExpressionSpecification<T> other) =>
        new CombinedSpecification<T>(this, other, Expression.OrElse);

    public ExpressionSpecification<T> Not() => new NotExpressionSpecification<T>(this);
}

public sealed class CheaperThanExpression(decimal limit) : ExpressionSpecification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression() => p => p.Price < limit;
}

// Usable directly in a query: EF Core turns it into "WHERE Stock > 0 AND Price < @limit" (the captured value becomes a SQL parameter).
var cheapInStock = new InStockExpression().And(new CheaperThanExpression(20m));
var result = products.AsQueryable().Where(cheapInStock.ToExpression()).ToList();
```

Combining two expressions is the subtle part. Each lambda has its **own parameter** (`p` in one, another `p` in the other), and `Expression.AndAlso(left.Body, right.Body)` would mix two different parameters in one lambda. Building it works; it fails later, when you `Compile()` it ("variable 'p' … referenced from scope '', but it is not defined") or when EF Core tries to translate it. `And` and `Or` build a `CombinedSpecification<T>` that uses a tiny `ExpressionVisitor` ([6.11](#611-visitor)), `ParameterReplacer`, to **replace** the right lambda's parameter with the left one's, then joins the bodies with `AndAlso` or `OrElse` under a single parameter:

```csharp
var rightBody = new ParameterReplacer(rightLambda.Parameters[0], leftLambda.Parameters[0]).Visit(rightLambda.Body);
return Expression.Lambda<Func<T, bool>>(join(leftLambda.Body, rightBody), leftLambda.Parameters);

internal sealed class ParameterReplacer(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node) => node == from ? to : node;
}
```

The test `JoiningBodies_WithoutReplacingTheParameter_FailsToCompile` builds the naive version and watches `Compile()` throw; `DotNet_CombinedExpression_HasOneParameter` checks the real one. `InCategoryExpression` compares with a case conversion, for the reason given in [7.3](#73-repository).

#### In the ecosystem

**Ardalis.Specification** (MIT) is a popular implementation for EF Core: specifications that hold criteria plus includes, ordering and paging, and a repository base class that applies them.

#### When to use it

- The same business rules are reused in many queries and combinations.
- Rules must be named and tested on their own, or used both to validate and to select.
- Rules must run in the database (expression-based specifications).

#### When NOT to use it

- **A few fixed queries:** a LINQ `Where` in the method that needs it is clearer.
- **Rules used once:** a named private method or a static `Expression<Func<T, bool>>` field is enough.
- **To build a generic query language** for every screen: it becomes a home-made, less capable LINQ.

#### Costs

- More classes; combining expression trees needs care (parameters, translatability).
- Debugging a combined expression is harder than reading a `Where`.

#### Relevance today

⭐⭐ **Useful.** Common in DDD codebases and with Ardalis.Specification; in simpler code, LINQ with a few reusable `Expression` fields covers most needs.

#### Relatives

- **Composite** ([5.3](#53-composite)) and **Interpreter** ([6.3](#63-interpreter)): `And`/`Or`/`Not` form a tree that is evaluated.
- **Repository** ([7.3](#73-repository)): a repository can accept specifications instead of growing a method per query.
- **Strategy** ([6.9](#69-strategy)): a specification is a strategy that answers a yes/no question.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- specification
```

1. Combine `InCategory("books").Or(InCategory("home")).And(new InStock())` and predict the result first.
2. Try `expression.Compile()` on a combined expression without the parameter replacement and read the error.
3. Add an `OnSale` rule (price below 10) and use it in two combinations.

#### Interview questions

<details>
<summary>Why do Specification implementations for EF Core use expression trees?</summary>

EF Core translates expression trees into SQL. A compiled `Func<T, bool>` or a method like `IsSatisfiedBy` is opaque code that can only run in memory, after loading the rows.
</details>

<details>
<summary>What is tricky about combining two <code>Expression&lt;Func&lt;T, bool&gt;&gt;</code>?</summary>

Each lambda has its own parameter object. The bodies must be rewritten to share one parameter (with an `ExpressionVisitor`) before joining them with `AndAlso` or `OrElse`.
</details>

### 7.6 Result

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Modern (from functional programming) |
| Intent | Return the outcome of an operation that can fail in an expected way as a value (success or error), instead of throwing an exception. |
| Also known as | Either, Outcome, Try pattern (its BCL form) |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- result` |
| Code | [`src/Patterns.Modern/Result/`](../src/Patterns.Modern/Result/) |

#### The problem

Reserving stock can fail for **ordinary business reasons**: asking for 21 books when there are 20 left, or a quantity of zero. These are not bugs; they happen every day and the caller must handle them (show a message, suggest a smaller quantity). The errors have a code and a message: `stock.insufficient` → `"Only 20 left of Clean Code."`, `quantity.invalid`.

*Analogy:* a vending machine. When a product is sold out, it shows "sold out" and returns your coin. It does not shut down and call the technician: a sold-out product is an expected outcome, not a breakdown.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/Result/0-Problem/):

```csharp
public sealed class StockService
{
    // PAIN: the signature says "returns a Reservation"; nothing says it can fail, and callers learn
    // about InsufficientStockException by reading the body or by crashing.
    public Reservation Reserve(Product product, int quantity) =>
        product.Stock >= quantity
            ? new Reservation(product.Id, quantity)
            : throw new InsufficientStockException(product, quantity);
}

try { var reservation = stock.Reserve(book, 21); /* … */ }
catch (InsufficientStockException ex) { /* PAIN: exceptions used for normal control flow */ }
```

Exceptions for expected outcomes hide the failure from the signature, turn ordinary branches into `try`/`catch`, and are slow when they happen often (an exception captures a stack trace).

#### Structure

```mermaid
classDiagram
    class Result~T~ {
        <<Result>>
        +bool IsSuccess
        +T Value
        +BusinessError Error
        +Map(Func mapper) Result
        +Bind(Func next) Result
        +Match(Func onSuccess, Func onFailure) TOut
    }
    class Result {
        <<Factory>>
        +Success(T value)$ Result~T~
        +Failure(BusinessError error)$ Result~T~
    }
    class BusinessError {
        +string Code
        +string Message
    }
    class StockService {
        +Reserve(Product product, int quantity)$ Result~Reservation~
    }
    Result~T~ --> BusinessError
    Result ..> Result~T~ : creates
    StockService ..> Result~T~ : returns
```

| Role | Our type | Responsibility |
|---|---|---|
| Result | `Result<T>` | Holds either a value or an error, never both. Reading the wrong one throws. |
| Factory | `Result` (static, non-generic) | `Result.Success(value)` and `Result.Failure<T>(error)` create results. |
| Error | `BusinessError(Code, Message)` | A machine-readable code and a human message. |
| Operation | `StockService.Reserve` | Its return type says it can fail. |

(The error type is `BusinessError`, not `Error`: `Error` is a Visual Basic keyword, which analyzer CA1716 rejects as a type name. The factory methods live in a separate non-generic `Result` class because CA1000 rejects static members on a generic type: they are awkward to call, `Result<Reservation>.Success(…)`, while `Result.Success(reservation)` infers `T`. One wrinkle of this repository's layout: the folder's namespace is also called `Result` (`Patterns.Modern.Result`), so code outside `…Result.Classic` writes `Classic.Result.Success(…)`.)

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Service as StockService
    Client->>Service: Reserve(book, 21)
    Note right of Service: stock is 20: an expected failure
    Service-->>Client: Failure(stock.insufficient)
    Client->>Client: Match(ok: confirm, error: show message)
```

#### By hand

```csharp
public sealed record BusinessError(string Code, string Message);

public static class Result
{
    public static Result<T> Success<T>(T value) => new(value, null);
    public static Result<T> Failure<T>(BusinessError error) => new(default!, error);
}

// Role: Result — a success with a value, or a failure with an error.
// Guide: §7.6
public readonly struct Result<T>
{
    private readonly T _value;
    private readonly BusinessError? _error;

    internal Result(T value, BusinessError? error) => (_value, _error) = (value, error);

    public bool IsSuccess => _error is null;
    public T Value => IsSuccess ? _value : throw new InvalidOperationException("A failed result has no value.");
    public BusinessError Error => _error ?? throw new InvalidOperationException("A successful result has no error.");

    // Transform the value if there is one; pass the error through untouched.
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper) =>
        IsSuccess ? Result.Success(mapper(_value)) : Result.Failure<TOut>(_error!);

    // Chain another operation that can fail; the first failure stops the chain.
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> next) =>
        IsSuccess ? next(_value) : Result.Failure<TOut>(_error!);

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<BusinessError, TOut> onFailure) =>
        IsSuccess ? onSuccess(_value) : onFailure(_error!);
}

// In the static class StockService (it keeps no state, so CA1822 asks for static, like int.TryParse):
public static Result<Reservation> Reserve(Product product, int quantity) =>
    quantity < 1 ? Result.Failure<Reservation>(new("quantity.invalid", "Quantity must be at least 1."))
    : product.Stock < quantity
        ? Result.Failure<Reservation>(new("stock.insufficient", $"Only {product.Stock} left of {product.Name}."))
        : Result.Success(new Reservation(product.Id, quantity));

var message = StockService.Reserve(SampleData.Book, 21).Match(
    onSuccess: r => $"Reserved {r.Quantity}",
    onFailure: e => e.Message); // "Only 20 left of Clean Code."
```

One trap of making `Result<T>` a `struct`: `default(Result<T>)` has no error, so it looks like a success with a default value. Never create one with `default` or `new()`, only through `Result.Success` and `Result.Failure`. The compiler cannot stop you: every struct has `default` and a public parameterless `new()`, and making the two-argument constructor `internal` hides only that one. If the trap matters in your code, store a `bool _isSuccess` field instead of deriving success from the missing error: then `default` is a failure, which is the safer mistake.

The signature now says the operation can fail, the caller handles both outcomes with ordinary code, and `Bind` chains several steps that stop at the first failure (test `Bind_StopsAtFirstFailure`).

**Exceptions or results?**

| Use an exception when… | Use a result when… |
|---|---|
| Something **unexpected** happened: a bug, a broken invariant, the database is down. | The failure is an **expected business outcome**: out of stock, invalid input, not found. |
| The caller usually cannot do anything useful about it, and it should travel up to a global handler. | The immediate caller must decide what to do (show it, try something else). |
| It is rare. | It may happen on every request. |

Both coexist in good code: results for business outcomes, exceptions for everything else.

#### In .NET

The BCL's own result pattern is **`TryXxx`**: `int.TryParse`, `Dictionary.TryGetValue`, `TryPop`. A `bool` says whether it worked, and an `out` parameter carries the value ([`2-DotNet/`](../src/Patterns.Modern/Result/2-DotNet/)):

```csharp
// Guide: §7.6
public static bool TryReserve(Product product, int quantity, [NotNullWhen(true)] out Reservation? reservation)
{
    reservation = quantity >= 1 && product.Stock >= quantity ? new Reservation(product.Id, quantity) : null;
    return reservation is not null;
}

if (StockService.TryReserve(SampleData.Book, 2, out var reservation))
    Confirm(reservation); // the compiler knows reservation is not null here
```

`[NotNullWhen(true)]` tells the nullable analysis that the `out` value is not null when the method returns `true`. What `TryXxx` cannot do is say **why** it failed; when the reason matters, return a result type.

In ASP.NET Core minimal APIs, **`TypedResults`** with `Results<Ok<T>, NotFound, ValidationProblem>` lets an endpoint return one of several typed HTTP outcomes, which is the same idea at the HTTP boundary.

#### In the ecosystem

**FluentResults** (MIT) and **ErrorOr** (MIT) are popular result types with error lists, metadata and helpers. C# has no built-in discriminated union (a type that is exactly one of a fixed set of cases, such as "success with a value" or "failure with an error") yet (the C# team has been designing union types; check what your C# version offers), which is why these libraries, and hand-written types like the one above, exist.

#### When to use it

- Expected failures that the caller must handle: validation, business rules, "not found".
- Pipelines of steps where the first failure should stop the rest (`Bind`).
- Hot paths where exceptions would be thrown often.

#### When NOT to use it

- **Unexpected errors:** wrapping every exception in a result hides bugs and forces every caller to check. Let them throw.
- **When the reason does not matter:** a `TryXxx` method or a nullable return is simpler.
- **Only half of the codebase uses results:** mixing both styles for the same kind of failure is worse than either. Agree on a rule.

#### Costs

- Every caller must check or propagate; without language support, the code is more verbose than `throw`.
- A result can be ignored silently (unlike an exception), unless analyzers enforce using it.

#### Relevance today

⭐⭐ **Useful.** Increasingly common in .NET APIs and domain code; `TryXxx` is everywhere in the BCL. Knowing when to throw and when to return is a core design skill.

#### Relatives

- **Null Object** ([7.7](#77-null-object)): another way to avoid special cases at the call site, for "no value" rather than "failed".
- **Chain of Responsibility** ([6.1](#61-chain-of-responsibility)): the validation chain returns an `OrderCheck`, a small result type.
- **Specification** ([7.5](#75-specification)) can produce the errors a result carries.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- result
```

1. Chain `Reserve` and a `Charge` step with `Bind`; make the first fail and check the second never runs.
2. Read `.Value` on a failed result and read the message.
3. Measure 100,000 failed reservations with exceptions and with results.

#### Interview questions

<details>
<summary>When should a method throw instead of returning a result?</summary>

When the failure is unexpected (a bug, an unavailable resource) and the immediate caller cannot handle it meaningfully. Expected business outcomes are better returned as results.
</details>

<details>
<summary>What does <code>Bind</code> do on a result?</summary>

If the result is a success, it runs the next operation (which itself returns a result); if it is a failure, it skips the operation and passes the error on. It chains fallible steps.
</details>

### 7.7 Null Object

#### Card

| | |
|---|---|
| Relevance | ⭐⭐ Useful |
| Family | Modern (Bobby Woolf, *Pattern Languages of Program Design 3*, 1997) |
| Intent | Provide an object with neutral, do-nothing behaviour to use instead of `null`. |
| Also known as | — |
| Levels | Problem · Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- null-object` |
| Code | [`src/Patterns.Modern/NullObject/`](../src/Patterns.Modern/NullObject/) |

#### The problem

The price service may apply a discount, and may log. Many orders have **no** discount; tests and small tools have **no** logger. Greeting a customer is similar: registered customers have a name, guests do not. Every "maybe there is none" becomes a `null` check.

Examples: 100.00 with no discount stays 100.00; with 10 % off it is 90.00. A registered customer is greeted `"Hello, Ana"`, a guest `"Hello, guest"`.

*Analogy:* a placeholder in a seating plan: an empty chair with a "reserved" card. Everyone can treat it like any other seat (count it, walk around it) without checking whether a person is there.

#### Without the pattern

From [`0-Problem/`](../src/Patterns.Modern/NullObject/0-Problem/):

```csharp
public sealed partial class PriceService(IDiscount? discount, ILogger? logger)
{
    public decimal PriceOf(decimal amount)
    {
        // PAIN: a null check at every use. Forget one and it is a NullReferenceException.
        var price = discount is not null ? discount.Apply(amount) : amount;
        if (logger is not null) LogPriced(logger, amount, price); // a [LoggerMessage] method, as in the .NET level
        return price;
    }
}

// PAIN: every place that shows the customer's name repeats this branch.
public static string GreetingFor(Customer customer) => customer.IsGuest ? "Hello, guest" : $"Hello, {customer.Name}";
```

Nullable reference types (section [2.2](#22-root-build-files)) make the compiler warn about forgotten checks, which helps; Null Object removes the checks altogether.

#### Structure

```mermaid
classDiagram
    class IDiscount {
        <<AbstractObject>>
        +Apply(decimal amount) decimal
    }
    class PercentageDiscount {
        <<RealObject>>
    }
    class NoDiscount {
        <<NullObject>>
        +NoDiscount Instance$
    }
    class PriceService {
        <<Client>>
    }
    PriceService --> IDiscount
    IDiscount <|.. PercentageDiscount
    IDiscount <|.. NoDiscount
```

| Role | Our class | Responsibility |
|---|---|---|
| AbstractObject | `IDiscount` | The interface clients use. |
| RealObject | `PercentageDiscount` | Does the real work. |
| NullObject | `NoDiscount` | Implements the interface by doing nothing (returns the amount unchanged). Usually one shared instance. |
| Client | `PriceService` | Always has a discount; never checks for `null`. |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Service as PriceService
    participant Discount as NoDiscount.Instance
    Client->>Service: PriceOf(100.00)
    Service->>Discount: Apply(100.00)
    Note right of Discount: does nothing
    Discount-->>Service: 100.00
    Service-->>Client: 100.00
```

#### By hand

```csharp
// Role: NullObject — a discount that discounts nothing.
// Guide: §7.7
public sealed class NoDiscount : IDiscount
{
    public static NoDiscount Instance { get; } = new(); // stateless: one shared instance is enough
    private NoDiscount() { }
    public decimal Apply(decimal amount) => amount;
}

// Role: Client — always has a discount object, so it never checks for null.
public sealed class PriceService(IDiscount discount)
{
    public decimal PriceOf(decimal amount) => discount.Apply(amount);
}

// The same idea for customers: CustomerProfiles.For(customer) decides once (GuestCustomer.Instance or a
// RegisteredCustomer), and the greeting has no "if guest" branch.
public static string GreetingFor(ICustomerProfile profile) => $"Hello, {profile.DisplayName}"; // GuestCustomer.DisplayName == "guest"
```

`new PriceService(NoDiscount.Instance).PriceOf(100.00m)` is 100.00; with `new PercentageDiscount(10)` it is 90.00. The decision "is there a discount?" is made **once**, where the service is created, not at every use.

#### In .NET

**`NullLogger<T>.Instance`** (package `Microsoft.Extensions.Logging.Abstractions`) is the framework's null object for logging. A common idiom makes the logger optional without null checks ([`2-DotNet/`](../src/Patterns.Modern/NullObject/2-DotNet/)):

```csharp
// Guide: §7.7
public sealed partial class PriceService(IDiscount discount, ILogger<PriceService>? logger = null)
{
    private readonly ILogger<PriceService> _logger = logger ?? NullLogger<PriceService>.Instance;

    public decimal PriceOf(decimal amount)
    {
        var price = discount.Apply(amount);
        LogPriced(_logger, amount, price); // no null check
        return price;
    }

    // [LoggerMessage] generates a fast, allocation-free logging method at compile time (hence `partial`).
    [LoggerMessage(Level = LogLevel.Information, Message = "Priced {Amount} at {Price}")]
    private static partial void LogPriced(ILogger logger, decimal amount, decimal price);
}
```

Other null objects in the BCL: `Stream.Null` and `TextWriter.Null` (accept writes and discard them), `Enumerable.Empty<T>()` and `Array.Empty<T>()` (instead of returning `null` collections), `CancellationToken.None`, `Task.CompletedTask`, `NullLoggerFactory.Instance`.

#### When to use it

- An optional collaborator (logger, discount, notifier) whose absence means "do nothing".
- Methods that would return `null` for "nothing": return an empty collection or a neutral object instead.

#### When NOT to use it

- **When "absent" must be handled differently** (show "no discount available", ask the user): a null object would hide a case that matters. Use `null` with nullable reference types, or a Result ([7.6](#76-result)).
- **When doing nothing is wrong:** a null `IPaymentGateway` that silently "succeeds" is a bug factory.
- **Objects with return values that have no neutral answer** (what does a null repository return for `GetById`?).

#### Costs

- One more class per interface; it must be kept in sync with the interface.
- It can hide configuration errors: a missing logger goes unnoticed.

#### Relevance today

⭐⭐ **Useful.** Small and everywhere: `NullLogger`, empty collections, `Stream.Null`. Nullable reference types reduced the need, but returning empty instead of `null` is still the better default.

#### Relatives

- **Strategy** ([6.9](#69-strategy)): a null object is often the "do nothing" strategy.
- **Singleton** ([4.1](#41-singleton)): null objects are stateless, so one shared instance is enough.
- **Result** ([7.6](#76-result)): when "nothing" carries a reason, a result is the better tool.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- null-object
```

1. Remove one null check in the Problem `PriceService` and pass `null`: what happens?
2. Pass `NullLogger<PriceService>.Instance` explicitly and then nothing at all; compare.
3. Write a method that returns `IReadOnlyList<Order>` and returns `null` when there are none; refactor it to `[]` and see what changes at the call sites.

#### Interview questions

<details>
<summary>What is a Null Object?</summary>

An implementation of an interface that does nothing (or returns a neutral value), used instead of `null` so that clients can call it without checks.
</details>

<details>
<summary>Give examples of null objects in .NET.</summary>

`NullLogger<T>.Instance`, `Stream.Null`, `TextWriter.Null`, `Enumerable.Empty<T>()`, `CancellationToken.None`.
</details>

### 7.8 Object Pool

#### Card

| | |
|---|---|
| Relevance | ⭐ Niche — measure first |
| Family | Modern (creational in spirit) |
| Intent | Reuse objects that are expensive to create by keeping them in a pool, lending them out and taking them back. |
| Also known as | Resource pool |
| Levels | Classic · .NET |
| Run | `dotnet run --project src/Patterns.Runner -- object-pool` |
| Code | [`src/Patterns.Modern/ObjectPool/`](../src/Patterns.Modern/ObjectPool/) |

#### The problem

Generating invoices builds a lot of text and bytes. Under heavy load, the shop creates thousands of large `StringBuilder`s and `byte[]` buffers per second, and the garbage collector spends time cleaning them up. Reusing a few buffers would avoid most of those allocations.

*Analogy:* a bike-sharing station. You take a bike, use it, put it back; the next person uses the same bike. Building a new bike per trip, and scrapping it afterwards, would be absurd.

#### Without the pattern

```csharp
// A new builder for every invoice: fine at 10 invoices per second, measurable at 10,000.
string Render(Order order)
{
    var text = new StringBuilder(capacity: 4096);
    // … append the invoice …
    return text.ToString();
}
```

What hurts (only when measured): allocation rate and GC pauses on a hot path.

#### Structure

```mermaid
classDiagram
    class InvoiceBufferPool {
        <<ObjectPool>>
        -Stack~StringBuilder~ _free
        +int Created
        +Rent() StringBuilder
        +Return(StringBuilder builder)
    }
    class InvoiceRenderer {
        <<Client>>
    }
    class StringBuilder {
        <<Reusable>>
    }
    InvoiceRenderer --> InvoiceBufferPool
    InvoiceBufferPool o-- StringBuilder
```

| Role | Our class | Responsibility |
|---|---|---|
| ObjectPool | `InvoiceBufferPool` | Hands out a free object (or creates one), takes it back, resets it, and keeps at most `maxRetained`. |
| Reusable | `StringBuilder` | The expensive object. |
| Client | the invoice renderer | Rents, uses, and **always** returns (in a `finally`). |

#### How it runs

```mermaid
sequenceDiagram
    participant Client
    participant Pool as InvoiceBufferPool
    Client->>Pool: Rent()
    Note right of Pool: empty: create one (Created = 1)
    Pool-->>Client: builder
    Client->>Pool: Return(builder)
    Note right of Pool: clear it, keep it
    Client->>Pool: Rent()
    Pool-->>Client: the same builder, empty
```

#### By hand

```csharp
// Role: ObjectPool — lends StringBuilders and takes them back, keeping a bounded number.
// Guide: §7.8
public sealed class InvoiceBufferPool
{
    private readonly Stack<StringBuilder> _free = new();
    private readonly int _maxRetained;

    public InvoiceBufferPool(int maxRetained)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxRetained);
        _maxRetained = maxRetained;
    }

    public int Created { get; private set; }

    public StringBuilder Rent()
    {
        if (_free.TryPop(out var builder)) return builder;
        Created++;
        return new StringBuilder();
    }

    public void Return(StringBuilder builder)
    {
        builder.Clear(); // the next user must not see the previous invoice
        if (_free.Count < _maxRetained) _free.Push(builder); // full pool: let the GC have it
    }
}

var builder = pool.Rent();
try { /* render the invoice */ return builder.ToString(); } // copy the text out before the builder goes back
finally { pool.Return(builder); }
```

The client is `InvoiceRenderer(InvoiceBufferPool pool)`, whose `Render(order)` writes `"Invoice 1a2b3c4d"`, one line per order line (`"Clean Code x2 25.00"`) and the total. The demo renders 1,000 invoices and the pool creates **one** builder.

Three rules make a pool correct: **reset** objects when they come back (or the next user sees old data), **bound** the pool (or it becomes a memory leak), and **never use an object after returning it** (someone else may have it now). This hand-written pool is not thread-safe; the framework ones are.

#### In .NET

**`ArrayPool<T>.Shared`** (in the BCL) pools arrays and is used throughout the framework (streams, JSON, ASP.NET Core). **`ObjectPool<T>`** (package `Microsoft.Extensions.ObjectPool`) pools any object with a policy that creates and resets it ([`2-DotNet/`](../src/Patterns.Modern/ObjectPool/2-DotNet/)):

```csharp
// Guide: §7.8
ObjectPool<StringBuilder> builders = new DefaultObjectPoolProvider().CreateStringBuilderPool();
var text = builders.Get();
try { text.Append("Invoice …"); }
finally { builders.Return(text); } // the policy clears it; very large builders are discarded

byte[] buffer = ArrayPool<byte>.Shared.Rent(100); // may be LONGER than 100
try
{
    var written = Encoding.UTF8.GetBytes("Invoice 0001", buffer);
    Save(buffer.AsSpan(0, written)); // use only what you wrote, never buffer.Length
}
finally { ArrayPool<byte>.Shared.Return(buffer, clearArray: true); } // clear: invoices contain personal data
```

`Rent(100)` returns an array of **at least** 100 elements, usually rounded up to a power of two (128). The test `ArrayPool_MayReturnALargerArray` checks `Length >= 100`. In the code, `InvoiceRenderer` takes an `ObjectPool<StringBuilder>` (same rent/use/return shape as the Classic level), and `InvoiceEncoder.ToUtf8(text)` rents `Encoding.UTF8.GetMaxByteCount(text.Length)` bytes and copies out only the `written` ones (test `DotNet_EncodedInvoice_HasOnlyTheBytesWritten`). Always keep track of the length you asked for or wrote; `buffer.Length` includes leftovers from previous users. Pass `clearArray: true` when the data is sensitive.

**When pooling pays off: measure first.** Allocating small objects in .NET is very cheap, and the GC is designed for short-lived objects. Pooling adds complexity and bugs (use after return, stale data). It pays off for **large** or **expensive-to-create** objects on **hot paths** (thousands of times per second), and you should prove it with a benchmark (BenchmarkDotNet, MIT, is the standard tool) and memory metrics before and after. Database connections are pooled for you by ADO.NET providers; `HttpClient` handlers by `IHttpClientFactory`; `DbContext` pooling exists via `AddDbContextPool`.

#### When to use it

- Objects that are expensive to create (large buffers, objects with costly initialisation) used at high frequency.
- When a profiler shows allocations or GC time on that path.

#### When NOT to use it

- **Without a measurement:** ordinary allocations are fast; the pool may make things slower and buggier.
- **Small or cheap objects:** pooling costs more than allocating.
- **Objects with complex state that is hard to reset:** a missed reset leaks data between users.

#### Costs

- Rent/return discipline everywhere (`try`/`finally`); a forgotten `Return` drains the pool, a double `Return` corrupts it.
- Data leaks between users if objects are not cleared.
- Thread safety, bounds and sizing to get right.

#### Relevance today

⭐ **Niche.** The framework pools what matters (connections, arrays, handlers). You write pooling code only in performance-critical paths, after measuring.

#### Relatives

- **Flyweight** ([5.6](#56-flyweight)): shares immutable objects among everyone at once; a pool lends mutable objects to one user at a time.
- **Singleton** ([4.1](#41-singleton)): one shared instance, never returned.
- **Factory Method** ([4.2](#42-factory-method)): the pool's policy is a factory that also knows how to reset.

#### Try it

```bash
dotnet run --project src/Patterns.Runner -- object-pool
```

1. Rent and return 1,000 times with `maxRetained: 1` and check `Created`.
2. Forget the `Clear()` in `Return` and rent again: what does the next invoice contain?
3. Rent 100 bytes from `ArrayPool<byte>.Shared` and print the length.

#### Interview questions

<details>
<summary>Why can <code>ArrayPool.Rent(n)</code> return a larger array, and what does it mean for your code?</summary>

The pool keeps arrays in size buckets (powers of two) to reuse them; you get the smallest bucket that fits. Your code must track the length it needs and never rely on `array.Length`.
</details>

<details>
<summary>When does object pooling make performance worse?</summary>

For small or cheap objects, where the pool's bookkeeping and synchronisation cost more than allocating, and in code where it is not a measured hot path. It also adds risks: stale data and use after return.
</details>

---

## 8. Patterns that get confused

Many patterns share a class diagram and differ only in **intent**: why the structure exists. These groups are the classic interview traps. Each section has a comparison table and the difference in one sentence.

### 8.1 Decorator, Proxy, Adapter, Facade

All four **wrap** something and forward calls to it.

```mermaid
flowchart TB
    subgraph adapter [Adapter]
        direction LR
        CA[client] -->|our interface| AA[adapter] -->|their interface| XA[adaptee]
    end
    subgraph decorator [Decorator]
        direction LR
        CD[client] -->|IPriceCalculator| D1[VAT] -->|IPriceCalculator| D2[coupon] -->|IPriceCalculator| D3[base]
    end
    subgraph proxy [Proxy]
        direction LR
        CP[client] -->|IPriceEditor| PP[admin-only proxy] -.->|only if allowed| RP[real editor]
    end
    subgraph facade [Facade]
        direction LR
        CF[client] -->|one simple call| FF[facade]
        FF --> S1[inventory]
        FF --> S2[payments]
        FF --> S3[shipping]
    end
```

| | Adapter ([5.1](#51-adapter)) | Decorator ([5.4](#54-decorator)) | Proxy ([5.7](#57-proxy)) | Facade ([5.5](#55-facade)) |
|---|---|---|---|---|
| Intent | Make an incompatible interface fit. | Add behaviour. | Control access. | Simplify a subsystem. |
| Interface towards the client | **Different** from the wrapped object's (it is ours). | **Same** as the wrapped object. | **Same** as the wrapped object. | **New**, simpler than the subsystem's. |
| Wraps | one object | one object (and they stack) | one object | several objects |
| Who creates the wrapped object | usually injected | the client composes the chain | often the proxy itself (lazy, remote) | injected or created by the facade |
| Typical .NET example | `StreamReader` over `Stream` | `DelegatingHandler`, `GZipStream` | `Lazy<T>`, EF Core lazy loading, `DispatchProxy` | `File.WriteAllText`, `WebApplication` |

**In one sentence:** an adapter **converts**, a decorator **adds**, a proxy **guards**, a facade **simplifies**.

### 8.2 Strategy, State, Template Method

All three let part of an algorithm vary.

| | Strategy ([6.9](#69-strategy)) | State ([6.8](#68-state)) | Template Method ([6.10](#610-template-method)) |
|---|---|---|---|
| What varies | the whole algorithm | the behaviour, according to the object's current state | some steps of a fixed algorithm |
| Mechanism | composition (holds an interface) | composition (holds a state object) | inheritance (subclass overrides steps) |
| Who chooses the variant | the client, from outside | the states themselves, as the object moves | whoever picks the subclass, at creation |
| Does it change over time? | usually fixed after creation | yes, on every transition | no |
| Typical .NET example | `IComparer<T>`, a lambda, keyed services | (the light alternative) a `switch` on `(status, action)` | `BackgroundService.ExecuteAsync`, `Stream` |

**In one sentence:** a strategy is **chosen** from outside, a state **replaces itself**, a template method is **filled in** by a subclass.

### 8.3 Factory Method, Abstract Factory, Builder

All three take object creation out of the code that uses the objects.

| | Factory Method ([4.2](#42-factory-method)) | Abstract Factory ([4.3](#43-abstract-factory)) | Builder ([4.4](#44-builder)) |
|---|---|---|---|
| Creates | one product | a family of related products | one complex product |
| How | one overridable method (or a registration) | an object with one method per product | several steps, then `Build()` |
| Decided by | a subclass (or the container's key) | the factory object passed in | the calls the client makes |
| Main benefit | the client does not name the class | the products always match | the product is valid and immutable when handed out |
| Typical .NET example | keyed services, `ILoggerFactory.CreateLogger` | `DbProviderFactory` | `StringBuilder`, `WebApplication.CreateBuilder` |

**In one sentence:** Factory Method decides **which class**, Abstract Factory decides **which family**, Builder decides **how it is assembled**.

### 8.4 Observer and Mediator

Both decouple objects that need to react to each other.

| | Observer ([6.7](#67-observer)) | Mediator ([6.5](#65-mediator)) |
|---|---|---|
| Direction | one-to-many: a subject announces to subscribers | many-to-many through a centre |
| Who decides what happens | each subscriber decides how to react | the mediator decides what each colleague does |
| Does the sender know the receivers? | no, only that "someone may be listening" | no, but it knows the mediator |
| How many receivers per message | zero, one or many | exactly one handler per request (request/handler form) |
| Typical .NET example | `event`, `IObservable<T>`, `IOptionsMonitor<T>.OnChange` | a request dispatcher over DI, MediatR |

**In one sentence:** an observer subject **broadcasts** and lets others react; a mediator **coordinates** and tells others what to do.

### 8.5 Command and Strategy

Both put behaviour in an object (or a delegate).

| | Command ([6.2](#62-command)) | Strategy ([6.9](#69-strategy)) |
|---|---|---|
| Represents | a **request** to do something, with its parameters | a **way** of doing something |
| Typically has | `Execute` (and often `Undo`), all data captured inside | a method that receives the data to work on |
| Lifecycle | created per request; stored, queued, logged, undone | created once, used many times |
| Typical .NET example | `ICommand` in WPF/MAUI, request objects, work items | `IComparer<T>`, pricing rules, `Func<Order, decimal>` |

**In one sentence:** a command is **what** to do (a noun you can store); a strategy is **how** to do it (a policy you plug in).

### 8.6 Composite and Decorator

Both hold objects of their own interface, so diagrams look alike.

| | Composite ([5.3](#53-composite)) | Decorator ([5.4](#54-decorator)) |
|---|---|---|
| Holds | **many** children | exactly **one** inner object |
| Intent | treat a group as one (sum, count, render a tree) | add behaviour around one object |
| Result of an operation | combines the children's results | changes the inner object's result |
| Typical .NET example | `IConfiguration` sections, UI control trees | `DelegatingHandler`, streams |

**In one sentence:** a composite **aggregates** many, a decorator **enhances** one.

---

## 9. Combining and choosing

### 9.1 Combinations you will meet in real code

Patterns rarely appear alone. These pairs show up again and again:

| Combination | How they fit | Where you see it |
|---|---|---|
| **Composite + Visitor** | The composite holds the tree; visitors add operations over it without changing the node classes. | Expression trees and `ExpressionVisitor`; Roslyn syntax trees; document models. |
| **Composite + Iterator** | An iterator (`yield return`, recursively) walks the tree as a flat sequence. | Walking menus, folders, bundles. |
| **Decorator + DI** | The container builds the decorator chain from registrations, so consumers receive the decorated service without knowing it. | `IHttpClientFactory` handlers; a caching repository registered around the real one. |
| **Chain of Responsibility + Decorator (middleware)** | Each middleware wraps the rest of the pipeline (decorator) and may stop it (chain). | ASP.NET Core, `HttpClient` handlers. |
| **Command + Memento (undo)** | Each command saves a memento of what it changes before executing, and restores it on undo: correct undo without writing the inverse of every operation. | Editors, design tools. |
| **Strategy + Factory / keyed services** | A factory, or the container by key, picks the strategy from run-time data; the client only sees the interface. | Shipping or payment method chosen by the customer. |
| **Observer + Mediator** | The mediator observes its colleagues' events and decides what the others do. | UI forms, game loops. |
| **Repository + Unit of Work + Specification** | Specifications describe what to load, repositories load it, the unit of work commits the changes. | EF Core applications in DDD style. |
| **Facade + Unit of Work** | An application service (facade) runs one use case inside one unit of work. | "Place order" services. |
| **Proxy + DI** | Interception: the container hands out a proxy that adds logging, caching or authorisation around the real service. | Castle DynamicProxy with Autofac; `DispatchProxy`. |

### 9.2 From symptom to pattern

Start from the pain, not from the pattern. Each row also names the simpler option to try first.

| Symptom in your code | Pattern that addresses it | Or simpler: |
|---|---|---|
| A `switch` on a "type" or "mode" string grows with every feature. | Strategy ([6.9](#69-strategy)) | keep the `switch` while it has three or four stable cases |
| The same `switch` appears in several methods, on the same status field. | State ([6.8](#68-state)) | one `switch` expression on `(status, action)` |
| `new ConcreteClass()` inside business logic makes it untestable. | Dependency Injection ([7.1](#71-dependency-injection)) | pass it in the constructor by hand (pure DI), without a container |
| A method has a growing list of `if (rule) return error;` that changes often. | Chain of Responsibility ([6.1](#61-chain-of-responsibility)) | a list of validator objects in a `foreach` |
| A class calls more and more collaborators when something happens. | Observer ([6.7](#67-observer)) | a list of handlers passed in; or keep the direct calls if there are two |
| Every client calls the same four services in the same order. | Facade ([5.5](#55-facade)) | one application-service method |
| Our code is full of a vendor SDK's types and formats. | Adapter ([5.1](#51-adapter)) | one class that wraps the SDK, without an interface until you need a fake |
| Logging, caching or retries are copied into many methods. | Decorator ([5.4](#54-decorator)) or middleware | a helper method, if it is in two places |
| An object is half-built and invalid while being filled in. | Builder ([4.4](#44-builder)) | a constructor with required parameters, or `required` properties |
| Tests need many objects that differ in one detail. | Test data builder ([4.4](#44-builder)) | a helper method with optional parameters |
| `if (x is not null)` before every use of an optional collaborator. | Null Object ([7.7](#77-null-object)) | nullable reference types and one check where it is created |
| `try`/`catch` used for ordinary business outcomes. | Result ([7.6](#76-result)) | a `TryXxx` method |
| Settings read with string keys and parsed all over the code. | Options ([7.2](#72-options)) | one settings class read once at start-up |
| A method per combination of filters (`FindCheapInStockBooks`). | Specification ([7.5](#75-specification)) | LINQ `Where` with a few named `Expression` fields |
| Several writes must succeed or fail together. | Unit of Work ([7.4](#74-unit-of-work)) | one `SaveChanges` per operation |
| Undo needs to reverse operations. | Command ([6.2](#62-command)) or Memento ([6.6](#66-memento)) | immutable state and a stack of previous values |
| Classes multiply as kinds × channels (`OrderShippedEmail`, `OrderShippedSms`…). | Bridge ([5.2](#52-bridge)) | one class per dimension, composed by a parameter |
| Group and single item need the same `if (isGroup)` in every operation. | Composite ([5.3](#53-composite)) | a recursive function over a record |
| Expensive objects created thousands of times per second (measured). | Object Pool ([7.8](#78-object-pool)) | `ArrayPool<T>.Shared` |
| The same cross-cutting steps (validation, logging, transactions) are repeated around every use-case handler. | Mediator with pipeline behaviours ([6.5](#65-mediator)) | a decorator per handler, or a helper method |

### 9.3 When not to use each pattern

One line per pattern, from each section's "When NOT to use it".

| Pattern | Do not use it when… | Use instead |
|---|---|---|
| Singleton | you want global access, or the object has per-request state | an ordinary class with `AddSingleton` / `AddScoped` |
| Factory Method | there are two or three stable variants, or the factory only calls `new` | a `switch` expression or the container |
| Abstract Factory | there is one family, or the products are independent | inject the objects directly |
| Builder | there are two or three required values and no rules | a constructor, named arguments, `required` properties |
| Prototype | the type is a record with immutable members | `with` |
| Adapter | you own both sides, or the external interface already fits | change one side, or use it directly |
| Bridge | only one dimension varies | a plain interface (Strategy) |
| Composite | the structure is flat | a `List<T>` and LINQ |
| Decorator | one fixed extra step always applies | put it in the class |
| Facade | it forwards a single call, or grows into a god object | call the object directly, or split per use case |
| Flyweight | there is no measured memory problem | immutable records shared naturally |
| Proxy | the real object is cheap, or lazy loading hides queries in loops | load eagerly |
| Chain of Responsibility | a few fixed checks, or you need all the errors | a method with `if`s, or a list of validators |
| Command | the action runs immediately and is never stored or undone | call the method, or a delegate |
| Interpreter | the language is large or is really a set of options | a parser generator, an existing language, or a form |
| Iterator | you would hand-write the iterator class | `IEnumerable<T>` with `yield return` |
| Mediator | the API is small, or it only hides big constructors | direct calls; split classes |
| Memento | the state is large and mutable, or includes external resources | Command, or immutable state |
| Observer | there is one reaction, or reactions must be transactional or durable | a direct call, a facade, or a message queue |
| State | states differ only in allowed moves | a transition `switch` |
| Strategy | there are few stable variants | a `switch`, or a `Func<>` |
| Template Method | the varying parts are independent of each other | Strategy (composition) |
| Visitor | you own a small hierarchy, or element types change often | a `switch` expression over records |
| Dependency Injection | the object is a value, a DTO or a simple object | `new` |
| Options | it is a single constant | a `const` |
| Repository | simple CRUD over EF Core, or as a generic wrapper | `DbContext` and `DbSet<T>` directly |
| Unit of Work | a single change, or a wrapper that only calls `SaveChanges` | `DbContext` |
| Specification | a few fixed or single-use queries | a LINQ `Where` |
| Result | the error is unexpected, or the reason does not matter | an exception, or `TryXxx` |
| Null Object | absence must be handled differently, or doing nothing is wrong | `null` with nullable reference types, or a Result |
| Object Pool | there is no measurement, or the objects are small | plain allocation |

### 9.4 Out of scope

These patterns are important but belong to **architecture and distributed systems** rather than to the design of a few classes, so this repository does not implement them:

- **Retry** — repeat a failed call that may succeed later (a network glitch), with a delay that grows each time. In .NET: `Microsoft.Extensions.Http.Resilience` and Polly (BSD-3-Clause).
- **Circuit Breaker** — after repeated failures, stop calling a broken service for a while, so it can recover and callers fail fast. Same libraries as Retry.
- **Transactional Outbox** — save an outgoing message in the same database transaction as the data change, and publish it afterwards, so neither is lost.
- **Saga** — a long business process across several services, with a compensating action for each step to undo it if a later step fails.
- **Messaging** — durable message queues and brokers (Azure Service Bus, RabbitMQ) for reactions that must survive a crash or run in another service; the in-process Observer of [6.7](#67-observer) is not enough for them.
- **CQRS** (Command Query Responsibility Segregation) — separate models (and sometimes stores) for writing and for reading.

A good next step for all of them is the documentation of .NET's resilience libraries, Microsoft's *.NET Microservices: Architecture for Containerized .NET Applications* guide, and the "Enterprise Integration Patterns" catalog by Gregor Hohpe and Bobby Woolf.

---

## 10. Glossary

Alphabetical. Each term links to the section that explains it.

- **`.editorconfig`** — A file with code-style rules and analyzer severities, read by the IDE and the build. → [2.2](#22-root-build-files)
- **Abstract syntax tree (AST)** — The tree of objects a parser builds from a sentence; each node is a construct of the language. → [6.3](#63-interpreter)
- **Abstraction / implementor (Bridge)** — The two sides of a bridge: what the client uses, and the lower-level mechanism it is built on. → [5.2](#52-bridge)
- **ADO.NET** — The low-level database API of .NET (`DbConnection`, `DbCommand`), under Entity Framework and Dapper. → [4.3](#43-abstract-factory)
- **Aggregate (DDD)** — A cluster of objects (an order and its lines) loaded, changed and saved as one unit. → [7.3](#73-repository)
- **`AggregateException`** — An exception that carries several inner exceptions, used to report many failures at once. → [6.7](#67-observer)
- **Aggregation** — A "has" relation where the parts can exist without the whole (hollow diamond). → [3.4](#34-how-to-read-the-diagrams)
- **Ambient transaction** — A transaction available to any code that runs inside it, through `Transaction.Current`, without being passed around; `TransactionScope` creates one. → [7.4](#74-unit-of-work)
- **Analyzer** — A compiler extension that reports code problems as diagnostics (`CA1305`, `IDE0005`); here, warnings fail the build. → [2.2](#22-root-build-files)
- **Anti-pattern** — A common solution that looks reasonable but causes more problems than it solves (static Singleton, Service Locator). → [4.1](#41-singleton)
- **Architecture pattern** — A pattern at the scale of a whole application (layers, hexagonal), larger than a design pattern. → [3.2](#32-pattern-idiom-architecture)
- **Architecture test** — A test that checks structural rules of the code instead of behaviour. → [2.6](#26-rules-enforced-by-tests)
- **ArchUnitNET** — A library to write tests about code structure (which namespaces may depend on which). → [2.6](#26-rules-enforced-by-tests)
- **Assembly** — The compiled output of a project (a `.dll` or `.exe`). → [2.1](#21-solution-and-projects)
- **Association** — A class keeps a reference to another and uses it over time (a field). → [3.4](#34-how-to-read-the-diagrams)
- **`BackgroundService`** — Base class for long-running work in a .NET host; you implement `ExecuteAsync`. → [6.10](#610-template-method)
- **BCL (Base Class Library)** — The standard library that ships with .NET (`System.*`). → [5.6](#56-flyweight)
- **BenchmarkDotNet** — The standard .NET library for measuring the speed and allocations of code reliably (MIT). → [7.8](#78-object-pool)
- **BOM (byte order mark)** — Optional bytes at the start of a text file that announce its encoding; `File.WriteAllText` writes UTF-8 without one. → [5.5](#55-facade)
- **Builder (fluent)** — An object that assembles another step by step, each step returning the builder so calls chain. → [4.4](#44-builder)
- **Captive dependency** — A longer-lived service holding a shorter-lived one (a singleton holding a scoped `DbContext`). → [7.1](#71-dependency-injection)
- **Central package management** — Declaring every NuGet package version once in `Directory.Packages.props`. → [2.2](#22-root-build-files)
- **Change tracker (EF Core)** — The part of `DbContext` that records every entity added, modified or removed, so `SaveChanges` can write them all at once. → [7.4](#74-unit-of-work)
- **Class adapter / object adapter** — An adapter that inherits from the adaptee, versus one that holds it; C# uses object adapters. → [5.1](#51-adapter)
- **Class diagram** — A diagram of types and their relations. → [3.4](#34-how-to-read-the-diagrams)
- **Clean Architecture** — A layered style in which the domain depends on nothing and outer layers (database, web) depend on it. → [7.3](#73-repository)
- **Closure** — A function (lambda) that keeps references to variables from where it was created. → [6.2](#62-command)
- **Cohesion** — How much the things inside one piece of code belong together; aim for high. → [3.5](#35-the-principles-under-the-patterns)
- **Colleague (Mediator)** — An object that talks to others only through the mediator. → [6.5](#65-mediator)
- **Compensation** — An action that undoes the effect of an earlier step when a later one fails. → [5.5](#55-facade)
- **Composite node / leaf** — In a tree, a node that holds children versus one that does not. → [5.3](#53-composite)
- **Composition** — A "part of" relation: the parts live and die with the whole (filled diamond). → [3.4](#34-how-to-read-the-diagrams)
- **Composition over inheritance** — Reuse behaviour by holding objects rather than by deriving from classes. → [3.5](#35-the-principles-under-the-patterns)
- **Composition root** — The one place, at the entry point, where the object graph is assembled. → [7.1](#71-dependency-injection)
- **Configuration provider** — A source of configuration values (JSON file, environment variables, memory) plugged into `IConfiguration`; it can signal reloads through a change token. → [7.2](#72-options)
- **Correlation id** — An identifier attached to a request so all logs and calls about it can be linked. → [5.4](#54-decorator)
- **Coupling** — How much one piece of code depends on another; aim for low. → [3.5](#35-the-principles-under-the-patterns)
- **Creational / structural / behavioral** — The three GoF families: creating objects, combining them, and how they interact. → [3.3](#33-the-gof-book-and-why-some-patterns-aged)
- **Cross-cutting concern** — Behaviour needed in many places that is not the main job of any of them: logging, caching, retries, security. → [5.4](#54-decorator)
- **CSV / JSON** — Comma-separated values (a spreadsheet-friendly text format) and JavaScript Object Notation (the usual format for web APIs). → [6.10](#610-template-method)
- **`DbSet<T>`** — EF Core's collection-like set of entities of one type, with `Find`, `Add`, `Remove` and LINQ queries: a repository in shape. → [7.3](#73-repository)
- **`decimal`** — The .NET type for exact decimal numbers, used for money (unlike `double`). → [3.8](#38-the-shop)
- **Delegate** — A type-safe reference to a method (`Action`, `Func<T>`); the language form of Command and Strategy. → [6.2](#62-command)
- **`DelegatingHandler`** — An `HttpClient` message handler that wraps an inner handler: the decorator of the HTTP pipeline. → [5.4](#54-decorator)
- **Dependency** — In diagrams, a brief use (a parameter); in DI, an object a class needs to do its job. → [3.4](#34-how-to-read-the-diagrams)
- **Dependency Injection (DI) / DI container** — Giving objects their collaborators from outside; the container is the library that builds them from registrations. → [7.1](#71-dependency-injection)
- **Design pattern** — A named, reusable solution to a recurring design problem, with its consequences. → [3.1](#31-what-a-design-pattern-is)
- **`Directory.Build.props`** — An MSBuild file whose settings apply to every project below its folder. → [2.2](#22-root-build-files)
- **Discriminated union** — A type that is exactly one of a fixed set of cases (success or failure); C# does not have it built in yet. → [7.6](#76-result)
- **`DispatchProxy`** — A BCL class that generates, at run time, an interface implementation routing every call to one method. → [5.7](#57-proxy)
- **Double dispatch** — Choosing code by the run-time types of two objects; Visitor does it with two virtual calls. → [6.11](#611-visitor)
- **Double-checked locking** — A hand-written lazy, thread-safe initialisation with two null checks around a lock; replaced by `Lazy<T>`. → [4.1](#41-singleton)
- **DTO (data transfer object)** — A plain object that only carries data across a boundary (an API, a message). → [5.1](#51-adapter)
- **Encapsulate what varies** — Put the part that changes behind its own boundary so the rest does not change with it. → [3.5](#35-the-principles-under-the-patterns)
- **Enlist (in a transaction)** — Register a resource with a transaction so it is asked to prepare, commit or roll back with the others. → [7.4](#74-unit-of-work)
- **`[EnumeratorCancellation]`** — An attribute on an async iterator's `CancellationToken` parameter, so a token passed with `WithCancellation` reaches the method. → [6.4](#64-iterator)
- **Equivalence test** — A test proving that the levels of a pattern give the same result for the same input. → [2.5](#25-kinds-of-tests)
- **`event`** — C#'s built-in Observer: a multicast delegate that outsiders can only subscribe to and unsubscribe from. → [6.7](#67-observer)
- **Expression tree** — Code represented as a tree of objects (`System.Linq.Expressions`) that can be inspected, translated to SQL or compiled. → [6.3](#63-interpreter)
- **`ExpressionVisitor`** — The BCL's Visitor for expression trees; override the `Visit…` methods you care about. → [6.11](#611-visitor)
- **Extension method** — A static method callable as if it were an instance method (`services.AddCheckout(clock)`); the usual way a feature offers its DI registrations. → [7.1](#71-dependency-injection)
- **Factory** — Anything whose job is to create objects; the GoF Factory Method is one specific form. → [4.2](#42-factory-method)
- **Flowchart / state diagram** — Diagrams of a process, and of the states something can be in with the moves between them. → [3.4](#34-how-to-read-the-diagrams)
- **Fragile base class** — Subclasses that break when their base class changes, a cost of inheritance. → [6.10](#610-template-method)
- **Garbage collector (GC)** — The part of the runtime that frees memory no longer referenced. → [5.6](#56-flyweight)
- **`global.json`** — The file that pins which .NET SDK version builds the repository. → [1.1](#11-the-net-sdk)
- **GoF (Gang of Four)** — Gamma, Helm, Johnson and Vlissides, authors of *Design Patterns* (1994). → [3.3](#33-the-gof-book-and-why-some-patterns-aged)
- **Grammar** — The rules that say which sentences of a language are valid. → [6.3](#63-interpreter)
- **Hexagonal architecture (ports and adapters)** — An architecture where the core defines ports (interfaces) and adapters connect them to the outside world. → [5.1](#51-adapter)
- **Hollywood principle** — "Don't call us, we'll call you": the framework calls your code at defined points. → [6.10](#610-template-method)
- **Hook** — An optional overridable step with a default implementation. → [6.10](#610-template-method)
- **`IAsyncEnumerable<T>`** — A sequence whose elements arrive asynchronously; consumed with `await foreach`. → [6.4](#64-iterator)
- **`IChangeToken`** — A pull-style change notification used by configuration and file providers. → [6.7](#67-observer)
- **`IConfiguration` section** — A node of the configuration tree; itself an `IConfiguration`. → [5.3](#53-composite)
- **Idiom** — A pattern at the scale of one language construct (`using`, `TryParse`). → [3.2](#32-pattern-idiom-architecture)
- **Immutable** — Cannot change after creation; immutable objects can be shared safely. → [4.4](#44-builder)
- **Indirection** — Reaching something through an intermediate step (an interface, a factory); costs readability, buys flexibility. → [3.6](#36-patternitis)
- **Inheritance** — A class extends another class (solid line, hollow triangle). → [3.4](#34-how-to-read-the-diagrams)
- **Intrinsic / extrinsic state** — In Flyweight, the shared immutable part versus the per-use part kept outside. → [5.6](#56-flyweight)
- **Invariant globalization** — Running without culture data, so formatting is culture-neutral everywhere. → [2.2](#22-root-build-files)
- **Inversion of control** — The framework, not your code, controls the flow; DI and Template Method are forms of it. → [6.10](#610-template-method)
- **`IObservable<T>` / `IObserver<T>`** — BCL interfaces for a stream of notifications (`OnNext`, `OnError`, `OnCompleted`). → [6.7](#67-observer)
- **`IOptions<T>` / `IOptionsSnapshot<T>` / `IOptionsMonitor<T>`** — The three ways to receive options: read once, per scope, or always current. → [7.2](#72-options)
- **`IQueryable<T>`** — A query described as an expression tree and executed by a provider (EF Core translates it to SQL); `IEnumerable<T>` runs compiled code in memory instead. → [7.3](#73-repository)
- **`IStartupValidator`** — The service (.NET 8+) that validates options registered with `ValidateOnStart`; the generic host calls it at start-up. → [7.2](#72-options)
- **Keyed services** — Several implementations of one interface registered under keys and resolved by key (.NET 8+). → [4.2](#42-factory-method)
- **Lazy evaluation (deferred execution)** — Work is done only when the result is requested; `yield return` and LINQ are lazy. → [6.4](#64-iterator)
- **Lazy initialization / `Lazy<T>`** — Creating an object on first use, once; `Lazy<T>` does it thread-safely. → [4.1](#41-singleton)
- **Level (Problem / Classic / .NET)** — The two or three versions of each pattern in this repository. → [2.3](#23-the-folder-of-a-pattern)
- **`[LoggerMessage]`** — An attribute that makes the compiler generate a fast, allocation-free logging method from a message template (the method is `partial`). → [7.7](#77-null-object)
- **Logging provider** — A destination for logs (console, file, a service) behind `ILogger`. → [5.2](#52-bridge)
- **Materialised (list)** — The results of a query copied into a list now, so later reads do not run the query again. → [7.3](#73-repository)
- **Memory leak (forgotten subscription)** — A subscriber kept alive by a long-lived publisher's event. → [6.7](#67-observer)
- **Mermaid** — A text format for diagrams that GitHub and VS Code render. → [3.4](#34-how-to-read-the-diagrams)
- **Microsoft.Testing.Platform** — The test runner used by `dotnet test` in this repository. → [1.1](#11-the-net-sdk)
- **Middleware** — A component of the ASP.NET Core request pipeline; can act before and after the next one, or stop. → [6.1](#61-chain-of-responsibility)
- **Mock** — A test double generated by a library that records and checks calls; often built on dynamic proxies. → [5.7](#57-proxy)
- **MSBuild** — The build engine behind `dotnet build`; reads `.csproj` and `.props` files. → [2.2](#22-root-build-files)
- **Multicast delegate** — A delegate holding a list of methods, called in order. → [6.7](#67-observer)
- **Multiple enumeration** — Enumerating a lazy sequence more than once, which runs its work (a query) again each time. → [6.4](#64-iterator)
- **N+1 queries** — One query for a list plus one per item, typically caused by lazy loading in a loop. → [5.7](#57-proxy)
- **`[NotNullWhen(true)]`** — An attribute telling the nullable analysis that an `out` value is not null when the method returns `true` (the `TryXxx` shape). → [7.6](#76-result)
- **Nullable reference types** — The compiler feature that tracks which references may be `null` and warns about unchecked uses. → [2.2](#22-root-build-files)
- **`NullLogger<T>`** — The framework's null object for logging: a logger that accepts every message and writes nothing. → [7.7](#77-null-object)
- **Object Pool / `ArrayPool<T>`** — Reusing expensive objects or arrays instead of allocating new ones. → [7.8](#78-object-pool)
- **`ObjectPool<T>`** — The pool of `Microsoft.Extensions.ObjectPool`: `Get` and `Return` objects, with a policy that creates and resets them. → [7.8](#78-object-pool)
- **`OptionsValidationException`** — The exception thrown when bound options fail their validation, with the validation messages. → [7.2](#72-options)
- **Originator / caretaker / memento** — Memento's roles: the object that saves and restores itself, the keeper of snapshots, and the opaque snapshot. → [6.6](#66-memento)
- **Pattern matching** — C# syntax to test a value's type and shape (`switch` expressions, property patterns). → [6.11](#611-visitor)
- **Patternitis** — Applying patterns because they exist, not because the code needs them. → [3.6](#36-patternitis)
- **Pipeline** — A chain where each step does its part and passes the request on. → [6.1](#61-chain-of-responsibility)
- **Port** — An interface owned by our code that describes what we need from the outside world. → [5.1](#51-adapter)
- **Profiler** — A tool that measures where a program spends time or memory. → [5.6](#56-flyweight)
- **Program to an interface** — Depend on what a collaborator does (its contract), not on which class does it. → [3.5](#35-the-principles-under-the-patterns)
- **Project / `.csproj`** — A unit of compilation; the `.csproj` file describes it. → [2.1](#21-solution-and-projects)
- **Project reference** — A dependency of one project on another in the same solution. → [2.1](#21-solution-and-projects)
- **Proxy kinds (virtual, protection, remote)** — Proxies that delay creation, check access, or stand for an object elsewhere. → [5.7](#57-proxy)
- **Pure DI** — Wiring dependencies by hand in the composition root, without a container. → [7.1](#71-dependency-injection)
- **Push / pull (Observer)** — Notifications that carry the data, versus ones that only say something changed. → [6.7](#67-observer)
- **Receiver / invoker (Command)** — The object that does the work, and the one that runs and stores commands. → [6.2](#62-command)
- **Record / value equality / `with`** — C# types that compare by value and can be copied with changes. → [3.8](#38-the-shop)
- **Recursive descent parser** — A parser with one method per grammar rule, calling each other. → [6.3](#63-interpreter)
- **Refactoring towards a pattern** — Introducing a pattern when the code starts to hurt, not up front. → [3.6](#36-patternitis)
- **Reflection** — Inspecting and invoking code at run time through its metadata; flexible and slower. → [5.7](#57-proxy)
- **Relevance mark** — ⭐⭐⭐ Essential, ⭐⭐ Useful, ⭐ Niche, 🕰 Historical: how much a pattern matters today. → [2.4](#24-the-runner)
- **Request / handler / dispatcher** — The request/handler form of Mediator: a message, the code that handles it, and the router between them. → [6.5](#65-mediator)
- **Role (pattern role)** — The part a class plays in a pattern (`ConcreteStrategy`, `Context`), named in its `// Role:` comment. → [2.3](#23-the-folder-of-a-pattern)
- **Rule of three** — Write it once, notice it twice, refactor the third time. → [3.6](#36-patternitis)
- **SDK (Software Development Kit)** — The tools to build .NET code (`dotnet` CLI, compilers); also a vendor's client library. → [1.1](#11-the-net-sdk)
- **Sequence diagram / participant** — A diagram of calls over time between objects (participants). → [3.4](#34-how-to-read-the-diagrams)
- **Service lifetime (singleton, scoped, transient)** — How long a DI-created instance lives: the container, a scope, or one resolution. → [7.1](#71-dependency-injection)
- **Service Locator** — Asking a global registry for dependencies inside methods; an anti-pattern. → [7.1](#71-dependency-injection)
- **Shallow copy / deep copy** — Copying references to inner objects versus copying the inner objects too. → [4.5](#45-prototype)
- **Short-circuit** — A link of a chain answering without calling the next one. → [6.1](#61-chain-of-responsibility)
- **Single dispatch** — Choosing a method by the run-time type of one object (a normal virtual call). → [6.11](#611-visitor)
- **SKU (Stock Keeping Unit)** — A shop's own product code, like `BOOK-001`. → [3.8](#38-the-shop)
- **Smart reference** — A proxy that adds logging, caching or counting around access to an object. → [5.7](#57-proxy)
- **SOLID** — Five principles of object-oriented design: Single responsibility, Open/closed, Liskov substitution, Interface segregation, Dependency inversion. → [3.5](#35-the-principles-under-the-patterns)
- **Solution / `.slnx`** — A file that groups projects so they build and open together. → [2.1](#21-solution-and-projects)
- **State machine** — A model of states and transitions; also what the compiler generates for `yield` and `async`. → [6.4](#64-iterator)
- **String interning / intern pool** — One shared instance per distinct string value. → [5.6](#56-flyweight)
- **Subject / observer** — The object that announces changes, and those notified. → [6.7](#67-observer)
- **Subsystem** — A part of the system with its own job, coordinated by a facade. → [5.5](#55-facade)
- **TDD (Test-Driven Development)** — Write a failing test, make it pass, clean up. → [2.5](#25-kinds-of-tests)
- **Telescoping constructor** — A constructor with many (mostly optional) parameters, or a series of overloads; Builder avoids it. → [4.4](#44-builder)
- **Template method** — A non-virtual method fixing an algorithm's steps, some of which subclasses provide. → [6.10](#610-template-method)
- **Test data builder** — A builder in tests that gives every field a default, so each test sets only what matters. → [4.4](#44-builder)
- **Thread safety** — Correct behaviour when several threads use an object at the same time. → [4.1](#41-singleton)
- **`TimeProvider`** — The .NET 8+ abstraction over the clock, replaceable in tests. → [7.1](#71-dependency-injection)
- **Token** — The smallest meaningful piece of a sentence for a parser (a word, a number, `>=`). → [6.3](#63-interpreter)
- **Transaction / two-phase commit** — A group of changes applied completely or not at all; two-phase commit coordinates several resources. → [7.4](#74-unit-of-work)
- **`TransactionScope`** — The `System.Transactions` block that creates an ambient transaction; `Complete()` votes to commit, disposing without it rolls back. → [7.4](#74-unit-of-work)
- **Transition table** — The legal (state, action) → next state pairs, often one `switch` expression. → [6.8](#68-state)
- **Transparency vs safety (Composite)** — Putting `Add` on the common interface (uniform) or only on the composite (type-safe). → [5.3](#53-composite)
- **`TryXxx` pattern** — Returning `bool` and the value in an `out` parameter: the BCL's own result pattern. → [7.6](#76-result)
- **UML relations** — Realization, inheritance, association, aggregation, composition and dependency, as drawn in class diagrams. → [3.4](#34-how-to-read-the-diagrams)
- **Unit of Work** — Collecting changes and committing them together; `DbContext` is one. → [7.4](#74-unit-of-work)
- **`ValidateOnStart`** — Options registration that makes the generic host validate the options at start-up instead of at first use. → [7.2](#72-options)
- **`ValidateScopes` / `ValidateOnBuild`** — DI container options: throw when a scoped service is resolved from the root, and check every registration when the provider is built. → [7.1](#71-dependency-injection)
- **Value type** — A `struct` or enum, stored inline rather than as a separate object on the heap. → [5.6](#56-flyweight)
- **Wrapper** — An object that holds another and forwards calls to it (Adapter, Decorator, Proxy, Facade). → [5](#5-structural-patterns)
- **xUnit v3** — The test framework used in this repository. → [2.5](#25-kinds-of-tests)
- **YAGNI (You Aren't Gonna Need It)** — Do not build for a change you only imagine. → [3.6](#36-patternitis)
- **`yield return`** — Makes a method an iterator; the compiler generates the state machine. → [6.4](#64-iterator)
