using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class Parameter
{
    public static Doc Print(ParameterSyntax node, PrintingContext context)
    {
        var hasAttribute = node.AttributeLists.Any();

        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null]);

        if (hasAttribute)
        {
            docs.Append(AttributeLists.Print(node, node.AttributeLists, context));
            if (
                node.AttributeLists.Count < 2
                && (
                    Enumerable.Any(node.GetLeadingTrivia(), o => o.IsComment())
                    || node.Parent is ParameterListSyntax { Parameters.Count: 0 }
                )
            )
            {
                docs.Append(" ");
            }
            else
            {
                docs.Append(Doc.Indent(Doc.Line));
            }
        }

        if (node.Modifiers.Any())
        {
            docs.Append(Modifiers.Print(node.Modifiers, context));
        }

        if (node.Type != null)
        {
            docs.Append(Node.Print(node.Type, context));

            if (node.Identifier.RawSyntaxKind() is not SyntaxKind.None)
            {
                docs.Append(" ");
            }
        }

        docs.Append(Token.Print(node.Identifier, context));
        if (node.Default != null)
        {
            docs.Append(EqualsValueClause.Print(node.Default, context));
        }

        return hasAttribute ? Doc.Group(docs.AsSpan().ToArray()) : Doc.Concat(ref docs);
    }
}
