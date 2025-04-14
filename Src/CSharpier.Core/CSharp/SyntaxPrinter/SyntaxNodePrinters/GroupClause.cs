using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class GroupClause
{
    public static Doc Print(GroupClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.GroupKeyword, " ", context),
            Node.Print(node.GroupExpression, context),
            " ",
            Token.PrintWithSuffix(node.ByKeyword, " ", context),
            Node.Print(node.ByExpression, context)
        );
    }
}
