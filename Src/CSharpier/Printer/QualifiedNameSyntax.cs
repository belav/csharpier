using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQualifiedNameSyntax(QualifiedNameSyntax node)
        {
            return Concat(
                this.Print(node.Left),
                this.PrintSyntaxToken(node.DotToken),
                this.Print(node.Right)
            );
        }
    }
}
