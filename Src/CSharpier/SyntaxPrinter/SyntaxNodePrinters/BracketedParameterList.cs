using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BracketedParameterList
{
    public static Doc Print(BracketedParameterListSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.OpenBracketToken),
            SeparatedSyntaxList.Print(node.Parameters, Parameter.Print, " "),
            Token.Print(node.CloseBracketToken)
        );
    }
}
