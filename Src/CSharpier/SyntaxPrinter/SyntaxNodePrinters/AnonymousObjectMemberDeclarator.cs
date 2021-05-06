using System.Collections.Generic;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AnonymousObjectMemberDeclarator
    {
        public static Doc Print(AnonymousObjectMemberDeclaratorSyntax node)
        {
            var docs = new List<Doc>();
            if (node.NameEquals != null)
            {
                docs.Add(Token.PrintWithSuffix(node.NameEquals.Name.Identifier, " "));
                docs.Add(Token.PrintWithSuffix(node.NameEquals.EqualsToken, " "));
            }
            docs.Add(Node.Print(node.Expression));
            return Doc.Concat(docs);
        }
    }
}
