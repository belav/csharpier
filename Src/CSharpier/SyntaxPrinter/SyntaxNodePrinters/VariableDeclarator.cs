using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class VariableDeclarator
    {
        public static Doc Print(VariableDeclaratorSyntax node)
        {
            var docs = new List<Doc> { Token.Print(node.Identifier) };
            if (node.ArgumentList != null)
            {
                docs.Add(BracketedArgumentList.Print(node.ArgumentList));
            }
            if (node.Initializer != null)
            {
                docs.Add(EqualsValueClause.Print(node.Initializer));
            }
            return Doc.Concat(docs);
        }
    }
}
