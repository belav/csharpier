using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CSharpier.Cli.Options;

internal static class CSharpierConfigParser
{
    private static readonly string[] validExtensions = [".csharpierrc", ".json", ".yml", ".yaml"];
    private static readonly JsonSerializerOptions CaseInsensitiveJson = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    internal static CSharpierConfigData? FindForDirectoryName(
        string directoryName,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        var directoryInfo = fileSystem.DirectoryInfo.New(directoryName);

        while (directoryInfo is not null)
        {
            try
            {
                var file = directoryInfo
                    .EnumerateFiles(".csharpierrc*", SearchOption.TopDirectoryOnly)
                    .Where(o =>
                        validExtensions.Contains(o.Extension, StringComparer.OrdinalIgnoreCase)
                    )
                    .MinBy(o => o.Extension);

                if (file != null)
                {
                    return new CSharpierConfigData(
                        file.DirectoryName!,
                        Create(file.FullName, fileSystem, logger)
                    );
                }
            }
            catch (UnauthorizedAccessException) { }

            directoryInfo = directoryInfo.Parent;
        }

        return null;
    }

    internal static ConfigurationFileOptions Create(
        string configPath,
        IFileSystem fileSystem,
        ILogger? logger = null
    )
    {
        var directoryName = fileSystem.Path.GetDirectoryName(configPath)!;
        var content = fileSystem.File.ReadAllText(configPath);

        if (!string.IsNullOrWhiteSpace(content))
        {
            var configFile = CreateFromContent(content);
            configFile.Init(directoryName);
            return configFile;
        }

        logger?.LogWarning("The configuration file at " + configPath + " was empty.");

        return new();
    }

    internal static ConfigurationFileOptions CreateFromContent(string content)
    {
        return content.TrimStart().StartsWith('{') ? ReadJson(content) : ReadYaml(content);
    }

    private static ConfigurationFileOptions ReadJson(string contents)
    {
        return JsonSerializer.Deserialize<ConfigurationFileOptions>(contents, CaseInsensitiveJson)
            ?? new();
    }

    private static ConfigurationFileOptions ReadYaml(string contents)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        return deserializer.Deserialize<ConfigurationFileOptions>(contents);
    }
}
