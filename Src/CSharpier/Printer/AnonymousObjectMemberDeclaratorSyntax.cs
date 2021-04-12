using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAnonymousObjectMemberDeclaratorSyntax(
            AnonymousObjectMemberDeclaratorSyntax node
        ) {
            var docs = new List<Doc>();
            if (node.NameEquals != null)
            {
                docs.Add(
                    this.PrintSyntaxToken(
                        node.NameEquals.Name.Identifier,
                        afterTokenIfNoTrailing: " "
                    )
                );
                docs.Add(
                    this.PrintSyntaxToken(
                        node.NameEquals.EqualsToken,
                        afterTokenIfNoTrailing: " "
                    )
                );
            }
            docs.Add(this.Print(node.Expression));
            return Docs.Concat(docs);
        }
    }
}
