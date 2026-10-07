using Microsoft.Extensions.DependencyInjection;
using Patterns.Demo;
using Patterns.Modern.DependencyInjection.DotNet;
using Patterns.Shop;

namespace Patterns.Modern.DependencyInjection;

public sealed class DependencyInjectionDemo : IDemo
{
    public string Key => "dependency-injection";
    public string Name => "Dependency Injection";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Essential;
    public string GuideSection => "§7.1";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);
        var order = SampleData.OrderOf((SampleData.Book, 2)) with { Id = new Guid("1a2b3c4d-0000-0000-0000-000000000000") };
        var clock = new TenThirty(); // a fixed clock, so the demo prints the same text on every run

        narrator.Level(0, "Problem");
        narrator.Step("new Problem.CheckoutService(): it creates an SMTP sender and reads DateTime.Now itself");
        narrator.Result(new Problem.CheckoutService().Confirm(order) + "  (the real clock: this line changes every minute)");
        narrator.Step("LocatorCheckoutService: constructed fine, then asks a global registry inside Confirm");
        try
        {
            new Problem.LocatorCheckoutService().Confirm(order);
        }
        catch (InvalidOperationException e)
        {
            narrator.Result(e.Message);
        }
        narrator.Step("register both in the global locator first, and it works: until some code forgets to");
        Problem.ServiceLocator.Register<Problem.IEmailSender>(new Problem.SmtpEmailSender());
        Problem.ServiceLocator.Register<TimeProvider>(clock);
        narrator.Result(new Problem.LocatorCheckoutService().Confirm(order));
        Problem.ServiceLocator.Clear(); // global state: leave it as it was found

        narrator.Level(1, "Classic");
        narrator.Step("CompositionRoot.CreateCheckout(new FakeEmailSender(), clock): the dependencies are constructor parameters");
        var sender = new Classic.FakeEmailSender();
        narrator.Result(Classic.CompositionRoot.CreateCheckout(sender, clock).Confirm(order));
        narrator.Result($"sent: {sender.Sent[0]}");

        narrator.Level(2, ".NET");
        narrator.Step("ServiceCollection: singleton clock, scoped sender, transient checkout; ValidateScopes and ValidateOnBuild on");
        using var provider = CheckoutContainer.Build(clock);
        using (var scope = provider.CreateScope())
        {
            narrator.Result(scope.ServiceProvider.GetRequiredService<DotNet.CheckoutService>().Confirm(order));
        }
        narrator.Step("resolving the scoped IEmailSender from the root provider");
        try
        {
            provider.GetRequiredService<DotNet.IEmailSender>();
        }
        catch (InvalidOperationException e)
        {
            narrator.Result(e.Message);
        }

        narrator.Takeaway("A class declares what it needs in its constructor; one place (the composition root) decides what it gets.");
    }

    private sealed class TenThirty : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(2026, 10, 7, 10, 30, 0, TimeSpan.Zero);
    }
}
