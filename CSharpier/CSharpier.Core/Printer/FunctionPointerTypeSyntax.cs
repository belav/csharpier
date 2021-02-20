using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFunctionPointerTypeSyntax(FunctionPointerTypeSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.DelegateKeyword),
                this.PrintSyntaxToken(node.AsteriskToken, " "),
                node.CallingConvention != null ? this.PrintSyntaxToken(node.CallingConvention.ManagedOrUnmanagedKeyword) : null,
                this.PrintSyntaxToken(node.ParameterList.LessThanToken),
                Indent(Group(
                    SoftLine,
                    this.PrintSeparatedSyntaxList(node.ParameterList.Parameters, o => 
                        Concat(this.PrintAttributeLists(o, o.AttributeLists),
                            this.PrintModifiers(o.Modifiers),
                            this.Print(o.Type)), Line)
                )),
                this.PrintSyntaxToken(node.ParameterList.GreaterThanToken)
            );
        }
    }
}
