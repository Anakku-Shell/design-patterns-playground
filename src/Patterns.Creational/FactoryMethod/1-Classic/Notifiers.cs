using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod.Classic;

// Role: Product — what the factory method returns.
// Guide: §4.2
public interface INotifier
{
    string Send(Customer customer, string message);
}

// Role: ConcreteProduct — sends by email.
public sealed class EmailNotifier : INotifier
{
    public string Send(Customer customer, string message) => $"EMAIL to {customer.Email}: {message}";
}

// Role: ConcreteProduct — sends by SMS (by name here; a real one would use a phone number).
public sealed class SmsNotifier : INotifier
{
    public string Send(Customer customer, string message) => $"SMS to {customer.Name}: {message}";
}
