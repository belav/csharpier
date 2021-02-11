using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectCreationExpressionSyntax(AnonymousObjectCreationExpressionSyntax node)
        {
            // TODO trivia this one is seperatedSyntaxList, vs statements which is not.
            // maybe I just don't share the code between them.
            return Concat("new", this.PrintStatements(node.OpenBraceToken, node.Initializers, node.CloseBraceToken, Line, ","));
        }
    }
}
