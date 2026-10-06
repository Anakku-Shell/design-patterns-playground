using System.Globalization;
using Patterns.Demo;

namespace Patterns.Runner;

/// <summary>
/// The runner's commands: no arguments or <c>list</c> lists the patterns, <c>all</c> runs every demo, a key runs
/// that demo. Takes the output and the catalog as parameters, so tests can drive it. Guide: §2.4.
/// </summary>
public static class RunnerApp
{
    private const int KeyWidth = 25;
    private const int MarkColumns = 6; // "⭐⭐⭐": the widest mark, three symbols of two columns each

    public static int Run(string[] args, TextWriter output, IReadOnlyList<IDemo> catalog)
    {
        ArgumentNullException.ThrowIfNull(args);
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(catalog);

        var command = args.Length == 0 ? "list" : args[0].Trim();

        if (command.Equals("list", StringComparison.OrdinalIgnoreCase))
        {
            List(output, catalog);
            return 0;
        }

        if (command.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            for (var i = 0; i < catalog.Count; i++)
            {
                if (i > 0) { output.WriteLine(); }
                catalog[i].Run(output);
            }
            return 0;
        }

        var demo = catalog.FirstOrDefault(d => d.Key.Equals(command, StringComparison.OrdinalIgnoreCase));
        if (demo is null)
        {
            // Exit code 1 lets scripts notice the mistake; the list shows what was meant.
            output.WriteLine($"Unknown pattern '{args[0]}'.");
            output.WriteLine();
            List(output, catalog);
            return 1;
        }

        demo.Run(output);
        return 0;
    }

    private static void List(TextWriter output, IReadOnlyList<IDemo> catalog)
    {
        foreach (var category in Enum.GetValues<PatternCategory>())
        {
            output.WriteLine($"{category} patterns");
            var demos = catalog.Where(d => d.Category == category).ToList();
            if (demos.Count == 0) { output.WriteLine("  (none yet)"); }
            foreach (var demo in demos)
            {
                output.WriteLine($"  {PadMark(demo.Relevance.Mark())}  {demo.Key.PadRight(KeyWidth)}{demo.Name}");
            }
            output.WriteLine();
        }
        output.WriteLine("Run one: dotnet run --project src/Patterns.Runner -- <key>");
    }

    // Terminals draw ⭐ and 🕰 two columns wide, and "🕰" is two chars in UTF-16: pad by symbols
    // (text elements) times two, not by chars, or the key column would not line up.
    private static string PadMark(string mark) =>
        mark + new string(' ', MarkColumns - (2 * new StringInfo(mark).LengthInTextElements));
}
