using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ScriptBlockExpression
{
    public static Doc Print(ScriptBlockExpressionAst node, PrintContext context)
    {
        var scriptBlock = node.ScriptBlock;

        var singleLine =
            !Layout.IsMultiLine(node)
            && scriptBlock.UsingStatements.Count == 0
            && scriptBlock.EndBlock is { Unnamed: true, Traps: null or { Count: 0 } }
            && scriptBlock.EndBlock.Statements.Count <= 1
            && scriptBlock.ParamBlock is null or { Attributes.Count: 0 }
            && !context.HasCommentIn(node.Extent);

        Doc contents;
        if (
            singleLine
            && scriptBlock.ParamBlock is not null
            && scriptBlock.EndBlock!.Statements.Count == 1
        )
        {
            contents = Doc.Concat(
                Node.Print(scriptBlock.ParamBlock, context),
                Doc.Line,
                Doc.SoftLine,
                Node.Print(scriptBlock.EndBlock.Statements[0], context)
            );
        }
        else
        {
            contents = ScriptBlock.PrintContents(scriptBlock, context);
        }

        if (contents == Doc.Null)
        {
            return "{ }";
        }

        return singleLine
            ? Doc.Group("{", Doc.Indent(Doc.Line, contents), Doc.Line, "}")
            : Doc.Concat("{", Doc.Indent(Doc.HardLine, contents), Doc.HardLine, "}");
    }
}
