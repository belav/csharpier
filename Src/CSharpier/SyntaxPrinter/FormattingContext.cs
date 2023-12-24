namespace CSharpier.SyntaxPrinter;

internal class FormattingContext
{
    public int PrintingDepth { get; set; }
    public bool NextTriviaNeedsLine { get; set; }
    public bool ShouldSkipNextLeadingTrivia { get; set; }
    public required string LineEnding { get; init; }

    // we need to keep track if we reordered modifiers because when modifiers are moved inside
    // of an #if, then we can't compare the before and after disabled text in the source file
    public bool ReorderedModifiers { get; set; }

    // we also need to keep track if we move around usings with disabledText
    public bool ReorderedUsingsWithDisabledText { get; set; }
}
