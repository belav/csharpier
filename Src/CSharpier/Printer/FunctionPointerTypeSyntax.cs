using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFunctionPointerTypeSyntax(
            FunctionPointerTypeSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.DelegateKeyword),
                this.PrintSyntaxToken(
                    node.AsteriskToken,
                    afterTokenIfNoTrailing: " "
                ),
                node.CallingConvention != null
                    ? this.PrintCallingConvention(node.CallingConvention)
                    : Doc.Null,
                Token.Print(node.ParameterList.LessThanToken),
                Doc.Indent(
                    Doc.Group(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(
                            node.ParameterList.Parameters,
                            o =>
                                Doc.Concat(
                                    this.PrintAttributeLists(
                                        o,
                                        o.AttributeLists
                                    ),
                                    Modifiers.Print(o.Modifiers),
                                    this.Print(o.Type)
                                ),
                            Doc.Line
                        )
                    )
                ),
                Token.Print(node.ParameterList.GreaterThanToken)
            );
        }

        private Doc PrintCallingConvention(
            FunctionPointerCallingConventionSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.ManagedOrUnmanagedKeyword),
                node.UnmanagedCallingConventionList != null
                    ? Doc.Concat(
                            this.PrintSyntaxToken(
                                node.UnmanagedCallingConventionList.OpenBracketToken
                            ),
                            SeparatedSyntaxList.Print(
                                node.UnmanagedCallingConventionList.CallingConventions,
                                o => Token.Print(o.Name),
                                " "
                            ),
                            this.PrintSyntaxToken(
                                node.UnmanagedCallingConventionList.CloseBracketToken
                            )
                        )
                    : Doc.Null
            );
        }
    }
}
