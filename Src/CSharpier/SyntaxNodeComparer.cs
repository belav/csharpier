using Microsoft.CodeAnalysis.Text;

namespace CSharpier;

internal partial class SyntaxNodeComparer
{
    protected string OriginalSourceCode { get; }
    protected string NewSourceCode { get; }
    protected SyntaxTree OriginalSyntaxTree { get; }
    protected SyntaxTree NewSyntaxTree { get; }
    protected bool ReorderedModifiers { get; }
    protected bool ReorderedUsingsWithDisabledText { get; }

    private static readonly CompareResult Equal = new();

    public SyntaxNodeComparer(
        string originalSourceCode,
        string newSourceCode,
        bool reorderedModifiers,
        bool reorderedUsingsWithDisabledText,
        CancellationToken cancellationToken
    )
    {
        this.OriginalSourceCode = originalSourceCode;
        this.NewSourceCode = newSourceCode;
        this.ReorderedModifiers = reorderedModifiers;
        this.ReorderedUsingsWithDisabledText = reorderedUsingsWithDisabledText;

        var cSharpParseOptions = new CSharpParseOptions(CSharpFormatter.LanguageVersion);
        this.OriginalSyntaxTree = CSharpSyntaxTree.ParseText(
            this.OriginalSourceCode,
            cSharpParseOptions,
            cancellationToken: cancellationToken
        );
        this.NewSyntaxTree = CSharpSyntaxTree.ParseText(
            this.NewSourceCode,
            cSharpParseOptions,
            cancellationToken: cancellationToken
        );
    }

    public string CompareSource()
    {
        return this.CompareSourceAsync(CancellationToken.None).Result;
    }

    public async Task<string> CompareSourceAsync(CancellationToken cancellationToken)
    {
        // this seems almost impossible to figure out with the current way this is written
        // the usings could be in disabled text on namespaces, or on the modifiers of any base types.
        // parts of the #if or #endif could be leading trivia in different places
        if (this.ReorderedUsingsWithDisabledText)
        {
            return string.Empty;
        }

        var result = this.AreEqualIgnoringWhitespace(
            await this.OriginalSyntaxTree.GetRootAsync(cancellationToken),
            await this.NewSyntaxTree.GetRootAsync(cancellationToken)
        );

        if (!result.IsInvalid)
        {
            return string.Empty;
        }

        var message =
            $"----------------------------- Original: {GetLine(result.OriginalSpan, this.OriginalSyntaxTree, this.OriginalSourceCode)}";

        message +=
            $"----------------------------- Formatted: {GetLine(result.NewSpan, this.NewSyntaxTree, this.NewSourceCode)}";
        return message;
    }

    private static string GetLine(TextSpan? textSpan, SyntaxTree syntaxTree, string source)
    {
        if (!textSpan.HasValue)
        {
            return "Missing";
        }

        var line = syntaxTree.GetLineSpan(textSpan.Value).StartLinePosition.Line;
        var endLine = syntaxTree.GetLineSpan(textSpan.Value).EndLinePosition.Line;

        var result = $"Around Line {line} -----------------------------{Environment.NewLine}";

        var stringReader = new StringReader(source);
        var x = 0;
        var linesWritten = 0;
        var currentLine = stringReader.ReadLine();
        while (x <= endLine + 2 || linesWritten < 8)
        {
            if (x >= line - 2)
            {
                result += currentLine + Environment.NewLine;
                linesWritten++;
            }

            if (linesWritten > 15)
            {
                break;
            }

            currentLine = stringReader.ReadLine();
            if (currentLine == null)
            {
                break;
            }

            x++;
        }

        return result;
    }

    private readonly Stack<(SyntaxNode? node, SyntaxNode? parent)> originalStack = new();
    private readonly Stack<(SyntaxNode? node, SyntaxNode? parent)> formattedStack = new();

