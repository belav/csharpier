using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExternAliasDirective
{
    public static Doc Print(
        ExternAliasDirectiveSyntax node,
        PrintingContext context,
        bool printExtraLines = true
    )
    {
        return Doc.Concat(
            printExtraLines ? ExtraNewLines.Print(node) : Doc.Null,
            Token.PrintWithSuffix(node.ExternKeyword, " ", context),
            Token.PrintWithSuffix(node.AliasKeyword, " ", context),
            Token.Print(node.Identifier, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
