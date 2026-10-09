namespace Patterns.Demo;

/// <summary>
/// What the runner needs from a pattern: its identity and a demo that narrates the levels. Demos write to the
/// <see cref="TextWriter"/> they receive (never to <c>Console</c>), so tests can capture the output. Guide: §2.4.
/// </summary>
public interface IDemo
{
    /// <summary>What you type after <c>--</c>: "strategy".</summary>
    string Key { get; }

    string Name { get; }

    PatternCategory Category { get; }

    Relevance Relevance { get; }

    /// <summary>The guide section that explains the pattern: "§6.9".</summary>
    string GuideSection { get; }

    void Run(TextWriter output);
}
