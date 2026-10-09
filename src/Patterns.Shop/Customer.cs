namespace Patterns.Shop;

/// <summary>Who buys. A guest bought without an account and has no email. Guide: §3.8.</summary>
public sealed record Customer(Guid Id, string Name, string Email, bool IsGuest);
