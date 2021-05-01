using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQualifiedNameSyntax(QualifiedNameSyntax node)
        {
            return Doc.Concat(
                this.Print(node.Left),
                Token.Print(node.DotToken),
                this.Print(node.Right)
            );
        }
    }
}
