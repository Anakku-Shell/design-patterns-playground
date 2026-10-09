using Patterns.Shop;

namespace Patterns.Structural.Bridge.Classic;

// Role: Implementor — how a message physically reaches a customer.
// Guide: §5.2
public interface IMessageChannel
{
    string Deliver(Customer customer, string title, string body);
}

// Role: ConcreteImplementor — email: addressed to the customer's email.
public sealed class EmailChannel : IMessageChannel
{
    public string Deliver(Customer customer, string title, string body)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return $"[email] to {customer.Email} | {title} | {body}";
    }
}

// Role: ConcreteImplementor — SMS: short, addressed by name (a real one would use a phone number).
public sealed class SmsChannel : IMessageChannel
{
    public string Deliver(Customer customer, string title, string body)
    {
        ArgumentNullException.ThrowIfNull(customer);
        return $"[sms] to {customer.Name} | {title}: {body}";
    }
}
