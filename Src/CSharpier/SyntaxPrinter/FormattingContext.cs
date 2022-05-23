namespace CSharpier.SyntaxPrinter;

public record FormattingContext
{
    public int PrintingDepth { get; set; }
    public bool NextTriviaNeedsLine { get; set; }
    public bool ShouldSkipNextLeadingTrivia { get; set; }
}
