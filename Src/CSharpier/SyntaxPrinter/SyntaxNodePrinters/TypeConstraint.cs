namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeConstraint
{
    public static Doc Print(TypeConstraintSyntax node)
    {
        return Node.Print(node.Type);
    }
}
