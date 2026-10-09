namespace Patterns.Demo;

/// <summary>The mark and the word the runner and the guide print for each relevance level.</summary>
public static class RelevanceExtensions
{
    public static string Mark(this Relevance relevance) => relevance switch
    {
        Relevance.Essential => "⭐⭐⭐",
        Relevance.Useful => "⭐⭐",
        Relevance.Niche => "⭐",
        Relevance.Historical => "🕰",
        _ => throw new ArgumentOutOfRangeException(nameof(relevance)),
    };

    public static string Label(this Relevance relevance) => relevance.ToString();
}
