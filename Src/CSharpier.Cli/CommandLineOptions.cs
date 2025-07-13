using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class CommandLineOptions
{
    public string[] DirectoryOrFilePaths { get; init; } = [];
    public bool Check { get; init; }
    public bool SkipValidation { get; init; }
    public LogFormat LogFormat { get; init; }
    public bool Fast { get; init; }
    public bool SkipWrite { get; init; }
    public bool WriteStdout { get; init; }
    public bool NoCache { get; init; }
    public bool NoMSBuildCheck { get; init; }
    public bool CompilationErrorsAsWarnings { get; init; }
    public bool IncludeGenerated { get; init; }
    public string? StandardInFileContents { get; init; }
    public string? ConfigPath { get; init; }
    public string[] OriginalDirectoryOrFilePaths { get; init; } = [];
    public string? IgnorePath { get; init; }
}
