using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class DelegateDeclaration
{
    public static Doc Print(DelegateDeclarationSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>(
            [null, null, null, null, null, null, null, null, null, null]
        );
        docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
        docs.Add(Modifiers.PrintSorted(node.Modifiers, context));
        docs.Add(Token.PrintWithSuffix(node.DelegateKeyword, " ", context));
        docs.Add(Node.Print(node.ReturnType, context));
        docs.Add(" ");
        docs.Add(Token.Print(node.Identifier, context));

        if (node.TypeParameterList != null)
        {
            docs.Add(Node.Print(node.TypeParameterList, context));
        }
        docs.Add(
            Node.Print(node.ParameterList, context),
            ConstraintClauses.Print(node.ConstraintClauses, context),
            Token.Print(node.SemicolonToken, context)
        );
        return Doc.Concat(ref docs);
    }
}
