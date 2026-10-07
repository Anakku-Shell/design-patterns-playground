using System.Linq.Expressions;

namespace Patterns.Modern.Specification.DotNet;

// Role: Specification — the rule as an expression tree, so a LINQ provider (EF Core) can translate it to SQL.
// Guide: §7.5
public abstract class ExpressionSpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public ExpressionSpecification<T> And(ExpressionSpecification<T> other) =>
        new CombinedSpecification<T>(this, other, Expression.AndAlso);

    public ExpressionSpecification<T> Or(ExpressionSpecification<T> other) =>
        new CombinedSpecification<T>(this, other, Expression.OrElse);

    public ExpressionSpecification<T> Not() => new NotExpressionSpecification<T>(this);
}

// Role: Composite specification — joins two rules (AndAlso or OrElse) under ONE parameter.
internal sealed class CombinedSpecification<T>(
    ExpressionSpecification<T> left,
    ExpressionSpecification<T> right,
    Func<Expression, Expression, BinaryExpression> join) : ExpressionSpecification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftLambda = left.ToExpression();
        var rightLambda = right.ToExpression();

        // Each lambda has its own parameter object. Joining the bodies as they are would leave the right one's
        // parameter undefined (Compile fails, EF Core cannot translate it): rewrite it to the left one's first.
        var rightBody = new ParameterReplacer(rightLambda.Parameters[0], leftLambda.Parameters[0]).Visit(rightLambda.Body);
        return Expression.Lambda<Func<T, bool>>(join(leftLambda.Body, rightBody), leftLambda.Parameters);
    }
}

// Role: Composite specification — the rule does not hold.
internal sealed class NotExpressionSpecification<T>(ExpressionSpecification<T> inner) : ExpressionSpecification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var lambda = inner.ToExpression();
        return Expression.Lambda<Func<T, bool>>(Expression.Not(lambda.Body), lambda.Parameters);
    }
}

/// <summary>Rewrites every use of one parameter into another (a Visitor, guide §6.11).</summary>
internal sealed class ParameterReplacer(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node) => node == from ? to : node;
}
