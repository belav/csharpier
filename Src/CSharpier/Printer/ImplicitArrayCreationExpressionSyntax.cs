using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintImplicitArrayCreationExpressionSyntax(
    ImplicitArrayCreationExpressionSyntax node
  ) {
    var commas = node.Commas.Select(SyntaxTokens.Print).ToArray();
    return Docs.Concat(
      SyntaxTokens.Print(node.NewKeyword),
      SyntaxTokens.Print(node.OpenBracketToken),
      Docs.Concat(commas),
      this.PrintSyntaxToken(
        node.CloseBracketToken,
        afterTokenIfNoTrailing: " "
      ),
      this.Print(node.Initializer)
    );
  }
}

}
