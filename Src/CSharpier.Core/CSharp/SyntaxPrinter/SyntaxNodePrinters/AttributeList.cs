using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class AttributeList
{
    public static Doc Print(AttributeListSyntax node, PrintingContext context)
    {
        if (node.Parent is BaseMethodDeclarationSyntax && CSharpierIgnore.HasIgnoreComment(node))
        {
            return CSharpierIgnore.PrintWithoutFormatting(node, context).Trim();
        }

        var docs = new DocListBuilder(8);
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

                var singleCollectionExpression =
                    attributeNode.ArgumentList.Arguments
                    is [
                        {
                            Expression: CollectionExpressionSyntax,
                            NameColon: null,
                            NameEquals: null
                        },
                    ];

                return Doc.Group(
                    name,
                    Token.Print(attributeNode.ArgumentList.OpenParenToken, context),
                    Doc.IndentIf(
                        !singleCollectionExpression,
                        Doc.Concat(
                            singleCollectionExpression ? Doc.Null : Doc.SoftLine,
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
                        )
                    ),
                    singleCollectionExpression ? Doc.Null : Doc.SoftLine,
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

        if (node.Attributes.Count > 1)
        {
            docs.Add(Doc.SoftLine);
        }

        docs.Add(Token.Print(node.CloseBracketToken, context));

        return Doc.Group(docs.ToArray());
    }
}
