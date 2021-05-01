using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ExpressionStatement
    {
        public static Doc Print(ExpressionStatementSyntax node)
        {
            return Doc.Group(
                ExtraNewLines.Print(node),
                Node.Print(node.Expression),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
