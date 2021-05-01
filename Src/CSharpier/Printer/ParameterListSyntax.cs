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
            return Doc.GroupWithId(
                groupId ?? string.Empty,
                Token.Print(node.OpenParenToken),
                node.Parameters.Count > 0
                    ? Doc.Concat(
                            Doc.Indent(
                                Doc.SoftLine,
                                SeparatedSyntaxList.Print(
                                    node.Parameters,
                                    this.PrintParameterSyntax,
                                    Doc.Line
                                )
                            ),
                            Doc.SoftLine
                        )
                    : Doc.Null,
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
