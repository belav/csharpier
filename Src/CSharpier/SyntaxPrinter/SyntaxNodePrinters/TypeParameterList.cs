using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TypeParameterList
    {
        public static Doc Print(TypeParameterListSyntax node)
        {
            if (node.Parameters.Count == 0)
            {
                return Doc.Null;
            }
            return Doc.Group(
                Token.Print(node.LessThanToken),
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(node.Parameters, TypeParameter.Print, Doc.Line)
                ),
                Doc.SoftLine,
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
