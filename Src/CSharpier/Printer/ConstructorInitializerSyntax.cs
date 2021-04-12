using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorInitializerSyntax(
            ConstructorInitializerSyntax node
        ) {
            return Indent(
                HardLine,
                this.PrintSyntaxToken(
                    node.ColonToken,
                    afterTokenIfNoTrailing: " "
                ),
                this.PrintSyntaxToken(node.ThisOrBaseKeyword),
                this.PrintArgumentListSyntax(node.ArgumentList)
            );
        }
    }
}
