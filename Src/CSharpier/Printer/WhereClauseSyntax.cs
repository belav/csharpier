using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
  {
    return Docs.Group(
      SyntaxTokens.Print(node.WhereKeyword),
      Docs.Indent(Docs.Line, this.Print(node.Condition))
    );
  }
}

}
