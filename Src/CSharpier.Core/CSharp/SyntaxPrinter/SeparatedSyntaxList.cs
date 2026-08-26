using System.Runtime.CompilerServices;
using System.Text;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class SeparatedSyntaxList
{
    public static Doc Print<T>(
        SeparatedSyntaxList<T> list,
        Func<T, CSharpPrintingContext, Doc> printFunc,
        Doc afterSeparator,
        CSharpPrintingContext context,
        int startingIndex = 0
    )
        where T : SyntaxNode
    {
        return Print(list, printFunc, afterSeparator, context, startingIndex, null);
    }

    public static Doc PrintWithTrailingComma<T>(
        SeparatedSyntaxList<T> list,
        Func<T, CSharpPrintingContext, Doc> printFunc,
        Doc afterSeparator,
        CSharpPrintingContext context,
        SyntaxToken? closingToken = null
    )
        where T : SyntaxNode
    {
        return Print(list, printFunc, afterSeparator, context, 0, closingToken);
    }

    // the names above aren't totally accurate
    // sometimes there are trailing commas with calls to Print (some patterns do that)
    // and if you pass null to PrintWithTrailingComma it won't add a trailing comma if there isn't one
    [SkipLocalsInit]
    private static Doc Print<T>(
        in SeparatedSyntaxList<T> list,
        Func<T, CSharpPrintingContext, Doc> printFunc,
        Doc afterSeparator,
        CSharpPrintingContext context,
        int startingIndex,
        SyntaxToken? closingToken
    )
        where T : SyntaxNode
    {
        var docs = list.Count <= 3 ? new DocListBuilder(8) : new DocListBuilder(list.Count * 3);
        StringBuilder? unFormattedCode = null;
        var printUnformatted = false;
        for (var x = startingIndex; x < list.Count; x++)
        {
            var member = list[x];

            if (Token.HasLeadingCommentMatching(member, CSharpierIgnore.IgnoreEndRegex))
            {
                docs.Add(unFormattedCode?.ToString().Trim() ?? string.Empty);
                unFormattedCode?.Clear();
                printUnformatted = false;
            }
            else if (Token.HasLeadingCommentMatching(member, CSharpierIgnore.IgnoreStartRegex))
            {
                if (!printUnformatted && x > 0)
                {
                    docs.Add(Doc.HardLine);
                }
                printUnformatted = true;
            }

            if (printUnformatted)
            {
                unFormattedCode ??= new StringBuilder();
                unFormattedCode.Append(CSharpierIgnore.PrintWithoutFormatting(member, context));
                if (x < list.SeparatorCount)
                {
                    unFormattedCode.Append(list.GetSeparator(x).ToFullString().Trim());
                    unFormattedCode.Append(context.LineEnding);
                }

                continue;
            }

            // GetTrailingTrivia walks the right hand spine of the member, and the result is only
            // ever read for the last member, so don't compute it for the rest
            var isLastWithoutSeparator = x >= list.SeparatorCount;
            var firstTrailingComment =
                isLastWithoutSeparator && closingToken is not null
                    ? member.GetTrailingTrivia().FirstOrDefault(o => o.IsComment())
                    : default;

            // we want a trailing comma, but we need to get it printed in place before a trailing comment
            // shove it in the context so the token printing can pick it up and put it in place
            if (
                isLastWithoutSeparator
                && closingToken is not null
                && firstTrailingComment != default
            )
            {
                context.WithTrailingComma(
                    firstTrailingComment,
                    TrailingComma.Print(closingToken.Value, context, true)
                );
            }

            docs.Add(printFunc(member, context));

            // if the syntax tree doesn't have a trailing comma but we want want, then add it
            if (isLastWithoutSeparator)
            {
                if (closingToken != null && firstTrailingComment == default)
                {
                    docs.Add(TrailingComma.Print(closingToken.Value, context));
                }

                continue;
            }

            var isTrailingSeparator = x == list.Count - 1;

            if (isTrailingSeparator)
            {
                var trailingSeparatorToken = list.GetSeparator(x);
                // when the trailing separator has trailing comments, we have to print it normally to prevent it from collapsing
                // when the closing token has a directive, we can't assume the comma should be added/removed so just print it normally
                if (
                    trailingSeparatorToken.TrailingTrivia.AnyComment()
                    || closingToken != null && closingToken.Value.LeadingTrivia.AnyDirective()
                )
                {
                    docs.Add(Token.Print(trailingSeparatorToken, context));
                }
                else if (closingToken != null)
                {
                    docs.Add(TrailingComma.Print(closingToken.Value, context));
                }
                else
                {
                    docs.Add(Doc.IfBreak(Token.Print(list.GetSeparator(x), context), Doc.Null));
                }
            }
            else
            {
                docs.Add(Token.Print(list.GetSeparator(x), context));
                docs.Add(afterSeparator);
            }
        }

        if (unFormattedCode is { Length: > 0 })
        {
            docs.Add(unFormattedCode.ToString().Trim());
        }

        var output = Doc.Concat(ref docs);
        docs.Dispose();

        return output;
    }
}
