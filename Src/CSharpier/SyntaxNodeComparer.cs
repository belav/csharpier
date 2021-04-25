using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpier {

public partial class SyntaxNodeComparer
{
  protected string OriginalSourceCode { get; }
  protected string NewSourceCode { get; }
  protected SyntaxTree OriginalSyntaxTree { get; }
  protected SyntaxTree NewSyntaxTree { get; }

  private static readonly CompareResult Equal = new();

  public SyntaxNodeComparer(
    string originalSourceCode,
    string newSourceCode,
    CancellationToken cancellationToken
  ) {
    this.OriginalSourceCode = originalSourceCode;
    this.NewSourceCode = newSourceCode;

    var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp9);
    this.OriginalSyntaxTree = CSharpSyntaxTree.ParseText(
      this.OriginalSourceCode,
      cSharpParseOptions,
      cancellationToken: cancellationToken
    );
    this.NewSyntaxTree = CSharpSyntaxTree.ParseText(
      this.NewSourceCode,
      cSharpParseOptions,
      cancellationToken: cancellationToken
    );
  }

  public string CompareSource()
  {
    return this.CompareSourceAsync(CancellationToken.None).Result;
  }

  public async Task<string> CompareSourceAsync(
    CancellationToken cancellationToken
  ) {
    var result = this.AreEqualIgnoringWhitespace(
      await OriginalSyntaxTree.GetRootAsync(cancellationToken),
      await NewSyntaxTree.GetRootAsync(cancellationToken)
    );

    if (!result.IsInvalid)
    {
      return string.Empty;
    }

    var message =
      "    Original: " +
      GetLine(
        result.OriginalSpan,
        this.OriginalSyntaxTree,
        this.OriginalSourceCode
      );

    message += "    Formatted: " +
    GetLine(result.NewSpan, this.NewSyntaxTree, this.NewSourceCode);
    return message;
  }

  private string GetLine(
    TextSpan? textSpan,
    SyntaxTree syntaxTree,
    string source
  ) {
    if (!textSpan.HasValue)
    {
      return "Missing";
    }

    var line = syntaxTree.GetLineSpan(textSpan.Value).StartLinePosition.Line;
    var endLine = syntaxTree.GetLineSpan(textSpan.Value).EndLinePosition.Line;

    var result = "Around Line " + line + Environment.NewLine;

    var stringReader = new StringReader(source);
    var x = 0;
    var linesWritten = 0;
    var currentLine = stringReader.ReadLine();
    while (x <= endLine + 2 || linesWritten < 8)
    {
      if (x >= line - 2)
      {
        result += currentLine + Environment.NewLine;
        linesWritten++;
      }

      if (linesWritten > 15)
      {
        break;
      }

      currentLine = stringReader.ReadLine();
      if (currentLine == null)
      {
        break;
      }

      x++;
    }

    return result;
  }

  private readonly Stack<SyntaxNode> originalStack = new();
  private readonly Stack<SyntaxNode> formattedStack = new();

  private CompareResult AreEqualIgnoringWhitespace(
    SyntaxNode originalStart,
    SyntaxNode formattedStart
  ) {
    originalStack.Push(originalStart);
    formattedStack.Push(formattedStart);
    while (originalStack.Count > 0)
    {
      var result = this.Compare(originalStack.Pop(), formattedStack.Pop());
      if (result.IsInvalid)
      {
        return result;
      }
    }

    return Equal;
  }

  private CompareResult CompareLists<T>(
    IReadOnlyList<T> originalList,
    IReadOnlyList<T> formattedList,
    Func<T, T, CompareResult> comparer,
    Func<T, TextSpan> getSpan,
    TextSpan originalParentSpan,
    TextSpan newParentSpan
  ) {
    for (var x = 0; x < originalList.Count || x < formattedList.Count; x++)
    {
      if (x == originalList.Count)
      {
        return NotEqual(originalParentSpan, getSpan(formattedList[x]));
      }

      if (x == formattedList.Count)
      {
        return NotEqual(getSpan(originalList[x]), newParentSpan);
      }

      if (
        originalList[x] is SyntaxNode originalNode &&
        formattedList[x] is SyntaxNode formattedNode
      ) {
        originalStack.Push(originalNode);
        formattedStack.Push(formattedNode);
      }
      else
      {
        var result = comparer(originalList[x], formattedList[x]);
        if (result.IsInvalid)
        {
          return result;
        }
      }
    }

    return Equal;
  }

  private CompareResult NotEqual(
    SyntaxNode originalNode,
    SyntaxNode formattedNode
  ) {
    return new()
    {
      IsInvalid = true,
      OriginalSpan = originalNode?.Span,
      NewSpan = formattedNode?.Span
    };
  }

  private CompareResult NotEqual(
    TextSpan? originalSpan,
    TextSpan? formattedSpan
  ) {
    return new()
    {
      IsInvalid = true,
      OriginalSpan = originalSpan,
      NewSpan = formattedSpan
    };
  }

  // TODO this is used by compare lists, but then we don't have parents if one is missing
  private CompareResult Compare(
    SyntaxToken originalToken,
    SyntaxToken formattedToken
  ) {
    return Compare(originalToken, formattedToken, null, null);
  }

  private CompareResult Compare(
    SyntaxToken originalToken,
    SyntaxToken formattedToken,
    SyntaxNode? originalNode,
    SyntaxNode? formattedNode
  ) {
    if (originalToken.Text != formattedToken.Text)
    {
      return NotEqual(
        originalToken.RawKind == 0 ? originalNode?.Span : originalToken.Span,
        formattedToken.RawKind == 0 ? formattedNode?.Span : formattedToken.Span
      );
    }

    var result = this.Compare(
      originalToken.LeadingTrivia,
      formattedToken.LeadingTrivia
    );
    if (result.IsInvalid)
    {
      return result;
    }

    var result2 = this.Compare(
      originalToken.TrailingTrivia,
      formattedToken.TrailingTrivia
    );

    return result2.IsInvalid ? result2 : Equal;
  }

  private CompareResult Compare(
    SyntaxTrivia originalTrivia,
    SyntaxTrivia formattedTrivia
  ) {
    if (
      originalTrivia.ToString().TrimEnd() !=
      formattedTrivia.ToString().TrimEnd()
    ) {
      return NotEqual(originalTrivia.Span, formattedTrivia.Span);
    }

    return Equal;
  }

  private CompareResult Compare(
    SyntaxTriviaList originalList,
    SyntaxTriviaList formattedList
  ) {
    var cleanedOriginal = originalList.Where(
        o =>
          o.Kind() != SyntaxKind.EndOfLineTrivia &&
          o.Kind() != SyntaxKind.WhitespaceTrivia
      )
      .ToList();
    var cleanedFormatted = formattedList.Where(
        o =>
          o.Kind() != SyntaxKind.EndOfLineTrivia &&
          o.Kind() != SyntaxKind.WhitespaceTrivia
      )
      .ToList();
    var result = CompareLists(
      cleanedOriginal,
      cleanedFormatted,
      Compare,
      o => o.Span,
      originalList.Span,
      formattedList.Span
    );
    return result.IsInvalid ? result : Equal;
  }
}

public struct CompareResult
{
  public bool IsInvalid;
  public TextSpan? OriginalSpan;
  public TextSpan? NewSpan;
}

}
