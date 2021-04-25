using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintCheckedStatementSyntax(CheckedStatementSyntax node)
  {
    return Docs.Concat(
      SyntaxTokens.Print(node.Keyword),
      this.PrintBlockSyntax(node.Block)
    );
  }
}

}
