using System.Globalization;
using System.Text;

namespace Patterns.Structural.Adapter.DotNet;

// Role: Client of a framework adapter — StreamReader adapts a Stream (bytes) to a TextReader (lines).
// Guide: §5.1
public static class CarrierRateFile
{
    // The carrier's rate file has one "<country>;<days>" per line, for example "ES;2\nPT;4".
    public static IReadOnlyDictionary<string, int> ReadDeliveryDays(Stream file)
    {
        // Adapter: bytes → text lines. leaveOpen: the caller owns the stream and disposes it.
        using var reader = new StreamReader(file, Encoding.UTF8, leaveOpen: true);
        var days = new Dictionary<string, int>();
        while (reader.ReadLine() is { } line)
        {
            var parts = line.Split(';');
            days[parts[0]] = int.Parse(parts[1], CultureInfo.InvariantCulture);
        }
        return days;
    }
}
