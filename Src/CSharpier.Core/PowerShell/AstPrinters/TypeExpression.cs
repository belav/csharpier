using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TypeExpression
{
    internal static Doc Print(TypeExpressionAst node, PrintContext context)
    {
        return Doc.Concat("[", node.TypeName.FullName, "]");
    }
}
