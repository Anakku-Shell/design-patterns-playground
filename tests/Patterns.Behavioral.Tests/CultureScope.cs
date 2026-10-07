using System.Globalization;

namespace Patterns.Behavioral.Tests;

/// <summary>
/// Runs code under a culture that writes 12,50. The build sets InvariantGlobalization, so the current culture
/// is always invariant and a missing CultureInfo.InvariantCulture would go unnoticed without this.
/// </summary>
internal static class CultureScope
{
    public static T WithDecimalComma<T>(Func<T> action)
    {
        var previous = CultureInfo.CurrentCulture;
        var comma = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        comma.NumberFormat.NumberDecimalSeparator = ",";
        comma.NumberFormat.NumberGroupSeparator = ".";
        CultureInfo.CurrentCulture = comma;
        try
        {
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }
}
