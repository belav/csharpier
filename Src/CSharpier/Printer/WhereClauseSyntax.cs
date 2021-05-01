using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhereClauseSyntax(WhereClauseSyntax node)
        {
            return Doc.Group(
                Token.Print(node.WhereKeyword),
                Doc.Indent(Doc.Line, this.Print(node.Condition))
            );
        }
    }
}
