using System.Management.Automation.Language;

namespace CSharpier.Core.PowerShell;

internal static class Layout
{
    internal static bool IsMultiLine(Ast node)
    {
        return node.Extent.StartLineNumber != node.Extent.EndLineNumber;
    }

    // True when the expression is the only thing between a pair of parentheses, so the
    // parentheses already supply the indentation for it.
    internal static bool IsSoleContentOfParens(ExpressionAst node)
    {
        return node.Parent is CommandExpressionAst { Redirections.Count: 0 } commandExpression
            && commandExpression.Parent is PipelineAst { PipelineElements.Count: 1 } pipeline
            && (
                pipeline.Parent
                    is ParenExpressionAst
                        or IfStatementAst
                        or WhileStatementAst
                        or DoWhileStatementAst
                        or DoUntilStatementAst
                || (
                    pipeline.Parent is SwitchStatementAst switchStatement
                    && (switchStatement.Flags & SwitchFlags.File) == 0
                )
                || pipeline.Parent
                    is StatementBlockAst
                    {
                        Statements.Count: 1,
                        Parent: ArrayExpressionAst or SubExpressionAst
                    }
            );
    }

    // True for content that goes on its own lines between the parentheses when it does not fit.
    internal static bool BreaksInsideParens(PipelineBaseAst node)
    {
        return node is PipelineAst { PipelineElements.Count: 1, Background: false } pipeline
            && pipeline.PipelineElements[0]
                is CommandExpressionAst
                {
                    Redirections.Count: 0,
                    Expression: BinaryExpressionAst or TernaryExpressionAst or ArrayLiteralAst
                };
    }

    // Constructs that open and close with their own delimiters, so the code around them can
    // stay on the lines of those delimiters when the construct spans lines.
    internal static bool IsDelimitedBlock(Ast node)
    {
        return node
            is ScriptBlockExpressionAst
                or HashtableAst
                or ArrayExpressionAst
                or SubExpressionAst
                or ParenExpressionAst
                or ConvertExpressionAst { Child: HashtableAst or ScriptBlockExpressionAst };
    }

    internal static bool IsMultiLineString(Ast node)
    {
        return node is StringConstantExpressionAst or ExpandableStringExpressionAst
            && IsMultiLine(node);
    }
}
