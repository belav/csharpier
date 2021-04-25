using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintClassDeclarationSyntax(ClassDeclarationSyntax node)
  {
    return this.PrintBaseTypeDeclarationSyntax(node);
  }
}

}
