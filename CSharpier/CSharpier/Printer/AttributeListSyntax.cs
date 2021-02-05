using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAttributeListSyntax(AttributeListSyntax node)
        {
            var parts = new Parts("[");
            if (node.Target != null)
            {
                parts.Push(
                    node.Target.Identifier.Text,
                    String(": ")
                );
            }

            var attributes = new List<Doc>();
            foreach (var attributeNode in node.Attributes)
            {
                var name = this.Print(attributeNode.Name);
                if (attributeNode.ArgumentList == null)
                {
                    attributes.Add(name);
                    continue;
                }

                var innerParts = new Parts(name, "(");
                innerParts.Add(
                    Join(
                        String(", "),
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
                innerParts.Add(String(")"));
                attributes.Add(Concat(innerParts));
            };
            parts.Push(Join(String(", "), attributes), String("]"));
            return Concat(parts);
        }
    }
}