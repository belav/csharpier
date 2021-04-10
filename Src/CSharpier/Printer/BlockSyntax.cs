using System.Collections.Generic;
using System.Linq;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBlockSyntax(BlockSyntax node)
        {
            Doc statementSeparator = node.Parent is AccessorDeclarationSyntax
                && node.Statements.Count <= 1
                ? Docs.Line
                : Docs.HardLine;

            /* TODO 0 other possible node types that could get this block syntax formatting
            AccessorDeclaration
            AnonymousMethodExpression
            CatchClause
            CheckedStatement
            ConstructorDeclaration
            ConversionOperatorDeclaration
            DestructorDeclaration
            LocalFunctionStatement
            MethodDeclaration
            OperatorDeclaration
            ParenthesizedLambdaExpression
            SimpleLambdaExpression
            UnsafeStatement
            */
            var keepBracesOnPreviousLineIfBreak =
                node.Parent is IfStatementSyntax or WhileStatementSyntax or ForEachStatementSyntax or UsingStatementSyntax;

            var docs = new List<Doc>
            {
                keepBracesOnPreviousLineIfBreak
                    ? Docs.IfBreak(
                        " ",
                        Docs.Line,
                        GroupIdGenerator.GroupIdFor(node.Parent!)
                    )
                    : Docs.Line,
                SyntaxTokens.Print(node.OpenBraceToken)
            };
            if (node.Statements.Count > 0)
            {
                var innerDoc = Docs.Indent(
                    statementSeparator,
                    Join(statementSeparator, node.Statements.Select(this.Print))
                );

                DocUtilities.RemoveInitialDoubleHardLine(innerDoc);

                docs.Add(Docs.Concat(innerDoc, statementSeparator));
            }

            docs.Add(
                this.PrintSyntaxToken(
                    node.CloseBraceToken,
                    null,
                    node.Statements.Count == 0 ? " " : Docs.Null
                )
            );
            return Docs.Group(docs);
        }
    }
}
