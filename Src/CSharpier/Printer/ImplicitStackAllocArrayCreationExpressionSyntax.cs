using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintImplicitStackAllocArrayCreationExpressionSyntax(
    ImplicitStackAllocArrayCreationExpressionSyntax node
  ) {
    return Docs.Concat(
      SyntaxTokens.Print(node.StackAllocKeyword),
      SyntaxTokens.Print(node.OpenBracketToken),
      this.PrintSyntaxToken(
        node.CloseBracketToken,
        afterTokenIfNoTrailing: " "
      ),
      this.Print(node.Initializer)
    );
  }
}

}
