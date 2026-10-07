using System.Text;

namespace Patterns.Structural.Facade.DotNet;

// Role: Client of a framework facade — File hides the stream, the encoder and the disposal behind one call.
// Guide: §5.5
public static class InvoiceFile
{
    // UTF-8 without a byte order mark: what File.WriteAllText writes by default.
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    // The facade: one line opens the file, encodes the text, writes it and closes everything.
    public static string SaveSimple(string path, string invoice)
    {
        File.WriteAllText(path, invoice);
        return path;
    }

    // What the facade does for you, step by step: a stream, an encoding, a writer, and their disposal.
    public static string SaveTheLongWay(string path, string invoice)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream, Utf8NoBom);
        writer.Write(invoice);
        return path;
    }

    public static string ReadSimple(string path) => File.ReadAllText(path);

    public static string ReadTheLongWay(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream, Utf8NoBom);
        return reader.ReadToEnd();
    }
}
