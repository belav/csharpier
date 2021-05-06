using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class VarPattern
    {
        public static Doc Print(VarPatternSyntax node)
        {
            return Doc.Concat(Token.Print(node.VarKeyword, " "), Node.Print(node.Designation));
        }
    }
}
