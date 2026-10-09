using Patterns.Structural.Proxy.Common;

namespace Patterns.Structural.Proxy.Problem;

// Guide: §5.7
public sealed class ProductPage(ImageStore store, IEnumerable<string> imageFiles)
{
    // PAIN: every image is read when the page is built, even those nobody looks at.
    private readonly Dictionary<string, byte[]> _images = imageFiles.ToDictionary(file => file, store.Read);

    public byte[] Image(string fileName) => _images[fileName];
}
