using Patterns.Shop;

namespace Patterns.Modern.NullObject.Classic;

// Role: AbstractObject — what the greeting needs to know about a customer.
// Guide: §7.7
public interface ICustomerProfile
{
    string DisplayName { get; }
}

// Role: RealObject — a customer with an account and a name.
public sealed class RegisteredCustomer(Customer customer) : ICustomerProfile
{
    public string DisplayName => customer.Name;
}

// Role: NullObject — the guest: no account, a neutral name.
public sealed class GuestCustomer : ICustomerProfile
{
    private GuestCustomer()
    {
    }

    public static GuestCustomer Instance { get; } = new();

    public string DisplayName => "guest";
}

/// <summary>The one place that decides "registered or guest"; everything after it has no branch.</summary>
public static class CustomerProfiles
{
    public static ICustomerProfile For(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return customer.IsGuest ? GuestCustomer.Instance : new RegisteredCustomer(customer);
    }
}

public static class Greeter
{
    public static string GreetingFor(ICustomerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return $"Hello, {profile.DisplayName}"; // no "if guest" branch
    }
}
