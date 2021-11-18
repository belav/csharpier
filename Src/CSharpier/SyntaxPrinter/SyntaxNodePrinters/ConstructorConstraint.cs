using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ConstructorConstraint
{
    public static Doc Print(ConstructorConstraintSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.NewKeyword),
            Token.Print(node.OpenParenToken),
            Token.Print(node.CloseParenToken)
        );
    }
}
