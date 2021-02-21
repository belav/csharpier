using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintBinaryPatternSyntax(BinaryPatternSyntax node)
        {
            return Concat(this.Print(node.Left),
                SpaceIfNoPreviousComment,
                this.PrintSyntaxToken(node.OperatorToken, " "),
                this.Print(node.Right));
        }
    }
}
