namespace Patterns.Behavioral.ChainOfResponsibility.Problem;

/// <summary>The answer of a validation: valid, or the first error found. Guide: §6.1.</summary>
public sealed record OrderCheck(bool IsValid, string? Error)
{
    public static OrderCheck Ok { get; } = new(true, null);

    public static OrderCheck Fail(string error) => new(false, error);
}
