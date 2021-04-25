using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintCasePatternSwitchLabelSyntax(
    CasePatternSwitchLabelSyntax node
  ) {
    var docs = new List<Doc>();
    docs.Add(
      this.PrintSyntaxToken(node.Keyword, afterTokenIfNoTrailing: " "),
      this.Print(node.Pattern)
    );
    if (node.WhenClause != null)
    {
      docs.Add(" ", this.Print(node.WhenClause));
    }
    docs.Add(SyntaxTokens.Print(node.ColonToken));
    return Docs.Concat(docs);
  }
}

}
