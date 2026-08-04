using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class NullableType
{
    public static Doc Print(NullableTypeSyntax node, CSharpPrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.ElementType, context),
            Token.Print(node.QuestionToken, context)
        );
    }
}
