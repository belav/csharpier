using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVariableDeclaratorSyntax(
            VariableDeclaratorSyntax node
        ) {
            var docs = new List<Doc> { SyntaxTokens.Print(node.Identifier) };
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
