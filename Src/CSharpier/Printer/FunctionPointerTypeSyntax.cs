using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFunctionPointerTypeSyntax(
            FunctionPointerTypeSyntax node
        ) {
            return Docs.Concat(
                this.PrintSyntaxToken(node.DelegateKeyword),
                this.PrintSyntaxToken(
                    node.AsteriskToken,
                    afterTokenIfNoTrailing: " "
                ),
                node.CallingConvention != null
                    ? this.PrintCallingConvention(node.CallingConvention)
                    : Docs.Null,
                this.PrintSyntaxToken(node.ParameterList.LessThanToken),
                Docs.Indent(
                    Docs.Group(
                        Docs.SoftLine,
                        this.PrintSeparatedSyntaxList(
                            node.ParameterList.Parameters,
                            o =>
                                Docs.Concat(
                                    this.PrintAttributeLists(
                                        o,
                                        o.AttributeLists
                                    ),
                                    this.PrintModifiers(o.Modifiers),
                                    this.Print(o.Type)
                                ),
                            Docs.Line
                        )
                    )
                ),
                this.PrintSyntaxToken(node.ParameterList.GreaterThanToken)
            );
        }

        private Doc PrintCallingConvention(
            FunctionPointerCallingConventionSyntax node
        ) {
            return Docs.Concat(
                this.PrintSyntaxToken(node.ManagedOrUnmanagedKeyword),
                node.UnmanagedCallingConventionList != null
                    ? Docs.Concat(
                            this.PrintSyntaxToken(
                                node.UnmanagedCallingConventionList.OpenBracketToken
                            ),
                            this.PrintSeparatedSyntaxList(
                                node.UnmanagedCallingConventionList.CallingConventions,
                                o => this.PrintSyntaxToken(o.Name),
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
