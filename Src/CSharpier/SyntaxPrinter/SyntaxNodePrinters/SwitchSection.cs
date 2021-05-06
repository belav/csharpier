using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SwitchSection
    {
        public static Doc Print(SwitchSectionSyntax node)
        {
            var docs = new List<Doc> { Doc.Join(Doc.HardLine, node.Labels.Select(Node.Print)) };
            if (node.Statements.Count == 1 && node.Statements[0] is BlockSyntax blockSyntax)
            {
                docs.Add(Block.Print(blockSyntax));
            }
            else
            {
                docs.Add(
                    Doc.Indent(
                        Doc.HardLine,
                        Doc.Join(Doc.HardLine, node.Statements.Select(Node.Print).ToArray())
                    )
                );
            }
            return Doc.Concat(docs);
        }
    }
}