    private CompareResult AreEqualIgnoringWhitespace(
        SyntaxNode originalStart,
        SyntaxNode formattedStart
    )
    {
        this.originalStack.Push((originalStart, originalStart));
        this.formattedStack.Push((formattedStart, formattedStart));
        while (this.originalStack.Count > 0)
        {
            var result = this.Compare(this.originalStack.Pop(), this.formattedStack.Pop());
            if (result.IsInvalid)
            {
                return result;
            }
        }

        return Equal;
    }

    private CompareResult CompareLists<T>(
        IReadOnlyList<T> originalList,
        IReadOnlyList<T> formattedList,
        Func<T, T, CompareResult> comparer,
        Func<T, TextSpan> getSpan,
        TextSpan originalParentSpan,
        TextSpan newParentSpan
    )
    {
        for (var x = 0; x < originalList.Count || x < formattedList.Count; x++)
        {
            if (x == originalList.Count)
            {
                return NotEqual(originalParentSpan, getSpan(formattedList[x]));
            }

            if (x == formattedList.Count)
            {
                return NotEqual(getSpan(originalList[x]), newParentSpan);
            }

            if (
                originalList[x] is SyntaxNode originalNode
                && formattedList[x] is SyntaxNode formattedNode
            )
            {
                this.originalStack.Push((originalNode, originalNode.Parent));
                this.formattedStack.Push((formattedNode, formattedNode.Parent));
            }
            else
            {
                var result = comparer(originalList[x], formattedList[x]);
                if (result.IsInvalid)
                {
                    return result;
                }
            }
        }

        return Equal;
    }

    private static CompareResult NotEqual(SyntaxNode? originalNode, SyntaxNode? formattedNode)
    {
        return new()
        {
            IsInvalid = true,
            OriginalSpan = originalNode?.Span,
            NewSpan = formattedNode?.Span
        };
    }

    private static CompareResult NotEqual(TextSpan? originalSpan, TextSpan? formattedSpan)
    {
        return new()
        {
            IsInvalid = true,
            OriginalSpan = originalSpan,
            NewSpan = formattedSpan
        };
    }

    private CompareResult Compare(SyntaxToken originalToken, SyntaxToken formattedToken)
    {
        return this.Compare(originalToken, formattedToken, null, null);
    }

    private CompareResult Compare(
        SyntaxToken originalToken,
        SyntaxToken formattedToken,
        SyntaxNode? originalNode,
        SyntaxNode? formattedNode
    )
    {
        if (
            this.ReorderedModifiers
            && (
                (
                    formattedNode is NamespaceDeclarationSyntax nd
                    && nd.NamespaceKeyword == formattedToken
                )
                || (
                    formattedNode is FileScopedNamespaceDeclarationSyntax fsnd
                    && fsnd.NamespaceKeyword == formattedToken
                )
            )
        )
        {
            if (formattedNode.GetLeadingTrivia().ToFullString().Contains("#endif"))
            {
                return Equal;
            }
        }

        // when a verbatim string contains mismatched line endings they will become consistent
        // this validation will fail unless we also get them consistent here
        // adding a semi-complicated if check to determine when to do the string replacement
        // did not appear to have any performance benefits
        if (originalToken.Text.Replace("\r", "") != formattedToken.Text.Replace("\r", ""))
        {
            return NotEqual(
                originalToken.RawSyntaxKind() == SyntaxKind.None
                    ? originalNode?.Span
                    : originalToken.Span,
                formattedToken.RawSyntaxKind() == SyntaxKind.None
                    ? formattedNode?.Span
                    : formattedToken.Span
            );
        }

        var result = this.Compare(originalToken.LeadingTrivia, formattedToken.LeadingTrivia);
        if (result.IsInvalid)
        {
            return result;
        }

        var result2 = this.Compare(originalToken.TrailingTrivia, formattedToken.TrailingTrivia);

        return result2.IsInvalid ? result2 : Equal;
    }

