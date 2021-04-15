using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorConstraintSyntax(
            ConstructorConstraintSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.NewKeyword),
                SyntaxTokens.Print(node.OpenParenToken),
                SyntaxTokens.Print(node.CloseParenToken)
            );
        }
    }
}
