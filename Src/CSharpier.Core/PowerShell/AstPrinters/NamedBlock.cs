using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class NamedBlock
{
    internal static Doc Print(NamedBlockAst node, PrintContext context)
    {
        var statements = Statements.Print(node.Statements, node.Traps, context, node);
        if (node.Unnamed)
        {
            return statements;
        }

        var keyword = node.BlockKind switch
        {
            TokenKind.Begin => "begin",
            TokenKind.Process => "process",
            TokenKind.End => "end",
            TokenKind.Dynamicparam => "dynamicparam",
            TokenKind.Clean => "clean",
            _ => null,
        };
        if (keyword is null)
        {
            return Verbatim.Print(node.Extent, context);
        }

        return statements == Doc.Null
            ? keyword + " { }"
            : Doc.Concat(keyword, " {", Doc.Indent(Doc.HardLine, statements), Doc.HardLine, "}");
    }
}
