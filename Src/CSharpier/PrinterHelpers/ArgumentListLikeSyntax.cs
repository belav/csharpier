using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
            Doc.Concat(
                Token.Print(openParenToken),
                arguments.Any()
                    ? Doc.Indent(
                            Doc.SoftLine,
                            SeparatedSyntaxList.Print(
                                arguments,
                                this.PrintArgumentSyntax,
                                Doc.Line
                            )
                        )
                    : Doc.Null,
                arguments.Any() ? Doc.SoftLine : Doc.Null,
                Token.Print(closeParenToken)
            );
    }
}
