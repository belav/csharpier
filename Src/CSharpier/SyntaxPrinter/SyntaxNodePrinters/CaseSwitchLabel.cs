using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CaseSwitchLabel
    {
        public static Doc Print(CaseSwitchLabelSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.PrintWithSuffix(node.Keyword, " "),
                Doc.Group(Node.Print(node.Value)),
                Token.Print(node.ColonToken)
            );
        }
    }
}
