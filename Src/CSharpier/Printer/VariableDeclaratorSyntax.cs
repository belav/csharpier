using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclaratorSyntax(
            VariableDeclaratorSyntax node
        ) {
            var docs = new List<Doc> { this.PrintSyntaxToken(node.Identifier) };
            if (node.ArgumentList != null)
            {
                docs.Add(
                    this.PrintBracketedArgumentListSyntax(node.ArgumentList)
                );
            }
            if (node.Initializer != null)
            {
                docs.Add(this.PrintEqualsValueClauseSyntax(node.Initializer));
            }
            return Docs.Concat(docs);
        }
    }
}
