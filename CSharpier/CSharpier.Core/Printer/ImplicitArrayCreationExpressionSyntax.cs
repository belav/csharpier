using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintImplicitArrayCreationExpressionSyntax(ImplicitArrayCreationExpressionSyntax node)
        {
            var commas = node.Commas.Select(o => String(",")).ToArray();
            return Concat(
                node.NewKeyword.Text,
                "[",
                Concat(commas),
                "]",
                " ",
                this.Print(node.Initializer)
            );
        }
    }
}
