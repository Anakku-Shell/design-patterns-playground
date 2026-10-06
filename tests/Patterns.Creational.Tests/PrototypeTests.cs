using Patterns.Creational.Prototype.Classic;
using Patterns.Creational.Prototype.DotNet;
using Patterns.Shop;
using Xunit;

namespace Patterns.Creational.Tests;

public sealed class PrototypeTests
{
    private static OrderTemplate MonthlyCoffee() => new("Monthly coffee", [new OrderLine(SampleData.Mug, 1)]);

    [Fact]
    public void Clone_ChangingTheCopy_DoesNotChangeTheOriginal()
    {
        var template = MonthlyCoffee();

        var copy = template.Clone();
        copy.Name = "Monthly coffee and a book";
        copy.Lines.Add(new OrderLine(SampleData.Book, 1));

        Assert.Equal("Monthly coffee", template.Name);
        Assert.Single(template.Lines);
        Assert.Equal(2, copy.Lines.Count);
    }

    [Fact]
    public void ShallowClone_SharesTheLines()
    {
        var template = MonthlyCoffee();

        var copy = template.ShallowClone();
        copy.Lines.Add(new OrderLine(SampleData.Book, 1));

        // The trap: MemberwiseClone copied the reference to the list, not the list.
        Assert.Equal(2, template.Lines.Count);
        Assert.Same(template.Lines, copy.Lines);
    }

    [Fact]
    public void With_CreatesANewOrder_AndKeepsTheOriginal()
    {
        var march = new RecurringOrder(Guid.NewGuid(), "Monthly coffee",
            [new OrderLine(SampleData.Mug, 1)], new DateOnly(2026, 3, 1));

        var april = march.ForNextMonth();

        Assert.NotEqual(march.Id, april.Id);
        Assert.Equal(new DateOnly(2026, 4, 1), april.NextDelivery);
        Assert.Equal(new DateOnly(2026, 3, 1), march.NextDelivery);
        Assert.Equal(march.Lines, april.Lines); // shared safely: an ImmutableArray cannot change
    }

    [Fact]
    public void ForNextMonth_ChangesOnlyTheIdAndTheDate()
    {
        var march = new RecurringOrder(Guid.NewGuid(), "Monthly coffee",
            [new OrderLine(SampleData.Mug, 1)], new DateOnly(2026, 3, 1));

        var april = march.ForNextMonth();

        Assert.Equal(march with { Id = april.Id, NextDelivery = april.NextDelivery }, april);
    }
}
