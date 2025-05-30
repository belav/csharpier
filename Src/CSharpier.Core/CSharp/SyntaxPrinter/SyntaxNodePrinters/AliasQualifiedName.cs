using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class AliasQualifiedName
{
    public static Doc Print(AliasQualifiedNameSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Alias, context),
            Token.Print(node.ColonColonToken, context),
            Node.Print(node.Name, context)
        );
    }
}
