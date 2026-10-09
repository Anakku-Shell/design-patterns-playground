using Patterns.Demo;

namespace Patterns.Runner.Tests;

/// <summary>A demo that only writes "ran &lt;key&gt;", so runner tests do not depend on real patterns.</summary>
internal sealed class FakeDemo(string key, string name, PatternCategory category, Relevance relevance, string guideSection)
    : IDemo
{
    public FakeDemo(string key, PatternCategory category = PatternCategory.Behavioral)
        : this(key, char.ToUpperInvariant(key[0]) + key[1..], category, Relevance.Essential, "§0.0")
    {
    }

    public string Key => key;
    public string Name => name;
    public PatternCategory Category => category;
    public Relevance Relevance => relevance;
    public string GuideSection => guideSection;

    public void Run(TextWriter output) => output.WriteLine($"ran {key}");
}
