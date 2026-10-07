using Microsoft.Extensions.Logging;
using Patterns.Demo;
using Patterns.Shop;
using Patterns.Structural.Bridge.Classic;
using Patterns.Structural.Bridge.DotNet;

namespace Patterns.Structural.Bridge;

public sealed class BridgeDemo : IDemo
{
    public string Key => "bridge";
    public string Name => "Bridge";
    public PatternCategory Category => PatternCategory.Structural;
    public Relevance Relevance => Relevance.Niche;
    public string GuideSection => "§5.2";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var orderId = SampleData.OrderOf((SampleData.Book, 2)).Id;

        narrator.Level(1, "Classic");
        narrator.Step("Two kinds of notification × two channels: four combinations from four small classes");
        IMessageChannel[] channels = [new EmailChannel(), new SmsChannel()];
        foreach (var channel in channels)
        {
            narrator.Result(new OrderShippedNotification(channel, orderId).Send(SampleData.Ana));
            narrator.Result(new PaymentFailedNotification(channel, 25.00m).Send(SampleData.Ana));
        }

        narrator.Level(2, ".NET");
        narrator.Step("The code logs through ILogger; our ListLoggerProvider receives the line");
        var provider = new ListLoggerProvider();
        using (var factory = LoggerFactory.Create(logging => logging.AddProvider(provider)))
        {
            var logger = factory.CreateLogger("Checkout");
            CheckoutLog.OrderPlaced(logger, orderId.ToString()[..8]);
        }
        foreach (var line in provider.Lines)
        {
            narrator.Result(line);
        }

        narrator.Takeaway("Two dimensions that vary independently become two hierarchies joined by one reference.");
    }
}
