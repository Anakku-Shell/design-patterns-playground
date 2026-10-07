using Patterns.Behavioral.ChainOfResponsibility;
using Patterns.Behavioral.Command;
using Patterns.Behavioral.Interpreter;
using Patterns.Behavioral.Iterator;
using Patterns.Behavioral.Mediator;
using Patterns.Behavioral.Memento;
using Patterns.Behavioral.Observer;
using Patterns.Behavioral.State;
using Patterns.Behavioral.Strategy;
using Patterns.Behavioral.TemplateMethod;
using Patterns.Behavioral.Visitor;
using Patterns.Demo;

namespace Patterns.Behavioral;

/// <summary>The behavioral demos, in guide order. The runner concatenates the four category lists (guide §2.4).</summary>
public static class Demos
{
    public static IReadOnlyList<IDemo> All { get; } =
    [
        new ChainOfResponsibilityDemo(),
        new CommandDemo(),
        new InterpreterDemo(),
        new IteratorDemo(),
        new MediatorDemo(),
        new MementoDemo(),
        new ObserverDemo(),
        new StateDemo(),
        new StrategyDemo(),
        new TemplateMethodDemo(),
        new VisitorDemo(),
    ];
}
