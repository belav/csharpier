using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class EnumMemberDeclaration
{
    public static Doc Print(EnumMemberDeclarationSyntax node, PrintingContext context)
    {
        var docs = new DocListBuilder(4);
        docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Add(Modifiers.Print(node.Modifiers, context));
        docs.Add(Token.Print(node.Identifier, context));

        if (node.EqualsValue != null)
        {
            docs.Add(EqualsValueClause.Print(node.EqualsValue, context));
        }
        return Doc.Concat(ref docs);
    }
}
