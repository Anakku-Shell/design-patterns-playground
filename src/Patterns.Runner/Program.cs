using System.Text;
using Patterns.Runner;

// UTF-8 so the relevance marks (⭐, 🕰) and the narrator's symbols print correctly on every console.
Console.OutputEncoding = Encoding.UTF8;
return RunnerApp.Run(args, Console.Out, Catalog.All);
