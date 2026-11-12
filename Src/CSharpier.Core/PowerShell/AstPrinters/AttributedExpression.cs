using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class AttributedExpression
{
    public static Doc Print(AttributedExpressionAst node, PrintContext context)
    {
        return Doc.Concat(Node.Print(node.Attribute, context), Node.Print(node.Child, context));
    }
}
