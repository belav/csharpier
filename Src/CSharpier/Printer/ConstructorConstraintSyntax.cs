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
            return Doc.Concat(
                Token.Print(node.NewKeyword),
                Token.Print(node.OpenParenToken),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
