using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleExpressionSyntax(TupleExpressionSyntax node) =>
            Doc.Group(
                PrintArgumentListLikeSyntax(
                    node.OpenParenToken,
                    node.Arguments,
                    node.CloseParenToken
                )
            );
    }
}
