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
                Token.Print(node.TryKeyword),
                this.PrintBlockSyntax(node.Block),
                Doc.HardLine,
                Doc.Join(
                    Doc.HardLine,
                    node.Catches.Select(this.PrintCatchClauseSyntax)
                )
            };
            if (node.Finally != null)
            {
                docs.Add(
                    Doc.HardLine,
                    this.PrintFinallyClauseSyntax(node.Finally)
                );
            }
            return Doc.Concat(docs);
        }
    }
}
