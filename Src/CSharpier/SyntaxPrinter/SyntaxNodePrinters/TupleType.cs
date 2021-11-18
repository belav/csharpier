using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleType
{
    public static Doc Print(TupleTypeSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.OpenParenToken),
            SeparatedSyntaxList.Print(node.Elements, Node.Print, " "),
            Token.Print(node.CloseParenToken)
        );
    }
}
