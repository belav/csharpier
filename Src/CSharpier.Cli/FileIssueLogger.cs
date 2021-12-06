using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class FileIssueLogger
{
    private readonly string filePath;
    private readonly ILogger logger;

    public FileIssueLogger(string filePath, ILogger logger)
    {
        this.filePath = filePath;
        this.logger = logger;
    }

    public void WriteError(string value, Exception? exception = null)
    {
        this.logger.LogError(exception, $"{this.GetPath()} - {value}");
    }

    public void WriteWarning(string value)
    {
        this.logger.LogWarning($"{this.GetPath()} - {value}");
    }

    private string GetPath()
    {
        return this.filePath;
    }
}
