using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class UsingStatement
{
    internal static Doc Print(UsingStatementAst node, PrintContext context)
    {
        var kind = node.UsingStatementKind switch
        {
            UsingStatementKind.Assembly => "assembly",
            UsingStatementKind.Command => "command",
            UsingStatementKind.Module => "module",
            UsingStatementKind.Namespace => "namespace",
            UsingStatementKind.Type => "type",
            _ => null,
        };

        var first = (Ast?)node.Name ?? node.ModuleSpecification;
        if (kind is null || first is null || node.Alias is not null)
        {
            return Verbatim.Print(node.Extent, context);
        }

        return Doc.Concat("using ", kind, " ", Node.Print(first, context));
    }
}
