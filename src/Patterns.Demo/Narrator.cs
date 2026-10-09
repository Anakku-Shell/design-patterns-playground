namespace Patterns.Demo;

/// <summary>
/// Prints every demo in the same shape: a title, one header per level, steps (→), results (=) and a
/// takeaway (✔). Guide: §2.4.
/// </summary>
public sealed class Narrator(TextWriter output)
{
    public void Title(IDemo demo)
    {
        ArgumentNullException.ThrowIfNull(demo);
        output.WriteLine(
            $"═══ {demo.Name} ({demo.Category} · {demo.Relevance.Mark()} {demo.Relevance.Label()}) · guide {demo.GuideSection} ═══");
    }

    /// <summary>Level 0 is "Problem", 1 "Classic", 2 ".NET".</summary>
    public void Level(int number, string name) => output.WriteLine($"── Level {number} · {name} ──");

    public void Step(string text) => output.WriteLine($"  → {text}");

    public void Result(string text) => output.WriteLine($"    = {text}");

    public void Takeaway(string text) => output.WriteLine($"  ✔ {text}");
}
