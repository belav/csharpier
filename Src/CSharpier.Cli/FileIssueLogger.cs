using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class FileIssueLogger(string filePath, ILogger logger)
{
    public void WriteError(string value, Exception? exception = null)
    {
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
