using Patterns.Demo;
using Patterns.Modern.DependencyInjection;
using Patterns.Modern.NullObject;
using Patterns.Modern.ObjectPool;
using Patterns.Modern.Options;
using Patterns.Modern.Repository;
using Patterns.Modern.Result;
using Patterns.Modern.Specification;
using Patterns.Modern.UnitOfWork;

namespace Patterns.Modern;

/// <summary>The modern demos, in guide order. The runner concatenates the four category lists (guide §2.4).</summary>
public static class Demos
{
    public static IReadOnlyList<IDemo> All { get; } =
    [
        new DependencyInjectionDemo(),
        new OptionsDemo(),
        new RepositoryDemo(),
        new UnitOfWorkDemo(),
        new SpecificationDemo(),
        new ResultDemo(),
        new NullObjectDemo(),
        new ObjectPoolDemo(),
    ];
}
