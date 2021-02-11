using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConstructorInitializerSyntax(ConstructorInitializerSyntax node)
        {
            return Indent(HardLine,
                this.PrintSyntaxToken(node.ColonToken, " "),
                this.PrintSyntaxToken(node.ThisOrBaseKeyword),
                this.PrintArgumentListSyntax(node.ArgumentList));
        }
    }
}