using Patterns.Behavioral.Memento.Classic;
using Patterns.Behavioral.Memento.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Behavioral.Memento;

public sealed class MementoDemo : IDemo
{
    public string Key => "memento";
    public string Name => "Memento";
    public PatternCategory Category => PatternCategory.Behavioral;
    public Relevance Relevance => Relevance.Niche;
    public string GuideSection => "§6.6";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(1, "Classic");
        var cart = new Cart();
        var caretaker = new CartCaretaker(cart);
        narrator.Step("add 2 books, Save(), add 1 mug, remove the books");
        cart.Add(SampleData.Book, 2);
        caretaker.Save();
        cart.Add(SampleData.Mug, 1);
        cart.Remove(SampleData.Book.Id);
        narrator.Result(Describe(cart.Items));
        narrator.Step("Undo(): the caretaker hands the opaque snapshot back to the cart");
        caretaker.Undo();
        narrator.Result(Describe(cart.Items));
        narrator.Step("Undo() again, with nothing saved");
        narrator.Result(caretaker.Undo() ? "true" : "false: nothing to restore");

        narrator.Level(2, ".NET");
        narrator.Step("immutable CartState: each change returns a new state, the old ones stay as they were");
        var history = new Stack<CartState>();
        var state = CartState.Empty;
        history.Push(state);
        state = state.Add(SampleData.Book, 2);
        history.Push(state);
        state = state.Add(SampleData.Mug, 1);
        narrator.Result(Describe(state.Items));
        narrator.Step("undo = history.Pop()");
        state = history.Pop();
        narrator.Result(Describe(state.Items));

        narrator.Takeaway("Save a snapshot only the originator can read; with immutable state, the state is its own memento.");
    }

    private static string Describe(IReadOnlyDictionary<Guid, int> items) =>
        items.Count == 0
            ? "empty cart"
            : string.Join(", ", items.Select(i => $"{SampleData.Products.Single(p => p.Id == i.Key).Name} × {i.Value}"));
}
