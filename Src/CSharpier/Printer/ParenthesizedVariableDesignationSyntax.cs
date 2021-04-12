using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintParenthesizedVariableDesignationSyntax(
            ParenthesizedVariableDesignationSyntax node
        ) {
            return Docs.Group(
                this.PrintSyntaxToken(node.OpenParenToken),
                Docs.Indent(
                    Docs.SoftLine,
                    this.PrintSeparatedSyntaxList(
                        node.Variables,
                        this.Print,
                        Docs.Line
                    ),
                    Docs.SoftLine
                ),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}
