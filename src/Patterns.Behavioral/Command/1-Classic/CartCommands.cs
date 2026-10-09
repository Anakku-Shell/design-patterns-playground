using Patterns.Shop;

namespace Patterns.Behavioral.Command.Classic;

// Role: Command — a cart operation that can be done and undone.
// Guide: §6.2
public interface ICartCommand
{
    void Execute();

    void Undo();
}

// Role: ConcreteCommand — adds units, remembering how many there were before.
public sealed class AddItemCommand(Cart cart, Product product, int quantity) : ICartCommand
{
    private int _quantityBefore;

    public void Execute()
    {
        _quantityBefore = cart.Items.GetValueOrDefault(product.Id);
        cart.Add(product, quantity);
    }

    public void Undo()
    {
        cart.Remove(product.Id);
        if (_quantityBefore > 0) { cart.Add(product, _quantityBefore); }
    }
}

// Role: ConcreteCommand — removes a product, remembering how many there were. Takes the product, not just
// its id, so Undo can add it back.
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
        ArgumentNullException.ThrowIfNull(command);
        command.Execute();
        _done.Push(command);
    }

    /// <summary>Undoes the last command still on the stack; false when there is nothing to undo.</summary>
    public bool Undo()
    {
        if (!_done.TryPop(out var command)) { return false; }
        command.Undo();
        return true;
    }
}
