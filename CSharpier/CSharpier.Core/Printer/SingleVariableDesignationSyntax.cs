using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSingleVariableDesignationSyntax(SingleVariableDesignationSyntax node)
        {
            return node.Identifier.Text;
        }
    }
}
