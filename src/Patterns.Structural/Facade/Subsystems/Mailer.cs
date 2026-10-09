using Patterns.Shop;

namespace Patterns.Structural.Facade.Subsystems;

/// <summary>Sends emails (here, keeps their text in a list). A simulated subsystem, shared by every level. Guide: §5.5.</summary>
// Role: Subsystem — tells the customer.
// Guide: §5.5
public sealed class Mailer
{
    private readonly List<string> _sent = [];

    public IReadOnlyList<string> Sent => _sent;

    public void Send(Customer to, string text)
    {
        ArgumentNullException.ThrowIfNull(to);
        _sent.Add(text);
    }
}
