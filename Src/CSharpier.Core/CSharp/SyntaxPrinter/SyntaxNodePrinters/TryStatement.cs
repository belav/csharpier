using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TryStatement
{
    public static Doc Print(TryStatementSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null, null]);
        docs.Append(ExtraNewLines.Print(node));
        docs.Append(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Append(Token.Print(node.TryKeyword, context));
        docs.Append(Block.Print(node.Block, context));
        docs.Append(node.Catches.Any() ? Doc.HardLine : Doc.Null);
        docs.Append(
            Doc.Join(Doc.HardLine, node.Catches.Select(o => CatchClause.Print(o, context)))
        );

        if (node.Finally != null)
        {
            docs.Append(Doc.HardLine, FinallyClause.Print(node.Finally, context));
        }
        return Doc.Concat(ref docs);
    }
}
