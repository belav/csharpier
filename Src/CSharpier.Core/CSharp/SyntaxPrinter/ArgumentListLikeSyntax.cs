using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class ArgumentListLike
{
    public static Doc Print(
        SyntaxToken openParenToken,
        SeparatedSyntaxList<ArgumentSyntax> arguments,
        SyntaxToken closeParenToken,
        PrintingContext context
    )
    {
        Doc? args;
        if (
            arguments is [{ Expression: SimpleLambdaExpressionSyntax simpleLambda }]
            && !simpleLambda.GetLeadingTrivia().Any(o => o.IsComment())
        )
        {
            var groupId = context.GroupFor("LambdaArguments");
            args = Doc.Concat(
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(
                        Doc.SoftLine,
                        Argument.PrintModifiers(arguments[0], context),
                        SimpleLambdaExpression.PrintHead(simpleLambda, context)
                    )
                ),
                Doc.IfBreak(
                    Doc.Indent(Doc.Group(SimpleLambdaExpression.PrintBody(simpleLambda, context))),
                    SimpleLambdaExpression.PrintBody(simpleLambda, context),
                    groupId
                ),
                simpleLambda.Body
                    is BlockSyntax
                        or ObjectCreationExpressionSyntax
                        or AnonymousObjectCreationExpressionSyntax
                    ? Doc.IfBreak(Doc.SoftLine, Doc.Null, groupId)
                    : Doc.SoftLine
            );
        }
        else if (
            arguments
            is [
                {
                    Expression: ParenthesizedLambdaExpressionSyntax
                    {
                        ParameterList.Parameters: []
                    } lambda
                },
            ]
        )
        {
            var groupId = context.GroupFor("LambdaArguments");
            args = Doc.Concat(
                Doc.GroupWithId(
                    groupId,
                    Doc.Indent(
                        Doc.SoftLine,
                        Argument.PrintModifiers(arguments[0], context),
                        ParenthesizedLambdaExpression.PrintHead(lambda, context)
                    )
                ),
                Doc.IndentIfBreak(
                    ParenthesizedLambdaExpression.PrintBody(lambda, context),
                    groupId
                ),
                lambda.Block is not null
                    ? Doc.IfBreak(Doc.SoftLine, Doc.Null, groupId)
                    : Doc.SoftLine
            );
        }
        else if (arguments is [{ Expression: CollectionExpressionSyntax, NameColon: null }])
        {
            args = SeparatedSyntaxList.Print(arguments, Argument.Print, Doc.Line, context);
        }
        else if (arguments.Count > 0)
        {
            args = Doc.Concat(
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(arguments, Argument.Print, Doc.Line, context)
                ),
                Doc.SoftLine
            );
        }
        else
        {
            args = Doc.Null;
        }

        return Doc.Concat(
            Token.Print(openParenToken, context),
            args,
            Token.Print(closeParenToken, context)
        );
    }
}
