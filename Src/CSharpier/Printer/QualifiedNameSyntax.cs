using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQualifiedNameSyntax(QualifiedNameSyntax node)
        {
            return Docs.Concat(
                this.Print(node.Left),
                SyntaxTokens.Print(node.DotToken),
                this.Print(node.Right)
            );
        }
    }
}
