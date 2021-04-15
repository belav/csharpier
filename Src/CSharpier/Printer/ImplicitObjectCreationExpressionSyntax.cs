using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitObjectCreationExpressionSyntax(
            ImplicitObjectCreationExpressionSyntax node
        ) {
            // TODO 1 more tests for this?
            return Docs.Concat(
                SyntaxTokens.Print(node.NewKeyword),
                this.PrintArgumentListSyntax(node.ArgumentList),
                this.Print(node.Initializer)
            );
        }
    }
}
