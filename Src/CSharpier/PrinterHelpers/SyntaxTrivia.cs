using System.Collections.Generic;
using System.Linq;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO where does this belong? It is kinda sorta related to syntax tokens, because leading trivia comes from them
        private Doc PrintExtraNewLines(CSharpSyntaxNode node)
        {
            var parts = new Parts();
            foreach (var leadingTrivia in node.GetLeadingTrivia())
            {
                if (leadingTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
                {
                    parts.Push(HardLine);
                    // ensures we only print a single new line
                    break;
                }
                else if (leadingTrivia.Kind() != SyntaxKind.WhitespaceTrivia)
                {
                    break;
                }
            }

            return parts.Any() ? Concat(parts) : Doc.Null;
        }

        // TODO 0 multiline comments need lots of testing, formatting is real weird
        // TODO get rid of this after we figure out what SyntaxTokens is really gonna look like
        private Doc PrintSyntaxToken(
            SyntaxToken syntaxToken,
            Doc? afterTokenIfNoTrailing = null,
            Doc? beforeTokenIfNoLeading = null)
        {
            return SyntaxTokens.PrintSyntaxToken(
                syntaxToken,
                afterTokenIfNoTrailing,
                beforeTokenIfNoLeading
            );
        }
    }
}
