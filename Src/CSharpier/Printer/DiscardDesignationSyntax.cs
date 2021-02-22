using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintDiscardDesignationSyntax(DiscardDesignationSyntax node)
        {
            return this.PrintSyntaxToken(node.UnderscoreToken);
        }
    }
}
