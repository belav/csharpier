using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class ArgumentListWithTrailingLambda
{
    public static Doc Print(
        SeparatedSyntaxList<ArgumentSyntax> arguments,
        LambdaExpressionSyntax lastLambda,
        PrintingContext context
    )
    {
        // Build chop fall back (can't use headParts from below because ForceFlat)
        var chop = Doc.Concat(
            Doc.Indent(
                Doc.SoftLine,
                SeparatedSyntaxList.Print(arguments, Argument.Print, Doc.Line, context)
            ),
            Doc.SoftLine
        );

        Doc lambdaHead;
        Doc lambdaBody;
        bool bodyIsBraced;
        if (lastLambda is SimpleLambdaExpressionSyntax simpleLambda)
        {
            bodyIsBraced =
                simpleLambda.Body
                    is BlockSyntax
                        or ObjectCreationExpressionSyntax
                        or AnonymousObjectCreationExpressionSyntax;

            lambdaHead = SimpleLambdaExpression.PrintHead(simpleLambda, context);
            lambdaBody = SimpleLambdaExpression.PrintBody(simpleLambda, context);
        }
        else if (lastLambda is ParenthesizedLambdaExpressionSyntax parenthesizedLambda)
        {
            bodyIsBraced = parenthesizedLambda.Block is not null;

            lambdaHead = ParenthesizedLambdaExpression.PrintHead(parenthesizedLambda, context);

            // PrintBody wraps expression bodies in a group, which could fit on the head's
            // line and leave the closing paren on its own line, so break it ourselves
            lambdaBody = bodyIsBraced
                ? ParenthesizedLambdaExpression.PrintBody(parenthesizedLambda, context)
                : Doc.Indent(Doc.Line, Node.Print(parenthesizedLambda.Body, context));
        }
        else
        {
            // New lambda type, default to chopping
            return chop;
        }

        // Accumulate the head parts: flat arguments with lambda parameters and "=>" symbol
        var flatParts = new List<Doc>((arguments.Count - 1) * 3 + 2);
        for (var x = 0; x < arguments.Count - 1; x++)
        {
            flatParts.Add(Argument.Print(arguments[x], context));
            flatParts.Add(Token.Print(arguments.GetSeparator(x), context));
            flatParts.Add(" ");
        }

        flatParts.Add(Argument.PrintModifiers(arguments[^1], context));
        flatParts.Add(lambdaHead);

        // If any flat part has a break then just chop immediately
        if (flatParts.Any(DocUtilities.ContainsBreak))
        {
            return chop;
        }

        // Build wrapped lambda - either flat or with the lambda body starting on a new line
        var wrap = Doc.Concat(
            Doc.ForceFlat(flatParts),
            lambdaBody,
            bodyIsBraced ? Doc.Null : Doc.SoftLine
        );

        return Doc.ConditionalGroup(
            wrap, // Try flat (all arguments fit on one line)
            wrap, // Try break (all arguments syntax except the lamba's body)
            chop // Default to chopping
        );
    }
}
