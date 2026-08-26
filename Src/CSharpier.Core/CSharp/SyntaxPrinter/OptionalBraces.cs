using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class OptionalBraces
{
    public static Doc Print(StatementSyntax node, CSharpPrintingContext context)
    {
        return node is BlockSyntax blockSyntax
            ? Block.Print(blockSyntax, context)
            : DocUtilities.RemoveInitialDoubleHardLine(
                Doc.Indent(Doc.HardLine, Node.Print(node, context))
            );
    }

    // a while inside a while, a for inside a for and so on is printed on its own line rather than
    // indented, so that a chain of them does not march off the right hand side of the page
    public static Doc PrintWithSelfNesting<TSelf>(
        StatementSyntax node,
        CSharpPrintingContext context
    )
        where TSelf : StatementSyntax
    {
        return node is TSelf
            ? Doc.Group(Doc.HardLine, Node.Print(node, context))
            : Print(node, context);
    }
}
