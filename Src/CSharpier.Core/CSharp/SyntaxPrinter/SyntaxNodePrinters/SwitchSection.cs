using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class SwitchSection
{
    public static Doc Print(SwitchSectionSyntax node, PrintingContext context)
    {
        var docs = new DocListBuilder(2);
        docs.Add(Doc.Join(Doc.HardLine, node.Labels.Select(o => Node.Print(o, context))));
        if (node.Statements is [BlockSyntax blockSyntax])
        {
            docs.Add(Block.Print(blockSyntax, context));
        }
        else
        {
            docs.Add(
                Doc.Indent(
                    node.Statements.First() is BlockSyntax ? Doc.Null : Doc.HardLine,
                    Doc.Join(
                        Doc.HardLine,
                        node.Statements.Select(o => Node.Print(o, context)).ToArray()
                    )
                )
            );
        }
        return Doc.Concat(ref docs);
    }
}
