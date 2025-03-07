using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class FileIssueLogger(string filePath, ILogger logger, bool isMsBuildFormat)
{
    public void WriteError(string value, Exception? exception = null)
    {
        logger.LogError(exception, GetMessageTemplate(), this.GetPath(), value);
    }

    public void WriteWarning(string value)
    {
        logger.LogWarning($"{this.GetPath()} - {value}");
    }

    private string GetMessageTemplate()
    {
        return logFormat switch
        {
            LogFormat.MsBuild => "{Path}: error: {Message}",
            LogFormat.Console => "{Path} - {Message}",
            _ => throw new NotImplementedException($"LogFormat: {Enum.GetName(logFormat)}"),
        };
    }

    private string GetPath()
    {
        return filePath;
    }
}
