using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        // TODO 0 do I need tests for comments on each and every one of these? If so start from this class and work down.
        private Doc PrintAliasQualifiedNameSyntax(AliasQualifiedNameSyntax node)
        {
            return Concat(this.Print(node.Alias), this.PrintSyntaxToken(node.ColonColonToken), this.Print(node.Name));
        }
    }
}
