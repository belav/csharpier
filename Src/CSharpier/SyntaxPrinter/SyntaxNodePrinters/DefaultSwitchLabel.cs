using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DefaultSwitchLabel
    {
        public static Doc Print(DefaultSwitchLabelSyntax node)
        {
            return Doc.Concat(Token.Print(node.Keyword), Token.Print(node.ColonToken));
        }
    }
}
