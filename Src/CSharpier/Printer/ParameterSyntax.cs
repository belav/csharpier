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
                this.PrintModifiers(node.Modifiers)
            };
            if (node.Type != null)
            {
                this.Print(node.Type, docs);
                docs.Add(" ");
            }

            SyntaxTokens.Print(node.Identifier, docs);
            if (node.Default != null)
            {
                docs.Add(this.PrintEqualsValueClauseSyntax(node.Default));
            }

            return Docs.Concat(docs);
        }
    }
}
