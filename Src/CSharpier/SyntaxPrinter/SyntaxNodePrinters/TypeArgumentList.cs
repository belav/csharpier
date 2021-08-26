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
            Doc separator =
                node.Arguments.Count > 1 || node.Arguments.Any(o => o is GenericNameSyntax)
                    ? Doc.SoftLine
                    : Doc.Null;

            return Doc.Concat(
                Token.Print(node.LessThanToken),
                Doc.Indent(
                    separator,
                    SeparatedSyntaxList.Print(node.Arguments, Node.Print, Doc.Line)
                ),
                separator,
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
