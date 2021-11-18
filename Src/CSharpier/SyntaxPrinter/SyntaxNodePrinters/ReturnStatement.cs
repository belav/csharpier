using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ReturnStatement
{
    public static Doc Print(ReturnStatementSyntax node)
    {
        return Doc.Group(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.ReturnKeyword, node.Expression != null ? " " : Doc.Null),
            node.Expression != null
              ? node.Expression is BinaryExpressionSyntax
                  ? Doc.Indent(Node.Print(node.Expression))
                  : Node.Print(node.Expression)
              : Doc.Null,
            Token.Print(node.SemicolonToken)
        );
    }
}
