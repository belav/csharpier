using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPredefinedTypeSyntax(PredefinedTypeSyntax node)
        {
            return SyntaxTokens.Print(node.Keyword);
        }

        private void PrintPredefinedTypeSyntax(
            PredefinedTypeSyntax node,
            IList<Doc> docs
        ) {
            SyntaxTokens.Print(node.Keyword, docs);
        }
    }
}
