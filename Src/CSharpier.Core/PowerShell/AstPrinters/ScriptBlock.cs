using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

internal static class ScriptBlock
{
    internal static Doc Print(ScriptBlockAst node, PrintContext context)
    {
        return PrintContents(node, context);
    }

    // Prints the using statements, param block, statements or named blocks, and dangling comments
    // of a script block without any surrounding braces.
    internal static Doc PrintContents(ScriptBlockAst node, PrintContext context)
    {
        var lines = new LineList();

        foreach (var usingStatement in node.UsingStatements)
        {
            lines.AddWithComments(usingStatement, Node.Print(usingStatement, context), context);
        }

        if (node.ParamBlock is not null)
        {
            lines.Add(
                Node.Print(node.ParamBlock, context),
                context.GetStartLine(node.ParamBlock),
                context.GetEndLine(node.ParamBlock),
                forceBlankLine: true
            );
        }

        var namedBlocks = new List<NamedBlockAst>();
        foreach (
            var block in new[]
            {
                node.DynamicParamBlock,
                node.BeginBlock,
                node.ProcessBlock,
                node.EndBlock,
                node.CleanBlock,
            }
        )
        {
            if (block is not null)
            {
                namedBlocks.Add(block);
            }
        }

        if (namedBlocks is [{ Unnamed: true } unnamedBlock])
        {
            var statements = Statements.Print(
                unnamedBlock.Statements,
                unnamedBlock.Traps,
                context,
                node
            );
            if (statements != Doc.Null)
            {
                lines.Add(statements, 0, 0, forceBlankLine: true);
            }

            return lines.ToDoc();
        }

        namedBlocks.Sort(
            (first, second) => first.Extent.StartOffset.CompareTo(second.Extent.StartOffset)
        );
        foreach (var block in namedBlocks)
        {
            lines.AddWithComments(block, Node.Print(block, context), context, forceBlankLine: true);
        }

        lines.AddComments(context.GetDanglingComments(node), context);

        return lines.ToDoc();
    }
}
