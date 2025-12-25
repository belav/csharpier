using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class VariableDeclarator
{
    public static Doc Print(VariableDeclaratorSyntax node, PrintingContext context)
    {
        var docs = new DocListBuilder(3);
        docs.Add(Token.Print(node.Identifier, context));

        if (node.ArgumentList != null)
        {
            docs.Add(BracketedArgumentList.Print(node.ArgumentList, context));
        }
        if (node.Initializer != null)
        {
            docs.Add(EqualsValueClause.Print(node.Initializer, context));
        }
        return Doc.Concat(ref docs);
    }
}
