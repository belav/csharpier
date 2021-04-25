using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintOperatorDeclarationSyntax(OperatorDeclarationSyntax node)
  {
    return this.PrintBaseMethodDeclarationSyntax(node);
  }
}

}
