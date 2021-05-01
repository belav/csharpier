using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConstructorDeclaration
    {
        public static Doc Print(ConstructorDeclarationSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            return Doc.Group(
                ExtraNewLines.Print(node),
                new Printer().PrintAttributeLists(node, node.AttributeLists),
                Doc.Group(
                    Modifiers.Print(node.Modifiers),
                    Token.Print(node.Identifier),
                    ParameterList.Print(node.ParameterList, groupId),
                    Doc.IfBreak(Doc.Null, Doc.SoftLine)
                ),
                node.Initializer != null
                    ? Node.Print(node.Initializer)
                    : Doc.Null,
                node.Body != null
                    ? Block.PrintWithConditionalSpace(node.Body, groupId)
                    : Doc.Null,
                node.ExpressionBody != null
                    ? ArrowExpressionClause.Print(node.ExpressionBody)
                    : Doc.Null,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
