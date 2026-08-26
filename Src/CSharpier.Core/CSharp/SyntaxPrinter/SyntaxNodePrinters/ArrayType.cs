using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayType
{
    public static Doc Print(ArrayTypeSyntax node, CSharpPrintingContext context)
    {
        var rankSpecifiers = new Doc[node.RankSpecifiers.Count];
        for (var index = 0; index < rankSpecifiers.Length; index++)
        {
            rankSpecifiers[index] = Node.Print(node.RankSpecifiers[index], context);
        }

        return Doc.Concat(Node.Print(node.ElementType, context), Doc.Concat(rankSpecifiers));
    }
}
