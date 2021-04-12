using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParameterSyntax(ParameterSyntax node)
        {
            var docs = new List<Doc>
            {
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintModifiers(node.Modifiers)
            };
            if (node.Type != null)
            {
                docs.Add(this.Print(node.Type), Docs.SpaceIfNoPreviousComment);
            }

            docs.Add(this.PrintSyntaxToken(node.Identifier));
            if (node.Default != null)
            {
                docs.Add(this.PrintEqualsValueClauseSyntax(node.Default));
            }

            return Docs.Concat(docs);
        }
    }
}
