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
                return Doc.Null;
            }
            return Doc.Group(
                Token.Print(node.LessThanToken),
                Doc.Indent(
                    Doc.SoftLine,
                    SeparatedSyntaxList.Print(
                        node.Parameters,
                        this.PrintTypeParameterSyntax,
                        Doc.Line
                    )
                ),
                Doc.SoftLine,
                Token.Print(node.GreaterThanToken)
            );
        }
    }
}
