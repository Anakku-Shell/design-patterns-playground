namespace Patterns.Modern.DependencyInjection.Classic;

// Role: Service — the abstraction the client depends on.
// Guide: §7.1
public interface IEmailSender
{
    void Send(string recipient, string text);
}

// Role: ConcreteService — records what it would send, for tests and the demo.
public sealed class FakeEmailSender : IEmailSender
{
    private readonly List<string> _sent = [];

    public IReadOnlyList<string> Sent => _sent;

    public void Send(string recipient, string text) => _sent.Add($"{recipient}: {text}");
}
