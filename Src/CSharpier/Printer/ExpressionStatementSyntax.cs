using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExpressionStatementSyntax(
            ExpressionStatementSyntax node
        ) {
            return Doc.Group(
                ExtraNewLines.Print(node),
                this.Print(node.Expression),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
