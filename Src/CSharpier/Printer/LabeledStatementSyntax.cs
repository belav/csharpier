using System.Collections.Generic;
using CSharpier.DocTypes;
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
                ExtraNewLines.Print(node),
                this.PrintAttributeLists(node, node.AttributeLists),
                Token.Print(node.Identifier),
                Token.Print(node.ColonToken)
            };
            if (node.Statement is BlockSyntax blockSyntax)
            {
                docs.Add(this.PrintBlockSyntax(blockSyntax));
            }
            else
            {
                docs.Add(Doc.HardLine, Node.Print(node.Statement));
            }
            return Doc.Concat(docs);
        }
    }
}
