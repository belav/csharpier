using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier {

public partial class Printer
{
  private Doc PrintForStatementSyntax(ForStatementSyntax node)
  {
    var groupId = Guid.NewGuid().ToString();

    var docs = new List<Doc>
    {
      this.PrintExtraNewLines(node),
      this.PrintSyntaxToken(node.ForKeyword, afterTokenIfNoTrailing: " "),
      SyntaxTokens.Print(node.OpenParenToken)
    };

    var innerGroup = new List<Doc> { Docs.SoftLine };
    if (node.Declaration != null)
    {
      innerGroup.Add(this.PrintVariableDeclarationSyntax(node.Declaration));
    }
    innerGroup.Add(
      this.PrintSeparatedSyntaxList(node.Initializers, this.Print, " ")
    );
    innerGroup.Add(SyntaxTokens.Print(node.FirstSemicolonToken));
    if (node.Condition != null)
    {
      innerGroup.Add(Docs.Line, this.Print(node.Condition));
    }
    else
    {
      innerGroup.Add(Docs.SoftLine);
    }

    innerGroup.Add(SyntaxTokens.Print(node.SecondSemicolonToken));
    if (node.Incrementors.Any())
    {
      innerGroup.Add(Docs.Line);
    }
    else
    {
      innerGroup.Add(Docs.SoftLine);
    }
    innerGroup.Add(
      Docs.Indent(
        this.PrintSeparatedSyntaxList(node.Incrementors, this.Print, Docs.Line)
      )
    );
    docs.Add(Docs.GroupWithId(groupId, Docs.Indent(innerGroup), Docs.SoftLine));
    docs.Add(SyntaxTokens.Print(node.CloseParenToken));
    if (node.Statement is BlockSyntax blockSyntax)
    {
      docs.Add(this.PrintBlockSyntaxWithConditionalSpace(blockSyntax, groupId));
    }
    else
    {
      // TODO 1 force braces? we do in if and else
      docs.Add(Docs.Indent(Docs.HardLine, this.Print(node.Statement)));
    }

    return Docs.Concat(docs);
  }
}

}
