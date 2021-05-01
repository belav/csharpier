using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO 0 multiline comments need lots of testing, formatting is real weird
        // TODO get rid of this after we figure out what SyntaxTokens is really gonna look like
        private Doc PrintSyntaxToken(
            SyntaxToken syntaxToken,
            Doc? afterTokenIfNoTrailing = null
        ) {
            return Token.PrintSyntaxToken(syntaxToken, afterTokenIfNoTrailing);
        }
    }
}
