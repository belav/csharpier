using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintLocalFunctionStatementSyntax(
    LocalFunctionStatementSyntax node
  ) {
    return this.PrintBaseMethodDeclarationSyntax(node);
  }
}

}
