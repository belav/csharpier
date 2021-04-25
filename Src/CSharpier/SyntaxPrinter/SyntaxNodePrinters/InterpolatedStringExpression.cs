using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters {

public static class InterpolatedStringExpression
{
  public static Doc Print(InterpolatedStringExpressionSyntax node)
  {
    var docs = new List<Doc>
    {
      SyntaxTokens.PrintWithoutLeadingTrivia(node.StringStartToken)
    };

    docs.AddRange(node.Contents.Select(SyntaxNodes.Print));
    docs.Add(SyntaxTokens.Print(node.StringEndToken));

    return Docs.Concat(
      // pull out the leading trivia so it doesn't get forced flat
      SyntaxTokens.PrintLeadingTrivia(node.StringStartToken),
      Docs.ForceFlat(docs)
    );
  }
}

}
