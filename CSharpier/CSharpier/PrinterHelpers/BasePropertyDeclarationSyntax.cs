using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBasePropertyDeclarationSyntax(BasePropertyDeclarationSyntax node)
        {
            EqualsValueClauseSyntax initializer = null;
            ArrowExpressionClauseSyntax expressionBody = null;
            Doc identifier = "";
            var eventKeyword = "";
            if (node is PropertyDeclarationSyntax propertyDeclarationSyntax)
            {
                expressionBody = propertyDeclarationSyntax.ExpressionBody;
                initializer = propertyDeclarationSyntax.Initializer;
                identifier = propertyDeclarationSyntax.Identifier.Text;
            }
            else if (node is IndexerDeclarationSyntax indexerDeclarationSyntax)
            {
                expressionBody = indexerDeclarationSyntax.ExpressionBody;
                identifier = Concat(
                    indexerDeclarationSyntax.ThisKeyword.Text,
                    "[",
                    Join(", ", indexerDeclarationSyntax.ParameterList.Parameters.Select(this.PrintParameterSyntax)),
                    "]"
                );
            }
            else if (node is EventDeclarationSyntax eventDeclarationSyntax)
            {
                eventKeyword = eventDeclarationSyntax.EventKeyword.Text + " ";
                identifier = eventDeclarationSyntax.Identifier.Text;
            }

            Doc contents = "";
            if (node.AccessorList != null)
            {
                contents = Group(Concat(Line, "{", Group(Indent(Concat(node.AccessorList.Accessors.Select(this.PrintAccessorDeclarationSyntax).ToArray()))), Line, "}"));
            }
            else if (expressionBody != null)
            {
                contents = Concat(this.PrintArrowExpressionClauseSyntax(expressionBody), ";");
            }


            var parts = new Parts();

            this.PrintAttributeLists(node, node.AttributeLists, parts);

            return Group(
                Concat(
                    Concat(parts),
                    this.PrintModifiers(node.Modifiers),
                    eventKeyword,
                    this.Print(node.Type),
                    " ",
                    identifier,
                    contents,
                    initializer != null
                        ? Concat(this.PrintEqualsValueClauseSyntax(initializer), ";")
                        : ""
                )
            );
        }
    }
}