namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BasePropertyDeclaration
{
    public static Doc Print(BasePropertyDeclarationSyntax node, PrintingContext context)
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

        return Doc.Group(
            Doc.Concat(
                AttributeLists.Print(node, node.AttributeLists, context),
                Modifiers.PrintSorted(node.Modifiers, context),
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
        PrintingContext context
    )
    {
        Doc contents = string.Empty;
        if (node.AccessorList != null)
        {
            Doc separator = " ";
            if (
                node.AccessorList.Accessors.Any(o =>
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
                        node.AccessorList.Accessors.Select(o =>
                                PrintAccessorDeclarationSyntax(o, separator, context)
                            )
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
        PrintingContext context
    )
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null]);
        if (
            node.AttributeLists.Count > 1
            || node.Body != null
            || node.ExpressionBody != null
            || (
                node.AttributeLists.FirstOrDefault() is { } attributeListSyntax
                && attributeListSyntax.Attributes.First().ArgumentList?.Arguments.Count > 0
            )
        )
        {
            docs.Append(Doc.HardLine);
        }
        else
        {
            docs.Append(separator);
        }

        docs.Append(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Append(Modifiers.PrintSorted(node.Modifiers, context));
        docs.Append(Token.Print(node.Keyword, context));

        if (node.Body != null)
        {
            docs.Append(Block.Print(node.Body, context));
        }
        else if (node.ExpressionBody != null)
        {
            docs.Append(ArrowExpressionClause.Print(node.ExpressionBody, context));
        }

        docs.Append(Token.Print(node.SemicolonToken, context));

        return Doc.Concat(ref docs);
    }
}
