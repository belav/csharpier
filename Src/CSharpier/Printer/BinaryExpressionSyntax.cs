using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBinaryExpressionSyntax(BinaryExpressionSyntax node)
        {
            // TODO see prettier/src/language-js/print/binaryish.js for what we really need to do in here
            var useLine =
                node.OperatorToken.Kind() is SyntaxKind.BarBarToken or SyntaxKind.BarToken or SyntaxKind.AmpersandAmpersandToken or SyntaxKind.AmpersandToken;

            return Docs.Concat(
                this.Print(node.Left),
                useLine ? Docs.Line : Docs.SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.OperatorToken),
                Docs.SpaceIfNoPreviousComment,
                this.Print(node.Right)
            );
        }
    }
}
