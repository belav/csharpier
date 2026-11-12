using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class FunctionMember
{
    internal static Doc Print(FunctionMemberAst node, PrintContext context)
    {
        FunctionDefinitionAst? definition = null;
        foreach (var child in node.FindAll(o => o.Parent == node, searchNestedScriptBlocks: false))
        {
            if (child is FunctionDefinitionAst functionDefinition)
            {
                definition = functionDefinition;
            }
        }

        if (definition is null)
        {
            return Verbatim.Print(node.Extent, context);
        }

        var docs = new List<Doc>();
        foreach (var attribute in node.Attributes)
        {
            docs.Add(context.PrintLeadingComments(attribute));
            docs.Add(Node.Print(attribute, context));
            docs.Add(context.PrintTrailingComments(attribute));
            docs.Add(Doc.HardLine);
        }

        if (node.IsHidden)
        {
            docs.Add("hidden ");
        }

        if (node.IsStatic)
        {
            docs.Add("static ");
        }

        if (node.ReturnType is not null)
        {
            docs.Add(Node.Print(node.ReturnType, context));
            docs.Add(" ");
        }

        docs.Add(node.Name);

        Doc? baseConstructorCall = null;
        if (
            node.Body.EndBlock?.Statements
                is [CommandExpressionAst { Expression: BaseCtorInvokeMemberExpressionAst call }, ..]
            && call.Extent.EndOffset <= node.Body.Extent.StartOffset
            && call.Extent.EndOffset > call.Extent.StartOffset
        )
        {
            baseConstructorCall = Node.Print(call, context);
        }

        docs.Add(
            FunctionDefinition.PrintParametersAndBody(definition, baseConstructorCall, context)
        );
        return Doc.Concat(docs);
    }
}
