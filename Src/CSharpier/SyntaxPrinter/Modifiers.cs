using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;

namespace CSharpier.SyntaxPrinter
{
    internal static class Modifiers
    {
        public static Doc Print(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
            {
                return Doc.Null;
            }

            return Doc.Group(Doc.Join(" ", modifiers.Select(Token.Print)), " ");
        }

        public static Doc PrintWithoutLeadingTrivia(SyntaxTokenList modifiers)
        {
            if (modifiers.Count == 0)
            {
                return Doc.Null;
            }

            return Doc.Group(
                Token.PrintWithoutLeadingTrivia(modifiers[0]),
                " ",
                modifiers.Count > 1
                  ? Doc.Concat(
                        modifiers.Skip(1).Select(o => Token.PrintWithSuffix(o, " ")).ToArray()
                    )
                  : Doc.Null
            );
        }
    }
}
