using System.Collections.Generic;
using System.Linq;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBasePropertyDeclarationSyntax(
            BasePropertyDeclarationSyntax node
        ) {
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
            else if (
                node is IndexerDeclarationSyntax indexerDeclarationSyntax
            ) {
                expressionBody = indexerDeclarationSyntax.ExpressionBody;
                explicitInterfaceSpecifierSyntax = indexerDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = Docs.Concat(
                    SyntaxTokens.Print(indexerDeclarationSyntax.ThisKeyword),
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

            Doc contents = string.Empty;
            if (node.AccessorList != null)
            {
                Doc separator = Docs.SpaceIfNoPreviousComment;
                if (
                    node.AccessorList.Accessors.Any(
                        o =>
                            o.Body != null
                            || o.ExpressionBody != null
                            || o.Modifiers.Any()
                            || o.AttributeLists.Any()
                    )
                ) {
                    separator = Docs.Line;
                }

                contents = Docs.Group(
                    Docs.Concat(
                        separator,
                        SyntaxTokens.Print(node.AccessorList.OpenBraceToken),
                        Docs.Group(
                            Docs.Indent(
                                node.AccessorList.Accessors.Select(
                                        o =>
                                            this.PrintAccessorDeclarationSyntax(
                                                o,
                                                separator
                                            )
                                    )
                                    .ToArray()
                            )
                        ),
                        separator,
                        SyntaxTokens.Print(node.AccessorList.CloseBraceToken)
                    )
                );
            }
            else if (expressionBody != null)
            {
                contents = Docs.Concat(
                    this.PrintArrowExpressionClauseSyntax(expressionBody)
                );
            }

            var docs = new List<Doc>();

            docs.Add(this.PrintExtraNewLines(node));
            docs.Add(this.PrintAttributeLists(node, node.AttributeLists));

            return Docs.Group(
                Docs.Concat(
                    Docs.Concat(docs),
                    this.PrintModifiers(node.Modifiers),
                    eventKeyword,
                    this.Print(node.Type),
                    " ",
                    explicitInterfaceSpecifierSyntax != null
                        ? Docs.Concat(
                                this.Print(
                                    explicitInterfaceSpecifierSyntax.Name
                                ),
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
                        ? SyntaxTokens.Print(semicolonToken.Value)
                        : Doc.Null
                )
            );
        }

        private Doc PrintAccessorDeclarationSyntax(
            AccessorDeclarationSyntax node,
            Doc separator
        ) {
            var docs = new List<Doc>();
            if (
                node.Modifiers.Count > 0
                || node.AttributeLists.Count > 0
                || node.Body != null
                || node.ExpressionBody != null
            ) {
                docs.Add(Docs.HardLine);
            }
            else
            {
                docs.Add(separator);
            }

            docs.Add(this.PrintAttributeLists(node, node.AttributeLists));
            docs.Add(this.PrintModifiers(node.Modifiers));
            docs.Add(SyntaxTokens.Print(node.Keyword));

            if (node.Body != null)
            {
                docs.Add(this.PrintBlockSyntax(node.Body));
            }
            else if (node.ExpressionBody != null)
            {
                docs.Add(
                    this.PrintArrowExpressionClauseSyntax(node.ExpressionBody)
                );
            }

            docs.Add(SyntaxTokens.Print(node.SemicolonToken));

            return Docs.Concat(docs);
        }
    }
}
