using System.Linq.Expressions;
using Patterns.Shop;

namespace Patterns.Behavioral.Visitor.DotNet;

/// <summary>A closed hierarchy of records: the elements know nothing about the operations. Guide: §6.11.</summary>
public abstract record CatalogItem;

public sealed record ProductItem(Product Product) : CatalogItem;

public sealed record BundleItem(string Name, IReadOnlyList<CatalogItem> Children) : CatalogItem;

/// <summary>
/// The modern alternative to a visitor: an operation is a function whose <c>switch</c> expression does the
/// "dispatch on the node type". Guide: §6.11.
/// </summary>
public static class VatCalculator
{
    public static decimal VatOf(CatalogItem item) =>
        decimal.Round(RawVat(item), 2, MidpointRounding.AwayFromZero);

    private static decimal RawVat(CatalogItem item) => item switch
    {
        ProductItem { Product.Category.Name: "Books" } p => p.Product.Price * 0.04m,
        ProductItem p => p.Product.Price * 0.21m,
        BundleItem b => b.Children.Sum(RawVat),
        // C# does not check that a switch covers every subtype: a new record type lands here at run time.
        _ => throw new NotSupportedException(item?.GetType().Name),
    };
}

// Role: ConcreteVisitor — the BCL's visitor for expression trees; overrides only the node kind it cares about.
public sealed class ConstantCollector : ExpressionVisitor
{
    private readonly List<object?> _constants = [];

    public IReadOnlyList<object?> Constants => _constants;

    protected override Expression VisitConstant(ConstantExpression node)
    {
        ArgumentNullException.ThrowIfNull(node);
        _constants.Add(node.Value);
        return base.VisitConstant(node);
    }
}
