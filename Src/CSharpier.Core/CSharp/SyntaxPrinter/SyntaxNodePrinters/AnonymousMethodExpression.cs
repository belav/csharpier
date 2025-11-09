using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousMethodExpression
{
    public static Doc Print(AnonymousMethodExpressionSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null]);
        docs.Add(Modifiers.Print(node.Modifiers, context));
        docs.Add(Token.Print(node.DelegateKeyword, context));

        if (node.ParameterList != null)
        {
            docs.Add(ParameterList.Print(node.ParameterList, context));
        }

        docs.Add(Block.Print(node.Block, context));

        return Doc.Concat(ref docs);
    }
}
