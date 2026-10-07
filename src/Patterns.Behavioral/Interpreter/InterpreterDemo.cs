using Patterns.Behavioral.Interpreter.Classic;
using Patterns.Behavioral.Interpreter.DotNet;
using Patterns.Demo;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Behavioral.Interpreter;

public sealed class InterpreterDemo : IDemo
{
    public string Key => "interpreter";
    public string Name => "Interpreter";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Historical;
    public string GuideSection => "§6.3";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        const string rule = "total > 100 AND category = 'books'";
        var orders = new[]
        {
            ("9 books", SampleData.OrderOf((SampleData.Book, 9))),
            ("7 books", SampleData.OrderOf((SampleData.Book, 7))),
            ("2 headphones", SampleData.OrderOf((SampleData.Headphones, 2))),
        };

        narrator.Level(1, "Classic");
        narrator.Step($"parse \"{rule}\"");
        var tree = DiscountRuleParser.Parse(rule);
        narrator.Result(Describe(tree));
        foreach (var (name, order) in orders)
        {
            narrator.Step(Invariant($"Interpret({name}, {order.Total:0.00})"));
            narrator.Result(tree.Interpret(order) ? "true" : "false");
        }
        narrator.Step("parse \"total >> 5\"");
        try
        {
            DiscountRuleParser.Parse("total >> 5");
        }
        catch (FormatException e)
        {
            narrator.Result(e.Message);
        }

        narrator.Level(2, ".NET");
        narrator.Step("the same tree translated once into an expression tree, then compiled to a delegate");
        var compiled = RuleCompiler.Compile(rule);
        narrator.Result(string.Join(", ", orders.Select(o => $"{o.Item1}: {(compiled(o.Item2) ? "true" : "false")}")));

        narrator.Takeaway("Each grammar rule is a class and a sentence is a tree of them; in .NET, expression trees and Regex are the interpreters you use.");
    }

    // The tree as text, culture-safe (a record's generated ToString formats decimals with the current culture).
    private static string Describe(IRuleExpression node) => node switch
    {
        TotalGreaterThan t => Invariant($"Total{(t.OrEqual ? ">=" : ">")}({t.Amount})"),
        HasCategory c => $"HasCategory('{c.Name}')",
        AndExpression a => $"And({Describe(a.Left)}, {Describe(a.Right)})",
        _ => node.GetType().Name,
    };
}
