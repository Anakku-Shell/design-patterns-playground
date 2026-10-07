using System.Globalization;

namespace Patterns.Behavioral.Visitor.Classic;

// Role: Visitor — one method per kind of node: the operation, split by node type.
// Guide: §6.11
public interface ICatalogVisitor
{
    void Visit(ProductNode node);

    void Visit(BundleNode node);
}

// Role: ConcreteVisitor — computes VAT over a whole tree: books 4 %, everything else 21 %.
public sealed class VatVisitor : ICatalogVisitor
{
    private decimal _raw; // summed unrounded; rounded once, at the end

    public decimal Total => decimal.Round(_raw, 2, MidpointRounding.AwayFromZero);

    public void Visit(ProductNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        _raw += node.Product.Price * (node.Product.Category.Name == "Books" ? 0.04m : 0.21m);
    }

    public void Visit(BundleNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        foreach (var child in node.Children)
        {
            child.Accept(this); // walk into the bundle
        }
    }
}

// Role: ConcreteVisitor — an indented outline, two spaces per level.
public sealed class OutlineVisitor : ICatalogVisitor
{
    private readonly List<string> _lines = [];
    private int _depth;

    /// <summary>The lines joined with "\n", without a trailing newline.</summary>
    public string Text => string.Join('\n', _lines);

    public void Visit(ProductNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        _lines.Add(string.Create(CultureInfo.InvariantCulture, $"{Indent}{node.Product.Name} {node.Product.Price:0.00}"));
    }

    public void Visit(BundleNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        _lines.Add($"{Indent}{node.Name}");
        _depth++;
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        _depth--;
    }

    private string Indent => new(' ', _depth * 2);
}
