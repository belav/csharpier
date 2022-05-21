namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class VariableDeclaration
{
    public static Doc Print(VariableDeclarationSyntax node, FormattingContext context)
    {
        if (node.Variables.Count > 1)
        {
            return Doc.Concat(
                Node.Print(node.Type, context),
                " ",
                Doc.Indent(
                    SeparatedSyntaxList.Print(
                        node.Variables,
                        VariableDeclarator.Print,
                        node.Parent is ForStatementSyntax ? Doc.Line : Doc.HardLine,
                        context
                    )
                )
            );
        }

        var variable = node.Variables[0];

        var leftDoc = Doc.Concat(
            Node.Print(node.Type, context),
            " ",
            Token.Print(variable.Identifier, context),
            variable.ArgumentList != null
              ? BracketedArgumentList.Print(variable.ArgumentList, context)
              : Doc.Null
        );

        var initializer = variable.Initializer;

        return initializer == null
          ? Doc.Group(leftDoc)
          : RightHandSide.Print(
                node,
                Doc.Concat(leftDoc, " "),
                Token.Print(initializer.EqualsToken, context),
                initializer.Value,
                context
            );
    }
}
