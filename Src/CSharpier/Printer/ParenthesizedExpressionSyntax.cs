using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintParenthesizedExpressionSyntax(
    ParenthesizedExpressionSyntax node
  ) {
    return Docs.Concat(
      SyntaxTokens.Print(node.OpenParenToken),
      this.Print(node.Expression),
      SyntaxTokens.Print(node.CloseParenToken)
    );
  }
}

}
