using Microsoft.Extensions.Logging;

namespace Patterns.Structural.Bridge.DotNet;

/// <summary>
/// What the checkout logs. The code that logs knows only ILogger; which providers receive the line (console,
/// a file, our list…) is decided where the LoggerFactory is built. Guide: §5.2.
/// </summary>
public static partial class CheckoutLog
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Order {OrderNumber} placed")]
    public static partial void OrderPlaced(ILogger logger, string orderNumber);
}
