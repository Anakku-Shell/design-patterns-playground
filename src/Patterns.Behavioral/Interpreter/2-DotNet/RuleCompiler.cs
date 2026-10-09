using System.Linq.Expressions;
using Patterns.Behavioral.Interpreter.Classic;
using Patterns.Shop;

namespace Patterns.Behavioral.Interpreter.DotNet;

/// <summary>
/// Compiles a discount rule into a delegate: the Classic parser builds the tree, this class translates it once
/// into a .NET expression tree and compiles that to IL. The one level in the repo that uses another level's
/// types, on purpose: its input is the Classic tree. Guide: §6.3.
/// </summary>
public static class RuleCompiler
{
    public static Func<Order, bool> Compile(string rule)
    {
        var order = Expression.Parameter(typeof(Order), "order");
        var body = Translate(DiscountRuleParser.Parse(rule), order);
        return Expression.Lambda<Func<Order, bool>>(body, order).Compile(); // IL, as if hand-written
    }

    private static Expression Translate(IRuleExpression node, ParameterExpression order) => node switch
    {
        TotalGreaterThan { OrEqual: true } t => Expression.GreaterThanOrEqual(Total(order), Expression.Constant(t.Amount)),
        TotalGreaterThan t => Expression.GreaterThan(Total(order), Expression.Constant(t.Amount)),
        HasCategory c => AnyLineIn(order, c.Name),
        AndExpression a => Expression.AndAlso(Translate(a.Left, order), Translate(a.Right, order)),
        _ => throw new NotSupportedException(node.GetType().Name),
    };

    private static MemberExpression Total(ParameterExpression order) => Expression.Property(order, nameof(Order.Total));

    // order.Lines.Any(line => string.Equals(line.Product.Category.Name, name, StringComparison.OrdinalIgnoreCase))
    private static MethodCallExpression AnyLineIn(ParameterExpression order, string name)
    {
        var line = Expression.Parameter(typeof(OrderLine), "line");
        var category = Expression.Property(
            Expression.Property(Expression.Property(line, nameof(OrderLine.Product)), nameof(Product.Category)),
            nameof(Category.Name));
        var sameName = Expression.Call(
            typeof(string), nameof(string.Equals), null,
            category, Expression.Constant(name), Expression.Constant(StringComparison.OrdinalIgnoreCase));
        var predicate = Expression.Lambda<Func<OrderLine, bool>>(sameName, line);
        return Expression.Call(
            typeof(Enumerable), nameof(Enumerable.Any), [typeof(OrderLine)],
            Expression.Property(order, nameof(Order.Lines)), predicate);
    }
}
