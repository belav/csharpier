using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberBindingExpression
{
    public static Doc Print(MemberBindingExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(Token.Print(node.OperatorToken, context), Node.Print(node.Name, context));
    }
}
