namespace CSharpier.SyntaxPrinter;

using System.Collections;

internal static class UsingDirectives
{
    private static readonly DefaultOrder Comparer = new();

    /*
     global first?
     then system
     then static
     then alias, ordered by the alias?
     what about alias of any type?
     */

    // TODO alias!!
    // TODO https://github.com/belav/csharpier-repos/pull/80/files has one weird #else thingie

    // TODO what does the analyzer do with some of these sorts?
    // TODO what about validation?
    // TODO what about alias any type with c# 12?

    public static Doc PrintWithSorting(
        SyntaxList<UsingDirectiveSyntax> usings,
        FormattingContext context,
        bool printExtraLines
    )
    {
        var docs = new List<Doc>();

        var initialComments = new List<SyntaxTrivia>();
        var otherStuff = new List<SyntaxTrivia>();
        var foundOtherStuff = false;
        foreach (var leadingTrivia in usings.First().GetLeadingTrivia())
        {
            if (leadingTrivia.RawSyntaxKind() == SyntaxKind.IfDirectiveTrivia)
            {
                foundOtherStuff = true;
            }

            if (foundOtherStuff)
            {
                otherStuff.Add(leadingTrivia);
            }
            else
            {
                initialComments.Add(leadingTrivia);
            }
        }

        docs.Add(Token.PrintLeadingTrivia(new SyntaxTriviaList(initialComments), context));
        var isFirst = true;
        foreach (
            var groupOfUsingData in GroupUsings(usings, new SyntaxTriviaList(otherStuff), context)
        )
        {
            foreach (var usingData in groupOfUsingData)
            {
                if (!isFirst)
                {
                    docs.Add(Doc.HardLine);
                }

                if (usingData.LeadingTrivia != Doc.Null)
                {
                    docs.Add(usingData.LeadingTrivia);
                }
                if (usingData.Using is UsingDirectiveSyntax usingDirective)
                {
                    docs.Add(
                        UsingDirective.Print(
                            usingDirective,
                            context,
                            printExtraLines // TODO keeping lines is hard, maybe don't? : printExtraLines || !isFirst
                        )
                    );
                }

                isFirst = false;
            }
        }

        return Doc.Concat(docs);
    }

    private static IEnumerable<List<UsingData>> GroupUsings(
        SyntaxList<UsingDirectiveSyntax> usings,
        SyntaxTriviaList otherStuff,
        FormattingContext context
    )
    {
        var globalUsings = new List<UsingData>();
        var regularUsings = new List<UsingData>();
        var staticUsings = new List<UsingData>();
        var aliasUsings = new List<UsingData>();
        var directiveGroup = new List<UsingData>();
        var ifCount = 0;
        var isFirst = true;
        foreach (var usingDirective in usings)
        {
            var openIf = ifCount > 0;
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

            Doc PrintStuff(UsingDirectiveSyntax value)
            {
                // TODO what about something with comments and a close #endif?
                return isFirst
                    ? Token.PrintLeadingTrivia(otherStuff, context)
                    : Token.PrintLeadingTrivia(value.GetLeadingTrivia(), context);
            }

            if (ifCount > 0)
            {
                directiveGroup.Add(
                    new UsingData
                    {
                        Using = usingDirective,
                        LeadingTrivia = PrintStuff(usingDirective)
                    }
                );
            }
            else
            {
                if (openIf)
                {
                    directiveGroup.Add(
                        new UsingData { LeadingTrivia = PrintStuff(usingDirective) }
                    );
                }

                var usingData = new UsingData
                {
                    Using = usingDirective,
                    LeadingTrivia = !openIf ? PrintStuff(usingDirective) : Doc.Null
                };

                // TODO what about IF on these?
                if (usingDirective.GlobalKeyword.RawSyntaxKind() != SyntaxKind.None)
                {
                    globalUsings.Add(usingData);
                }
                else if (usingDirective.StaticKeyword.RawSyntaxKind() != SyntaxKind.None)
                {
                    staticUsings.Add(usingData);
                }
                else if (usingDirective.Alias is not null)
                {
                    aliasUsings.Add(usingData);
                }
                else
                {
                    regularUsings.Add(usingData);
                }
            }

            isFirst = false;
        }

        yield return globalUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return regularUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return directiveGroup;
        yield return staticUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return aliasUsings.OrderBy(o => o.Using!, Comparer).ToList();
    }

    private class UsingData
    {
        public Doc LeadingTrivia { get; init; } = Doc.Null;
        public UsingDirectiveSyntax? Using { get; init; }
    }

    private static bool IsSystemName(NameSyntax value)
    {
        while (true)
        {
            if (value is not QualifiedNameSyntax qualifiedNameSyntax)
            {
                return value is IdentifierNameSyntax { Identifier.Text: "System" };
            }
            value = qualifiedNameSyntax.Left;
        }
    }

    private class DefaultOrder : IComparer<UsingDirectiveSyntax>
    {
        public int Compare(UsingDirectiveSyntax? x, UsingDirectiveSyntax? y)
        {
            if (x?.Name is null)
            {
                return -1;
            }

            if (y?.Name is null)
            {
                return 1;
            }

            var xIsSystem = IsSystemName(x.Name);
            var yIsSystem = IsSystemName(y.Name);

            int Return(int value)
            {
                return value;
            }

            if (xIsSystem && !yIsSystem)
            {
                return Return(-1);
            }

            if (!xIsSystem && yIsSystem)
            {
                return Return(1);
            }

            return Return(x.Name.ToFullString().CompareTo(y.Name.ToFullString()));
        }
    }
}
