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
        var docs = new List<Doc> { Token.Print(openParenToken, context) };

        switch (arguments)
        {
            case [{ Expression: SimpleLambdaExpressionSyntax lambda } arg]:
            {
                docs.Add(
                    Doc.GroupWithId(
                        "LambdaArguments",
                        Doc.Indent(
                            Doc.SoftLine,
                            Argument.PrintModifiers(arg, context),
                            SimpleLambdaExpression.PrintHead(lambda, context)
                        )
                    ),
                    Doc.IndentIfBreak(
                        SimpleLambdaExpression.PrintBody(lambda, context),
                        "LambdaArguments"
                    ),
                    lambda.Body
                        is BlockSyntax
                            or ObjectCreationExpressionSyntax
                            or AnonymousObjectCreationExpressionSyntax
                        ? Doc.IfBreak(Doc.SoftLine, Doc.Null, "LambdaArguments")
                        : Doc.SoftLine
                );
                break;
            }
            case [
                {
                    Expression: ParenthesizedLambdaExpressionSyntax
                    {
                        ParameterList.Parameters.Count: 0,
                        Block: { }
                    } lambda
                } arg
            ]:
            {
                docs.Add(
                    Doc.GroupWithId(
                        "LambdaArguments",
                        Doc.Indent(
                            Doc.SoftLine,
                            Argument.PrintModifiers(arg, context),
                            ParenthesizedLambdaExpression.PrintHead(lambda, context)
                        )
                    ),
                    Doc.IndentIfBreak(
                        ParenthesizedLambdaExpression.PrintBody(lambda, context),
                        "LambdaArguments"
                    ),
                    Doc.IfBreak(Doc.SoftLine, Doc.Null, "LambdaArguments")
                );
                break;
            }
            default:
            {
                docs.Add(
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(arguments, Argument.Print, Doc.Line, context)
                    ),
                    Doc.SoftLine
                );
                break;
            }
        }

        docs.Add(Token.Print(closeParenToken, context));

        return Doc.Concat(docs);
    }
}
