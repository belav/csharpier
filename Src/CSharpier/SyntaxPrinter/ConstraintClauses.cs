using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class ConstraintClauses
    {
        public static Doc PrintWithConditionalSpace(
            IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses,
            string groupId
        ) {
            return Print(constraintClauses, groupId);
        }

        public static Doc Print(IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            return Print(constraintClauses, null);
        }

        private static Doc Print(
            IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses,
            string? groupId
        ) {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return Doc.Null;
            }
            var prefix = constraintClausesList.Count >= 2 ? Doc.HardLine : Doc.Line;
            var body = Doc.Join(
                Doc.HardLine,
                constraintClausesList.Select(TypeParameterConstraintClause.Print)
            );

            return Doc.Group(
                Doc.Indent(groupId != null ? Doc.IfBreak(" ", prefix, groupId) : prefix),
                groupId != null
                    ? Doc.IfBreak(Doc.Align(2, body), Doc.Indent(body), groupId)
                    : Doc.Indent(body)
            );
        }
    }
}
