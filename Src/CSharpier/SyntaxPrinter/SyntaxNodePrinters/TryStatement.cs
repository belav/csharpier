using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TryStatement
    {
        public static Doc Print(TryStatementSyntax node)
        {
            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                AttributeLists.Print(node, node.AttributeLists),
                Token.Print(node.TryKeyword),
                Block.Print(node.Block),
                Doc.HardLine,
                Doc.Join(Doc.HardLine, node.Catches.Select(CatchClause.Print))
            };
            if (node.Finally != null)
            {
                docs.Add(Doc.HardLine, FinallyClause.Print(node.Finally));
            }
            return Doc.Concat(docs);
        }
    }
}