    private CompareResult Compare(SyntaxTrivia originalTrivia, SyntaxTrivia formattedTrivia)
    {
        if (originalTrivia.RawSyntaxKind() is SyntaxKind.DisabledTextTrivia)
        {
            if (this.ReorderedModifiers)
            {
                return Equal;
            }

            return DisabledTextComparer.IsCodeBasicallyEqual(
                originalTrivia.ToString(),
                formattedTrivia.ToString()
            )
                ? Equal
                : NotEqual(originalTrivia.Span, formattedTrivia.Span);
        }

        if (originalTrivia.IsComment())
        {
            var originalStringReader = new StringReader(originalTrivia.ToString());
            var formattedStringReader = new StringReader(formattedTrivia.ToString());
            var originalLine = originalStringReader.ReadLine();
            var formattedLine = formattedStringReader.ReadLine();
            while (originalLine != null)
            {
                if (formattedLine == null || originalLine.Trim() != formattedLine.Trim())
                {
                    return NotEqual(originalTrivia.Span, formattedTrivia.Span);
                }

                originalLine = originalStringReader.ReadLine();
                formattedLine = formattedStringReader.ReadLine();
            }

            return Equal;
        }

        return originalTrivia.ToString().TrimEnd() == formattedTrivia.ToString().TrimEnd()
            ? Equal
            : NotEqual(originalTrivia.Span, formattedTrivia.Span);
    }

    private CompareResult Compare(SyntaxTriviaList originalList, SyntaxTriviaList formattedList)
    {
        static SyntaxTrivia? FindNextSyntaxTrivia(SyntaxTriviaList list, ref int next)
        {
            SyntaxTrivia result;
            do
            {
                if (next >= list.Count)
                {
                    return null;
                }

                result = list[next];
                next++;
            } while (
                result.RawSyntaxKind() is SyntaxKind.EndOfLineTrivia or SyntaxKind.WhitespaceTrivia
                || result.RawSyntaxKind() is SyntaxKind.DisabledTextTrivia
                    && string.IsNullOrWhiteSpace(result.ToString())
            );

            return result;
        }

        var nextOriginal = 0;
        var nextFormatted = 0;
        var original = FindNextSyntaxTrivia(originalList, ref nextOriginal);
        var formatted = FindNextSyntaxTrivia(formattedList, ref nextFormatted);
        while (original != null && formatted != null)
        {
            var result = this.Compare(original.Value, formatted.Value);
            if (result.IsInvalid)
            {
                return result;
            }

            original = FindNextSyntaxTrivia(originalList, ref nextOriginal);
            formatted = FindNextSyntaxTrivia(formattedList, ref nextFormatted);
        }

        if (original != formatted)
        {
            return NotEqual(originalList.Span, formattedList.Span);
        }

        return Equal;
    }

    private CompareResult CompareUsingDirectives(
        SyntaxList<UsingDirectiveSyntax> original,
        SyntaxList<UsingDirectiveSyntax> formatted,
        SyntaxNode originalParent,
        SyntaxNode formattedParent
    )
    {
        if (original.Count > 0 && original.First().GetLeadingTrivia().Any())
        {
            return Equal;
        }

        if (original.Count != formatted.Count)
        {
            return NotEqual(originalParent, formattedParent);
        }

        var sortedOriginal = original.OrderBy(o => o.ToFullString().Trim()).ToList();
        var sortedFormatted = formatted.OrderBy(o => o.ToFullString().Trim()).ToList();

        for (var x = 0; x < original.Count; x++)
        {
            var result = this.Compare(
                (sortedOriginal[x], originalParent),
                (sortedFormatted[x], formattedParent)
            );

            if (result.IsInvalid)
            {
                return result;
            }
        }

        return Equal;
    }
}

internal struct CompareResult
{
    public bool IsInvalid;
    public TextSpan? OriginalSpan;
    public TextSpan? NewSpan;
}
