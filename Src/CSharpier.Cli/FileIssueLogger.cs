using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class FileIssueLogger(string filePath, ILogger logger, bool isMsBuildFormat)
{
    public void WriteError(string value, Exception? exception = null)
    {
        if (isMsBuildFormat)
            logger.LogError("{Path}: error: {Message}", this.GetPath(), value);
        else
            logger.LogError(exception, $"{this.GetPath()} - {value}");
    }

    public void WriteWarning(string value)
    {
        logger.LogWarning($"{this.GetPath()} - {value}");
    }

    private string GetPath()
    {
        return filePath;
    }
}
