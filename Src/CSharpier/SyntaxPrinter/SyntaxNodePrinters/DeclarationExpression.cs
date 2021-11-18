using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DeclarationExpression
{
    public static Doc Print(DeclarationExpressionSyntax node)
    {
        return Doc.Concat(Node.Print(node.Type), " ", Node.Print(node.Designation));
    }
}
