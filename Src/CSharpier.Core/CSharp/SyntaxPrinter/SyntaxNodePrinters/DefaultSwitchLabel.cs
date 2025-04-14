using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class DefaultSwitchLabel
{
    public static Doc Print(DefaultSwitchLabelSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.Keyword, context),
            Token.Print(node.ColonToken, context)
        );
    }
}
