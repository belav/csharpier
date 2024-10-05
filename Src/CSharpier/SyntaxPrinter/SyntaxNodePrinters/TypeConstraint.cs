namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeConstraint
{
    public static Doc Print(TypeConstraintSyntax node, PrintingContext context)
    {
        return Node.Print(node.Type, context);
    }
}
