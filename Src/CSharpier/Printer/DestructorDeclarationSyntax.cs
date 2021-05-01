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
            return Doc.Concat(
                ExtraNewLines.Print(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.TildeToken),
                Token.Print(node.Identifier),
                this.Print(node.ParameterList),
                this.Print(node.Body),
                this.Print(node.ExpressionBody),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
