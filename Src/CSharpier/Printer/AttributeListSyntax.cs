using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAttributeListSyntax(AttributeListSyntax node)
        {
            var parts = new Parts();
            parts.Push(this.PrintSyntaxToken(node.OpenBracketToken));
            if (node.Target != null)
            {
                parts.Push(
                    this.PrintSyntaxToken(node.Target.Identifier),
                    this.PrintSyntaxToken(node.Target.ColonToken, " ")
                );
            }

            parts.Push(
                Indent(
                    node.Attributes.Count > 1 ? SoftLine : null,
                    this.PrintSeparatedSyntaxList(
                        node.Attributes,
                        attributeNode =>
                        {
                            var name = this.Print(attributeNode.Name);
                            if (attributeNode.ArgumentList == null)
                            {
                                return name;
                            }

                            return Group(
                                name,
                                this.PrintSyntaxToken(
                                    attributeNode.ArgumentList.OpenParenToken
                                ),
                                Indent(
                                    SoftLine,
                                    this.PrintSeparatedSyntaxList(
                                        attributeNode.ArgumentList.Arguments,
                                        attributeArgumentNode => Concat(
                                            attributeArgumentNode.NameEquals != null
                                                ? this.PrintNameEqualsSyntax(
                                                    attributeArgumentNode.NameEquals
                                                )
                                                : null,
                                            attributeArgumentNode.NameColon != null
                                                ? this.PrintNameColonSyntax(
                                                    attributeArgumentNode.NameColon
                                                )
                                                : null,
                                            this.Print(
                                                attributeArgumentNode.Expression
                                            )
                                        ),
                                        Line
                                    ),
                                    this.PrintSyntaxToken(
                                        attributeNode.ArgumentList.CloseParenToken
                                    )
                                )
                            );
                        },
                        Line
                    )
                )
            );

            parts.Push(
                node.Attributes.Count > 1 ? SoftLine : null,
                this.PrintSyntaxToken(node.CloseBracketToken)
            );

            return Group(parts);
        }
    }
}
