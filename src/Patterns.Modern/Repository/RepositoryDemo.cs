using Patterns.Demo;
using Patterns.Modern.Repository.Classic;
using Patterns.Modern.Repository.DotNet;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Modern.Repository;

public sealed class RepositoryDemo : IDemo
{
    public string Key => "repository";
    public string Name => "Repository";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§7.3";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("CatalogService filters a list of products (the \"table\") inside every business method");
        var problem = new Problem.CatalogService([.. SampleData.Products]);
        narrator.Result($"books: {Names(problem.InCategory("books"))}");
        narrator.Step("AddProduct(Clean Code) a second time: nothing stops it");
        problem.AddProduct(SampleData.Book);
        narrator.Result($"books: {Names(problem.InCategory("books"))}");

        narrator.Level(1, "Classic");
        narrator.Step("CatalogService(IProductRepository): the business code asks for \"products in a category\"");
        var repository = new InMemoryProductRepository();
        foreach (var product in SampleData.Products)
        {
            repository.Add(product);
        }
        var catalog = new CatalogService(repository);
        narrator.Result($"books: {Names(catalog.InCategory("books"))}");
        narrator.Result(Invariant($"price of the headphones: {catalog.PriceOf(SampleData.Headphones.Id):0.00}"));
        narrator.Step("repository.Add(Clean Code) a second time");
        try
        {
            repository.Add(SampleData.Book);
        }
        catch (InvalidOperationException e)
        {
            narrator.Result(e.Message);
        }

        narrator.Level(2, ".NET");
        narrator.Step("ProductQueries(IQueryable<Product>): the shape DbSet<Product> gives you, here over a list");
        var queries = new ProductQueries(SampleData.Products.AsQueryable());
        narrator.Result($"books: {Names(queries.InCategory("books"))}");

        narrator.Takeaway("Business code asks a collection-like interface in domain words; with EF Core, DbSet<T> already plays that role.");
    }

    private static string Names(IEnumerable<Product> products) => string.Join(", ", products.Select(p => p.Name));
}
