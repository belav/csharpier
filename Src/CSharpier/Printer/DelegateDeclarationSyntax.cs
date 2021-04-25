using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintDelegateDeclarationSyntax(DelegateDeclarationSyntax node)
  {
    var docs = new List<Doc>();
    docs.Add(this.PrintExtraNewLines(node));
    docs.Add(this.PrintAttributeLists(node, node.AttributeLists));
    docs.Add(this.PrintModifiers(node.Modifiers));
    docs.Add(
      this.PrintSyntaxToken(node.DelegateKeyword, afterTokenIfNoTrailing: " ")
    );
    docs.Add(this.Print(node.ReturnType));
    docs.Add(" ", SyntaxTokens.Print(node.Identifier));
    if (node.TypeParameterList != null)
    {
      docs.Add(this.Print(node.TypeParameterList));
    }
    docs.Add(this.Print(node.ParameterList));
    docs.Add(this.PrintConstraintClauses(node, node.ConstraintClauses));
    docs.Add(SyntaxTokens.Print(node.SemicolonToken));
    return Docs.Concat(docs);
  }
}

}
