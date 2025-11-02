using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class LabeledStatement
{
    public static Doc Print(LabeledStatementSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null, null]);
        docs.Add(ExtraNewLines.Print(node));
        docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Add(Token.Print(node.Identifier, context));
        docs.Add(Token.Print(node.ColonToken, context));

        if (node.Statement is BlockSyntax blockSyntax)
        {
            docs.Add(Block.Print(blockSyntax, context));
        }
        else
        {
            docs.Add(Doc.HardLine, Node.Print(node.Statement, context));
        }
        return Doc.Concat(ref docs);
    }
}
