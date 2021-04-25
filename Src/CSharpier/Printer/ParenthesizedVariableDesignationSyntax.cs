using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintParenthesizedVariableDesignationSyntax(
    ParenthesizedVariableDesignationSyntax node
  ) {
    return Docs.Group(
      SyntaxTokens.Print(node.OpenParenToken),
      Docs.Indent(
        Docs.SoftLine,
        this.PrintSeparatedSyntaxList(node.Variables, this.Print, Docs.Line),
        Docs.SoftLine
      ),
      SyntaxTokens.Print(node.CloseParenToken)
    );
  }
}

}
