using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod.DotNet;

// The same small shape as the other levels; here the container decides which class each channel gets.
// Guide: §4.2
public enum NotificationChannel
{
    Email,
    Sms,
}

// Role: Product — what the container creates for each channel.
public interface INotifier
{
    string Send(Customer customer, string message);
}

// Role: ConcreteProduct — sends by email.
public sealed class EmailNotifier : INotifier
{
    public string Send(Customer customer, string message) => $"EMAIL to {customer.Email}: {message}";
}

// Role: ConcreteProduct — sends by SMS.
public sealed class SmsNotifier : INotifier
{
    public string Send(Customer customer, string message) => $"SMS to {customer.Name}: {message}";
}
