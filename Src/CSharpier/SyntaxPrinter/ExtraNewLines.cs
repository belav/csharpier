using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.SyntaxPrinter
{
    public static class ExtraNewLines
    {
        public static Doc Print(CSharpSyntaxNode node)
        {
            var docs = new List<Doc>();
            foreach (var leadingTrivia in node.GetLeadingTrivia())
            {
                if (leadingTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
                {
                    docs.Add(Doc.HardLine);
                    // ensures we only print a single new line
                    break;
                }
                else if (leadingTrivia.Kind() != SyntaxKind.WhitespaceTrivia)
                {
                    break;
                }
            }

            return docs.Any() ? Doc.Concat(docs) : Doc.Null;
        }
    }
}
