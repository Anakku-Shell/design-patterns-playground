using Microsoft.Extensions.DependencyInjection;
using Patterns.Creational.FactoryMethod.DotNet;
using Patterns.Shop;
using Xunit;
using Classic = Patterns.Creational.FactoryMethod.Classic;
using DotNet = Patterns.Creational.FactoryMethod.DotNet;
using Problem = Patterns.Creational.FactoryMethod.Problem;

namespace Patterns.Creational.Tests;

public sealed class FactoryMethodTests
{
    private const string Message = "Your order has shipped";

    [Fact]
    public void EveryLevel_SendsTheSameEmail()
    {
        const string expected = "EMAIL to ana@example.com: Your order has shipped";

        Assert.Equal(expected, new Problem.NotificationService().Notify(Problem.NotificationChannel.Email, SampleData.Ana, Message));
        Assert.Equal(expected, new Classic.EmailCampaign().Notify(SampleData.Ana, Message));
        using var provider = DotNetProvider();
        Assert.Equal(expected, provider.GetRequiredService<DotNet.NotificationService>().Notify(DotNet.NotificationChannel.Email, SampleData.Ana, Message));
    }

    [Fact]
    public void EveryLevel_SendsTheSameSms()
    {
        const string expected = "SMS to Ana: Your order has shipped";

        Assert.Equal(expected, new Problem.NotificationService().Notify(Problem.NotificationChannel.Sms, SampleData.Ana, Message));
        Assert.Equal(expected, new Classic.SmsCampaign().Notify(SampleData.Ana, Message));
        using var provider = DotNetProvider();
        Assert.Equal(expected, provider.GetRequiredService<DotNet.NotificationService>().Notify(DotNet.NotificationChannel.Sms, SampleData.Ana, Message));
    }

    [Fact]
    public void Classic_EachCreatorDecidesItsProduct()
    {
        // The factory method is protected by design; what each creator built shows in what it sends.
        Classic.NotificationCampaign email = new Classic.EmailCampaign();
        Classic.NotificationCampaign sms = new Classic.SmsCampaign();

        Assert.StartsWith("EMAIL", email.Notify(SampleData.Ana, Message), StringComparison.Ordinal);
        Assert.StartsWith("SMS", sms.Notify(SampleData.Ana, Message), StringComparison.Ordinal);
    }

    [Fact]
    public void DotNet_UnregisteredChannel_Throws()
    {
        using var provider = DotNetProvider();
        var service = provider.GetRequiredService<DotNet.NotificationService>();

        Assert.Throws<InvalidOperationException>(() => service.Notify((DotNet.NotificationChannel)99, SampleData.Ana, Message));
    }

    [Fact]
    public void Problem_UnknownChannel_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Problem.NotificationService().Notify((Problem.NotificationChannel)99, SampleData.Ana, Message));
    }

    private static ServiceProvider DotNetProvider() => new ServiceCollection().AddNotifiers().BuildServiceProvider();
}
