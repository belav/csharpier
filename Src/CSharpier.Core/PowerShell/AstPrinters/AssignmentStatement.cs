using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class AssignmentStatement
{
    public static Doc Print(AssignmentStatementAst node, PrintContext context)
    {
        return Doc.Concat(Node.Print(node.Left, context), " = ", Node.Print(node.Right, context));
    }
}
