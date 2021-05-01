using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class ConstraintClauses
    {
        public static Doc Print(
            IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses
        ) {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return Doc.Null;
            }

            var docs = new List<Doc>
            {
                Doc.Indent(
                    Doc.HardLine,
                    Doc.Join(
                        Doc.HardLine,
                        constraintClausesList.Select(
                            TypeParameterConstraintClause.Print
                        )
                    )
                )
            };

            return Doc.Concat(docs);
        }
    }
}
