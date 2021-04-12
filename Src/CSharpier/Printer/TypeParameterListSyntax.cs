using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeParameterListSyntax(TypeParameterListSyntax node)
        {
            if (node.Parameters.Count == 0)
            {
                return Docs.Null;
            }
            return Docs.Group(
                this.PrintSyntaxToken(node.LessThanToken),
                Docs.Indent(
                    Docs.SoftLine,
                    this.PrintSeparatedSyntaxList(
                        node.Parameters,
                        this.PrintTypeParameterSyntax,
                        Docs.Line
                    )
                ),
                Docs.SoftLine,
                this.PrintSyntaxToken(node.GreaterThanToken)
            );
        }
    }
}
