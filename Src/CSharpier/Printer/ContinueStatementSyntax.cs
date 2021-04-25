using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintContinueStatementSyntax(ContinueStatementSyntax node)
  {
    return Docs.Concat(
      SyntaxTokens.Print(node.ContinueKeyword),
      SyntaxTokens.Print(node.SemicolonToken)
    );
  }
}

}
