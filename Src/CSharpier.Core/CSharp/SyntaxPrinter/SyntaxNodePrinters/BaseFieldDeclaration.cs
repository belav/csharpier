using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseFieldDeclaration
{
    public static Doc Print(BaseFieldDeclarationSyntax node, PrintingContext context)
    {
        var docs = new DocListBuilder(5);
        docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Add(Modifiers.PrintSorted(node.Modifiers, context));
        if (node is EventFieldDeclarationSyntax eventFieldDeclarationSyntax)
        {
            docs.Add(Token.PrintWithSuffix(eventFieldDeclarationSyntax.EventKeyword, " ", context));
        }

        docs.Add(
            VariableDeclaration.Print(node.Declaration, context),
            Token.Print(node.SemicolonToken, context)
        );
        return Doc.Concat(ref docs);
    }
}
