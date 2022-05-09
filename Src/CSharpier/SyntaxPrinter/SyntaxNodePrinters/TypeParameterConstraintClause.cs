namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeParameterConstraintClause
{
    public static Doc Print(TypeParameterConstraintClauseSyntax node, FormattingContext context)
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
