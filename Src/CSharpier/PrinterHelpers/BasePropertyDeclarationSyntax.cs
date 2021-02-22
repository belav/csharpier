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
            ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifierSyntax = null;
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
                contents = Group(
                    Concat(
                        Line,
                        this.PrintSyntaxToken(node.AccessorList.OpenBraceToken),
                        Group(
                            Indent(
                                node.AccessorList.Accessors.Select(
                                    this.PrintAccessorDeclarationSyntax).ToArray())),
                        Line,
                        this.PrintSyntaxToken(
                            node.AccessorList.CloseBraceToken)));
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
                        ? this.PrintEqualsValueClauseSyntax(initializer)
                        : null,
                    semicolonToken.HasValue
                        ? this.PrintSyntaxToken(semicolonToken.Value)
                        : null));
        }
    }
}
