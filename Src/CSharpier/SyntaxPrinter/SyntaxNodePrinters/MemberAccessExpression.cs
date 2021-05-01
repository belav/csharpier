using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class MemberAccessExpression
    {
        public static Doc Print(MemberAccessExpressionSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Expression),
                Token.Print(node.OperatorToken),
                Node.Print(node.Name)
            );
        }
    }
}
