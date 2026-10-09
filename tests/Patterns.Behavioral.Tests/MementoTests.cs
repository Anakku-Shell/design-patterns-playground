using System.Reflection;
using Patterns.Behavioral.Memento.Classic;
using Patterns.Behavioral.Memento.DotNet;
using Patterns.Shop;
using Xunit;

namespace Patterns.Behavioral.Tests;

public sealed class MementoTests
{
    private static readonly Product Book = SampleData.Book;
    private static readonly Product Mug = SampleData.Mug;

    [Fact]
    public void Restore_GivesBackThePreviousItems()
    {
        var cart = new Cart();
        var caretaker = new CartCaretaker(cart);
        cart.Add(Book, 2);
        caretaker.Save();
        cart.Add(Mug, 1);
        cart.Remove(Book.Id);

        Assert.True(caretaker.Undo());

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, cart.Items);
    }

    [Fact]
    public void Undo_GoesBackOneSaveAtATime()
    {
        var cart = new Cart();
        var caretaker = new CartCaretaker(cart);
        caretaker.Save();          // empty
        cart.Add(Book, 2);
        caretaker.Save();          // 2 books
        cart.Add(Book, 1);

        caretaker.Undo();
        Assert.Equal(2, cart.Items[Book.Id]);
        caretaker.Undo();
        Assert.Empty(cart.Items);
    }

    [Fact]
    public void Snapshot_IsNotChangedByLaterEdits()
    {
        var cart = new Cart();
        cart.Add(Book, 2);
        var snapshot = cart.CreateSnapshot();

        cart.Add(Book, 5);
        cart.Add(Mug, 1);
        cart.Restore(snapshot);

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, cart.Items);
    }

    [Fact]
    public void Snapshot_ShowsNothingPublic()
    {
        const BindingFlags Public = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        Assert.Empty(typeof(CartSnapshot).GetProperties(Public));
        Assert.Empty(typeof(CartSnapshot).GetConstructors(Public));
    }

    [Fact]
    public void Undo_WithNothingSaved_ReturnsFalse()
    {
        var cart = new Cart();
        cart.Add(Book, 1);

        Assert.False(new CartCaretaker(cart).Undo());
        Assert.Equal(1, cart.Items[Book.Id]); // untouched
    }

    [Fact]
    public void DotNet_OldStatesStayIntact()
    {
        var twoBooks = CartState.Empty.Add(Book, 2);

        var withMug = twoBooks.Add(Mug, 1);
        var withoutBooks = withMug.Remove(Book.Id);

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, twoBooks.Items);
        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2, [Mug.Id] = 1 }, withMug.Items);
        Assert.Equal(new Dictionary<Guid, int> { [Mug.Id] = 1 }, withoutBooks.Items);
        Assert.Empty(CartState.Empty.Items);
    }

    [Fact]
    public void DotNet_UndoIsPoppingTheLastState()
    {
        var history = new Stack<CartState>();
        var cart = CartState.Empty;
        history.Push(cart);
        cart = cart.Add(Book, 2);
        history.Push(cart);
        cart = cart.Add(Mug, 1);

        cart = history.Pop();

        Assert.Equal(new Dictionary<Guid, int> { [Book.Id] = 2 }, cart.Items);
    }
}
