using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class BinaryPattern
{
    public static Doc Print(BinaryPatternSyntax node, PrintingContext context)
    {
        return Doc.IndentIf(
            node.Parent is SubpatternSyntax or IsPatternExpressionSyntax,
            Doc.Concat(
                Node.Print(node.Left, context),
                Doc.Line,
                Token.PrintWithSuffix(node.OperatorToken, " ", context),
                Node.Print(node.Right, context)
            )
        );
    }
}
