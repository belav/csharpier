using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class OptionalBraces
{
    public static Doc Print(StatementSyntax node, PrintingContext context)
    {
        return node is BlockSyntax blockSyntax
            ? Block.Print(blockSyntax, context)
            : DocUtilities.RemoveInitialDoubleHardLine(
                Doc.Indent(Doc.HardLine, Node.Print(node, context))
            );
    }
}
