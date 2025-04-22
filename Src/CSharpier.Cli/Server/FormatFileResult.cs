using System.Text.Json.Serialization;

namespace CSharpier.Cli.Server;

public class FormatFileResult(Status status)
{
#pragma warning disable IDE1006
    public string? formattedFile { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status status { get; set; } = status;
    public string? errorMessage { get; set; }
#pragma warning restore IDE1006
}
