using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class GenericName
{
    public static Doc Print(GenericNameSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintLeadingTrivia(node.Identifier, context),
            Doc.Group(
                Token.PrintWithoutLeadingTrivia(node.Identifier, context),
                Node.Print(node.TypeArgumentList, context)
            )
        );
    }
}
