using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class NamedAttributeArgument
{
    internal static Doc Print(NamedAttributeArgumentAst node, PrintContext context)
    {
        return node.ExpressionOmitted
            ? node.ArgumentName
            : Doc.Concat(node.ArgumentName, " = ", Node.Print(node.Argument, context));
    }
}
