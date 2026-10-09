using Microsoft.Extensions.Logging;

namespace Patterns.Structural.Bridge.DotNet;

// Role: ConcreteImplementor — a logging provider that keeps lines in a list (handy in tests). ILogger is
// the abstraction your code writes to; providers are the implementations behind it.
// Guide: §5.2
public sealed class ListLoggerProvider : ILoggerProvider
{
    private readonly List<string> _lines = [];

    public IReadOnlyList<string> Lines => _lines;

    public ILogger CreateLogger(string categoryName) => new ListLogger(categoryName, _lines);

    public void Dispose()
    {
        // Nothing to release: the lines stay readable after the logger factory is disposed.
    }

    private sealed class ListLogger(string category, List<string> lines) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            lines.Add($"{category}: {formatter(state, exception)}");
    }
}
