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
            Concat(
                this.PrintSyntaxToken(openParenToken),
                arguments.Any()
                    ? Indent(
                            SoftLine,
                            this.PrintSeparatedSyntaxList(
                                arguments,
                                this.PrintArgumentSyntax,
                                Line
                            )
                        )
                    : Doc.Null,
                arguments.Any() ? SoftLine : Doc.Null,
                this.PrintSyntaxToken(closeParenToken)
            );
    }
}
