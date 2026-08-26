using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitArrayCreationExpression
{
    public static Doc Print(
        ImplicitArrayCreationExpressionSyntax node,
        CSharpPrintingContext context
    )
    {
        var commas = new Doc[node.Commas.Count];
        for (var index = 0; index < commas.Length; index++)
        {
            commas[index] = Token.Print(node.Commas[index], context);
        }

        return Doc.Group(
            Token.Print(node.NewKeyword, context),
            Token.Print(node.OpenBracketToken, context),
            Doc.Concat(commas),
            Token.Print(node.CloseBracketToken, context),
            Doc.Line,
            InitializerExpression.Print(node.Initializer, context)
        );
    }
}
