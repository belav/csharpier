using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BaseList
    {
        public static Doc Print(BaseListSyntax node)
        {
            return Doc.Concat(
                " ",
                Token.Print(node.ColonToken),
                Doc.Indent(
                    Doc.Group(Doc.Line, SeparatedSyntaxList.Print(node.Types, Node.Print, Doc.Line))
                )
            );
        }
    }
}
