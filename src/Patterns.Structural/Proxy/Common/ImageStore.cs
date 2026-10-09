namespace Patterns.Structural.Proxy.Common;

/// <summary>
/// Where product images live (simulated: a disk or a remote store would be slow). Shared by every level, and
/// it counts its reads so tests and the demo can see when an image is actually loaded. Guide: §5.7.
/// </summary>
public sealed class ImageStore
{
    public int Reads { get; private set; }

    public byte[] Read(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        Reads++;
        return new byte[1024];
    }
}

/// <summary>Who is using the admin pages. Shared by every level. Guide: §5.7.</summary>
public sealed record User(string Name, bool IsAdmin);
