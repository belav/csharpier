namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringExpression
{
    internal static readonly string[] lineSeparators = ["\r\n", "\r", "\n"];

    public static Doc Print(InterpolatedStringExpressionSyntax node, PrintingContext context)
    {
        // if any of the expressions in the interpolation contain a newline then don't force this flat
        // ideally we would format the expressions in some way, but determining how much to indent is a hard problem
        // and new lines in expressions in them are rare.
        if (node.Contents.Any(o => o is InterpolationSyntax && o.ToString().Contains('\n')))
        {
            return Doc.Concat(
                Token.PrintLeadingTrivia(node.GetLeadingTrivia(), context),
                node.ToString()
            );
        }

        if (
            node.StringStartToken.RawSyntaxKind()
            == SyntaxKind.InterpolatedMultiLineRawStringStartToken
        )
        {
            return RawString(node, context);
        }

        var docs = new List<Doc>
        {
            Token.PrintWithoutLeadingTrivia(node.StringStartToken, context),
        };

        docs.AddRange(node.Contents.Select(o => Node.Print(o, context)));
        docs.Add(Token.PrintWithoutTrailingTrivia(node.StringEndToken, context));

        return Doc.Concat(
            // pull out the trivia so it doesn't get forced flat
            Token.PrintLeadingTrivia(node.StringStartToken, context),
            Doc.ForceFlat(docs),
            Token.PrintTrailingTrivia(node.StringEndToken)
        );
    }

    private static Doc RawString(InterpolatedStringExpressionSyntax node, PrintingContext context)
    {
        var endDelimiterIsIndented =
            node.StringEndToken.Text.Replace("\r", string.Empty).Replace("\n", string.Empty)[0]
                is '\t'
                    or ' ';

        var contents = new List<Doc>
        {
            Token.Print(node.StringStartToken, context),
            endDelimiterIsIndented ? Doc.HardLineNoTrim : Doc.LiteralLine,
        };
        foreach (var content in node.Contents)
        {
            if (content is InterpolationSyntax interpolationSyntax)
            {
                contents.Add(Interpolation.Print(interpolationSyntax, context));
            }
            else if (content is InterpolatedStringTextSyntax textSyntax)
            {
                if (textSyntax.TextToken.ValueText == string.Empty)
                {
                    continue;
                }

                var lines = textSyntax.TextToken.ValueText.Split(
                    lineSeparators,
                    StringSplitOptions.None
                );
                for (var index = 0; index < lines.Length; index++)
                {
                    var line = lines[index];
                    contents.Add(line);
                    if (index == lines.Length - 1)
                    {
                        continue;
                    }
                    contents.Add(
                        endDelimiterIsIndented
                            ? string.IsNullOrEmpty(line)
                                ? Doc.HardLine
                                : Doc.HardLineNoTrim
                            : Doc.LiteralLine
                    );
                }
            }
        }

        contents.Add(
            endDelimiterIsIndented
                ? contents[^1] is StringDoc { Value: "" }
                    ? Doc.HardLine
                    : Doc.HardLineNoTrim
                : Doc.LiteralLine
        );
        contents.Add(Token.Print(node.StringEndToken, context));

        return Doc.IndentIf(!node.HasParent(typeof(ArgumentSyntax)), Doc.Concat(contents));
    }
}
