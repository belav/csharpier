using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExternAliasDirective
{
    public static Doc Print(ExternAliasDirectiveSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.ExternKeyword, " "),
            Token.PrintWithSuffix(node.AliasKeyword, " "),
            Token.Print(node.Identifier),
            Token.Print(node.SemicolonToken)
        );
    }
}
