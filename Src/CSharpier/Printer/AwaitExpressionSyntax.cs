using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintAwaitExpressionSyntax(AwaitExpressionSyntax node)
  {
    return Docs.Concat(
      this.PrintSyntaxToken(node.AwaitKeyword, afterTokenIfNoTrailing: " "),
      this.Print(node.Expression)
    );
  }
}

}
