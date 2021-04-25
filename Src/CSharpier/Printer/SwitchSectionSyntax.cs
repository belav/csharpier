using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintSwitchSectionSyntax(SwitchSectionSyntax node)
  {
    var docs = new List<Doc>
    {
      Join(Docs.HardLine, node.Labels.Select(this.Print))
    };
    if (
      node.Statements.Count == 1 &&
      node.Statements[0] is BlockSyntax blockSyntax
    ) {
      docs.Add(this.PrintBlockSyntax(blockSyntax));
    }
    else
    {
      docs.Add(
        Docs.Indent(
          Docs.HardLine,
          Join(Docs.HardLine, node.Statements.Select(this.Print).ToArray())
        )
      );
    }
    return Docs.Concat(docs);
  }
}

}
