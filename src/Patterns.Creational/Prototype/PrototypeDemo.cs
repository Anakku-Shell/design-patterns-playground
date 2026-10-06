using Patterns.Creational.Prototype.Classic;
using Patterns.Creational.Prototype.DotNet;
using Patterns.Demo;
using Patterns.Shop;
using static System.FormattableString;

namespace Patterns.Creational.Prototype;

public sealed class PrototypeDemo : IDemo
{
    public string Key => "prototype";
    public string Name => "Prototype";
    public PatternCategory Category => PatternCategory.Creational;
    public Relevance Relevance => Relevance.Historical;
    public string GuideSection => "§4.5";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(1, "Classic");
        var template = new OrderTemplate("Monthly coffee", [new OrderLine(SampleData.Mug, 1)]);
        narrator.Step("Clone() the template and add a book to the copy");
        template.Clone().Lines.Add(new OrderLine(SampleData.Book, 1));
        narrator.Result($"template still has {template.Lines.Count} line");
        narrator.Step("ShallowClone() the template and add a book to the copy");
        template.ShallowClone().Lines.Add(new OrderLine(SampleData.Book, 1));
        narrator.Result($"template now has {template.Lines.Count} lines: the copy shared its list");

        narrator.Level(2, ".NET");
        var march = new RecurringOrder(Guid.NewGuid(), "Monthly coffee", [new OrderLine(SampleData.Mug, 1)], new DateOnly(2026, 3, 1));
        narrator.Step("march with { Id = new id, NextDelivery = +1 month }");
        var april = march.ForNextMonth();
        narrator.Result(Invariant($"march delivers {march.NextDelivery:yyyy-MM-dd}, april {april.NextDelivery:yyyy-MM-dd}; same id: {march.Id == april.Id}"));

        narrator.Takeaway("Records and `with` are Prototype built into C#; immutable members make the shallow copy safe.");
    }
}
