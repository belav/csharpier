using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class LocalDeclarationStatement
    {
        public static Doc Print(LocalDeclarationStatementSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.PrintWithSuffix(node.AwaitKeyword, " "),
                Token.PrintWithSuffix(node.UsingKeyword, " "),
                Modifiers.Print(node.Modifiers),
                VariableDeclaration.Print(node.Declaration),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
