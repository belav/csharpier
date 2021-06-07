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
                    node.Parameters.Count > 1 ? Doc.SoftLine : Doc.Null,
                    SeparatedSyntaxList.Print(node.Parameters, TypeParameter.Print, Doc.Line)
                ),
                node.Parameters.Count > 1 ? Doc.SoftLine : Doc.Null,
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
