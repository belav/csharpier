using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintParenthesizedVariableDesignationSyntax(ParenthesizedVariableDesignationSyntax node)
        {
            return Group(this.PrintSyntaxToken(node.OpenParenToken),
                Indent(SoftLine, this.PrintSeparatedSyntaxList(node.Variables, this.Print, Line), SoftLine),
                this.PrintSyntaxToken(node.CloseParenToken)
            );
        }
    }
}