using Patterns.Demo;

namespace Patterns.Structural;

/// <summary>The structural demos, in guide order. The runner concatenates the four category lists (guide §2.4).</summary>
public static class Demos
{
    public static IReadOnlyList<IDemo> All { get; } =
    [
        new Adapter.AdapterDemo(),
        new Bridge.BridgeDemo(),
        new Composite.CompositeDemo(),
        new Decorator.DecoratorDemo(),
        new Facade.FacadeDemo(),
        new Flyweight.FlyweightDemo(),
        new Proxy.ProxyDemo(),
    ];
}
