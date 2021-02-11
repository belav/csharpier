using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
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

            var attributes = new List<Doc>();
            foreach (var attributeNode in node.Attributes)
            {
                // TODO trivia!!
                var name = this.Print(attributeNode.Name);
                if (attributeNode.ArgumentList == null)
                {
                    attributes.Add(name);
                    continue;
                }

                var innerParts = new Parts(name, "(");
                innerParts.Push(
                    Join(
                        ", ",
                        attributeNode.ArgumentList.Arguments.Select(attributeArgumentNode => Concat(
                            attributeArgumentNode.NameEquals != null
                                ? this.PrintNameEqualsSyntax(attributeArgumentNode.NameEquals)
                                : "",
                            attributeArgumentNode.NameColon != null
                                ? this.PrintNameColonSyntax(attributeArgumentNode.NameColon)
                                : "",
                            this.Print(attributeArgumentNode.Expression)
                        ))
                    )
                );
                innerParts.Push(")");
                attributes.Add(Concat(innerParts));
            }

            ;
            parts.Push(Join(", ", attributes));
            parts.Push(this.PrintSyntaxToken(node.CloseBracketToken));

            return Concat(parts);
        }
    }
}