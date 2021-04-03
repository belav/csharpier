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
            if (node.Parent is CompilationUnitSyntax)
            {
                parts.Push(this.PrintExtraNewLines(node));
            }

            parts.Push(this.PrintSyntaxToken(node.OpenBracketToken));
            if (node.Target != null)
            {
                parts.Push(
                    this.PrintSyntaxToken(node.Target.Identifier),
                    this.PrintSyntaxToken(
                        node.Target.ColonToken,
                        afterTokenIfNoTrailing: " "
                    )
                );
            }

            parts.Push(
                Indent(
                    node.Attributes.Count > 1 ? SoftLine : Doc.Null,
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
                                                : Doc.Null,
                                            attributeArgumentNode.NameColon != null
                                                ? this.PrintNameColonSyntax(
                                                    attributeArgumentNode.NameColon
                                                )
                                                : Doc.Null,
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
                node.Attributes.Count > 1 ? SoftLine : Doc.Null,
                this.PrintSyntaxToken(node.CloseBracketToken)
            );

            return Group(parts);
        }
    }
}
