using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter.SyntaxNodePrinters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public static class AttributeLists
    {
        public static Doc Print(SyntaxNode node, SyntaxList<AttributeListSyntax> attributeLists)
        {
            if (attributeLists.Count == 0)
            {
                return Doc.Null;
            }

            var docs = new List<Doc>();
            Doc separator = node is TypeParameterSyntax || node is ParameterSyntax
                ? Doc.Line
                : Doc.HardLine;
            docs.Add(Doc.Join(separator, attributeLists.Select(AttributeList.Print)));

            if (!(node is ParameterSyntax))
            {
                docs.Add(separator);
            }

            return Doc.Concat(docs);
        }
    }
}
