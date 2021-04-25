using System;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;

namespace CSharpier.SyntaxPrinter {

public static class SyntaxNodes
{
  [ThreadStatic]
  private static Printer? printer;

  public static void Initialize(Printer value)
  {
    printer = value;
  }

  public static Doc Print(SyntaxNode node)
  {
    return printer!.Print(node);
  }
}

}
