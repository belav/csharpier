using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedVariableDesignationSyntax(
            ParenthesizedVariableDesignationSyntax node
        ) {
            return Doc.Group(
                Token.Print(node.OpenParenToken),
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(
                        node.Variables,
                        this.Print,
                        Doc.Line
                    ),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
