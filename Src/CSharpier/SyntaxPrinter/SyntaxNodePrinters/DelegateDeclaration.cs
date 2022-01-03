namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DelegateDeclaration
{
    public static Doc Print(DelegateDeclarationSyntax node)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers),
            Token.PrintWithSuffix(node.DelegateKeyword, " "),
            Node.Print(node.ReturnType),
            { " ", Token.Print(node.Identifier) }
        };
        if (node.TypeParameterList != null)
        {
            docs.Add(Node.Print(node.TypeParameterList));
        }
        docs.Add(
            Node.Print(node.ParameterList),
            ConstraintClauses.Print(node.ConstraintClauses),
            Token.Print(node.SemicolonToken)
        );
        return Doc.Concat(docs);
    }
}
