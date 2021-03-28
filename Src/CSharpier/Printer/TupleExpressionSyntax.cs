using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleExpressionSyntax(TupleExpressionSyntax node) =>
            Group(
                PrintArgumentListLikeSyntax(
                    node.OpenParenToken,
                    node.Arguments,
                    node.CloseParenToken
                )
            );
    }
}
