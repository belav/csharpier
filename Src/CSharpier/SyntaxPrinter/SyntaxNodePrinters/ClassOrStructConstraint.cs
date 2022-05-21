namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ClassOrStructConstraint
{
    public static Doc Print(ClassOrStructConstraintSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.ClassOrStructKeyword, context),
            Token.Print(node.QuestionToken, context)
        );
    }
}
