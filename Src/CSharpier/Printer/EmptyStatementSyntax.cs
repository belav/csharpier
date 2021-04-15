using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintEmptyStatementSyntax(EmptyStatementSyntax node)
        {
            return SyntaxTokens.Print(node.SemicolonToken);
        }
    }
}
