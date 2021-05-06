using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TypeParameterConstraintClause
    {
        public static Doc Print(TypeParameterConstraintClauseSyntax node)
        {
            return Doc.Group(
                Token.Print(node.WhereKeyword, " "),
                Node.Print(node.Name),
                " ",
                Token.Print(node.ColonToken, " "),
                Doc.Indent(SeparatedSyntaxList.Print(node.Constraints, Node.Print, Doc.Line))
            );
        }
    }
}
