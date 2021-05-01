using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TupleType
    {
        public static Doc Print(TupleTypeSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.OpenParenToken),
                SeparatedSyntaxList.Print(node.Elements, Node.Print, " "),
                Token.Print(node.CloseParenToken)
            );
        }
    }
}
