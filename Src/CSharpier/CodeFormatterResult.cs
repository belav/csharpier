namespace CSharpier;

public class CodeFormatterResult
{
    internal CodeFormatterResult() { }

    public string Code { get; internal init; } = string.Empty;
    internal string DocTree { get; init; } = string.Empty;
    internal string AST { get; init; } = string.Empty;
    public IEnumerable<Diagnostic> CompilationErrors { get; internal init; } =
        Enumerable.Empty<Diagnostic>();

    internal string FailureMessage { get; init; } = string.Empty;

    internal static readonly CodeFormatterResult Null = new();

    internal bool ReorderedModifiers { get; init; }
    internal bool ReorderedUsingsWithDisabledText { get; init; }
    internal bool MovedTrailingTrivia { get; init; }
}
