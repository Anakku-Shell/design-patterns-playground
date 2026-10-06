namespace Patterns.Creational.Builder.DotNet;

// Role: Director — drives .NET's UriBuilder, which assembles a valid URI from its parts.
// Guide: §4.4
public static class TrackingLink
{
    // UriBuilder adds the separators (://, /, ?) and the escaping; no string concatenation of URLs.
    public static Uri For(Guid orderId) =>
        new UriBuilder(Uri.UriSchemeHttps, "track.example.com")
        {
            Path = $"orders/{orderId}",
            Query = "lang=en",
        }.Uri;
}
