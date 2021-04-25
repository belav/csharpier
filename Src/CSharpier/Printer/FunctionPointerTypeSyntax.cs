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
            return Docs.Concat(
                SyntaxTokens.Print(node.DelegateKeyword),
                this.PrintSyntaxToken(
                    node.AsteriskToken,
                    afterTokenIfNoTrailing: " "
                ),
                node.CallingConvention != null
                    ? this.PrintCallingConvention(node.CallingConvention)
                    : Docs.Null,
                SyntaxTokens.Print(node.ParameterList.LessThanToken),
                Docs.Indent(
                    Docs.Group(
                        Docs.SoftLine,
                        SeparatedSyntaxList.Print(
                            node.ParameterList.Parameters,
                            o =>
                                Docs.Concat(
                                    this.PrintAttributeLists(
                                        o,
                                        o.AttributeLists
                                    ),
                                    Modifiers.Print(o.Modifiers),
                                    this.Print(o.Type)
                                ),
                            Docs.Line
                        )
                    )
                ),
                SyntaxTokens.Print(node.ParameterList.GreaterThanToken)
            );
        }

        private Doc PrintCallingConvention(
            FunctionPointerCallingConventionSyntax node
        ) {
            return Docs.Concat(
                SyntaxTokens.Print(node.ManagedOrUnmanagedKeyword),
                node.UnmanagedCallingConventionList != null
                    ? Docs.Concat(
                            this.PrintSyntaxToken(
                                node.UnmanagedCallingConventionList.OpenBracketToken
                            ),
                            SeparatedSyntaxList.Print(
                                node.UnmanagedCallingConventionList.CallingConventions,
                                o => SyntaxTokens.Print(o.Name),
                                " "
                            ),
                            this.PrintSyntaxToken(
                                node.UnmanagedCallingConventionList.CloseBracketToken
                            )
                        )
                    : Docs.Null
            );
        }
    }
}
