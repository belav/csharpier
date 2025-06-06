using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class FinallyClause
{
    public static Doc Print(FinallyClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.FinallyKeyword, context),
            Node.Print(node.Block, context)
        );
    }
}
