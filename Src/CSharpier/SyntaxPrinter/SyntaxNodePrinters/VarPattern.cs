using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class VarPattern
{
    public static Doc Print(VarPatternSyntax node)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.VarKeyword, " "),
            Node.Print(node.Designation)
        );
    }
}
