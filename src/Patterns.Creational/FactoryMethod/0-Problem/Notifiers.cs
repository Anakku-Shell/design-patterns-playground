using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod.Problem;

// The same small shape in every level (levels never share the pattern's own types). Guide: §4.2
public enum NotificationChannel
{
    Email,
    Sms,
}

public interface INotifier
{
    string Send(Customer customer, string message);
}

public sealed class EmailNotifier : INotifier
{
    public string Send(Customer customer, string message) => $"EMAIL to {customer.Email}: {message}";
}

public sealed class SmsNotifier : INotifier
{
    public string Send(Customer customer, string message) => $"SMS to {customer.Name}: {message}";
}
