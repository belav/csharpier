using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TypeArgumentList
    {
        public static Doc Print(TypeArgumentListSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.LessThanToken),
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(node.Arguments, Node.Print, Doc.Line)
                ),
                Doc.SoftLine,
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
