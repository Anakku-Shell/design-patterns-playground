# Interpreter

**Relevance:** 🕰 Historical. You rarely hand-write one; you use the interpreters .NET has (expression trees, `Regex`, LINQ providers) or a parser generator.

**Intent:** given a language, represent its grammar as classes and use them to interpret sentences in the language.

**Read first:** [`1-Classic/RuleExpressions.cs`](1-Classic/RuleExpressions.cs), then [`2-DotNet/RuleCompiler.cs`](2-DotNet/RuleCompiler.cs).

| Level | What it shows |
|---|---|
| [`1-Classic/`](1-Classic/) | `IRuleExpression` with `TotalGreaterThan`, `HasCategory` and `AndExpression`; `DiscountRuleParser` (recursive descent) turns `total > 100 AND category = 'books'` into that tree, or fails with the position of the problem. |
| [`2-DotNet/`](2-DotNet/) | `RuleCompiler`: translates the Classic tree once into a `System.Linq.Expressions` tree and compiles it to a `Func<Order, bool>`. The one level in the repo that uses another level's types: the Classic tree is its input. |

Historical patterns have no Problem level; the guide shows the "before" code as a snippet.

**Run it:** `dotnet run --project src/Patterns.Runner -- interpreter`

**Tests:** `dotnet test --project tests/Patterns.Behavioral.Tests --filter-class "*.InterpreterTests"`

**Guide:** [§6.3 Interpreter](../../../docs/DESIGN_PATTERNS_GUIDE.md#63-interpreter)
