using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintEnumDeclarationSyntax(EnumDeclarationSyntax node)
  {
    return this.PrintBaseTypeDeclarationSyntax(node);
  }
}

}
