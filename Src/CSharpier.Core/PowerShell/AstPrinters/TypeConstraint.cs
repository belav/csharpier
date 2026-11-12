using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class TypeConstraint
{
    internal static Doc Print(TypeConstraintAst node, PrintContext context)
    {
        return Verbatim.Print(node.Extent, context);
    }
}
