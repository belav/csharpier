using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintRecordDeclarationSyntax(RecordDeclarationSyntax node)
  {
    return this.PrintBaseTypeDeclarationSyntax(node);
  }
}

}
