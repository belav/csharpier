namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberBindingExpression
{
    public static Doc Print(MemberBindingExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(Token.Print(node.OperatorToken, context), Node.Print(node.Name, context));
    }
}
