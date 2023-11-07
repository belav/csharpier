namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BasePropertyDeclaration
{
    public static Doc Print(BasePropertyDeclarationSyntax node, FormattingContext context)
    {
        EqualsValueClauseSyntax? initializer = null;
        ExplicitInterfaceSpecifierSyntax? explicitInterfaceSpecifierSyntax = null;
        Func<Doc>? identifier = null;
        Doc eventKeyword = Doc.Null;
        ArrowExpressionClauseSyntax? expressionBody = null;
        SyntaxToken? semicolonToken = null;

        if (node is PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            expressionBody = propertyDeclarationSyntax.ExpressionBody;
            initializer = propertyDeclarationSyntax.Initializer;
            explicitInterfaceSpecifierSyntax = propertyDeclarationSyntax.ExplicitInterfaceSpecifier;
            identifier = () => Token.Print(propertyDeclarationSyntax.Identifier, context);
            semicolonToken = propertyDeclarationSyntax.SemicolonToken;
        }
        else if (node is IndexerDeclarationSyntax indexerDeclarationSyntax)
        {
            expressionBody = indexerDeclarationSyntax.ExpressionBody;
            explicitInterfaceSpecifierSyntax = indexerDeclarationSyntax.ExplicitInterfaceSpecifier;
            identifier = () =>
                Doc.Concat(
                    Token.Print(indexerDeclarationSyntax.ThisKeyword, context),
                    Node.Print(indexerDeclarationSyntax.ParameterList, context)
                );
            semicolonToken = indexerDeclarationSyntax.SemicolonToken;
        }
        else if (node is EventDeclarationSyntax eventDeclarationSyntax)
        {
            eventKeyword = Token.PrintWithSuffix(eventDeclarationSyntax.EventKeyword, " ", context);
            explicitInterfaceSpecifierSyntax = eventDeclarationSyntax.ExplicitInterfaceSpecifier;
            identifier = () => Token.Print(eventDeclarationSyntax.Identifier, context);
            semicolonToken = eventDeclarationSyntax.SemicolonToken;
        }

        var docs = new List<Doc> { AttributeLists.Print(node, node.AttributeLists, context) };

        return Doc.Group(
            Doc.Concat(
                Doc.Concat(docs),
                Modifiers.Print(node.Modifiers, context),
                eventKeyword,
                Node.Print(node.Type, context),
                " ",
                explicitInterfaceSpecifierSyntax != null
                    ? Doc.Concat(
                        Node.Print(explicitInterfaceSpecifierSyntax.Name, context),
                        Token.Print(explicitInterfaceSpecifierSyntax.DotToken, context)
                    )
                    : Doc.Null,
                identifier != null ? identifier() : Doc.Null,
                Contents(node, expressionBody, context),
                initializer != null ? EqualsValueClause.Print(initializer, context) : Doc.Null,
                semicolonToken.HasValue ? Token.Print(semicolonToken.Value, context) : Doc.Null
            )
        );
    }

    private static Doc Contents(
        BasePropertyDeclarationSyntax node,
        ArrowExpressionClauseSyntax? expressionBody,
        FormattingContext context
    )
    {
        Doc contents = string.Empty;
        if (node.AccessorList != null)
        {
            Doc separator = " ";
            if (
                node.AccessorList
                    .Accessors
                    .Any(
                        o =>
                            o.Body != null
                            || o.ExpressionBody != null
                            || o.Modifiers.Any()
                            || o.AttributeLists.Any()
                    )
            )
            {
                separator = Doc.Line;
            }

            contents = Doc.Group(
                Doc.Concat(
                    separator,
                    Token.Print(node.AccessorList.OpenBraceToken, context),
                    Doc.Indent(
                        node.AccessorList
                            .Accessors
                            .Select(o => PrintAccessorDeclarationSyntax(o, separator, context))
                            .ToArray()
                    ),
                    separator,
                    Token.Print(node.AccessorList.CloseBraceToken, context)
                )
            );
        }
        else if (expressionBody != null)
        {
            contents = ArrowExpressionClause.Print(expressionBody, context);
        }

        return contents;
    }

    private static Doc PrintAccessorDeclarationSyntax(
        AccessorDeclarationSyntax node,
        Doc separator,
        FormattingContext context
    )
    {
        var docs = new List<Doc>();
        if (node.AttributeLists.Count > 0 || node.Body != null || node.ExpressionBody != null)
        {
            docs.Add(Doc.HardLine);
        }
        else
        {
            docs.Add(separator);
        }

        docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Add(Modifiers.Print(node.Modifiers, context));
        docs.Add(Token.Print(node.Keyword, context));

        if (node.Body != null)
        {
            docs.Add(Block.Print(node.Body, context));
        }
        else if (node.ExpressionBody != null)
        {
            docs.Add(ArrowExpressionClause.Print(node.ExpressionBody, context));
        }

        docs.Add(Token.Print(node.SemicolonToken, context));

        return Doc.Concat(docs);
    }
}
