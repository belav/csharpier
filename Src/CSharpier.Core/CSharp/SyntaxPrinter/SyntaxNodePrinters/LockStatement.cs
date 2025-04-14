using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class LockStatement
{
    public static Doc Print(LockStatementSyntax node, PrintingContext context)
    {
        var statement = Node.Print(node.Statement, context);

        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.LockKeyword, " ", context),
            Token.Print(node.OpenParenToken, context),
            Node.Print(node.Expression, context),
            Token.Print(node.CloseParenToken, context),
            node.Statement is BlockSyntax ? statement : Doc.Indent(Doc.HardLine, statement)
        );
    }
}
