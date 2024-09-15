namespace CSharpier.SyntaxPrinter;

internal static class ArgumentListLike
{
    public static Doc Print(
        SyntaxToken openParenToken,
        SeparatedSyntaxList<ArgumentSyntax> arguments,
        SyntaxToken closeParenToken,
        FormattingContext context
    )
    {
        var lambdaId = Guid.NewGuid();

        var args = arguments switch
        {
            [{ Expression: SimpleLambdaExpressionSyntax lambda } arg] => Doc.Concat(
                Doc.GroupWithId(
                    $"LambdaArguments{lambdaId}",
                    Doc.Indent(
                        Doc.SoftLine,
                        Argument.PrintModifiers(arg, context),
                        SimpleLambdaExpression.PrintHead(lambda, context)
                    )
                ),
                Doc.IfBreak(
                    Doc.Indent(Doc.Group(SimpleLambdaExpression.PrintBody(lambda, context))),
                    SimpleLambdaExpression.PrintBody(lambda, context),
                    $"LambdaArguments{lambdaId}"
                ),
                lambda.Body
                    is BlockSyntax
                        or ObjectCreationExpressionSyntax
                        or AnonymousObjectCreationExpressionSyntax
                    ? Doc.IfBreak(Doc.SoftLine, Doc.Null, $"LambdaArguments{lambdaId}")
                    : Doc.SoftLine
            ),
            [{ Expression: ParenthesizedLambdaExpressionSyntax lambda } arg]
                when lambda is { ParameterList.Parameters: [], Block: { } } => Doc.Concat(
                Doc.GroupWithId(
                    $"LambdaArguments{lambdaId}",
                    Doc.Indent(
                        Doc.SoftLine,
                        Argument.PrintModifiers(arg, context),
                        ParenthesizedLambdaExpression.PrintHead(lambda, context)
                    )
                ),
                Doc.IndentIfBreak(
                    ParenthesizedLambdaExpression.PrintBody(lambda, context),
                    $"LambdaArguments{lambdaId}"
                ),
                Doc.IfBreak(Doc.SoftLine, Doc.Null, $"LambdaArguments{lambdaId}")
            ),
            [_, ..] => Doc.Concat(
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(arguments, Argument.Print, Doc.Line, context)
                ),
                Doc.SoftLine
            ),
            _ => Doc.Null,
        };

        return Doc.Concat(
            Token.Print(openParenToken, context),
            args,
            Token.Print(closeParenToken, context)
        );
    }
}
