using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
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
                SyntaxTokens.Print(node.LessThanToken),
                Docs.Indent(
                    Docs.SoftLine,
                    this.PrintSeparatedSyntaxList(
                        node.Parameters,
                        this.PrintTypeParameterSyntax,
                        Docs.Line
                    )
                ),
                Docs.SoftLine,
                SyntaxTokens.Print(node.GreaterThanToken)
            );
        }
    }
}
