using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class InitializerExpression
    {
        public static Doc Print(InitializerExpressionSyntax node)
        {
            Doc separator = node.Parent is AssignmentExpressionSyntax or EqualsValueClauseSyntax
                ? Doc.Line
                : Doc.Null;

            var result = Doc.Concat(
                separator,
                Token.Print(node.OpenBraceToken),
                Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(node.Expressions, Node.Print, Doc.Line)
                ),
                Doc.Line,
                Token.Print(node.CloseBraceToken)
            );
            return node.Parent
                is not (
                    ObjectCreationExpressionSyntax
                    or ArrayCreationExpressionSyntax
                    or ImplicitArrayCreationExpressionSyntax
                    or ImplicitObjectCreationExpressionSyntax
                )
            && node.Kind() is not SyntaxKind.WithInitializerExpression
                ? Doc.Group(result)
                : result;
        }
    }
}
