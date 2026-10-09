using Microsoft.Extensions.Logging;
using Patterns.Shop;
using Patterns.Structural.Bridge.Classic;
using Patterns.Structural.Bridge.DotNet;
using Xunit;

namespace Patterns.Structural.Tests;

public sealed class BridgeTests
{
    private static readonly Guid OrderId = new("1a2b3c4d-0000-0000-0000-000000000001");

    [Theory]
    [InlineData("shipped", "email", "[email] to ana@example.com | Order shipped | Order 1a2b3c4d is on its way.")]
    [InlineData("shipped", "sms", "[sms] to Ana | Order shipped: Order 1a2b3c4d is on its way.")]
    [InlineData("failed", "email", "[email] to ana@example.com | Payment failed | We could not charge 25.00.")]
    [InlineData("failed", "sms", "[sms] to Ana | Payment failed: We could not charge 25.00.")]
    public void AnyKind_RunsOnAnyChannel(string kind, string channelName, string expected)
    {
        IMessageChannel channel = channelName == "email" ? new EmailChannel() : new SmsChannel();
        Notification notification = kind == "shipped"
            ? new OrderShippedNotification(channel, OrderId)
            : new PaymentFailedNotification(channel, 25.00m);

        Assert.Equal(expected, notification.Send(SampleData.Ana));
    }

    [Fact]
    public void DotNet_LoggerWritesThroughOurProvider()
    {
        var provider = new ListLoggerProvider();
        using (var factory = LoggerFactory.Create(logging => logging.AddProvider(provider)))
        {
            var logger = factory.CreateLogger("Checkout");
            CheckoutLog.OrderPlaced(logger, "1a2b3c4d");
        }

        Assert.Equal(["Checkout: Order 1a2b3c4d placed"], provider.Lines);
    }
}
