using Microsoft.Extensions.DependencyInjection;
using Patterns.Creational.FactoryMethod.DotNet;
using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Creational.FactoryMethod;

public sealed class FactoryMethodDemo : IDemo
{
    private const string Message = "Your order has shipped";

    public string Key => "factory-method";
    public string Name => "Factory Method";
    public PatternCategory Category => PatternCategory.Creational;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§4.2";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("One service with a switch that news the notifier for each channel");
        var problem = new Problem.NotificationService();
        narrator.Result(problem.Notify(Problem.NotificationChannel.Email, SampleData.Ana, Message));
        narrator.Result(problem.Notify(Problem.NotificationChannel.Sms, SampleData.Ana, Message));

        narrator.Level(1, "Classic");
        narrator.Step("NotificationCampaign.Notify calls CreateNotifier(); each subclass decides the class");
        narrator.Result(new Classic.EmailCampaign().Notify(SampleData.Ana, Message));
        narrator.Result(new Classic.SmsCampaign().Notify(SampleData.Ana, Message));

        narrator.Level(2, ".NET");
        narrator.Step("Notifiers registered as keyed services; the container creates the one for each key");
        using var provider = new ServiceCollection().AddNotifiers().BuildServiceProvider();
        var service = provider.GetRequiredService<NotificationService>();
        narrator.Result(service.Notify(NotificationChannel.Email, SampleData.Ana, Message));
        narrator.Result(service.Notify(NotificationChannel.Sms, SampleData.Ana, Message));

        narrator.Takeaway("The code that sends no longer names a concrete notifier: a subclass or a registration does.");
    }
}
