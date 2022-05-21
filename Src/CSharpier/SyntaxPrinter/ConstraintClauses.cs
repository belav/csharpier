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
        var prefix = constraintClausesList.Count >= 2 ? Doc.HardLine : Doc.Line;
        var body = Doc.Join(
            Doc.HardLine,
            constraintClausesList.Select(o => TypeParameterConstraintClause.Print(o, context))
        );

        return Doc.Group(Doc.Indent(prefix, body));
    }
}
