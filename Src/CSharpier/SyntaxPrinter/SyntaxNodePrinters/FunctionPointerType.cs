using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FunctionPointerType
{
    public static Doc Print(FunctionPointerTypeSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.DelegateKeyword),
            Token.PrintWithSuffix(node.AsteriskToken, " "),
            node.CallingConvention != null
              ? PrintCallingConvention(node.CallingConvention)
              : Doc.Null,
            Token.Print(node.ParameterList.LessThanToken),
            Doc.Indent(
                Doc.Group(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(
                        node.ParameterList.Parameters,
                        o =>
                            Doc.Concat(
                                AttributeLists.Print(o, o.AttributeLists),
                                Modifiers.Print(o.Modifiers),
                                Node.Print(o.Type)
                            ),
                        Doc.Line
                    )
                )
            ),
            Token.Print(node.ParameterList.GreaterThanToken)
        );
    }

    private static Doc PrintCallingConvention(FunctionPointerCallingConventionSyntax node)
    {
        return Doc.Concat(
            Token.Print(node.ManagedOrUnmanagedKeyword),
            node.UnmanagedCallingConventionList != null
              ? Doc.Concat(
                    Token.Print(node.UnmanagedCallingConventionList.OpenBracketToken),
                    SeparatedSyntaxList.Print(
                        node.UnmanagedCallingConventionList.CallingConventions,
                        o => Token.Print(o.Name),
                        " "
                    ),
                    Token.Print(node.UnmanagedCallingConventionList.CloseBracketToken)
                )
              : Doc.Null
        );
    }
}
