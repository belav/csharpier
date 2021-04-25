using System;
using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;

namespace CSharpier.SyntaxPrinter
{
    // TODO partial
    // this conflicts with the Roslyn type name but hasn't caused issues
    // but maybe it should be SeparatedSyntaxLists
    // these files aren't currently consistent with if they are pluralized or not.
    // if we go all plural it would look like
    // BinaryExpressions.Print
    // SeparatedSyntaxLists.Print
    // etc
    // if we ditch the plural we would run into the following which conflict with Roslyn type names
    // SeparatedSyntaxList.Print
    // SyntaxToken.Print
    public static class SeparatedSyntaxList
    {
        public static Doc Print<T>(
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

                docs.Add(SyntaxTokens.Print(list.GetSeparator(x)));
                if (!isTrailingSeparator)
                {
                    docs.Add(afterSeparator);
                }
            }

            return docs.Count == 0 ? Doc.Null : Docs.Concat(docs);
        }
    }
}
