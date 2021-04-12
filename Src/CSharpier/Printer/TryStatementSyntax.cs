using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTryStatementSyntax(TryStatementSyntax node)
        {
            var docs = new List<Doc>
            {
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintSyntaxToken(node.TryKeyword),
                this.PrintBlockSyntax(node.Block),
                Docs.HardLine,
                Join(
                    Docs.HardLine,
                    node.Catches.Select(this.PrintCatchClauseSyntax)
                )
            };
            if (node.Finally != null)
            {
                docs.Add(
                    Docs.HardLine,
                    this.PrintFinallyClauseSyntax(node.Finally)
                );
            }
            return Docs.Concat(docs);
        }
    }
}
