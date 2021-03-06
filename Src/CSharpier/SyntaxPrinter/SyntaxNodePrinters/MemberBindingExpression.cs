using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class MemberBindingExpression
    {
        public static Doc Print(MemberBindingExpressionSyntax node)
        {
            return Doc.Concat(Token.Print(node.OperatorToken), Node.Print(node.Name));
        }
    }
}
