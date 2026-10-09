using System.Linq.Expressions;
using Patterns.Modern.Specification.Classic;
using Patterns.Modern.Specification.DotNet;
using Patterns.Shop;
using Xunit;
using Problem = Patterns.Modern.Specification.Problem;

namespace Patterns.Modern.Tests;

public sealed class SpecificationTests
{
    private static readonly Problem.ProductFinder Finder = new(SampleData.Products);

    private static string[] Names(IEnumerable<Product> products) => [.. products.Select(p => p.Name)];

    private static string[] Classic(Specification<Product> spec) => Names(SampleData.Products.Where(spec.IsSatisfiedBy));

    private static string[] DotNet(ExpressionSpecification<Product> spec) =>
        Names(SampleData.Products.AsQueryable().Where(spec.ToExpression()));

    [Fact]
    public void InStockAndCheap_IsTheSameInAllLevels()
    {
        Assert.Equal(["Clean Code"], Names(Finder.FindCheapInStock(20m)));
        Assert.Equal(["Clean Code"], Classic(new InStock().And(new CheaperThan(20m))));
        Assert.Equal(["Clean Code"], DotNet(new InStockExpression().And(new CheaperThanExpression(20m))));
    }

    [Fact]
    public void NotInStock_IsTheSameInAllLevels()
    {
        Assert.Equal(["Coffee Mug"], Names(Finder.FindOutOfStock()));
        Assert.Equal(["Coffee Mug"], Classic(new InStock().Not()));
        Assert.Equal(["Coffee Mug"], DotNet(new InStockExpression().Not()));
    }

    [Fact]
    public void BooksOrHome_IsTheSameInAllLevels()
    {
        Assert.Equal(["Clean Code", "Coffee Mug"], Names(Finder.FindBooksOrHome()));
        Assert.Equal(["Clean Code", "Coffee Mug"], Classic(new InCategory("books").Or(new InCategory("home"))));
        Assert.Equal(["Clean Code", "Coffee Mug"], DotNet(new InCategoryExpression("books").Or(new InCategoryExpression("home"))));
    }

    [Fact]
    public void Combinations_Nest()
    {
        // (books or home) and in stock: the mug drops out.
        Assert.Equal(["Clean Code"], Classic(new InCategory("books").Or(new InCategory("home")).And(new InStock())));
        Assert.Equal(["Clean Code"], DotNet(new InCategoryExpression("books").Or(new InCategoryExpression("home")).And(new InStockExpression())));
    }

    [Fact]
    public void InCategory_RejectsANullName()
    {
        Assert.Throws<ArgumentNullException>(() => new InCategory(null!));
        Assert.Throws<ArgumentNullException>(() => new InCategoryExpression(null!));
    }

    [Fact]
    public void DotNet_WorksWithIQueryable()
    {
        IQueryable<Product> table = SampleData.Products.AsQueryable(); // a DbSet<Product> in a real application
        var spec = new InStockExpression().And(new CheaperThanExpression(20m));

        var query = table.Where(spec.ToExpression());

        // The query holds the rule as data (an expression tree a provider can translate), not as compiled code.
        Assert.Contains("Stock", query.Expression.ToString(), StringComparison.Ordinal);
        Assert.Equal(["Clean Code"], Names(query));
    }

    [Fact]
    public void DotNet_CombinedExpression_HasOneParameter()
    {
        var combined = new InStockExpression().And(new CheaperThanExpression(20m)).ToExpression();

        Assert.Single(combined.Parameters);
        Assert.True(combined.Compile()(SampleData.Book));
    }

    [Fact]
    public void JoiningBodies_WithoutReplacingTheParameter_FailsToCompile()
    {
        // What the ExpressionVisitor in the combinators prevents: two lambdas, two different parameters.
        var left = new InStockExpression().ToExpression();
        var right = new CheaperThanExpression(20m).ToExpression();
        var naive = Expression.Lambda<Func<Product, bool>>(Expression.AndAlso(left.Body, right.Body), left.Parameters);

        Assert.Throws<InvalidOperationException>(() => naive.Compile());
    }
}
