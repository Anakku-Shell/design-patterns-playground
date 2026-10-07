using Patterns.Shop;

namespace Patterns.Modern.Specification.Classic;

// Role: Leaf specification — there is at least one unit.
// Guide: §7.5
public sealed class InStock : Specification<Product>
{
    public override bool IsSatisfiedBy(Product candidate)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        return candidate.Stock > 0;
    }
}

// Role: Leaf specification — the product belongs to a category, ignoring case.
public sealed class InCategory : Specification<Product>
{
    private readonly string _name;

    public InCategory(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        _name = name;
    }

    public override bool IsSatisfiedBy(Product candidate)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        return candidate.Category.Name.Equals(_name, StringComparison.OrdinalIgnoreCase);
    }
}

// Role: Leaf specification — the price is below a limit.
public sealed class CheaperThan(decimal limit) : Specification<Product>
{
    public override bool IsSatisfiedBy(Product candidate)
    {
        ArgumentNullException.ThrowIfNull(candidate);
        return candidate.Price < limit;
    }
}
