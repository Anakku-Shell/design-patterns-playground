using System.Globalization;
using Patterns.Shop;

namespace Patterns.Behavioral.Command.DotNet;

// Role: Command — a command as two delegates; the Name is for an "undo …" menu item or a log.
// Guide: §6.2
public sealed record UndoableAction(string Name, Action Do, Action Undo);

// Role: Invoker — the same stack as the Classic CartHistory, holding delegates instead of command classes.
public sealed class ActionHistory
{
    private readonly Stack<UndoableAction> _done = new();

    public void Run(UndoableAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        action.Do();
        _done.Push(action);
    }

    public bool Undo()
    {
        if (!_done.TryPop(out var action)) { return false; }
        action.Undo();
        return true;
    }
}

/// <summary>
/// The cart's commands, written as lambdas. Each lambda captures the cart, the product and what undo must
/// restore (a closure), which is what a ConcreteCommand's fields did.
/// </summary>
public static class CartActions
{
    public static UndoableAction Add(Cart cart, Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(product);
        var before = 0;
        return new UndoableAction(
            string.Create(CultureInfo.InvariantCulture, $"add {quantity} x {product.Name}"),
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

    public static UndoableAction Remove(Cart cart, Product product)
    {
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(product);
        var removed = 0;
        return new UndoableAction(
            $"remove {product.Name}",
            Do: () =>
            {
                removed = cart.Items.GetValueOrDefault(product.Id);
                cart.Remove(product.Id);
            },
            Undo: () =>
            {
                if (removed > 0) { cart.Add(product, removed); }
            });
    }
}
