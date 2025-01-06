using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrowExpressionClause
{
    public static Doc Print(ArrowExpressionClauseSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Doc.Indent(
                Doc.IfBreak("", " "),
                Doc.SoftLine,
                Token.Print(node.ArrowToken, context),
                " ",
                Doc.Indent(Node.Print(node.Expression, context))
            )
        );
    }
}
