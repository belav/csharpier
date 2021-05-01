using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AnonymousObjectCreationExpression
    {
        public static Doc Print(AnonymousObjectCreationExpressionSyntax node)
        {
            return Doc.Group(
                Token.PrintWithSuffix(node.NewKeyword, Doc.Line),
                Token.Print(node.OpenBraceToken),
                Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(
                        node.Initializers,
                        AnonymousObjectMemberDeclarator.Print,
                        Doc.Line
                    )
                ),
                Doc.Line,
                Token.Print(node.CloseBraceToken)
            );
        }
    }
}
