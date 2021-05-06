using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RefExpression
    {
        public static Doc Print(RefExpressionSyntax node)
        {
            // TODO 1 should all " " turn into spaceIfNoPreviousComment? Maybe we just make a type for space and make it do that?
            return Doc.Concat(Token.Print(node.RefKeyword, " "), Node.Print(node.Expression));
        }
    }
}
