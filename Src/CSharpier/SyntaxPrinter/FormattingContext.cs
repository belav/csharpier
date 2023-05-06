namespace CSharpier.SyntaxPrinter;

internal class FormattingContext
{
    public FormattingContext(string lineEnding)
    {
        this.LineEnding = lineEnding;
    }

    public int PrintingDepth { get; set; }
    public bool NextTriviaNeedsLine { get; set; }
    public bool ShouldSkipNextLeadingTrivia { get; set; }
    public string LineEnding { get; init; }
}
