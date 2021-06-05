using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CaseSwitchLabel
    {
        public static Doc Print(CaseSwitchLabelSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.Keyword, " "),
                Doc.Group(Node.Print(node.Value)),
                Token.Print(node.ColonToken)
            );
        }
    }
}
