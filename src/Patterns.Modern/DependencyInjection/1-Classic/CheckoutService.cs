using System.Globalization;
using Patterns.Shop;

namespace Patterns.Modern.DependencyInjection.Classic;

// Role: Client — says what it needs in its constructor; never creates or looks up a dependency.
// Guide: §7.1
public sealed class CheckoutService(IEmailSender sender, TimeProvider clock)
{
    public string Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var text = string.Create(CultureInfo.InvariantCulture,
            $"Order {order.Id.ToString()[..8]} confirmed at {clock.GetUtcNow():HH:mm}");
        sender.Send(order.Customer.Email, text);
        return text;
    }
}

// Role: Injector — the composition root, written by hand ("pure DI").
// Production: CreateCheckout(new SmtpEmailSender(…), TimeProvider.System); a test passes fakes.
public static class CompositionRoot
{
    public static CheckoutService CreateCheckout(IEmailSender sender, TimeProvider clock) => new(sender, clock);
}
