namespace Patterns.Structural.Flyweight.DotNet;

// Role: Client of the runtime's flyweight — the intern pool keeps one shared instance per distinct string.
// Literals are interned automatically; strings built at run time are not, unless you ask.
// Guide: §5.6
public static class SkuText
{
    /// <summary>A new string object every call, even for equal values.</summary>
    public static string Build(string prefix, string number) => string.Concat(prefix, number);

    /// <summary>
    /// The pool's single instance for that value. Interned strings live until the process ends, so interning
    /// everything is a memory leak, not an optimisation.
    /// </summary>
    public static string BuildInterned(string prefix, string number) => string.Intern(string.Concat(prefix, number));
}
