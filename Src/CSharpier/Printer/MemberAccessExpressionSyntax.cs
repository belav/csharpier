using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintMemberAccessExpressionSyntax(
            MemberAccessExpressionSyntax node
        ) {
            return Doc.Concat(
                this.Print(node.Expression),
                Token.Print(node.OperatorToken),
                this.Print(node.Name)
            );
        }
    }
}
