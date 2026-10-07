using Patterns.Behavioral.Command.Classic;
using Patterns.Behavioral.Command.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.Command;

public sealed class CommandDemo : IDemo
{
    public string Key => "command";
    public string Name => "Command";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§6.2";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var book = SampleData.Book;
        var mug = SampleData.Mug;

        narrator.Level(0, "Problem");
        var problemCart = new Problem.Cart();
        var undoable = new Problem.UndoableCart(problemCart);
        narrator.Step("add 2 books, add 1 mug, undo, undo");
        undoable.Add(book, 2);
        undoable.Add(mug, 1);
        undoable.Undo();
        undoable.Undo();
        narrator.Result($"{Describe(problemCart.Items)} (the second undo did nothing: one level only)");

        narrator.Level(1, "Classic");
        var cart = new Classic.Cart();
        var history = new CartHistory();
        narrator.Step("Run(add 2 books), Run(add 1 mug), Run(remove books)");
        history.Run(new AddItemCommand(cart, book, 2));
        history.Run(new AddItemCommand(cart, mug, 1));
        history.Run(new RemoveItemCommand(cart, book));
        narrator.Result(Describe(cart.Items));
        narrator.Step("Undo() three times: the stack pops each command and calls its Undo");
        while (history.Undo())
        {
            narrator.Result(Describe(cart.Items));
        }

        narrator.Level(2, ".NET");
        var dotNetCart = new DotNet.Cart();
        var actions = new ActionHistory();
        narrator.Step("the same commands as pairs of lambdas (UndoableAction)");
        actions.Run(CartActions.Add(dotNetCart, book, 2));
        var remove = CartActions.Remove(dotNetCart, book);
        actions.Run(remove);
        narrator.Result(Describe(dotNetCart.Items));
        narrator.Step($"undo \"{remove.Name}\"");
        actions.Undo();
        narrator.Result(Describe(dotNetCart.Items));

        narrator.Takeaway("A request turned into an object can be stored, undone, queued or logged; in C# a delegate is the lightest command.");
    }

    private static string Describe(IReadOnlyDictionary<Guid, int> items) =>
        items.Count == 0
            ? "empty cart"
            : string.Join(", ", items.Select(i => $"{SampleData.Products.Single(p => p.Id == i.Key).Name} × {i.Value}"));
}
