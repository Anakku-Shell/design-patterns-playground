using Patterns.Structural.Proxy.Common;

namespace Patterns.Structural.Proxy.Classic;

// Role: Subject — what the client uses; the real image and its proxy both implement it.
// Guide: §5.7
public interface IProductImage
{
    string FileName { get; }
    byte[] Content { get; }
}

// Role: RealSubject — reads the image as soon as it is created.
public sealed class StoredImage : IProductImage
{
    public StoredImage(ImageStore store, string fileName)
    {
        ArgumentNullException.ThrowIfNull(store);
        FileName = fileName;
        Content = store.Read(fileName);
    }

    public string FileName { get; }
    public byte[] Content { get; }
}

// Role: Proxy (virtual) — looks like an image, but reads it from the store only on first use. Not
// thread-safe: two threads could both read it (Lazy<T>, at the .NET level, is).
public sealed class LazyImageProxy(ImageStore store, string fileName) : IProductImage
{
    private byte[]? _content;

    public string FileName => fileName;
    public byte[] Content => _content ??= store.Read(fileName); // read once, then reuse
}
