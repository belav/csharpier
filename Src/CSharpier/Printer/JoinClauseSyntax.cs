using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintJoinClauseSyntax(JoinClauseSyntax node)
  {
    return Docs.Group(
      this.PrintSyntaxToken(node.JoinKeyword, afterTokenIfNoTrailing: " "),
      this.PrintSyntaxToken(node.Identifier, afterTokenIfNoTrailing: " "),
      this.PrintSyntaxToken(node.InKeyword, afterTokenIfNoTrailing: " "),
      this.Print(node.InExpression),
      Docs.Indent(
        Docs.Line,
        this.PrintSyntaxToken(node.OnKeyword, afterTokenIfNoTrailing: " "),
        this.Print(node.LeftExpression),
        " ",
        this.PrintSyntaxToken(node.EqualsKeyword, afterTokenIfNoTrailing: " "),
        this.Print(node.RightExpression),
        node.Into != null
          ? Docs.Concat(
              Docs.Line,
              this.PrintSyntaxToken(
                node.Into.IntoKeyword,
                afterTokenIfNoTrailing: " "
              ),
              SyntaxTokens.Print(node.Into.Identifier)
            )
          : Docs.Null
      )
    );
  }
}

}
