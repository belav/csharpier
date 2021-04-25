using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintRefValueExpressionSyntax(RefValueExpressionSyntax node)
  {
    return Docs.Concat(
      SyntaxTokens.Print(node.Keyword),
      SyntaxTokens.Print(node.OpenParenToken),
      this.Print(node.Expression),
      this.PrintSyntaxToken(node.Comma, afterTokenIfNoTrailing: " "),
      this.Print(node.Type),
      SyntaxTokens.Print(node.CloseParenToken)
    );
  }
}

}
