using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DestructorDeclaration
    {
        public static Doc Print(DestructorDeclarationSyntax node)
        {
            return Doc.Concat(
                ExtraNewLines.Print(node),
                new Printer().PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.TildeToken),
                Token.Print(node.Identifier),
                Node.Print(node.ParameterList),
                Node.Print(node.Body),
                Node.Print(node.ExpressionBody),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
