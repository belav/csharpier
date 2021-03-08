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
            EqualsValueClauseSyntax initializer = null;
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax =
                null;
            Doc identifier = null;
            Doc eventKeyword = null;
            ArrowExpressionClauseSyntax expressionBody = null;
            SyntaxToken? semicolonToken = null;

            if (node is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                expressionBody = propertyDeclarationSyntax.ExpressionBody;
                initializer = propertyDeclarationSyntax.Initializer;
                explicitInterfaceSpecifierSyntax = propertyDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = this.PrintSyntaxToken(
                    propertyDeclarationSyntax.Identifier);
                semicolonToken = propertyDeclarationSyntax.SemicolonToken;
            }
            else if (node is IndexerDeclarationSyntax indexerDeclarationSyntax)
            {
                expressionBody = indexerDeclarationSyntax.ExpressionBody;
                explicitInterfaceSpecifierSyntax = indexerDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = Concat(
                    this.PrintSyntaxToken(indexerDeclarationSyntax.ThisKeyword),
                    this.Print(indexerDeclarationSyntax.ParameterList));
                semicolonToken = indexerDeclarationSyntax.SemicolonToken;
            }
            else if (node is EventDeclarationSyntax eventDeclarationSyntax)
            {
                eventKeyword = this.PrintSyntaxToken(
                    eventDeclarationSyntax.EventKeyword,
                    " ");
                explicitInterfaceSpecifierSyntax = eventDeclarationSyntax.ExplicitInterfaceSpecifier;
                identifier = this.PrintSyntaxToken(
                    eventDeclarationSyntax.Identifier);
                semicolonToken = eventDeclarationSyntax.SemicolonToken;
            }

            Doc contents = "";
            if (node.AccessorList != null)
            {
                // if (node.AccessorList.Accessors.Any(o => o.Body != null || o.ExpressionBody != null))
                // {
                contents = Group(
                    Concat(
                        Line,
                        this.PrintSyntaxToken(node.AccessorList.OpenBraceToken),
                        Group(
                            Indent(
                                node.AccessorList.Accessors.Select(
                                        this.PrintAccessorDeclarationSyntax)
                                    .ToArray())),
                        Line,
                        this.PrintSyntaxToken(
                            node.AccessorList.CloseBraceToken)));
            // }
            // else
            // {
            //     // TODO GH-6 I don't know that we should force flat here. Maybe I need to look more at what prettier does for complicated stuff.
            //     contents = ForceFlat(
            //         SpaceIfNoPreviousComment,
            //         this.PrintSyntaxToken(node.AccessorList.OpenBraceToken),
            //         Concat(node.AccessorList.Accessors.Select(this.PrintAccessorDeclarationSyntax).ToArray()),
            //         SpaceIfNoPreviousComment,
            //         this.PrintSyntaxToken(node.AccessorList.CloseBraceToken));
            // }
            }
            else if (expressionBody != null)
            {
                contents = Concat(
                    this.PrintArrowExpressionClauseSyntax(expressionBody));
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
                                explicitInterfaceSpecifierSyntax.DotToken))
                        : null,
                    identifier,
                    contents,
                    initializer != null
                        ? Indent(
                            this.PrintEqualsValueClauseSyntax(initializer))
                        : null,
                    semicolonToken.HasValue
                        ? this.PrintSyntaxToken(semicolonToken.Value)
                        : null));
        }
    }
}
