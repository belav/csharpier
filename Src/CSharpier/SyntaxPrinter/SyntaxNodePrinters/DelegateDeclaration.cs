using System.Collections.Generic;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
                new Printer().PrintAttributeLists(node, node.AttributeLists),
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
                new Printer().PrintConstraintClauses(node.ConstraintClauses),
                Token.Print(node.SemicolonToken)
            );
            return Doc.Concat(docs);
        }
    }
}
