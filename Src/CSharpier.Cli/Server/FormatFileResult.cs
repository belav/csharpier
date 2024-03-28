namespace CSharpier.Cli.Server;

using System.Text.Json.Serialization;

public class FormatFileResult(Status status)
{
    public string? formattedFile { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status status { get; set; } = status;
    public string? errorMessage { get; set; }
}
