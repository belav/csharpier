namespace CSharpier;

public class CodeFormatterOptions
{
    public int Width { get; init; } = 100;
    public IndentStyle IndentStyle { get; init; } = IndentStyle.Spaces;
    public int IndentSize { get; init; } = 4;
    public EndOfLine EndOfLine { get; init; } = EndOfLine.Auto;
    public bool IncludeGenerated { get; init; }
    public BraceNewLine NewLineBeforeOpenBrace { get; init; } = BraceNewLine.All;
    public bool NewLineBeforeElse { get; init; } = true;
    public bool NewLineBeforeCatch { get; init; } = true;
    public bool NewLineBeforeFinally { get; init; } = true;
}

public enum IndentStyle
{
    Spaces,
    Tabs
}
