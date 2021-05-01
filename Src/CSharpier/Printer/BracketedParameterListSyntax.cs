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
            return Doc.Concat(
                Token.Print(node.OpenBracketToken),
                SeparatedSyntaxList.Print(
                    node.Parameters,
                    this.PrintParameterSyntax,
                    " "
                ),
                Token.Print(node.CloseBracketToken)
            );
        }
    }
}
