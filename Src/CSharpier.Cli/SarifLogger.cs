using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace CSharpier.Cli;

internal class SarifLogger(IConsole console, LogLevel loggingLevel) : ILogger
{
    private readonly ConcurrentQueue<SarifResult> results = new();
    private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    };

    public void WriteSarifLog()
    {
        var report = new SarifLog
        {
            Schema = "https://json.schemastore.org/sarif-2.1.0.json",
            Version = "2.1.0",
            Runs =
            [
                new SarifRun
                {
                    Tool = new SarifTool
                    {
                        Driver = new SarifDriver
                        {
                            Name = "CSharpier",
                            InformationUri = "https://csharpier.com",
                        },
                    },
                    Results = [.. this.results],
                },
            ],
        };
        
        console.WriteLine(
            JsonSerializer.Serialize(
                report,
                jsonSerializerOptions
            )
        );
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception, string> formatter
    )
    {
        if (logLevel < loggingLevel || logLevel < LogLevel.Warning || !this.IsEnabled(logLevel))
        {
            return;
        }

        var (path, messageFromState) = ExtractState(state);
        var messageText = messageFromState ?? formatter(state, exception!);

        if (exception is not null)
        {
            messageText += "\n" + exception;
        }

        this.results.Enqueue(
            new SarifResult
            {
                RuleId = "CSharpier",
                Level = MapLevel(logLevel),
                Message = new SarifMessage { Text = messageText },
                Locations = path is null
                    ? null
                    : [new SarifLocation { PhysicalLocation = CreatePhysicalLocation(path) }],
            }
        );
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        throw new NotImplementedException();
    }

    private static string MapLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Critical or LogLevel.Error => "error",
            LogLevel.Warning => "warning",
            _ => "note",
        };
    }

    private static (string? Path, string? Message) ExtractState<TState>(TState state)
    {
        string? path = null;
        string? message = null;

        if (state is IEnumerable<KeyValuePair<string, object?>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                if (keyValuePair.Key == "Path")
                {
                    path = keyValuePair.Value?.ToString();
                }
                else if (keyValuePair.Key == "Message")
                {
                    message = keyValuePair.Value?.ToString();
                }
            }
        }

        return (path, message);
    }

    private static SarifPhysicalLocation CreatePhysicalLocation(string path)
    {
        return new SarifPhysicalLocation
        {
            ArtifactLocation = new SarifArtifactLocation { Uri = BuildUri(path) },
        };
    }

    private static string BuildUri(string path)
    {
        try
        {
            if (Path.IsPathRooted(path))
            {
                return new Uri(path).AbsoluteUri;
            }
        }
        catch (UriFormatException)
        {
            // if Uri parsing fails then fallback to a normalized path
        }

        return path.Replace('\\', '/');
    }

    private class SarifLog
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public SarifRun[] Runs { get; set; } = [];
    }

    private class SarifRun
    {
        public SarifTool Tool { get; set; } = new();
        public SarifResult[] Results { get; set; } = [];
    }

    private class SarifTool
    {
        public SarifDriver Driver { get; set; } = new();
    }

    private class SarifDriver
    {
        public string Name { get; set; } = string.Empty;
        public string? InformationUri { get; set; }
    }

    private class SarifResult
    {
        public string RuleId { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public SarifMessage Message { get; set; } = new();
        public SarifLocation[]? Locations { get; set; }
    }

    private class SarifMessage
    {
        public string Text { get; set; } = string.Empty;
    }

    private class SarifLocation
    {
        public SarifPhysicalLocation PhysicalLocation { get; set; } = new();
    }

    private class SarifPhysicalLocation
    {
        public SarifArtifactLocation ArtifactLocation { get; set; } = new();
    }

    private class SarifArtifactLocation
    {
        public string Uri { get; set; } = string.Empty;
    }
}
