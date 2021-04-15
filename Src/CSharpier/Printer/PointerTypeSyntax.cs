using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPointerTypeSyntax(PointerTypeSyntax node)
        {
            return Docs.Concat(
                this.Print(node.ElementType),
                SyntaxTokens.Print(node.AsteriskToken)
            );
        }
    }
}
