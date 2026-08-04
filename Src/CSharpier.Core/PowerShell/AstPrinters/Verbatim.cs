using System.Management.Automation.Language;
using CSharpier.Core.DocTypes;

namespace CSharpier.Core.PowerShell.AstPrinters;

// TODO 1894 this should probably go away completely
internal static class Verbatim
{
    // Emits the exact source text of an extent. Single-line text becomes a plain string; multi-line
    // text keeps its interior lines verbatim (via LiteralLine, which does not add indentation) so
    // here-strings and multi-line pipelines round-trip unchanged.
    internal static Doc Print(IScriptExtent extent)
    {
        var text = extent.Text;
        if (!text.Contains('\n'))
        {
            return text;
        }

        var lines = text.Replace("\r\n", "\n").Split('\n');
        var docs = new List<Doc> { lines[0] };
        for (var i = 1; i < lines.Length; i++)
        {
            docs.Add(Doc.LiteralLine);
            docs.Add(lines[i]);
        }

        return Doc.Concat(docs);
    }
}
