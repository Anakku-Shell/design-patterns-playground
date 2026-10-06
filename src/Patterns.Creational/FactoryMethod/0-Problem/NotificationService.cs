using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod.Problem;

// Guide: §4.2
public sealed class NotificationService
{
    public string Notify(NotificationChannel channel, Customer customer, string message)
    {
        // PAIN: the service knows every concrete notifier, and every new channel edits this method.
        INotifier notifier = channel switch
        {
            NotificationChannel.Email => new EmailNotifier(),
            NotificationChannel.Sms => new SmsNotifier(),
            _ => throw new ArgumentOutOfRangeException(nameof(channel)),
        };
        return notifier.Send(customer, message);
    }
}
