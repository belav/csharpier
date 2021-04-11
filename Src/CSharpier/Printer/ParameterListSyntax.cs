using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParameterListSyntax(ParameterListSyntax node)
        {
            return Group(
                this.PrintSyntaxToken(node.OpenParenToken),
                node.Parameters.Count > 0
                    ? Docs.Concat(
                        Docs.Indent(
                            Docs.SoftLine,
                            this.PrintSeparatedSyntaxList(
                                node.Parameters,
                                this.PrintParameterSyntax,
                                Line
                            )
                        ),
                        Docs.SoftLine
                    )
                    : Doc.Null,
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
