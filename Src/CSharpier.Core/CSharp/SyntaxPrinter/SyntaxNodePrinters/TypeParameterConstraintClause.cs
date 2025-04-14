using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeParameterConstraintClause
{
    public static Doc Print(TypeParameterConstraintClauseSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Token.PrintWithSuffix(node.WhereKeyword, " ", context),
            Node.Print(node.Name, context),
            " ",
            Token.PrintWithSuffix(node.ColonToken, " ", context),
            Doc.Indent(SeparatedSyntaxList.Print(node.Constraints, Node.Print, Doc.Line, context))
        );
    }
}
