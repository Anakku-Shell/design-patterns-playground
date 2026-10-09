namespace Patterns.Shop;

/// <summary>
/// The same products, customers and addresses for every demo and test. Ids are fixed so output is stable
/// from one run to the next; only orders built with <see cref="OrderOf"/> get a new id. Prices are chosen so
/// examples are easy to check by hand: 4 books are exactly 50.00, 8 books exactly 100.00. Guide: §3.8.
/// </summary>
public static class SampleData
{
    public static Category Books { get; } = new("Books");
    public static Category Electronics { get; } = new("Electronics");
    public static Category Home { get; } = new("Home");

    public static Product Book { get; } =
        new(new Guid("b0000000-0000-0000-0000-000000000001"), "Clean Code", "BOOK-001", 12.50m, Books, 20);

    public static Product Headphones { get; } =
        new(new Guid("e0000000-0000-0000-0000-000000000001"), "Wireless Headphones", "ELEC-001", 59.90m, Electronics, 5);

    // Out of stock on purpose: the "not enough stock" case of validation and specifications.
    public static Product Mug { get; } =
        new(new Guid("40000000-0000-0000-0000-000000000001"), "Coffee Mug", "HOME-001", 8.00m, Home, 0);

    public static IReadOnlyList<Product> Products { get; } = [Book, Headphones, Mug];

    public static Customer Ana { get; } =
        new(new Guid("c0000000-0000-0000-0000-000000000001"), "Ana", "ana@example.com", IsGuest: false);

    public static Customer Guest { get; } =
        new(new Guid("c0000000-0000-0000-0000-000000000002"), "Guest", "", IsGuest: true);

    public static Address Madrid { get; } = new("ES", "Madrid", "28001");

    public static Address Lisbon { get; } = new("PT", "Lisbon", "1100-148");

    /// <summary>A new draft order for Ana, shipped to Madrid.</summary>
    public static Order OrderOf(params (Product Product, int Quantity)[] lines) =>
        new(Guid.NewGuid(), Ana, [.. lines.Select(l => new OrderLine(l.Product, l.Quantity))], Madrid);
}
