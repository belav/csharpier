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

    // -and/-or/-xor and +/- break across lines. Other operators only break in a chain of two or
    // more, such as $a -replace 'b', 'c' -replace 'd', 'e', so a single $a -gt 0 stays together.
    // Nothing breaks between the brackets of an index.
    internal static bool CanBreakAfterOperator(BinaryExpressionAst node)
    {
        if (IsInsideIndex(node))
        {
            return false;
        }

        var precedence = GetPrecedence(node.Operator);
        if (precedence is TokenFlags.BinaryPrecedenceLogical or TokenFlags.BinaryPrecedenceAdd)
        {
            return true;
        }

        return precedence
                is not (TokenFlags.BinaryPrecedenceFormat or TokenFlags.BinaryPrecedenceRange)
            && (
                node.Left is BinaryExpressionAst left && GetPrecedence(left.Operator) == precedence
                || node.Parent is BinaryExpressionAst parent
                    && parent.Left == node
                    && GetPrecedence(parent.Operator) == precedence
            );
    }

    internal static TokenFlags GetPrecedence(TokenKind kind)
    {
        return kind.GetTraits() & TokenFlags.BinaryPrecedenceMask;
    }

    internal static bool IsInsideIndex(Ast node)
    {
        var child = node;
        var parent = node.Parent;
        while (
            parent
                is BinaryExpressionAst
                    or UnaryExpressionAst
                    or ConvertExpressionAst
                    or ArrayLiteralAst
        )
        {
            child = parent;
            parent = parent.Parent;
        }

        return parent is IndexExpressionAst index && index.Index == child;
    }

    // True for an expression that has no place of its own to break across lines.
    internal static bool HasNoBreakPoint(ExpressionAst node)
    {
        return node switch
        {
            VariableExpressionAst or TypeExpressionAst => true,
            StringConstantExpressionAst or ExpandableStringExpressionAst => !IsMultiLine(node),
            ConstantExpressionAst => true,
            InvokeMemberExpressionAst invokeMember => (invokeMember.Arguments?.Count ?? 0) == 0
                && HasNoBreakPoint(invokeMember.Expression),
            MemberExpressionAst member => HasNoBreakPoint(member.Expression),
            IndexExpressionAst index => HasNoBreakPoint(index.Target),
            ConvertExpressionAst convert => HasNoBreakPoint(convert.Child),
            UnaryExpressionAst unary => HasNoBreakPoint(unary.Child),
            BinaryExpressionAst binary => !CanBreakAfterOperator(binary)
                && HasNoBreakPoint(binary.Left)
                && HasNoBreakPoint(binary.Right),
            _ => false,
        };
    }
}
