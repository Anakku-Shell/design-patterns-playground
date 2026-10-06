using Patterns.Demo;
using Xunit;

namespace Patterns.Runner.Tests;

public sealed class RunnerAppTests
{
    private static readonly IReadOnlyList<IDemo> Catalog =
    [
        new FakeDemo("singleton", PatternCategory.Creational),
        new FakeDemo("strategy", PatternCategory.Behavioral),
        new FakeDemo("options", PatternCategory.Modern),
    ];

    private static (int ExitCode, string Output) Run(params string[] args)
    {
        var output = new StringWriter();
        var exitCode = RunnerApp.Run(args, output, Catalog);
        return (exitCode, output.ToString());
    }

    [Fact]
    public void NoArgs_ListsPatterns_Returns0()
    {
        var (exitCode, output) = Run();

        Assert.Equal(0, exitCode);
        Assert.Contains("strategy", output, StringComparison.Ordinal);
        Assert.Contains("Run one: dotnet run --project src/Patterns.Runner -- <key>", output, StringComparison.Ordinal);
    }

    [Fact]
    public void List_ListsPatterns()
    {
        var (exitCode, output) = Run("list");

        Assert.Equal(0, exitCode);
        Assert.Contains("  ⭐⭐⭐  strategy                 Strategy", output, StringComparison.Ordinal);
        Assert.DoesNotContain("ran ", output, StringComparison.Ordinal);
    }

    [Fact]
    public void List_PutsEachDemoUnderItsCategory()
    {
        var (_, output) = Run("list");

        int At(string text) => output.IndexOf(text, StringComparison.Ordinal);
        Assert.InRange(At("singleton"), At("Creational patterns"), At("Structural patterns"));
        Assert.InRange(At("strategy"), At("Behavioral patterns"), At("Modern patterns"));
        Assert.InRange(At("options"), At("Modern patterns"), At("Run one:"));
        Assert.Equal(At("strategy"), output.LastIndexOf("strategy", StringComparison.Ordinal)); // listed once
    }

    [Fact]
    public void List_AlignsTheKeys_WhateverTheMark()
    {
        IReadOnlyList<IDemo> catalog =
        [
            new FakeDemo("builder", "Builder", PatternCategory.Creational, Relevance.Essential, "§4.4"),
            new FakeDemo("singleton", "Singleton", PatternCategory.Creational, Relevance.Useful, "§4.1"),
            new FakeDemo("bridge", "Bridge", PatternCategory.Structural, Relevance.Niche, "§5.2"),
            new FakeDemo("prototype", "Prototype", PatternCategory.Creational, Relevance.Historical, "§4.5"),
        ];
        var output = new StringWriter();
        RunnerApp.Run(["list"], output, catalog);

        // Marks are drawn two terminal columns wide per symbol, so compare display columns, not chars.
        var columns = output.ToString().Split(Environment.NewLine)
            .Where(line => catalog.Any(d => line.Contains(d.Key, StringComparison.Ordinal)))
            .Select(line => DisplayColumn(line, catalog.First(d => line.Contains(d.Key, StringComparison.Ordinal)).Key))
            .Distinct();
        Assert.Single(columns);
    }

    private static int DisplayColumn(string line, string key)
    {
        var prefix = line[..line.IndexOf(key, StringComparison.Ordinal)];
        var enumerator = System.Globalization.StringInfo.GetTextElementEnumerator(prefix);
        var width = 0;
        while (enumerator.MoveNext())
        {
            width += enumerator.GetTextElement() is "⭐" or "🕰" ? 2 : 1; // the marks are wide symbols
        }
        return width;
    }

    [Fact]
    public void ExtraArguments_AreIgnored()
    {
        var (exitCode, output) = Run("strategy", "extra");

        Assert.Equal(0, exitCode);
        Assert.Equal("ran strategy" + Environment.NewLine, output);
    }

    [Fact]
    public void BlankKey_IsUnknown_Returns1()
    {
        var (exitCode, output) = Run("   ");

        Assert.Equal(1, exitCode);
        Assert.StartsWith("Unknown pattern", output, StringComparison.Ordinal);
    }

    [Fact]
    public void Key_RunsThatDemo()
    {
        var (exitCode, output) = Run("strategy");

        Assert.Equal(0, exitCode);
        Assert.Equal("ran strategy" + Environment.NewLine, output);
    }

    [Theory]
    [InlineData("Strategy")]
    [InlineData(" strategy ")]
    [InlineData("STRATEGY")]
    public void Key_IsTrimmedAndCaseInsensitive(string key)
    {
        var (exitCode, output) = Run(key);

        Assert.Equal(0, exitCode);
        Assert.Equal("ran strategy" + Environment.NewLine, output);
    }

    [Fact]
    public void UnknownKey_PrintsErrorAndList_Returns1()
    {
        var (exitCode, output) = Run("nope");

        Assert.Equal(1, exitCode);
        Assert.StartsWith("Unknown pattern 'nope'.", output, StringComparison.Ordinal);
        Assert.Contains("strategy", output, StringComparison.Ordinal);
    }

    [Fact]
    public void All_RunsEveryDemoInOrder()
    {
        var (exitCode, output) = Run("all");

        Assert.Equal(0, exitCode);
        var singleton = output.IndexOf("ran singleton", StringComparison.Ordinal);
        var strategy = output.IndexOf("ran strategy", StringComparison.Ordinal);
        var options = output.IndexOf("ran options", StringComparison.Ordinal);
        Assert.True(singleton >= 0 && singleton < strategy && strategy < options);
    }
}
