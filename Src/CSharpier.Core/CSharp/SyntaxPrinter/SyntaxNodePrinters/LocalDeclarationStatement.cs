using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class LocalDeclarationStatement
{
    public static Doc Print(LocalDeclarationStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.AwaitKeyword, " ", context),
            Token.PrintWithSuffix(node.UsingKeyword, " ", context),
            Modifiers.Print(node.Modifiers, context),
            VariableDeclaration.Print(node.Declaration, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
