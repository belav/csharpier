using System.Collections.Concurrent;
using System.Reflection;
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

    private readonly string toolVersion = GetToolVersion();
    private readonly string workingDirectory = Directory.GetCurrentDirectory();

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
                            Version = toolVersion,
                            InformationUri = "https://csharpier.com",
                            Rules =
                            [
                                new SarifRule { Id = "CSharpier/Formatting", ShortDescription = new SarifMessage { Text = "Formatting issue" } },
                                new SarifRule { Id = "CSharpier/Compilation", ShortDescription = new SarifMessage { Text = "Compilation error" } },
                                new SarifRule { Id = "CSharpier/Validation", ShortDescription = new SarifMessage { Text = "Validation failure" } },
                            ],
                        },
                    },
                    Invocations =
                    [
                        new SarifInvocation
                        {
                            ToolExecutionSuccessful = true,
                            ExecutionSuccessful = true,
                            WorkingDirectory = new SarifArtifactLocation { Uri = new Uri(workingDirectory).AbsoluteUri },
                        }
                    ],
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

        var (path, region, extractedRuleId, message) = LoggerExtensions.ExtractState(state);
        var messageText = message ?? formatter(state, exception!);

        if (exception is not null)
        {
            messageText += "\n" + exception;
        }

        var ruleId = extractedRuleId ?? DetermineRuleId(messageText, exception);
        var sarifRegion = region is not null
            ? new SarifRegion
            {
                StartLine = region.StartLine,
                StartColumn = region.StartCharacter,
                EndLine = region.EndLine,
                EndColumn = region.EndCharacter
            }
            : null;

        this.results.Enqueue(
            new SarifResult
            {
                RuleId = ruleId,
                Kind = "fail",
                Level = MapLevel(logLevel),
                Message = new SarifMessage { Text = messageText },
                Locations = path is null
                    ? null
                    : [new SarifLocation
                    {
                        PhysicalLocation = CreatePhysicalLocation(path, sarifRegion)
                    }],
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

    private static string DetermineRuleId(string message, Exception? exception)
    {
        return (message, exception) switch
        {
            (var m, _) when m.Contains("compilation", StringComparison.OrdinalIgnoreCase) => "CSharpier/Compilation",
            (var m, _) when m.Contains("validation", StringComparison.OrdinalIgnoreCase) => "CSharpier/Validation",
            (var m, _) when m.Contains("format", StringComparison.OrdinalIgnoreCase) => "CSharpier/Formatting",
            _ => "CSharpier/Formatting",
        };
    }



    private static SarifPhysicalLocation CreatePhysicalLocation(string path, SarifRegion? region = null)
    {
        return new SarifPhysicalLocation
        {
            ArtifactLocation = new SarifArtifactLocation { Uri = BuildUri(path) },
            Region = region,
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

    private static string GetToolVersion()
    {
        try
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version?.ToString() ?? "unknown";
        }
        catch
        {
            return "unknown";
        }
    }

    private class SarifLog
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("runs")]
        public SarifRun[] Runs { get; set; } = [];
    }

    private class SarifRun
    {
        [JsonPropertyName("tool")]
        public SarifTool Tool { get; set; } = new();

        [JsonPropertyName("invocations")]
        public SarifInvocation[]? Invocations { get; set; }

        [JsonPropertyName("results")]
        public SarifResult[] Results { get; set; } = [];
    }

    private class SarifTool
    {
        [JsonPropertyName("driver")]
        public SarifDriver Driver { get; set; } = new();
    }

    private class SarifDriver
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("informationUri")]
        public string? InformationUri { get; set; }

        [JsonPropertyName("rules")]
        public SarifRule[]? Rules { get; set; }
    }

    private class SarifRule
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("shortDescription")]
        public SarifMessage? ShortDescription { get; set; }
    }

    private class SarifInvocation
    {
        [JsonPropertyName("executionSuccessful")]
        public bool ExecutionSuccessful { get; set; }

        [JsonPropertyName("toolExecutionSuccessful")]
        public bool ToolExecutionSuccessful { get; set; }

        [JsonPropertyName("workingDirectory")]
        public SarifArtifactLocation? WorkingDirectory { get; set; }
    }

    private class SarifResult
    {
        [JsonPropertyName("ruleId")]
        public string RuleId { get; set; } = string.Empty;

        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public SarifMessage Message { get; set; } = new();

        [JsonPropertyName("locations")]
        public SarifLocation[]? Locations { get; set; }
    }

    private class SarifMessage
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    private class SarifLocation
    {
        [JsonPropertyName("physicalLocation")]
        public SarifPhysicalLocation PhysicalLocation { get; set; } = new();
    }

    private class SarifPhysicalLocation
    {
        [JsonPropertyName("artifactLocation")]
        public SarifArtifactLocation ArtifactLocation { get; set; } = new();

        [JsonPropertyName("region")]
        public SarifRegion? Region { get; set; }
    }

    private class SarifRegion
    {
        [JsonPropertyName("startLine")]
        public int? StartLine { get; set; }

        [JsonPropertyName("startColumn")]
        public int? StartColumn { get; set; }

        [JsonPropertyName("endLine")]
        public int? EndLine { get; set; }

        [JsonPropertyName("endColumn")]
        public int? EndColumn { get; set; }
    }

    private class SarifArtifactLocation
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;
    }
}
