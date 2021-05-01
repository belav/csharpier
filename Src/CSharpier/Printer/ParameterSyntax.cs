using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
                Modifiers.Print(node.Modifiers)
            };
            if (node.Type != null)
            {
                docs.Add(this.Print(node.Type), " ");
            }

            docs.Add(Token.Print(node.Identifier));
            if (node.Default != null)
            {
                docs.Add(this.PrintEqualsValueClauseSyntax(node.Default));
            }

            return Doc.Concat(docs);
        }
    }
}
