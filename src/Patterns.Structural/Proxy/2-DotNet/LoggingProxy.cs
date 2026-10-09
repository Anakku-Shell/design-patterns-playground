using System.Reflection;
using Patterns.Shop;

namespace Patterns.Structural.Proxy.DotNet;

// Role: Subject — the interface the generated proxy implements.
// Guide: §5.7
public interface IPriceEditor
{
    void ChangePrice(Product product, decimal newPrice);
}

// Role: RealSubject — stores the new price.
public sealed class PriceEditor : IPriceEditor
{
    private readonly Dictionary<Guid, decimal> _prices = [];

    public IReadOnlyDictionary<Guid, decimal> Prices => _prices;

    public void ChangePrice(Product product, decimal newPrice)
    {
        ArgumentNullException.ThrowIfNull(product);
        _prices[product.Id] = newPrice;
    }
}

/// <summary>
/// Creates a logging proxy for any interface. On a non-generic class so callers write
/// <c>LoggingProxy.Create&lt;IPriceEditor&gt;(…)</c> (CA1000: no static members on generic types).
/// </summary>
public static class LoggingProxy
{
    public static T Create<T>(T target, ICollection<string> log) where T : class
    {
        var proxy = DispatchProxy.Create<T, LoggingDispatchProxy<T>>(); // the runtime builds a class implementing T
        var self = (LoggingDispatchProxy<T>)(object)proxy;
        self.Target = target;
        self.Log = log;
        return proxy;
    }
}

// Role: Proxy (generated) — logs every call to any interface, then forwards it to the real object.
// Not sealed: DispatchProxy generates a class at run time that derives from this one.
public class LoggingDispatchProxy<T> : DispatchProxy where T : class
{
    internal T Target { get; set; } = default!;
    internal ICollection<string> Log { get; set; } = default!;

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        Log.Add($"{targetMethod.Name} called");
        // Reflection: slower than a direct call. DoNotWrapExceptions lets the target's own exception
        // (say, UnauthorizedAccessException) reach the caller instead of a TargetInvocationException.
        return targetMethod.Invoke(Target, BindingFlags.DoNotWrapExceptions, binder: null, args, culture: null);
    }
}
