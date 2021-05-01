using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUnsafeStatementSyntax(UnsafeStatementSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.UnsafeKeyword),
                this.Print(node.Block)
            );
        }
    }
}
