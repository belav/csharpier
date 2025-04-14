using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayType
{
    public static Doc Print(ArrayTypeSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.ElementType, context),
            Doc.Concat(node.RankSpecifiers.Select(o => Node.Print(o, context)).ToArray())
        );
    }
}
