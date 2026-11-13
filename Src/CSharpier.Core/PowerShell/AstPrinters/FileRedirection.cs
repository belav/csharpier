using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class FileRedirection
{
    public static Doc Print(FileRedirectionAst node, PrintContext context)
    {
        var redirectionOperator = context.GetTokenText(
            node.Extent.StartOffset,
            node.Location.Extent.StartOffset
        );

        return Doc.Concat(redirectionOperator, " ", Node.Print(node.Location, context));
    }
}
