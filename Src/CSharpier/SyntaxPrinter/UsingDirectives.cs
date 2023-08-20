namespace CSharpier.SyntaxPrinter;

internal static class UsingDirectives
{
    private static readonly DefaultOrder Comparer = new();

    /* TODO these all fail still
TODO review PRs, the full one is too big

Warning ./mono/mcs/class/corlib/System.Runtime.InteropServices/Marshal.cs - Failed to compile so was not formatted.
  (35,2): error CS1032: Cannot define/undefine preprocessor symbols after first token in file
Warning ./mono/mcs/class/System/Mono.AppleTls/MonoCertificatePal.OSX.cs - Failed to compile so was not formatted.
  (36,1): error CS1028: Unexpected preprocessor directive
  (30,1): error CS0439: An extern alias declaration must precede all other elements defined in the namespace
Warning ./mono/mcs/class/System/System.Security.Cryptography.X509Certificates/X509Certificate2ImplUnix.cs - Failed to compile so was not formatted.
  (32,1): error CS0439: An extern alias declaration must precede all other elements defined in the namespace
Warning ./mono/mcs/class/System/System.Security.Cryptography.X509Certificates/X509Helper2.cs - Failed to compile so was not formatted.
  (33,1): error CS0439: An extern alias declaration must precede all other elements defined in the namespace
Warning ./mono/mcs/class/System/System.Security.Cryptography.X509Certificates/X509Certificate2ImplMono.cs - Failed to compile so was not formatted.
  (39,1): error CS0439: An extern alias declaration must precede all other elements defined in the namespace
Warning ./mono/mcs/class/System/System.Security.Cryptography.X509Certificates/X509Certificate2.cs - Failed to compile so was not formatted.
  (38,1): error CS0439: An extern alias declaration must precede all other elements defined in the namespace
Warning ./mono/mcs/class/System/System.Net.Mail/SmtpClient.cs - Failed to compile so was not formatted.
  (50,1): error CS1028: Unexpected preprocessor directive
  (52,1): error CS1028: Unexpected preprocessor directive
  (1538,1): error CS1027: #endif directive expected
Warning ./mono/mcs/class/referencesource/System.Xml/System/Xml/XmlCharType.cs - Failed to compile so was not formatted.
  (3,2): error CS1032: Cannot define/undefine preprocessor symbols after first token in file
Warning ./mono/mcs/class/referencesource/mscorlib/system/diagnostics/eventing/TraceLogging/TraceLoggingEventSource.cs - Failed to compile so was not formatted.
  (11,2): error CS1032: Cannot define/undefine preprocessor symbols after first token in file


     */

    public static Doc PrintWithSorting(
        SyntaxList<UsingDirectiveSyntax> usings,
        FormattingContext context,
        bool printExtraLines
    )
    {
        var docs = new List<Doc>();

        var initialComments = new List<SyntaxTrivia>();
        var triviaWithinIf = new List<SyntaxTrivia>();
        var foundIfDirective = false;
        foreach (var leadingTrivia in usings.First().GetLeadingTrivia())
        {
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

        docs.Add(Token.PrintLeadingTrivia(new SyntaxTriviaList(initialComments), context));
        var isFirst = true;
        foreach (
            var groupOfUsingData in GroupUsings(
                usings,
                new SyntaxTriviaList(triviaWithinIf),
                context
            )
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
                if (usingData.Using is not null)
                {
                    docs.Add(UsingDirective.Print(usingData.Using, context, printExtraLines));
                }

                isFirst = false;
            }
        }

        return Doc.Concat(docs);
    }

    private static IEnumerable<List<UsingData>> GroupUsings(
        SyntaxList<UsingDirectiveSyntax> usings,
        SyntaxTriviaList triviaOnFirstUsing,
        FormattingContext context
    )
    {
        var globalUsings = new List<UsingData>();
        var systemUsings = new List<UsingData>();
        var aliasNameUsings = new List<UsingData>();
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

        yield return globalUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return systemUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return aliasNameUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return regularUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return directiveGroup;
        yield return staticUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield return aliasUsings.OrderBy(o => o.Using!, Comparer).ToList();
        yield break;

        Doc PrintStuff(UsingDirectiveSyntax value)
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

            if (x.Alias is not null && y.Alias is not null)
            {
                return x.Alias.ToFullString().CompareTo(y.Alias.ToFullString());
            }

            return x.Name.ToFullString().CompareTo(y.Name.ToFullString());
        }
    }
}
