using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class FunctionDefinition
{
    internal static Doc Print(FunctionDefinitionAst node, PrintContext context)
    {
        if (node.Parent is FunctionMemberAst)
        {
            return Verbatim.Print(node.Extent, context);
        }

        var keyword =
            node.IsFilter ? "filter "
            : node.IsWorkflow ? "workflow "
            : "function ";

        return Doc.Concat(
            keyword,
            GetName(node, context),
            PrintParametersAndBody(node, null, context)
        );
    }

    // The tree has the name with its escapes resolved, which is not always valid source text.
    private static string GetName(FunctionDefinitionAst node, PrintContext context)
    {
        var tokens = context.GetTokens(node.Extent.StartOffset, node.Body.Extent.StartOffset);
        return tokens.Count > 1 ? tokens[1].Text : node.Name;
    }

    // Prints everything after the name. The parentheses are only there when the source has them,
    // because "function f()" and "function f" are different trees.
    internal static Doc PrintParametersAndBody(
        FunctionDefinitionAst node,
        Doc? baseConstructorCall,
        PrintContext context
    )
    {
        var docs = new List<Doc>();

        var header = context.GetTokens(node.Extent.StartOffset, node.Body.Extent.StartOffset);
        if (node.Parameters is { Count: > 0 })
        {
            docs.Add(ArgumentList.Print("(", node.Parameters, ")", context));
        }
        else if (
            node.Parameters is not null
            || node.Parent is FunctionMemberAst
            || header is [.., { Kind: TokenKind.RParen }]
        )
        {
            docs.Add("()");
        }

        if (baseConstructorCall is not null)
        {
            docs.Add(" : ");
            docs.Add(baseConstructorCall);
        }

        var contentsOfBody = ScriptBlock.PrintContents(node.Body, context);
        if (contentsOfBody == Doc.Null)
        {
            docs.Add(" { }");
        }
        else
        {
            docs.Add(" {");
            docs.Add(Doc.Indent(Doc.HardLine, contentsOfBody));
            docs.Add(Doc.HardLine);
            docs.Add("}");
        }

        return Doc.Concat(docs);
    }
}
