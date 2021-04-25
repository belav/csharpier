using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintTypeParameterSyntax(TypeParameterSyntax node)
  {
    return Docs.Concat(
      this.PrintAttributeLists(node, node.AttributeLists),
      this.PrintSyntaxToken(node.VarianceKeyword, afterTokenIfNoTrailing: " "),
      SyntaxTokens.Print(node.Identifier)
    );
  }
}

}
