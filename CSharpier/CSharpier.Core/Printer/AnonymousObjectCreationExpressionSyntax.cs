using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectCreationExpressionSyntax(AnonymousObjectCreationExpressionSyntax node)
        {
            return Concat("new", this.PrintStatements(node.OpenBraceToken, node.Initializers, node.CloseBraceToken, Line, ","));
        }
    }
}
