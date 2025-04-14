using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class BracketedParameterList
{
    public static Doc Print(BracketedParameterListSyntax node, PrintingContext context)
    {
        return ParameterList.Print(node, node.OpenBracketToken, node.CloseBracketToken, context);
    }
}
