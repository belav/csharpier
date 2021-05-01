using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class OmittedTypeArgument
    {
        public static Doc Print(OmittedTypeArgumentSyntax node)
        {
            return Doc.Null;
        }
    }
}
