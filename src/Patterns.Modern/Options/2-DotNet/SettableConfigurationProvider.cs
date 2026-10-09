using Microsoft.Extensions.Configuration;

namespace Patterns.Modern.Options.DotNet;

/// <summary>
/// An in-memory configuration source whose values can change while the program runs, like a JSON file
/// edited on disk. The built-in memory provider does not signal changes; this one does.
/// </summary>
// Guide: §7.2
public sealed class SettableConfigurationSource(IEnumerable<KeyValuePair<string, string?>> initial) : IConfigurationSource
{
    public SettableConfigurationProvider Provider { get; } = new(initial);

    public IConfigurationProvider Build(IConfigurationBuilder builder) => Provider;
}

public sealed class SettableConfigurationProvider : ConfigurationProvider
{
    public SettableConfigurationProvider(IEnumerable<KeyValuePair<string, string?>> initial)
    {
        foreach (var (key, value) in initial)
        {
            Data[key] = value;
        }
    }

    /// <summary>Changes a value and raises the reload token, which is what IOptionsMonitor listens to.</summary>
    public override void Set(string key, string? value)
    {
        base.Set(key, value);
        OnReload();
    }
}
