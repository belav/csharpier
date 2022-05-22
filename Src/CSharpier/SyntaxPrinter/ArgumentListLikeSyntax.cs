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
        /*
         https://github.com/belav/csharpier-repos/pull/41/files
         
         prettier has a lot more logic around printing arguments, we could stick to just making () => better for now
         but it would be nice to get o => better as well, but that leads to more edge cases
         
        CallMethod(() =>
        {
            CallOtherMethod();
        });

        // when there is no block, I think sticking with this is better
        CallMethod(
            () => CallOtherMethod___________________________________________________________()
        );


        CallMethod(
            (
                longParameter_____________________________,
                longParameter_____________________________,
                longParameter_____________________________
            ) =>
            {
                CallOtherMethod();
            }
        );

        // this looks bad
        var y = someList.Where(o =>
            someLongValue_______________________
            && theseShouldNotIndent_________________
            && theseShouldNotIndent_________________
                > butThisOneShould_________________________________________);

        var y = someList.Where(o =>
        {
            return someLongValue_______________________
                && theseShouldNotIndent_________________
                && theseShouldNotIndent_________________
                    > butThisOneShould_________________________________________;
        });
         
         
         */

        var docs = new List<Doc> { Token.Print(openParenToken, context) };

        if (
            arguments.Count == 1
            && arguments[0].Expression
                is ParenthesizedLambdaExpressionSyntax
                    {
                        ParameterList.Parameters.Count: 0,
                        Block: { }
                    }
                    or SimpleLambdaExpressionSyntax { Block: { } }
        )
        {
            docs.Add(Argument.Print(arguments[0], context));
        }
        else if (arguments.Any())
        {
            docs.Add(
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(arguments, Argument.Print, Doc.Line, context)
                ),
                Doc.SoftLine
            );
        }

        docs.Add(Token.Print(closeParenToken, context));

        return Doc.Concat(docs);
    }
}
