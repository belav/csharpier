using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal record Region
{
    public int StartLine { get; set; }
    public int StartCharacter { get; set; }
    public int EndLine { get; set; }
    public int EndCharacter { get; set; }
}

internal class FileIssueLogger(string filePath, ILogger logger, LogFormat logFormat)
{
    public void WriteError(
        string message,
        Exception? exception = null,
        string? ruleId = null,
        Region? region = null
    )
    {
        logger.LogError(
            exception,
            "{Path}{Region}{RuleId}{Message}",
            filePath,
            region,
            ruleId,
            message
        );
    }

    public void WriteWarning(string message, string? ruleId = null, Region? region = null)
    {
        logger.LogWarning(
            "{Path}{Region}{RuleId}{Message}",
            filePath,
            region,
            ruleId,
            message
        );
    }
}

