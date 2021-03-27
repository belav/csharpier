using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBasePropertyDeclarationSyntax(
            BasePropertyDeclarationSyntax node)
        {
            EqualsValueClauseSyntax? initializer = null;
            ExplicitInterfaceSpecifierSyntax? explicitInterfaceSpecifierSyntax =
                null;
            Doc identifier = Doc.Null;
            Doc eventKeyword = Doc.Null;
            ArrowExpressionClauseSyntax? expressionBody = null;
            SyntaxToken? semicolonToken = null;

            if (node is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                expressionBody = propertyDeclarationSyntax.ExpressionBody;
                initializer = propertyDeclarationSyntax.Initializer;
                explicitInterfaceSpecifierSyntax = propertyDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = this.PrintSyntaxToken(
                    propertyDeclarationSyntax.Identifier
                );
                semicolonToken = propertyDeclarationSyntax.SemicolonToken;
            }
            else if (node is IndexerDeclarationSyntax indexerDeclarationSyntax)
            {
                expressionBody = indexerDeclarationSyntax.ExpressionBody;
                explicitInterfaceSpecifierSyntax = indexerDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = Concat(
                    this.PrintSyntaxToken(indexerDeclarationSyntax.ThisKeyword),
                    this.Print(indexerDeclarationSyntax.ParameterList)
                );
                semicolonToken = indexerDeclarationSyntax.SemicolonToken;
            }
            else if (node is EventDeclarationSyntax eventDeclarationSyntax)
            {
                eventKeyword = this.PrintSyntaxToken(
                    eventDeclarationSyntax.EventKeyword,
                    " "
                );
                explicitInterfaceSpecifierSyntax = eventDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = this.PrintSyntaxToken(
                    eventDeclarationSyntax.Identifier
                );
                semicolonToken = eventDeclarationSyntax.SemicolonToken;
            }

            Doc contents = "";
            if (node.AccessorList != null)
            {
                var separator = SpaceIfNoPreviousComment;
                if (
                    node.AccessorList.Accessors.Any(
                        o => o.Body != null
                        || o.ExpressionBody != null
                        || o.Modifiers.Any()
                        || o.AttributeLists.Any()
                    )
                )
                {
                    separator = Line;
                }

                contents = Group(
                    Concat(
                        separator,
                        this.PrintSyntaxToken(node.AccessorList.OpenBraceToken),
                        Group(
                            Indent(
                                node.AccessorList.Accessors.Select(
                                        o => this.PrintAccessorDeclarationSyntax(
                                            o,
                                            separator
                                        )
                                    )
                                    .ToArray()
                            )
                        ),
                        separator,
                        this.PrintSyntaxToken(node.AccessorList.CloseBraceToken)
                    )
                );
            }
            else if (expressionBody != null)
            {
                contents = Concat(
                    this.PrintArrowExpressionClauseSyntax(expressionBody)
                );
            }


            var parts = new Parts();

            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));

            return Group(
                Concat(
                    Concat(parts),
                    this.PrintModifiers(node.Modifiers),
                    eventKeyword,
                    this.Print(node.Type),
                    " ",
                    explicitInterfaceSpecifierSyntax != null
                        ? Concat(
                            this.Print(explicitInterfaceSpecifierSyntax.Name),
                            this.PrintSyntaxToken(
                                explicitInterfaceSpecifierSyntax.DotToken
                            )
                        )
                        : Doc.Null,
                    identifier,
                    contents,
                    initializer != null
                        ? this.PrintEqualsValueClauseSyntax(initializer)
                        : Doc.Null,
                    semicolonToken.HasValue
                        ? this.PrintSyntaxToken(semicolonToken.Value)
                        : Doc.Null
                )
            );
        }

        private Doc PrintAccessorDeclarationSyntax(
            AccessorDeclarationSyntax node,
            Doc separator)
        {
            var parts = new Parts();
            if (
                node.Modifiers.Count > 0
                || node.AttributeLists.Count > 0
                || node.Body != null
                || node.ExpressionBody != null
            )
            {
                parts.Push(HardLine);
            }
            else
            {
                parts.Push(separator);
            }

            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintSyntaxToken(node.Keyword));

            if (node.Body != null)
            {
                parts.Push(this.PrintBlockSyntax(node.Body));
            }
            else if (node.ExpressionBody != null)
            {
                parts.Push(
                    this.PrintArrowExpressionClauseSyntax(node.ExpressionBody)
                );
            }

            parts.Push(this.PrintSyntaxToken(node.SemicolonToken));

            return Concat(parts);
        }
    }
}
