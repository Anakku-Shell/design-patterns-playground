using System.Globalization;

namespace Patterns.Structural.Adapter.External;

/// <summary>
/// The carrier's SDK, simulated: "someone else's code" that every level of the pattern shares and nobody may
/// change. It speaks its own text protocol: payload "&lt;country&gt;;&lt;units&gt;;&lt;totalCents&gt;", answer
/// "PRICE=4.00;DAYS=2". Price is 3.00 + 0.50 per unit; 2 days to Spain, 4 anywhere else. Guide: §5.1.
/// </summary>
// Role: Adaptee — the existing class whose interface does not fit ours.
// Guide: §5.1
public sealed class LegacyCarrierClient
{
    /// <summary>The last payload received, so tests and the demo can see exactly what was sent.</summary>
    public string? LastPayload { get; private set; }

    public string RequestQuote(string payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        LastPayload = payload;

        var parts = payload.Split(';');
        if (parts.Length != 3
            || !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var units)
            || !int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out _))
        {
            throw new FormatException($"Malformed quote request '{payload}'.");
        }

        var price = 3.00m + 0.50m * units;
        var days = parts[0] == "ES" ? 2 : 4;
        return string.Create(CultureInfo.InvariantCulture, $"PRICE={price:0.00};DAYS={days}");
    }
}
