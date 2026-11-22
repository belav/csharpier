using System.Collections.Concurrent;

namespace CSharpier.Cli;

internal class CommandLineFormatterResult
{
    // these are fields instead of properties so that Interlocked.Increment may be used on them.
    public int FailedFormattingValidation;
    public int FailedCompilation;
    public int ExceptionsFormatting;
    public int ExceptionsValidatingSource;
    public int Files;
    public int UnformattedFiles;
    public int ChangedFiles;
    public int CachedFiles;
    public long ElapsedMilliseconds;
    public readonly ConcurrentQueue<string> FormattedFilePaths = new();
}
