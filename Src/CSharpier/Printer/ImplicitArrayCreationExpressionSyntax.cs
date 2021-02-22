using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitArrayCreationExpressionSyntax(
            ImplicitArrayCreationExpressionSyntax node)
        {
            var commas = node.Commas.Select(o => this.PrintSyntaxToken(o)).ToArray();
            return Concat(
                this.PrintSyntaxToken(node.NewKeyword),
                this.PrintSyntaxToken(node.OpenBracketToken),
                Concat(commas),
                this.PrintSyntaxToken(node.CloseBracketToken, " "),
                this.Print(node.Initializer));
        }
    }
}
