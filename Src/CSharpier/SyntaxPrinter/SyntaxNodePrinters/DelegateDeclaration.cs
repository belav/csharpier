namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DelegateDeclaration
{
    public static Doc Print(DelegateDeclarationSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.PrintSorted(node.Modifiers, context),
            Token.PrintWithSuffix(node.DelegateKeyword, " ", context),
            Node.Print(node.ReturnType, context),
            { " ", Token.Print(node.Identifier, context) },
        };
        if (node.TypeParameterList != null)
        {
            docs.Add(Node.Print(node.TypeParameterList, context));
        }
        docs.Add(
            Node.Print(node.ParameterList, context),
            ConstraintClauses.Print(node.ConstraintClauses, context),
            Token.Print(node.SemicolonToken, context)
        );
        return Doc.Concat(docs);
    }
}
