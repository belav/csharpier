using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBracketedParameterListSyntax(
            BracketedParameterListSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.OpenBracketToken),
                this.PrintSeparatedSyntaxList(
                    node.Parameters,
                    this.PrintParameterSyntax,
                    " "
                ),
                SyntaxTokens.Print(node.CloseBracketToken)
            );
        }
    }
}
