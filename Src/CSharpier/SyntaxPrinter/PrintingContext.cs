namespace CSharpier.SyntaxPrinter;

internal class PrintingContext
{
    public required PrintingContextOptions Options { get; init; }
    public PrintingContextState State { get; init; } = new();

    private readonly Dictionary<string, int> groupNumberByValue = new();

    public string GroupFor(string value)
    {
        var number = this.groupNumberByValue.GetValueOrDefault(value, 0) + 1;
        this.groupNumberByValue[value] = number;

        return value + " #" + number;
    }

    public PrintingContext WithSkipNextLeadingTrivia()
    {
        this.State.SkipNextLeadingTrivia = true;
        return this;
    }
    
    public PrintingContext WithTrailingComma(SyntaxTrivia syntaxTrivia, Doc doc)
    {
        this.State.TrailingComma = new TrailingCommaContext(syntaxTrivia, doc);
        return this;
    }

    public class PrintingContextOptions
    {
        public required string LineEnding { get; init; }
        public required int IndentSize { get; init; }
        public required bool UseTabs { get; init; }
    }

    public class PrintingContextState
    {
        public int PrintingDepth { get; set; }
        public bool NextTriviaNeedsLine { get; set; }
        public bool SkipNextLeadingTrivia { get; set; }

        // we need to keep track if we reordered modifiers because when modifiers are moved inside
        // of an #if, then we can't compare the before and after disabled text in the source file
        public bool ReorderedModifiers { get; set; }

        // we also need to keep track if we move around usings with disabledText
        public bool ReorderedUsingsWithDisabledText { get; set; }
        
        public TrailingCommaContext? TrailingComma { get; set; }
    }
    
    public record TrailingCommaContext(SyntaxTrivia TrailingComment, Doc PrintedTrailingComma);
}
