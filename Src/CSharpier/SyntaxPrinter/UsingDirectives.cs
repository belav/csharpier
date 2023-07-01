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

    // TODO what does the analyzer do with some of these sorts?
    // TODO what about validation?
    // TODO get rid of lines and keep them in blocks? - this does https://google.github.io/styleguide/javaguide.html#s3.3-import-statements
    // TODO what about alias any type with c# 12?

    public static Doc PrintWithSorting(
        SyntaxList<UsingDirectiveSyntax> usings,
        FormattingContext context,
        bool printExtraLines
    )
    {
        var docs = new List<Doc>();

        foreach (var usingGroup in GroupUsings(usings, context))
        {
            for (var i = 0; i < usingGroup.Count; i++)
            {
                if (i != 0)
                {
                    docs.Add(Doc.HardLine);
                }

                if (usingGroup[i].LeadingTrivia != Doc.Null)
                {
                    docs.Add(usingGroup[i].LeadingTrivia);
                }
                if (usingGroup[i].Using is UsingDirectiveSyntax usingDirective)
                {
                    docs.Add(
                        UsingDirective.Print(
                            usingDirective,
                            context,
                            printExtraLines: (i != 0 || printExtraLines)
                        )
                    );
                }
            }
        }

        return Doc.Concat(docs);
    }

    private static IEnumerable<List<UsingData>> GroupUsings(
        SyntaxList<UsingDirectiveSyntax> usings,
        FormattingContext context
    )
    {
        var regularUsings = new List<UsingData>();
        var staticUsings = new List<UsingData>();
        // TODO what about multiple ifs?
        var directiveGroup = new List<UsingData>();
        // TODO this is leftovers for the first group
        var leftOvers = new List<UsingData>();
        var ifCount = 0;
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
                return Doc.Concat(Token.PrintLeadingTrivia(value.GetLeadingTrivia(), context));
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
                    leftOvers.Add(new UsingData { LeadingTrivia = PrintStuff(usingDirective) });
                }

                if (usingDirective.StaticKeyword.RawSyntaxKind() == SyntaxKind.None)
                {
                    regularUsings.Add(
                        new UsingData
                        {
                            Using = usingDirective,
                            LeadingTrivia = !openIf ? PrintStuff(usingDirective) : Doc.Null
                        }
                    );
                }
                else
                {
                    // TODO what about IF on these?
                    staticUsings.Add(
                        new UsingData
                        {
                            Using = usingDirective,
                            LeadingTrivia = !openIf ? PrintStuff(usingDirective) : Doc.Null
                        }
                    );
                }
            }
        }

        if (regularUsings.Any())
        {
            yield return regularUsings.OrderBy(o => o.Using!, Comparer).ToList();
        }

        if (directiveGroup.Any())
        {
            yield return directiveGroup.OrderBy(o => o.Using!, Comparer).ToList();
        }

        if (leftOvers.Any())
        {
            yield return leftOvers;
        }

        if (staticUsings.Any())
        {
            yield return staticUsings.OrderBy(o => o.Using!, Comparer).ToList();
        }
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

    private class UsingGroup
    {
        public required List<UsingDirectiveSyntax> Usings { get; init; }
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
                DebugLogger.Log(
                    $"{x.ToFullString().Trim()} {xIsSystem} vs {y.ToFullString().Trim()} {yIsSystem} = {value}"
                );

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
