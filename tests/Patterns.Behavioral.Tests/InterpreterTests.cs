using Patterns.Behavioral.Interpreter.Classic;
using Patterns.Behavioral.Interpreter.DotNet;
using Patterns.Shop;
using Xunit;

namespace Patterns.Behavioral.Tests;

public sealed class InterpreterTests
{
    private const string BooksOver100 = "total > 100 AND category = 'books'";

    private static Order OrderFor(string name) => name switch
    {
        "9 books" => SampleData.OrderOf((SampleData.Book, 9)),             // 112.50
        "7 books" => SampleData.OrderOf((SampleData.Book, 7)),             // 87.50
        "2 headphones" => SampleData.OrderOf((SampleData.Headphones, 2)),  // 119.80, no book
        "8 books" => SampleData.OrderOf((SampleData.Book, 8)),             // 100.00
        _ => throw new ArgumentOutOfRangeException(nameof(name)),
    };

    [Theory]
    [InlineData(BooksOver100, "9 books", true)]
    [InlineData(BooksOver100, "7 books", false)]
    [InlineData(BooksOver100, "2 headphones", false)]
    [InlineData("total >= 100", "8 books", true)]
    [InlineData("total > 100", "8 books", false)]
    [InlineData("TOTAL > 1 and CATEGORY = 'BOOKS'", "7 books", true)]
    [InlineData("total > 99.90 AND category = 'Books' AND total >= 112.5", "9 books", true)]
    public void BothLevels_AgreeOnTheRule(string rule, string order, bool expected)
    {
        var o = OrderFor(order);

        Assert.Equal(expected, DiscountRuleParser.Parse(rule).Interpret(o));
        Assert.Equal(expected, RuleCompiler.Compile(rule)(o));
    }

    [Fact]
    public void Keywords_AreCaseInsensitive()
    {
        var rule = DiscountRuleParser.Parse("TOTAL > 1 and CATEGORY = 'BOOKS'");

        Assert.Equal(new AndExpression(new TotalGreaterThan(1m, OrEqual: false), new HasCategory("BOOKS")), rule);
    }

    [Fact]
    public void Parse_BuildsTheTree()
    {
        Assert.Equal(
            new AndExpression(new TotalGreaterThan(100m, OrEqual: false), new HasCategory("books")),
            DiscountRuleParser.Parse(BooksOver100));
    }

    [Theory]
    [InlineData("total >> 5", "Unexpected '>' at position 7.")]
    [InlineData("total >", "Unexpected end of rule.")]
    [InlineData("price > 5", "Unexpected 'price' at position 0.")]
    [InlineData("", "Unexpected end of rule.")]
    [InlineData("total > 5 category = 'books'", "Unexpected 'category' at position 10.")]
    [InlineData("category = 'books", "Unexpected end of rule.")]
    [InlineData("total > 5!", "Unexpected '!' at position 9.")]
    [InlineData("total > 5 AND", "Unexpected end of rule.")]
    [InlineData("total > 5.", "Unexpected '5.' at position 8.")]
    [InlineData("total > 1.2.3", "Unexpected '1.2.3' at position 8.")]
    [InlineData("category = ''", "Unexpected '''' at position 11.")]
    public void InvalidRules_ThrowWithThePosition(string rule, string message)
    {
        Assert.Equal(message, Assert.Throws<FormatException>(() => DiscountRuleParser.Parse(rule)).Message);
        Assert.Equal(message, Assert.Throws<FormatException>(() => RuleCompiler.Compile(rule)).Message);
    }

    [Fact]
    public void Numbers_IgnoreTheCurrentCulture()
    {
        var rule = CultureScope.WithDecimalComma(() => DiscountRuleParser.Parse("total >= 99.90"));

        Assert.Equal(new TotalGreaterThan(99.90m, OrEqual: true), rule); // not 9990: "." is not a group separator
    }
}
