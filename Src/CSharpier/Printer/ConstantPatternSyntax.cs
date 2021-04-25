using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintConstantPatternSyntax(ConstantPatternSyntax node)
  {
    return this.Print(node.Expression);
  }
}

}
