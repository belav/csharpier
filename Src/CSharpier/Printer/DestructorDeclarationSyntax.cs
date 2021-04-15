using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDestructorDeclarationSyntax(
            DestructorDeclarationSyntax node
        ) {
            return Docs.Concat(
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers),
                SyntaxTokens.Print(node.TildeToken),
                SyntaxTokens.Print(node.Identifier),
                this.Print(node.ParameterList),
                this.Print(node.Body),
                this.Print(node.ExpressionBody),
                SyntaxTokens.Print(node.SemicolonToken)
            );
        }
    }
}
