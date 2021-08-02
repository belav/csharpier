using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DefaultSwitchLabel
    {
        public static Doc Print(DefaultSwitchLabelSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.Keyword),
                Token.Print(node.ColonToken)
            );
        }
    }
}
