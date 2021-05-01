using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintNameColonSyntax(NameColonSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Name.Identifier),
                this.PrintSyntaxToken(
                    node.ColonToken,
                    afterTokenIfNoTrailing: " "
                )
            );
        }
    }
}
