using Patterns.Behavioral.Command.Classic;
using Patterns.Behavioral.Command.DotNet;
using Patterns.Shop;
using Xunit;
using Cart = Patterns.Behavioral.Command.Classic.Cart;
using DotNetCart = Patterns.Behavioral.Command.DotNet.Cart;
using Problem = Patterns.Behavioral.Command.Problem;

namespace Patterns.Behavioral.Tests;

public sealed class CommandTests
{
    private static readonly Product Book = SampleData.Book;
    private static readonly Product Mug = SampleData.Mug;

    [Fact]
    public void Classic_UndoEverything_LeavesTheCartEmpty()
    {
        var cart = new Cart();
        var history = new CartHistory();

        history.Run(new AddItemCommand(cart, Book, 2));
        history.Run(new AddItemCommand(cart, Mug, 1));
        history.Run(new RemoveItemCommand(cart, Book));
        Assert.Equal(new Dictionary<Guid, int> { [Mug.Id] = 1 }, cart.Items);

        Assert.True(history.Undo()); // the book comes back with its 2 units
        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2, [Mug.Id] = 1 }, cart.Items);
        Assert.True(history.Undo());
        Assert.True(history.Undo());

        Assert.Empty(cart.Items);
    }

    [Fact]
    public void DotNet_UndoEverything_LeavesTheCartEmpty()
    {
        var cart = new DotNetCart();
        var history = new ActionHistory();

        history.Run(CartActions.Add(cart, Book, 2));
        history.Run(CartActions.Add(cart, Mug, 1));
        history.Run(CartActions.Remove(cart, Book));
        Assert.Equal(new Dictionary<Guid, int> { [Mug.Id] = 1 }, cart.Items);

        Assert.True(history.Undo());
        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2, [Mug.Id] = 1 }, cart.Items);
        Assert.True(history.Undo());
        Assert.True(history.Undo());

        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Classic_UndoAdd_RestoresTheQuantityBefore()
    {
        var cart = new Cart();
        cart.Add(Book, 3);
        var history = new CartHistory();

        history.Run(new AddItemCommand(cart, Book, 2));
        Assert.Equal(5, cart.Items[Book.Id]);
        history.Undo();

        Assert.Equal(3, cart.Items[Book.Id]);
    }

    [Fact]
    public void DotNet_UndoAdd_RestoresTheQuantityBefore()
    {
        var cart = new DotNetCart();
        cart.Add(Book, 3);
        var history = new ActionHistory();

        history.Run(CartActions.Add(cart, Book, 2));
        history.Undo();

        Assert.Equal(3, cart.Items[Book.Id]);
    }

    [Fact]
    public void Undo_OnEmptyHistory_ReturnsFalse()
    {
        Assert.False(new CartHistory().Undo());
        Assert.False(new ActionHistory().Undo());
    }

    [Fact]
    public void RemovingAbsentProduct_UndoIsNoOp()
    {
        var cart = new Cart();
        cart.Add(Mug, 1);
        var history = new CartHistory();

        history.Run(new RemoveItemCommand(cart, Book));
        Assert.True(history.Undo());

        Assert.Equal(new Dictionary<Guid, int> { [Mug.Id] = 1 }, cart.Items);
    }

    [Fact]
    public void DotNet_RemovingAbsentProduct_UndoIsNoOp()
    {
        var cart = new DotNetCart();
        cart.Add(Mug, 1);
        var history = new ActionHistory();

        history.Run(CartActions.Remove(cart, Book));
        history.Undo();

        Assert.Equal(new Dictionary<Guid, int> { [Mug.Id] = 1 }, cart.Items);
    }

    [Fact]
    public void Problem_OnlyOneLevelOfUndo()
    {
        var cart = new Problem.Cart();
        var undoable = new Problem.UndoableCart(cart);

        undoable.Add(Book, 2);
        undoable.Add(Mug, 1);
        undoable.Undo(); // removes the mug
        undoable.Undo(); // PAIN: does nothing, the book stays

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, cart.Items);
    }

    [Fact]
    public void Problem_UndoAdd_RestoresTheQuantityBefore()
    {
        var cart = new Problem.Cart();
        var undoable = new Problem.UndoableCart(cart);

        undoable.Add(Book, 2);
        undoable.Add(Book, 1);
        undoable.Undo();

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, cart.Items);
    }

    [Fact]
    public void Problem_UndoRemove_PutsTheProductBack()
    {
        var cart = new Problem.Cart();
        cart.Add(Book, 2);
        var undoable = new Problem.UndoableCart(cart);

        undoable.Remove(Book);
        undoable.Undo();

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, cart.Items);
    }

    [Fact]
    public void ActionsHaveANameForMenusAndLogs()
    {
        Assert.Equal("add 2 x Clean Code", CartActions.Add(new DotNetCart(), Book, 2).Name);
        Assert.Equal("remove Clean Code", CartActions.Remove(new DotNetCart(), Book).Name);
    }
}
