namespace CSharpier.Cli;

public class CommandLineFormatterResult
{
    // these are fields instead of properties so that Interlocked.Increment may be used on them.
    public int FailedSyntaxTreeValidation;
    public int FailedCompilation;
    public int ExceptionsFormatting;
    public int ExceptionsValidatingSource;
    public int Files;
    public int UnformattedFiles;
    public long ElapsedMilliseconds;
    public bool ReturnExitCodeOne;
}
