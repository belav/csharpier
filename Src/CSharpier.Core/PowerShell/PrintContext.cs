using System.Management.Automation.Language;

namespace CSharpier.Core.PowerShell;

internal sealed class PrintContext(IReadOnlyList<IScriptExtent> comments)
{
    internal bool HasCommentIn(IScriptExtent extent)
    {
        foreach (var comment in comments)
        {
            if (comment.StartOffset >= extent.StartOffset && comment.StartOffset < extent.EndOffset)
            {
                return true;
            }
        }

        return false;
    }

    internal IEnumerable<IScriptExtent> CommentsBetween(int startOffset, int endOffset)
    {
        foreach (var comment in comments)
        {
            if (comment.StartOffset >= startOffset && comment.StartOffset < endOffset)
            {
                yield return comment;
            }
        }
    }
}
