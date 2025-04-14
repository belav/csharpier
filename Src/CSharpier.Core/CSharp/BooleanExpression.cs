namespace CSharpier.Core.CSharp;

internal class BooleanExpression
{
    public required List<string> Parameters { get; init; }
    public required Func<Dictionary<string, bool>, bool> Function { get; init; }
}
