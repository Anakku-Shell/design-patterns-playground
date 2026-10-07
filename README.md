# Design Patterns Playground

A learning playground for **design patterns in .NET**. Each of 31 patterns (the 23 of the Gang of Four book plus 8 modern .NET ones) is built on the same small online shop, in up to three levels: the code **without** the pattern and what hurts, the pattern **by hand**, and what **.NET already provides**. Every pattern has a runnable demo, tests, and a section in the guide with diagrams, when to use it and when **not** to.

**Start with the guide: [docs/DESIGN_PATTERNS_GUIDE.md](docs/DESIGN_PATTERNS_GUIDE.md).**

## Catalog

Relevance: ⭐⭐⭐ Essential · ⭐⭐ Useful · ⭐ Niche · 🕰 Historical (the language or framework already solves it). Levels: P = Problem, C = Classic (by hand), N = .NET.

| Category | Pattern | Relevance | Levels | Key | Status |
|---|---|---|---|---|---|
| Creational | [Singleton](docs/DESIGN_PATTERNS_GUIDE.md#41-singleton) | ⭐⭐ | P C N | `singleton` | ready |
| Creational | [Factory Method](docs/DESIGN_PATTERNS_GUIDE.md#42-factory-method) | ⭐⭐ | P C N | `factory-method` | ready |
| Creational | [Abstract Factory](docs/DESIGN_PATTERNS_GUIDE.md#43-abstract-factory) | ⭐ | C N | `abstract-factory` | ready |
| Creational | [Builder](docs/DESIGN_PATTERNS_GUIDE.md#44-builder) | ⭐⭐⭐ | P C N | `builder` | ready |
| Creational | [Prototype](docs/DESIGN_PATTERNS_GUIDE.md#45-prototype) | 🕰 | C N | `prototype` | ready |
| Structural | [Adapter](docs/DESIGN_PATTERNS_GUIDE.md#51-adapter) | ⭐⭐⭐ | P C N | `adapter` | ready |
| Structural | [Bridge](docs/DESIGN_PATTERNS_GUIDE.md#52-bridge) | ⭐ | C N | `bridge` | ready |
| Structural | [Composite](docs/DESIGN_PATTERNS_GUIDE.md#53-composite) | ⭐⭐ | P C N | `composite` | ready |
| Structural | [Decorator](docs/DESIGN_PATTERNS_GUIDE.md#54-decorator) | ⭐⭐⭐ | P C N | `decorator` | ready |
| Structural | [Facade](docs/DESIGN_PATTERNS_GUIDE.md#55-facade) | ⭐⭐⭐ | P C N | `facade` | ready |
| Structural | [Flyweight](docs/DESIGN_PATTERNS_GUIDE.md#56-flyweight) | 🕰 | C N | `flyweight` | ready |
| Structural | [Proxy](docs/DESIGN_PATTERNS_GUIDE.md#57-proxy) | ⭐⭐ | P C N | `proxy` | ready |
| Behavioral | [Chain of Responsibility](docs/DESIGN_PATTERNS_GUIDE.md#61-chain-of-responsibility) | ⭐⭐⭐ | P C N | `chain-of-responsibility` | planned |
| Behavioral | [Command](docs/DESIGN_PATTERNS_GUIDE.md#62-command) | ⭐⭐ | P C N | `command` | planned |
| Behavioral | [Interpreter](docs/DESIGN_PATTERNS_GUIDE.md#63-interpreter) | 🕰 | C N | `interpreter` | planned |
| Behavioral | [Iterator](docs/DESIGN_PATTERNS_GUIDE.md#64-iterator) | ⭐⭐⭐ | P C N | `iterator` | planned |
| Behavioral | [Mediator](docs/DESIGN_PATTERNS_GUIDE.md#65-mediator) | ⭐⭐ | P C N | `mediator` | planned |
| Behavioral | [Memento](docs/DESIGN_PATTERNS_GUIDE.md#66-memento) | ⭐ | C N | `memento` | planned |
| Behavioral | [Observer](docs/DESIGN_PATTERNS_GUIDE.md#67-observer) | ⭐⭐⭐ | P C N | `observer` | planned |
| Behavioral | [State](docs/DESIGN_PATTERNS_GUIDE.md#68-state) | ⭐⭐ | P C N | `state` | planned |
| Behavioral | [Strategy](docs/DESIGN_PATTERNS_GUIDE.md#69-strategy) | ⭐⭐⭐ | P C N | `strategy` | planned |
| Behavioral | [Template Method](docs/DESIGN_PATTERNS_GUIDE.md#610-template-method) | ⭐⭐ | P C N | `template-method` | planned |
| Behavioral | [Visitor](docs/DESIGN_PATTERNS_GUIDE.md#611-visitor) | ⭐ | C N | `visitor` | planned |
| Modern | [Dependency Injection](docs/DESIGN_PATTERNS_GUIDE.md#71-dependency-injection) | ⭐⭐⭐ | P C N | `dependency-injection` | planned |
| Modern | [Options](docs/DESIGN_PATTERNS_GUIDE.md#72-options) | ⭐⭐⭐ | P C N | `options` | planned |
| Modern | [Repository](docs/DESIGN_PATTERNS_GUIDE.md#73-repository) | ⭐⭐ | P C N | `repository` | planned |
| Modern | [Unit of Work](docs/DESIGN_PATTERNS_GUIDE.md#74-unit-of-work) | ⭐⭐ | P C N | `unit-of-work` | planned |
| Modern | [Specification](docs/DESIGN_PATTERNS_GUIDE.md#75-specification) | ⭐⭐ | P C N | `specification` | planned |
| Modern | [Result](docs/DESIGN_PATTERNS_GUIDE.md#76-result) | ⭐⭐ | P C N | `result` | planned |
| Modern | [Null Object](docs/DESIGN_PATTERNS_GUIDE.md#77-null-object) | ⭐⭐ | P C N | `null-object` | planned |
| Modern | [Object Pool](docs/DESIGN_PATTERNS_GUIDE.md#78-object-pool) | ⭐ | C N | `object-pool` | planned |

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) (pinned in `global.json`).
- An editor; [VS Code](https://code.visualstudio.com/) with the recommended extensions (`.vscode/extensions.json`) shows the guide's diagrams.

No database, Docker or third-party library is needed: everything runs in memory.

## Quick start

```bash
dotnet run --project src/Patterns.Runner                # list the patterns
dotnet run --project src/Patterns.Runner -- strategy    # run one demo (once its status is "ready")
dotnet test --solution DesignPatterns.slnx              # run every test
```

## License

[GPL-3.0](LICENSE).
