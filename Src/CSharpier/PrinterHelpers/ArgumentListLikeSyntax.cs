using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintArgumentListLikeSyntax(
            SyntaxToken openParenToken,
            SeparatedSyntaxList<ArgumentSyntax> arguments,
            SyntaxToken closeParenToken
        ) =>
            Docs.Concat(
                this.PrintSyntaxToken(openParenToken),
                arguments.Any()
                    ? Docs.Indent(
                            Docs.SoftLine,
                            this.PrintSeparatedSyntaxList(
                                arguments,
                                this.PrintArgumentSyntax,
                                Docs.Line
                            )
                        )
                    : Doc.Null,
                arguments.Any() ? Docs.SoftLine : Doc.Null,
                this.PrintSyntaxToken(closeParenToken)
            );
    }
}
