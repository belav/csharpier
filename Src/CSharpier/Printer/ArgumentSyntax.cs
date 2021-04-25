using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintArgumentSyntax(ArgumentSyntax node)
  {
    var docs = new List<Doc>();
    if (node.NameColon != null)
    {
      docs.Add(this.PrintNameColonSyntax(node.NameColon));
    }

    docs.Add(
      this.PrintSyntaxToken(node.RefKindKeyword, afterTokenIfNoTrailing: " ")
    );
    docs.Add(this.Print(node.Expression));
    return Docs.Concat(docs);
  }
}

}
