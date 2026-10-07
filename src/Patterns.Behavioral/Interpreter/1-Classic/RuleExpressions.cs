using Patterns.Shop;

namespace Patterns.Behavioral.Interpreter.Classic;

// Role: AbstractExpression — any piece of a discount rule.
// Guide: §6.3
public interface IRuleExpression
{
    bool Interpret(Order order);
}

// Role: TerminalExpression — "total > n" or "total >= n".
public sealed record TotalGreaterThan(decimal Amount, bool OrEqual) : IRuleExpression
{
    public bool Interpret(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return OrEqual ? order.Total >= Amount : order.Total > Amount;
    }
}

// Role: TerminalExpression — "category = 'x'": some line is in that category.
public sealed record HasCategory(string Name) : IRuleExpression
{
    public bool Interpret(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.Lines.Any(l => string.Equals(l.Product.Category.Name, Name, StringComparison.OrdinalIgnoreCase));
    }
}

// Role: NonterminalExpression — both sides must hold.
// Named AndExpression, not And: "And" is a Visual Basic keyword and analyzer CA1716 rejects it as a type name.
public sealed record AndExpression(IRuleExpression Left, IRuleExpression Right) : IRuleExpression
{
    public bool Interpret(Order order) => Left.Interpret(order) && Right.Interpret(order);
}
