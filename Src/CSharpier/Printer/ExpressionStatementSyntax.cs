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
            return Docs.Group(
                this.PrintExtraNewLines(node),
                this.Print(node.Expression),
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
