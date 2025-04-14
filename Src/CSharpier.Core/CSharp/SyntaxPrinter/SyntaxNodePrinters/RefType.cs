using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class RefType
{
    public static Doc Print(RefTypeSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.RefKeyword, " ", context),
            Token.PrintWithSuffix(node.ReadOnlyKeyword, " ", context),
            Node.Print(node.Type, context)
        );
    }
}
