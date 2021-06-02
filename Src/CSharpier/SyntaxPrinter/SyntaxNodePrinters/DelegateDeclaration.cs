using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using CSharpier.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DelegateDeclaration
    {
        public static Doc Print(DelegateDeclarationSyntax node)
        {
            var docs = new List<Doc>
            {
                ExtraNewLines.Print(node),
                AttributeLists.Print(node, node.AttributeLists),
                Modifiers.Print(node.Modifiers),
                Token.Print(node.DelegateKeyword, " "),
                Node.Print(node.ReturnType),
                { " ", Token.Print(node.Identifier) }
            };
            if (node.TypeParameterList != null)
            {
                docs.Add(Node.Print(node.TypeParameterList));
            }
            docs.Add(
                Node.Print(node.ParameterList),
                ConstraintClauses.Print(node.ConstraintClauses),
                Token.Print(node.SemicolonToken)
            );
            return Doc.Concat(docs);
        }
    }
}
