using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintDefaultSwitchLabelSyntax(DefaultSwitchLabelSyntax node)
  {
    return Docs.Concat(
      SyntaxTokens.Print(node.Keyword),
      SyntaxTokens.Print(node.ColonToken)
    );
  }
}

}
