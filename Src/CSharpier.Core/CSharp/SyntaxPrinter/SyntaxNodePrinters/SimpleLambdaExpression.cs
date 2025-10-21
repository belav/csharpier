using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class SimpleLambdaExpression
{
    public static Doc Print(SimpleLambdaExpressionSyntax node, PrintingContext context)
    {
        return Doc.Group(PrintHead(node, context), PrintBody(node, context));
    }

    public static Doc PrintHead(SimpleLambdaExpressionSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Modifiers.PrintSorted(node.Modifiers, context),
            Doc.Group(Node.Print(node.Parameter, context)),
            " ",
            Token.Print(node.ArrowToken, context)
        );
    }

    public static Doc PrintBody(SimpleLambdaExpressionSyntax node, PrintingContext context)
    {
        return node.Body switch
        {
            BlockSyntax blockSyntax => Block.Print(blockSyntax, context),
            ObjectCreationExpressionSyntax or AnonymousObjectCreationExpressionSyntax => Doc.Group(
                " ",
                Node.Print(node.Body, context)
            ),
            _ => Doc.Indent(Doc.Line, Node.Print(node.Body, context)),
        };
    }
}
