using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class ConstraintClauses
    {
        public static Doc Print(IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            var constraintClausesList = constraintClauses.ToList();

            if (constraintClausesList.Count == 0)
            {
                return Doc.Null;
            }
            else if (constraintClausesList.Count == 1)
            {
                return Doc.Group(
                    Doc.Indent(
                        Doc.Line,
                        Doc.Join(
                            Doc.Line,
                            constraintClausesList.Select(TypeParameterConstraintClause.Print)
                        )
                    )
                );
            }

            if (constraintClausesList[0].Parent is MethodDeclarationSyntax)
            {
                return Doc.Concat(
                    " ",
                    Doc.Align(
                        2,
                        Doc.Join(
                            Doc.HardLine,
                            constraintClausesList.Select(TypeParameterConstraintClause.Print)
                        )
                    )
                );
            }

            return Doc.Indent(
                Doc.HardLine,
                Doc.Join(
                    Doc.HardLine,
                    constraintClausesList.Select(TypeParameterConstraintClause.Print)
                )
            );
        }
    }
}
