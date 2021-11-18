namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class MemberBindingExpression
{
    public static Doc Print(MemberBindingExpressionSyntax node)
    {
        return Doc.Concat(Token.Print(node.OperatorToken), Node.Print(node.Name));
    }
}
