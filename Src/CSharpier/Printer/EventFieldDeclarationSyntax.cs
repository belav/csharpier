using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintEventFieldDeclarationSyntax(
    EventFieldDeclarationSyntax node
  ) {
    return this.PrintBaseFieldDeclarationSyntax(node);
  }
}

}
