using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO 0 where is this used? I changed it and nothing broke
        private Doc PrintConstructorInitializerSyntax(
            ConstructorInitializerSyntax node
        ) {
            return Doc.Indent(
                Doc.HardLine,
                this.PrintSyntaxToken(
                    node.ColonToken,
                    afterTokenIfNoTrailing: " "
                ),
                Token.Print(node.ThisOrBaseKeyword),
                this.PrintArgumentListSyntax(node.ArgumentList)
            );
        }
    }
}
