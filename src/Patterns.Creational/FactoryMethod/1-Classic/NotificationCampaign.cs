using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod.Classic;

// Role: Creator — the notification logic, with the "which notifier" step left open.
// Guide: §4.2
public abstract class NotificationCampaign
{
    public string Notify(Customer customer, string message)
    {
        var notifier = CreateNotifier(); // the factory method: subclasses decide the class
        return notifier.Send(customer, message);
    }

    protected abstract INotifier CreateNotifier();
}

// Role: ConcreteCreator — campaigns that reach the customer by email.
public sealed class EmailCampaign : NotificationCampaign
{
    protected override INotifier CreateNotifier() => new EmailNotifier();
}

// Role: ConcreteCreator — campaigns that reach the customer by SMS.
public sealed class SmsCampaign : NotificationCampaign
{
    protected override INotifier CreateNotifier() => new SmsNotifier();
}
