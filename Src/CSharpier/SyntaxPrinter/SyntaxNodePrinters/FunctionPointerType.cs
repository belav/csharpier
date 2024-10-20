namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FunctionPointerType
{
    public static Doc Print(FunctionPointerTypeSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.Print(node.DelegateKeyword, context),
            Token.PrintWithSuffix(node.AsteriskToken, " ", context),
            node.CallingConvention != null
                ? PrintCallingConvention(node.CallingConvention, context)
                : Doc.Null,
            Token.Print(node.ParameterList.LessThanToken, context),
            Doc.Indent(
                Doc.Group(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(
                        node.ParameterList.Parameters,
                        (o, _) =>
                            Doc.Concat(
                                AttributeLists.Print(o, o.AttributeLists, context),
                                Modifiers.Print(o.Modifiers, context),
                                Node.Print(o.Type, context)
                            ),
                        Doc.Line,
                        context
                    )
                )
            ),
            Token.Print(node.ParameterList.GreaterThanToken, context)
        );
    }

    private static Doc PrintCallingConvention(
        FunctionPointerCallingConventionSyntax node,
        PrintingContext context
    )
    {
        return Doc.Concat(
            Token.Print(node.ManagedOrUnmanagedKeyword, context),
            node.UnmanagedCallingConventionList != null
                ? Doc.Concat(
                    Token.Print(node.UnmanagedCallingConventionList.OpenBracketToken, context),
                    SeparatedSyntaxList.Print(
                        node.UnmanagedCallingConventionList.CallingConventions,
                        (o, _) => Token.Print(o.Name, context),
                        " ",
                        context
                    ),
                    Token.Print(node.UnmanagedCallingConventionList.CloseBracketToken, context)
                )
                : Doc.Null
        );
    }
}
