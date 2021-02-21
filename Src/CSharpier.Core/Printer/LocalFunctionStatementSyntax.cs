using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintLocalFunctionStatementSyntax(LocalFunctionStatementSyntax node)
        {
            return this.PrintBaseMethodDeclarationSyntax(node);
        }
    }
}
