using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
                    Token.PrintWithSuffix(node.ColonToken, " "),
                    Doc.Indent(
                        SeparatedSyntaxList.Print(
                            node.Types,
                            Node.Print,
                            Doc.Line
                        )
                    )
                )
            );
        }
    }
}
