using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class ConstraintClauses
{
    public static Doc Print(
        SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses,
        PrintingContext context
    )
    {
        if (constraintClauses.Count == 0)
        {
            return Doc.Null;
        }
        var body = Doc.Join(
            Doc.HardLine,
            constraintClauses.Select(o => TypeParameterConstraintClause.Print(o, context))
        );

        return Doc.Group(Doc.Indent(Doc.HardLine, body));
    }
}
