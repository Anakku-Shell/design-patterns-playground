using Patterns.Demo;

namespace Patterns.Creational;

/// <summary>The creational demos, in guide order. The runner concatenates the four category lists (guide §2.4).</summary>
public static class Demos
{
    public static IReadOnlyList<IDemo> All { get; } =
    [
        new Singleton.SingletonDemo(),
        new FactoryMethod.FactoryMethodDemo(),
        new AbstractFactory.AbstractFactoryDemo(),
        new Builder.BuilderDemo(),
        new Prototype.PrototypeDemo(),
    ];
}
