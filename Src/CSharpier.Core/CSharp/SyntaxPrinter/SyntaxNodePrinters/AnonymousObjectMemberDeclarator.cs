using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousObjectMemberDeclarator
{
    public static Doc Print(AnonymousObjectMemberDeclaratorSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null]);
        if (
            node.Parent is AnonymousObjectCreationExpressionSyntax parent
            && node != parent.Initializers.First()
        )
        {
            docs.Add(ExtraNewLines.Print(node));
        }

        if (node.NameEquals != null)
        {
            docs.Add(Token.PrintWithSuffix(node.NameEquals.Name.Identifier, " ", context));
            docs.Add(Token.PrintWithSuffix(node.NameEquals.EqualsToken, " ", context));
        }
        docs.Add(Node.Print(node.Expression, context));
        return Doc.Concat(ref docs);
    }
}
