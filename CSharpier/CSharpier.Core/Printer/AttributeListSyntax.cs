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
            parts.Push(this.PrintLeadingTrivia(node.OpenBracketToken));
            parts.Push(node.OpenBracketToken.Text);
            parts.Push(this.PrintTrailingTrivia(node.OpenBracketToken));
            if (node.Target != null)
            {
                parts.Push(this.PrintLeadingTrivia(node.Target));
                // TODO 2 more comments on target.identifier/target.colontoken??
                parts.Push(
                    node.Target.Identifier.Text,
                    ": "
                );
                parts.Push(this.PrintTrailingTrivia(node.Target));
            }

            var attributes = new List<Doc>();
            foreach (var attributeNode in node.Attributes)
            {
                // TODO 1 we are missing leading/trailing trivia in here
                var name = this.Print(attributeNode.Name);
                if (attributeNode.ArgumentList == null)
                {
                    attributes.Add(name);
                    continue;
                }

                var innerParts = new Parts(name, "(");
                innerParts.Add(
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
                innerParts.Add(")");
                attributes.Add(Concat(innerParts));
            }

            ;
            parts.Push(Join(", ", attributes));
            parts.Push(this.PrintLeadingTrivia(node.CloseBracketToken));
            parts.Push(node.CloseBracketToken.Text);
            parts.Push(this.PrintTrailingTrivia(node.CloseBracketToken));

            return Concat(parts);
        }
    }
}