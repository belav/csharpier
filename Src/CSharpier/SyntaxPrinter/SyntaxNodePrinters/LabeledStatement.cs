using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class LabeledStatement
{
    public static Doc Print(LabeledStatementSyntax node)
    {
        var docs = new List<Doc>
        {
            ExtraNewLines.Print(node),
            AttributeLists.Print(node, node.AttributeLists),
            Token.Print(node.Identifier),
            Token.Print(node.ColonToken)
        };
        if (node.Statement is BlockSyntax blockSyntax)
        {
            docs.Add(Block.Print(blockSyntax));
        }
        else
        {
            docs.Add(Doc.HardLine, Node.Print(node.Statement));
        }
        return Doc.Concat(docs);
    }
}
