using Patterns.Demo;
using Patterns.Shop;

namespace Patterns.Modern.Result;

public sealed class ResultDemo : IDemo
{
    public string Key => "result";
    public string Name => "Result";
    public PatternCategory Category => PatternCategory.Modern;
    public Relevance Relevance => Relevance.Useful;
    public string GuideSection => "§7.6";

    public void Run(TextWriter output)
    {
        var narrator = new Narrator(output);
        narrator.Title(this);

        narrator.Level(0, "Problem");
        narrator.Step("Reserve(book, 21) returns a Reservation… or throws InsufficientStockException");
        try
        {
            new Problem.StockService().Reserve(SampleData.Book, 21);
        }
        catch (Problem.InsufficientStockException e)
        {
            narrator.Result($"caught {nameof(Problem.InsufficientStockException)}: {e.Message} (try/catch for an everyday outcome)");
        }

        narrator.Level(1, "Classic");
        narrator.Step("Reserve returns Result<Reservation>; Match handles both outcomes with ordinary code");
        foreach (var quantity in new[] { 2, 21, 0 })
        {
            var message = Classic.StockService.Reserve(SampleData.Book, quantity).Match(
                onSuccess: r => $"reserved {r.Quantity}",
                onFailure: e => $"{e.Code}: {e.Message}");
            narrator.Result($"{quantity,2} books → {message}");
        }
        narrator.Step("Bind chains a Charge step; a failed reservation never reaches it");
        foreach (var quantity in new[] { 2, 21 })
        {
            var charged = Classic.StockService.Reserve(SampleData.Book, quantity).Bind(r => Classic.Result.Success($"charged for {r.Quantity}"));
            narrator.Result($"{quantity,2} books → {(charged.IsSuccess ? charged.Value : charged.Error.Code)}");
        }

        narrator.Level(2, ".NET");
        narrator.Step("TryReserve(book, n, out var reservation): the BCL's TryXxx shape");
        foreach (var quantity in new[] { 2, 21 })
        {
            var reserved = DotNet.StockService.TryReserve(SampleData.Book, quantity, out var reservation);
            narrator.Result($"{quantity,2} books → {(reserved ? $"reserved {reservation!.Quantity}" : "false (no reason given)")}");
        }

        narrator.Takeaway("Return expected business failures as values the signature shows; keep exceptions for the unexpected.");
    }
}
