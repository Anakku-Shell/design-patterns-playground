using Patterns.Demo;
using Xunit;

namespace Patterns.Runner.Tests;

public sealed class NarratorTests
{
    [Fact]
    public void Level_PrintsTheSeparator()
    {
        var output = new StringWriter();

        new Narrator(output).Level(1, "Classic");

        Assert.Equal("── Level 1 · Classic ──" + Environment.NewLine, output.ToString());
    }

    [Fact]
    public void Title_ShowsCategoryRelevanceAndGuideSection()
    {
        var output = new StringWriter();

        new Narrator(output).Title(new FakeDemo("strategy", "Strategy", PatternCategory.Behavioral, Relevance.Essential, "§6.9"));

        Assert.Equal("═══ Strategy (Behavioral · ⭐⭐⭐ Essential) · guide §6.9 ═══" + Environment.NewLine, output.ToString());
    }

    [Fact]
    public void StepResultAndTakeaway_AreIndented()
    {
        var output = new StringWriter();
        var narrator = new Narrator(output);

        narrator.Step("price the order");
        narrator.Result("4.99");
        narrator.Takeaway("same price");

        var nl = Environment.NewLine;
        Assert.Equal($"  → price the order{nl}    = 4.99{nl}  ✔ same price{nl}", output.ToString());
    }

    [Fact]
    public void Relevance_Marks()
    {
        Assert.Equal("⭐⭐⭐", Relevance.Essential.Mark());
        Assert.Equal("⭐⭐", Relevance.Useful.Mark());
        Assert.Equal("⭐", Relevance.Niche.Mark());
        Assert.Equal("🕰", Relevance.Historical.Mark());
        Assert.Equal("Historical", Relevance.Historical.Label());
    }
}
