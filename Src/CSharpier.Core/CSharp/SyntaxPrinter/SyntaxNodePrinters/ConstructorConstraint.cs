using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConstructorConstraint
{
    public static Doc Print(ConstructorConstraintSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.NewKeyword, context),
            Token.Print(node.OpenParenToken, context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
