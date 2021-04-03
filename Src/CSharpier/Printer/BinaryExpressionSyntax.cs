using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBinaryExpressionSyntax(BinaryExpressionSyntax node)
        {
            var useLine =
                node.OperatorToken.Kind() is SyntaxKind.BarBarToken or SyntaxKind.BarToken or SyntaxKind.AmpersandAmpersandToken or SyntaxKind.AmpersandToken or SyntaxKind.PlusToken;

            return Concat(
                this.Print(node.Left),
                useLine ? Line : SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.OperatorToken),
                SpaceIfNoPreviousComment,
                this.Print(node.Right)
            );
        }
    }
}
