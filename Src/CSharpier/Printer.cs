using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSharpier.SyntaxPrinter;

namespace CSharpier {

// TODO 1 can I use source generators for some stuff?
// https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/
public partial class Printer
{
  public Printer()
  {
    SyntaxNodes.Initialize(this);
  }

  public static Doc Join(Doc separator, IEnumerable<Doc> array)
  {
    var docs = new List<Doc>();

    var list = array.ToList();

    if (list.Count == 1)
    {
      return list[0];
    }

    for (var x = 0; x < list.Count; x++)
    {
      if (x != 0)
      {
        docs.Add(separator);
      }

      docs.Add(list[x]);
    }

    return Docs.Concat(docs);
  }

  private Doc PrintSeparatedSyntaxList<T>(
    SeparatedSyntaxList<T> list,
    Func<T, Doc> printFunc,
    Doc afterSeparator
  )
    where T : SyntaxNode {
    var docs = new List<Doc>();
    for (var x = 0; x < list.Count; x++)
    {
      docs.Add(printFunc(list[x]));

      if (x >= list.SeparatorCount)
      {
        continue;
      }

      var isTrailingSeparator = x == list.Count - 1;

      docs.Add(
        this.PrintSyntaxToken(
          list.GetSeparator(x),
          !isTrailingSeparator ? afterSeparator : null
        )
      );
    }

    return docs.Count == 0 ? Doc.Null : Docs.Concat(docs);
  }

  private Doc PrintAttributeLists(
    SyntaxNode node,
    SyntaxList<AttributeListSyntax> attributeLists
  ) {
    if (attributeLists.Count == 0)
    {
      return Doc.Null;
    }

    var docs = new List<Doc>();
    Doc separator = node is TypeParameterSyntax || node is ParameterSyntax
      ? Docs.Line
      : Docs.HardLine;
    docs.Add(
      Join(separator, attributeLists.Select(this.PrintAttributeListSyntax))
    );

    if (!(node is ParameterSyntax))
    {
      docs.Add(separator);
    }

    return Docs.Concat(docs);
  }

  private Doc PrintModifiers(SyntaxTokenList modifiers)
  {
    if (modifiers.Count == 0)
    {
      return Doc.Null;
    }

    var docs = modifiers.Select(
        modifier => this.PrintSyntaxToken(modifier, afterTokenIfNoTrailing: " ")
      )
      .ToList();

    return Docs.Group(Docs.Concat(docs));
  }

  private Doc PrintConstraintClauses(
    SyntaxNode node,
    IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses
  ) {
    var constraintClausesList = constraintClauses.ToList();

    if (constraintClausesList.Count == 0)
    {
      return Doc.Null;
    }

    var docs = new List<Doc>
    {
      Docs.Indent(
        Docs.HardLine,
        Join(
          Docs.HardLine,
          constraintClausesList.Select(
            this.PrintTypeParameterConstraintClauseSyntax
          )
        )
      )
    };

    return Docs.Concat(docs);
  }

  private Doc PrintBaseFieldDeclarationSyntax(BaseFieldDeclarationSyntax node)
  {
    var docs = new List<Doc>
    {
      this.PrintExtraNewLines(node),
      this.PrintAttributeLists(node, node.AttributeLists),
      this.PrintModifiers(node.Modifiers)
    };
    if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
    {
      docs.Add(
        this.PrintSyntaxToken(eventFieldDeclarationSyntax.EventKeyword, " ")
      );
    }

    docs.Add(this.Print(node.Declaration));
    docs.Add(SyntaxTokens.Print(node.SemicolonToken));
    return Docs.Concat(docs);
  }
}

}
