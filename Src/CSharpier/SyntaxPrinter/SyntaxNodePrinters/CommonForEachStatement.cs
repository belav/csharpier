using System;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class CommonForEachStatement
    {
        public static Doc Print(CommonForEachStatementSyntax node)
        {
            var groupId = Guid.NewGuid().ToString();

            var leadingTrivia = node.AwaitKeyword.Kind() != SyntaxKind.None
                ? Token.PrintLeadingTrivia(node.AwaitKeyword)
                : Token.PrintLeadingTrivia(node.ForEachKeyword);

            var variable = node switch
            {
                ForEachStatementSyntax forEach => Doc.Concat(
                    Node.Print(forEach.Type),
                    " ",
                    Token.Print(forEach.Identifier)
                ),
                ForEachVariableStatementSyntax forEachVariable => Node.Print(
                    forEachVariable.Variable
                ),
                _ => Doc.Null
            };

            var docs = Doc.Concat(
                ExtraNewLines.Print(node),
                leadingTrivia,
                Doc.Group(
                    Doc.Group(
                        Token.PrintWithoutLeadingTrivia(node.AwaitKeyword),
                        node.AwaitKeyword.Kind() is not SyntaxKind.None ? " " : Doc.Null,
                        node.AwaitKeyword.Kind() is SyntaxKind.None
                            ? Token.PrintWithoutLeadingTrivia(node.ForEachKeyword)
                            : Token.Print(node.ForEachKeyword),
                        " ",
                        Token.Print(node.OpenParenToken),
                        Doc.GroupWithId(
                            groupId,
                            Doc.Indent(
                                Doc.SoftLine,
                                variable,
                                " ",
                                Token.Print(node.InKeyword),
                                " ",
                                Node.Print(node.Expression)
                            ),
                            Doc.SoftLine
                        ),
                        Token.Print(node.CloseParenToken),
                        Doc.IfBreak(Doc.Null, Doc.SoftLine)
                    ),
                    OptionalBraces.Print(node.Statement, groupId)
                )
            );

            return docs;
        }
    }
}
