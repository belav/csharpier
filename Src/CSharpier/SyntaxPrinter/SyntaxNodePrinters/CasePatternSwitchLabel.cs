using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CasePatternSwitchLabel
    {
        public static Doc Print(CasePatternSwitchLabelSyntax node)
        {
            var docs = new List<Doc>();
            docs.Add(Token.Print(node.Keyword, " "), Node.Print(node.Pattern));
            if (node.WhenClause != null)
            {
                docs.Add(" ", Node.Print(node.WhenClause));
            }
            docs.Add(Token.Print(node.ColonToken));
            return Doc.Concat(docs);
        }
    }
}
