using Patterns.Modern.Repository.Classic;
using Patterns.Modern.Repository.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Modern.Repository.Classic;
using Problem = Patterns.Modern.Repository.Problem;

namespace Patterns.Modern.Tests;

public sealed class RepositoryTests
{
    private static InMemoryProductRepository Seeded()
    {
        var repository = new InMemoryProductRepository();
        foreach (var product in SampleData.Products)
        {
            repository.Add(product);
        }
        return repository;
    }

    private static string[] Names(IEnumerable<Product> products) => [.. products.Select(p => p.Name)];

    [Fact]
    public void AllLevels_FindTheBooks()
    {
        var problem = new Problem.CatalogService([.. SampleData.Products]);
        var classic = new Classic.CatalogService(Seeded());
        var dotnet = new ProductQueries(SampleData.Products.AsQueryable());

        Assert.Equal(["Clean Code"], Names(problem.InCategory("books")));
        Assert.Equal(["Clean Code"], Names(classic.InCategory("books")));
        Assert.Equal(["Clean Code"], Names(dotnet.InCategory("books")));
    }

    [Fact]
    public void ProblemAndClassic_PriceAProductTheSame()
    {
        var id = SampleData.Headphones.Id;

        Assert.Equal(59.90m, new Problem.CatalogService([.. SampleData.Products]).PriceOf(id));
        Assert.Equal(59.90m, new Classic.CatalogService(Seeded()).PriceOf(id));
    }

    [Fact]
    public void GetById_Unknown_ReturnsNull()
    {
        Assert.Null(Seeded().GetById(Guid.Empty));
        Assert.Same(SampleData.Mug, Seeded().GetById(SampleData.Mug.Id));
    }

    [Fact]
    public void Add_Duplicate_Throws()
    {
        var repository = Seeded();

        var error = Assert.Throws<InvalidOperationException>(() => repository.Add(SampleData.Book));

        Assert.Equal("Product b0000000-0000-0000-0000-000000000001 already exists.", error.Message);
    }

    [Fact]
    public void ListByCategory_ReturnsAMaterialisedList()
    {
        // A list, not a lazy query: adding a book afterwards does not change what the caller already holds.
        var repository = Seeded();
        var books = repository.ListByCategory("BOOKS");

        repository.Add(SampleData.Book with { Id = Guid.NewGuid(), Name = "Refactoring" });

        Assert.Equal(["Clean Code"], Names(books));
        Assert.Equal(2, repository.ListByCategory("books").Count);
    }

    [Fact]
    public void DotNet_IsAQueryOverIQueryable()
    {
        var source = SampleData.Products.AsQueryable();

        var books = new ProductQueries(source).InCategory("Books");

        Assert.Equal(["Clean Code"], Names(books));
        Assert.Empty(new ProductQueries(source).InCategory("toys"));
    }
}
