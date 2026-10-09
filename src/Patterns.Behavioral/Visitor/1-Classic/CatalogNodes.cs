using Patterns.Shop;

namespace Patterns.Behavioral.Visitor.Classic;

// Role: Element — any node of the catalog tree; it can only accept a visitor.
// Guide: §6.11
public abstract class CatalogNode
{
    public abstract void Accept(ICatalogVisitor visitor);
}

// Role: ConcreteElement — a product; its only job in the pattern is to say "I am a product".
public sealed class ProductNode(Product product) : CatalogNode
{
    public Product Product => product;

    public override void Accept(ICatalogVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this); // this: ProductNode, so the compiler picks Visit(ProductNode)
    }
}

// Role: ConcreteElement — a named bundle of nodes.
public sealed class BundleNode(string name, IReadOnlyList<CatalogNode> children) : CatalogNode
{
    public string Name => name;

    public IReadOnlyList<CatalogNode> Children => children;

    public override void Accept(ICatalogVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        visitor.Visit(this);
    }
}
