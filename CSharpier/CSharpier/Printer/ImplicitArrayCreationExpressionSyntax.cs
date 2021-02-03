using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitArrayCreationExpressionSyntax(ImplicitArrayCreationExpressionSyntax node)
        {
            var commas = node.Commas.Select(o => String(",")).ToArray();
            return Concat(
                String(node.NewKeyword.Text),
                String("["),
                Concat(commas),
                String("]"),
                String(" "),
                this.Print(node.Initializer)
            );
        }
    }
}
