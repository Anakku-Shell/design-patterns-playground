namespace Patterns.Shop;

/// <summary>Where an order is shipped. <c>Country</c> is a two-letter ISO code ("ES"). Guide: §3.8.</summary>
public sealed record Address(string Country, string City, string PostalCode);
