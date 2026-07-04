using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class UsingDirectives
{
    private static readonly DefaultOrder Comparer = new();

    public static Doc PrintWithSorting(
        SyntaxList<UsingDirectiveSyntax> usings,
        PrintingContext context,
        bool printExtraLines
    )
    {
        if (usings.Count == 0)
        {
            return Doc.Null;
        }

        var docs = new List<Doc>();
        var usingList = usings.ToList();

        var initialComments = new List<SyntaxTrivia>();
        var triviaWithinIf = new List<SyntaxTrivia>();
        var foundIfDirective = false;
        var keepUsingsUntilEndIf = false;
        foreach (var leadingTrivia in usings[0].GetLeadingTrivia())
        {
            if (
                leadingTrivia.RawSyntaxKind() == SyntaxKind.DisabledTextTrivia
                && leadingTrivia
                    .ToFullString()
                    .TrimStart()
                    .StartsWith("extern alias", StringComparison.Ordinal)
            )
            {
                initialComments = usings[0].GetLeadingTrivia().ToList();
                triviaWithinIf.Clear();
                keepUsingsUntilEndIf = true;
                break;
            }
            if (
                leadingTrivia.RawSyntaxKind()
                is SyntaxKind.DefineDirectiveTrivia
                    or SyntaxKind.UndefDirectiveTrivia
            )
            {
                initialComments = usings.First().GetLeadingTrivia().ToList();
                triviaWithinIf.Clear();
                break;
            }
            if (leadingTrivia.RawSyntaxKind() == SyntaxKind.IfDirectiveTrivia)
            {
                foundIfDirective = true;
            }

            if (foundIfDirective)
            {
                triviaWithinIf.Add(leadingTrivia);
            }
            else
            {
                initialComments.Add(leadingTrivia);
            }
        }

        if (Token.HasLeadingCommentMatching(usings[0], CSharpierIgnore.IgnoreStartRegex))
        {
            initialComments.Clear();
        }

        docs.Add(Token.PrintLeadingTrivia([.. initialComments], context));
        if (keepUsingsUntilEndIf)
        {
            while (usingList.Count != 0)
            {
                var firstUsing = usingList.First();

                usingList.RemoveAt(0);
                if (firstUsing != usings[0])
                {
                    docs.Add(Token.PrintLeadingTrivia(firstUsing.GetLeadingTrivia(), context));
                }
                docs.Add(UsingDirective.Print(firstUsing, context));

                if (
                    firstUsing
                        .GetLeadingTrivia()
                        .Any(o => o.RawSyntaxKind() == SyntaxKind.EndIfDirectiveTrivia)
                )
                {
                    break;
                }
            }
        }

        var isFirst = true;
        var prevShouldFormat = true;
        var index = 0;
        var reorderedDirectives = false;
        foreach (var groupOfUsingData in GroupUsings(usingList, [.. triviaWithinIf], context))
        {
            foreach (var usingData in groupOfUsingData)
            {
                if (!isFirst)
                {
                    if (usingData.ShouldFormat || prevShouldFormat)
                    {
                        docs.Add(Doc.HardLine);
                    }
                }

                prevShouldFormat = usingData.ShouldFormat;

                if (usingData.LeadingTrivia != Doc.Null && usingData.ShouldFormat)
                {
                    docs.Add(usingData.LeadingTrivia);
                }
                if (usingData.Using is not null)
                {
                    if (usingData.Using != usings[index])
                    {
                        reorderedDirectives = true;
                    }

                    index++;
                    docs.Add(
                        usingData.ShouldFormat
                            ? UsingDirective.Print(usingData.Using, context, printExtraLines)
                            : CSharpierIgnore.PrintWithoutFormatting(usingData.Using, context)
                    );
                }

                isFirst = false;
            }
        }

        if (
            reorderedDirectives
            && (
                usings.Any(o => o.ToFullString().Contains("#endif"))
                || (
                    usings[0].Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax
                    && namespaceDeclarationSyntax.GetLeadingTrivia().ToFullString().Contains("#if")
                )
            )
        )
        {
            context.State.ReorderedUsingsWithDisabledText = true;
        }

        return Doc.Concat(docs);
    }

    private static IEnumerable<List<UsingData>> GroupUsings(
        List<UsingDirectiveSyntax> usings,
        SyntaxTriviaList triviaOnFirstUsing,
        PrintingContext context
    )
    {
        var globalSystemUsings = new List<UsingData>();
        var globalUsings = new List<UsingData>();
        var globalAliasUsings = new List<UsingData>();
        var systemUsings = new List<UsingData>();
        var aliasNameUsings = new List<UsingData>();
        var regularUsings = new List<UsingData>();
        var staticSystemUsings = new List<UsingData>();
        var staticUsings = new List<UsingData>();
        var aliasUsings = new List<UsingData>();
        var directiveGroup = new List<UsingData>();
        var ifCount = 0;
        var isFirst = true;
        var shouldFormat = true;

        foreach (var usingDirective in usings)
        {
            var openIfOrIgnore = ifCount > 0;
            if (Token.HasLeadingCommentMatching(usingDirective, CSharpierIgnore.IgnoreEndRegex))
            {
                openIfOrIgnore = true;
                shouldFormat = true;
            }
            else if (
                Token.HasLeadingCommentMatching(usingDirective, CSharpierIgnore.IgnoreStartRegex)
            )
            {
                shouldFormat = false;
            }

            foreach (var directive in usingDirective.GetLeadingTrivia().Where(o => o.IsDirective))
            {
                if (directive.RawSyntaxKind() is SyntaxKind.IfDirectiveTrivia)
                {
                    ifCount++;
                }
                else if (directive.RawSyntaxKind() is SyntaxKind.EndIfDirectiveTrivia)
                {
                    ifCount--;
                }
            }

            if (ifCount > 0)
            {
                directiveGroup.Add(
                    new UsingData
                    {
                        Using = usingDirective,
                        LeadingTrivia = PrintLeadingTrivia(usingDirective),
                        ShouldFormat = shouldFormat,
                    }
                );
            }
            else
            {
                if (openIfOrIgnore)
                {
                    directiveGroup.Add(
                        new UsingData
                        {
                            LeadingTrivia = PrintLeadingTrivia(usingDirective),
                            ShouldFormat = shouldFormat,
                        }
                    );
                }

                var usingData = new UsingData
                {
                    Using = usingDirective,
                    LeadingTrivia = !openIfOrIgnore ? PrintLeadingTrivia(usingDirective) : Doc.Null,
                    ShouldFormat = shouldFormat,
                };

                if (!shouldFormat)
                {
                    directiveGroup.Add(usingData);
                }
                else if (usingDirective.GlobalKeyword.RawSyntaxKind() != SyntaxKind.None)
                {
                    if (usingDirective.Alias is not null)
                    {
                        globalAliasUsings.Add(usingData);
                    }
                    else if (usingDirective.Name is not null && IsSystemName(usingDirective.Name))
                    {
                        globalSystemUsings.Add(usingData);
                    }
                    else
                    {
                        globalUsings.Add(usingData);
                    }
                }
                else if (usingDirective.StaticKeyword.RawSyntaxKind() != SyntaxKind.None)
                {
                    if (usingDirective.Name is not null && IsSystemName(usingDirective.Name))
                    {
                        staticSystemUsings.Add(usingData);
                    }
                    else
                    {
                        staticUsings.Add(usingData);
                    }
                }
                else if (usingDirective.Alias is not null)
                {
                    aliasUsings.Add(usingData);
                }
                else if (usingDirective.Name is AliasQualifiedNameSyntax)
                {
                    aliasNameUsings.Add(usingData);
                }
                else if (usingDirective.Name is not null && IsSystemName(usingDirective.Name))
                {
                    systemUsings.Add(usingData);
                }
                else
                {
                    regularUsings.Add(usingData);
                }
            }
        }

        yield return globalSystemUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return globalUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return globalAliasUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return systemUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return aliasNameUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return regularUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return staticSystemUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return staticUsings.OrderBy(o => o.Using, Comparer).ToList();
        yield return aliasUsings.OrderBy(o => o.Using, Comparer).ToList();
        // we need the directive groups at the end, the #endif directive
        // will be attached to the first node after the usings making it very hard print it before any of these other groups
        yield return directiveGroup;
        yield break;

        Doc PrintLeadingTrivia(UsingDirectiveSyntax value)
        {
            var result = isFirst
                ? Token.PrintLeadingTrivia(triviaOnFirstUsing, context)
                : Token.PrintLeadingTrivia(value.GetLeadingTrivia(), context);

            isFirst = false;
            return result;
        }
    }

    private class UsingData
    {
        public Doc LeadingTrivia { get; init; } = Doc.Null;
        public UsingDirectiveSyntax? Using { get; init; }
        public required bool ShouldFormat { get; init; } = true;
    }

    private static bool IsSystemName(NameSyntax value)
    {
        while (value is QualifiedNameSyntax qualifiedNameSyntax)
        {
            value = qualifiedNameSyntax.Left;
        }

        return value is IdentifierNameSyntax { Identifier.Text: "System" };
    }

    private class DefaultOrder : IComparer<UsingDirectiveSyntax?>
    {
        public int Compare(UsingDirectiveSyntax? x, UsingDirectiveSyntax? y)
        {
            if (x?.Name is null && y?.Name is not null)
            {
                return -1;
            }

            if (y?.Name is null && x?.Name is not null)
            {
                return 1;
            }

            if (x?.Alias is not null && y?.Alias is not null)
            {
                return string.Compare(
                    x.Alias.ToFullString().Trim(),
                    y.Alias.ToFullString().Trim(),
#pragma warning disable CA1309
                    StringComparison.InvariantCultureIgnoreCase
#pragma warning restore CA1309
                );
            }

            return string.Compare(
                x?.Name?.ToFullString().Trim(),
                y?.Name?.ToFullString().Trim(),
#pragma warning disable CA1309
                StringComparison.InvariantCultureIgnoreCase
#pragma warning restore CA1309
            );
        }
    }
}
