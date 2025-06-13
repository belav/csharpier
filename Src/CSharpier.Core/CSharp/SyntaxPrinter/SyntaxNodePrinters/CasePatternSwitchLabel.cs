using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class CasePatternSwitchLabel
{
    public static Doc Print(CasePatternSwitchLabelSyntax node, PrintingContext context)
    {
        return Doc.Group(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.Keyword, " ", context),
            Node.Print(node.Pattern, context),
            node.WhenClause != null ? WhenClause.Print(node.WhenClause, context) : Doc.Null,
            Token.Print(node.ColonToken, context)
        );
    }
}
