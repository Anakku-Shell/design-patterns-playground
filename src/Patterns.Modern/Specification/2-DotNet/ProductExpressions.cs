using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Patterns.Shop;

namespace Patterns.Modern.Specification.DotNet;

// Role: Leaf specification — there is at least one unit.
// Guide: §7.5
public sealed class InStockExpression : ExpressionSpecification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression() => p => p.Stock > 0;
}

// Role: Leaf specification — the product belongs to a category, ignoring case.
public sealed class InCategoryExpression : ExpressionSpecification<Product>
{
    private readonly string _wanted;

    public InCategoryExpression(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        _wanted = name.ToUpperInvariant();
    }

    [SuppressMessage("Performance", "CA1862", Justification = "An IQueryable provider translates a case conversion to SQL, not a StringComparison overload.")]
    public override Expression<Func<Product, bool>> ToExpression()
    {
        var wanted = _wanted; // captured as a value: a SQL parameter, once translated
        return p => p.Category.Name.ToUpperInvariant() == wanted;
    }
}

// Role: Leaf specification — the price is below a limit.
public sealed class CheaperThanExpression(decimal limit) : ExpressionSpecification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression() => p => p.Price < limit;
}
