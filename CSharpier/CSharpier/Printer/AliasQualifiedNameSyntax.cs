using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAliasQualifiedNameSyntax(AliasQualifiedNameSyntax node)
        {
            return Concat(this.Print(node.Alias), String(node.ColonColonToken.Text), this.Print(node.Name));
        }
    }
}
