using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTryStatementSyntax(TryStatementSyntax node)
        {
            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                SyntaxTokens.Print(node.TryKeyword),
                this.PrintBlockSyntax(node.Block),
                Docs.HardLine,
                Docs.Join(
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
