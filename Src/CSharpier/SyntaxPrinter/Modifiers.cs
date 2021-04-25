using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;

namespace CSharpier.SyntaxPrinter
{
    public static class Modifiers
    {
        public static Doc Print(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
            {
                return Doc.Null;
            }

            var docs = modifiers.Select(
                    modifier => SyntaxTokens.PrintWithSuffix(modifier, " ")
                )
                .ToList();

            return Docs.Group(Docs.Concat(docs));
        }
    }
}
