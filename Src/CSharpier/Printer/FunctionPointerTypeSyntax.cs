using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFunctionPointerTypeSyntax(
            FunctionPointerTypeSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.DelegateKeyword),
                this.PrintSyntaxToken(node.AsteriskToken, " "),
                node.CallingConvention != null
                    ? this.PrintCallingConvention(node.CallingConvention)
                    : Doc.Null,
                this.PrintSyntaxToken(node.ParameterList.LessThanToken),
                Indent(
                    Group(
                        SoftLine,
                        this.PrintSeparatedSyntaxList(
                            node.ParameterList.Parameters,
                            o => Concat(
                                this.PrintAttributeLists(o, o.AttributeLists),
                                this.PrintModifiers(o.Modifiers),
                                this.Print(o.Type)
                            ),
                            Line
                        )
                    )
                ),
                this.PrintSyntaxToken(node.ParameterList.GreaterThanToken)
            );
        }

        private Doc PrintCallingConvention(
            FunctionPointerCallingConventionSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.ManagedOrUnmanagedKeyword),
                node.UnmanagedCallingConventionList != null
                    ? Concat(
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
                    : Doc.Null
            );
        }
    }
}
