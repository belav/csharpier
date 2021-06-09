using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ArgumentList
    {
        public static Doc Print(ArgumentListSyntax node)
        {
            return Doc.Group(
                ArgumentListLike.Print(
                    node.OpenParenToken,
                    node.Arguments,
                    node.CloseParenToken
                )
            );
    }
    }
}
