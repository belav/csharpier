using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitArrayCreationExpressionSyntax(
            ImplicitArrayCreationExpressionSyntax node
        ) {
            var commas = node.Commas.Select(Token.Print).ToArray();
            return Doc.Concat(
                Token.Print(node.NewKeyword),
                Token.Print(node.OpenBracketToken),
                Doc.Concat(commas),
                this.PrintSyntaxToken(
                    node.CloseBracketToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.Print(node.Initializer)
            );
        }
    }
}
