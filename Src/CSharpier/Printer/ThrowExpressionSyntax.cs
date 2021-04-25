using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintThrowExpressionSyntax(ThrowExpressionSyntax node)
  {
    return Docs.Concat(
      this.PrintSyntaxToken(node.ThrowKeyword, afterTokenIfNoTrailing: " "),
      this.Print(node.Expression)
    );
  }
}

}
