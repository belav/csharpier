using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class QualifiedName
{
    public static Doc Print(QualifiedNameSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Left, context),
            Token.Print(node.DotToken, context),
            Node.Print(node.Right, context)
        );
    }
}
