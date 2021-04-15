using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParameterListSyntax(
            ParameterListSyntax node,
            string? groupId = null
        ) {
            return Docs.GroupWithId(
                groupId ?? string.Empty,
                SyntaxTokens.Print(node.OpenParenToken),
                node.Parameters.Count > 0
                    ? Docs.Concat(
                            Docs.Indent(
                                Docs.SoftLine,
                                this.PrintSeparatedSyntaxList(
                                    node.Parameters,
                                    this.PrintParameterSyntax,
                                    Docs.Line
                                )
                            ),
                            Docs.SoftLine
                        )
                    : Doc.Null,
                SyntaxTokens.Print(node.CloseParenToken)
            );
        }
    }
}
