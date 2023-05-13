namespace CSharpier.SyntaxPrinter;

internal class FormattingContext
{
    public int PrintingDepth { get; set; }
    public bool NextTriviaNeedsLine { get; set; }
    public bool ShouldSkipNextLeadingTrivia { get; set; }
    public required string LineEnding { get; init; }
}
