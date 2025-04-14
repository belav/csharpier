using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhereClause
{
    public static Doc Print(WhereClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.WhereKeyword, context),
                Doc.Indent(Doc.Line, Node.Print(node.Condition, context))
            )
        );
    }
}
