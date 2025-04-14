using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitArrayCreationExpression
{
    public static Doc Print(ImplicitArrayCreationExpressionSyntax node, PrintingContext context)
    {
        var commas = node.Commas.Select(o => Token.Print(o, context)).ToArray();
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
