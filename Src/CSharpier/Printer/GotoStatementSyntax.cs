using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintGotoStatementSyntax(GotoStatementSyntax node)
  {
    Doc expression = node.Expression != null
      ? Docs.Concat(" ", this.Print(node.Expression))
      : string.Empty;
    return Docs.Concat(
      this.PrintExtraNewLines(node),
      SyntaxTokens.Print(node.GotoKeyword),
      node.CaseOrDefaultKeyword.RawKind != 0 ? " " : Docs.Null,
      SyntaxTokens.Print(node.CaseOrDefaultKeyword),
      expression,
      SyntaxTokens.Print(node.SemicolonToken)
    );
  }
}

}
