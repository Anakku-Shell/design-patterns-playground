using Patterns.Structural.Proxy.Common;

namespace Patterns.Structural.Proxy.DotNet;

// Role: Proxy (virtual), ready-made — Lazy<T> runs the read on the first .Value, once, thread-safely.
// Guide: §5.7
public sealed class ProductImage(ImageStore store, string fileName)
{
    private readonly Lazy<byte[]> _content = new(() => store.Read(fileName));

    public string FileName => fileName;
    public byte[] Content => _content.Value;
}
