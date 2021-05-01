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
                    modifier => Token.PrintWithSuffix(modifier, " ")
                )
                .ToList();

            return Doc.Group(Doc.Concat(docs));
        }
    }
}
