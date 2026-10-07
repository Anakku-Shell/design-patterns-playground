using Patterns.Shop;

namespace Patterns.Behavioral.Command.Problem;

// Guide: §6.2
public sealed class UndoableCart(Cart cart)
{
    private string? _lastAction;
    private Product? _lastProduct;
    private int _lastQuantity;

    public void Add(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        var before = cart.Items.GetValueOrDefault(product.Id); // what undo must restore
        cart.Add(product, quantity);
        (_lastAction, _lastProduct, _lastQuantity) = ("add", product, before);
    }

    public void Remove(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        var quantity = cart.Items.GetValueOrDefault(product.Id);
        cart.Remove(product.Id);
        (_lastAction, _lastProduct, _lastQuantity) = ("remove", product, quantity);
    }

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
