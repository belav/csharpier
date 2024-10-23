namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AttributeList
{
    public static Doc Print(AttributeListSyntax node, PrintingContext context)
    {
        if (node.Parent is BaseMethodDeclarationSyntax && CSharpierIgnore.HasIgnoreComment(node))
        {
            return CSharpierIgnore.PrintWithoutFormatting(node, context).Trim();
        }

        var docs = new List<Doc>();
        if (
            node.Parent is CompilationUnitSyntax compilationUnitSyntax
            && compilationUnitSyntax.AttributeLists.First() != node
        )
        {
            docs.Add(ExtraNewLines.Print(node));
        }

        docs.Add(Token.Print(node.OpenBracketToken, context));
        if (node.Target != null)
        {
            docs.Add(
                Token.Print(node.Target.Identifier, context),
                Token.PrintWithSuffix(node.Target.ColonToken, " ", context)
            );
        }

        var printSeparatedSyntaxList = SeparatedSyntaxList.Print(
            node.Attributes,
            (attributeNode, _) =>
            {
                var name = Node.Print(attributeNode.Name, context);
                if (attributeNode.ArgumentList == null)
                {
                    return name;
                }

                return Doc.Group(
                    name,
                    Token.Print(attributeNode.ArgumentList.OpenParenToken, context),
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(
                            attributeNode.ArgumentList.Arguments,
                            (attributeArgumentNode, _) =>
                                Doc.Concat(
                                    attributeArgumentNode.NameEquals != null
                                        ? NameEquals.Print(
                                            attributeArgumentNode.NameEquals,
                                            context
                                        )
                                        : Doc.Null,
                                    attributeArgumentNode.NameColon != null
                                        ? BaseExpressionColon.Print(
                                            attributeArgumentNode.NameColon,
                                            context
                                        )
                                        : Doc.Null,
                                    Node.Print(attributeArgumentNode.Expression, context)
                                ),
                            Doc.Line,
                            context
                        )
                    ),
                    Doc.SoftLine,
                    Token.Print(attributeNode.ArgumentList.CloseParenToken, context)
                );
            },
            Doc.Line,
            context
        );

        docs.Add(
            node.Attributes.Count > 1
                ? Doc.Indent(Doc.SoftLine, printSeparatedSyntaxList)
                : printSeparatedSyntaxList
        );

        docs.Add(
            node.Attributes.Count > 1 ? Doc.SoftLine : Doc.Null,
            Token.Print(node.CloseBracketToken, context)
        );

        return Doc.Group(docs);
    }
}
