using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using CSharpier.Cli.Options;
using CSharpier.Cli.Server;
using CSharpier.Core;

namespace CSharpier.Cli;

[JsonSerializable(typeof(ConcurrentDictionary<string, string>))]
[JsonSerializable(typeof(FormatFileParameter))]
[JsonSerializable(typeof(FormatFileResult))]
[JsonSerializable(typeof(PrinterOptions))]
internal sealed partial class CliJsonContext : JsonSerializerContext;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(ConfigurationFileOptions))]
internal sealed partial class ConfigurationJsonContext : JsonSerializerContext;
