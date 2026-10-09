using System.Globalization;
using Patterns.Shop;

namespace Patterns.Modern.DependencyInjection.Problem;

// Guide: §7.1
public sealed class CheckoutService
{
    public string Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // PAIN: a real SMTP sender and the real clock, hard-wired. A test would send a real email and get a
        // different time on every run; nothing can be replaced, and the constructor says it needs nothing.
        var sender = new SmtpEmailSender();
        var text = string.Create(CultureInfo.InvariantCulture,
            $"Order {order.Id.ToString()[..8]} confirmed at {DateTime.Now:HH:mm}");
        sender.Send(order.Customer.Email, text);
        return text;
    }
}

/// <summary>Stands in for a real sender that would open a connection to an SMTP server.</summary>
public sealed class SmtpEmailSender : IEmailSender
{
    public void Send(string recipient, string text)
    {
        // Simulated: a real one would connect to smtp.example.com here, in every test that confirms an order.
    }
}

public interface IEmailSender
{
    void Send(string recipient, string text);
}
