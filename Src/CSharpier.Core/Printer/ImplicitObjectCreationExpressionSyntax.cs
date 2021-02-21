using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintImplicitObjectCreationExpressionSyntax(ImplicitObjectCreationExpressionSyntax node)
        {
            // TODO 1 more tests for this?
            return Concat(this.PrintSyntaxToken(node.NewKeyword),
                this.PrintArgumentListSyntax(node.ArgumentList),
                this.Print(node.Initializer));
        }
    }
}
