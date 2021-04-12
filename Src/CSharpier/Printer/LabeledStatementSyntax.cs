using System.Collections.Generic;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLabeledStatementSyntax(LabeledStatementSyntax node)
        {
            var docs = new List<Doc>
            {
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                this.PrintSyntaxToken(node.Identifier),
                this.PrintSyntaxToken(node.ColonToken)
            };
            if (node.Statement is BlockSyntax blockSyntax)
            {
                docs.Add(this.PrintBlockSyntax(blockSyntax));
            }
            else
            {
                docs.Add(Docs.HardLine, SyntaxNodes.Print(node.Statement));
            }
            return Docs.Concat(docs);
        }
    }
}
