using System.Text;

namespace Patterns.Modern.ObjectPool.Classic;

// Role: ObjectPool — lends StringBuilders and takes them back, keeping at most maxRetained of them.
// Not thread-safe; the framework pools are.
// Guide: §7.8
public sealed class InvoiceBufferPool
{
    private readonly Stack<StringBuilder> _free = new();
    private readonly int _maxRetained;

    public InvoiceBufferPool(int maxRetained)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maxRetained);
        _maxRetained = maxRetained;
    }

    /// <summary>How many builders the pool had to create: what reuse saves.</summary>
    public int Created { get; private set; }

    public StringBuilder Rent()
    {
        if (_free.TryPop(out var builder))
        {
            return builder;
        }
        Created++;
        return new StringBuilder();
    }

    public void Return(StringBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Clear(); // reset: the next user must not see the previous invoice
        if (_free.Count < _maxRetained)
        {
            _free.Push(builder); // bounded: a full pool lets the garbage collector have it
        }
    }
}
