namespace CSharpier.SyntaxPrinter;

internal static class ConstraintClauses
{
    public static Doc Print(
        IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses,
        FormattingContext context
    )
    {
        var constraintClausesList = constraintClauses.ToList();

        if (constraintClausesList.Count == 0)
        {
            return Doc.Null;
        }
        var body = Doc.Join(
            Doc.HardLine,
            constraintClausesList.Select(o => TypeParameterConstraintClause.Print(o, context))
        );

        return Doc.Group(Doc.Indent(Doc.HardLine, body));
    }
}
