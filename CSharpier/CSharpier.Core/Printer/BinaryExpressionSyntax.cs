using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 1 this needs some formatting help, could be fun
        private Doc PrintBinaryExpressionSyntax(BinaryExpressionSyntax node)
        {
            return Concat(
                this.Print(node.Left),
                " ",
                this.PrintSyntaxToken(node.OperatorToken),
                " ",
                this.Print(node.Right));
        }
    }
}
