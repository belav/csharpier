using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDefaultSwitchLabelSyntax(
            DefaultSwitchLabelSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.Keyword),
                Token.Print(node.ColonToken)
            );
        }
    }
}
