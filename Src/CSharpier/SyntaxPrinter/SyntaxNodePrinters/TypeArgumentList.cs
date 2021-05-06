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
                Doc.Indent(SeparatedSyntaxList.Print(node.Arguments, Node.Print, Doc.Line)),
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
