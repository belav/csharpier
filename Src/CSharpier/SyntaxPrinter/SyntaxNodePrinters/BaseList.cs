using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BaseList
    {
        public static Doc Print(BaseListSyntax node)
        {
            return Doc.Group(
                Doc.Indent(
                    Doc.Line,
                    Token.Print(node.ColonToken),
                    " ",
                    Doc.Align(
                        2,
                        Doc.Concat(SeparatedSyntaxList.Print(node.Types, Node.Print, Doc.Line))
                    )
                )
            );
        }
    }
}
