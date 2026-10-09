using Microsoft.Extensions.DependencyInjection;
using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod.DotNet;

// Role: Client — sends notifications; the container is the factory, picking the notifier class by key.
// Guide: §4.2
public sealed class NotificationService(IServiceProvider services)
{
    public string Notify(NotificationChannel channel, Customer customer, string message)
    {
        // Asking the provider is justified here: the key is only known at run time. An unregistered key
        // throws InvalidOperationException.
        var notifier = services.GetRequiredKeyedService<INotifier>(channel);
        return notifier.Send(customer, message);
    }
}

// Guide: §4.2
public static class NotificationServiceCollectionExtensions
{
    // One line per channel: a new channel is a new class plus one registration; NotificationService does not change.
    public static IServiceCollection AddNotifiers(this IServiceCollection services) =>
        services
            .AddKeyedSingleton<INotifier, EmailNotifier>(NotificationChannel.Email)
            .AddKeyedSingleton<INotifier, SmsNotifier>(NotificationChannel.Sms)
            .AddSingleton<NotificationService>();
}
