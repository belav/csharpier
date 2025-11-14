using System.IO.Enumeration;

namespace CSharpier.Cli;

internal class ValidFilesEnumerator(string directory, EnumerationOptions? options = null)
    : FileSystemEnumerator<string>(directory, options)
{
    protected override string TransformEntry(ref FileSystemEntry entry)
    {
        return entry.ToSpecifiedFullPath();
    }

    protected override bool ShouldIncludeEntry(ref FileSystemEntry entry)
    {
        if (entry.IsDirectory)
        {
            return false;
        }

        var extension = Path.GetExtension(entry.FileName);
        if (
            extension
            is ".cs"
                or ".csx"
                or ".config"
                or ".csproj"
                or ".props"
                or ".slnx"
                or ".targets"
                or ".xaml"
                or ".xml"
        )
        {
            return true;
        }

        return false;
    }

    protected override bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
    {
        if (entry.FileName is ".git" or "bin" or "node_modules" or "obj")
        {
            return false;
        }

        return true;
    }
}
