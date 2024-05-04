namespace CSharpier.Cli.Options;

using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

internal static class ConfigFileParser
{
    private static readonly string[] validExtensions = { ".csharpierrc", ".json", ".yml", ".yaml" };

    /// <summary>Finds all configs above the given directory as well as within the subtree of this directory</summary>
    internal static List<CSharpierConfigData> FindForDirectoryName(
        string directoryName,
        IFileSystem fileSystem,
        ILogger logger,
        bool limitEditorConfigSearch
    )
    {
        var results = new List<CSharpierConfigData>();
        var directoryInfo = fileSystem.DirectoryInfo.New(directoryName);

        var filesByDirectory = directoryInfo
            .EnumerateFiles(
                ".csharpierrc*",
                limitEditorConfigSearch
                    ? SearchOption.TopDirectoryOnly
                    : SearchOption.AllDirectories
            )
            .GroupBy(o => o.DirectoryName);

        foreach (var group in filesByDirectory)
        {
            var firstFile = group
                .Where(o => validExtensions.Contains(o.Extension, StringComparer.OrdinalIgnoreCase))
                .MinBy(o => o.Extension);

            if (firstFile != null)
            {
                results.Add(
                    new CSharpierConfigData(
                        firstFile.DirectoryName!,
                        Create(firstFile.FullName, fileSystem, logger)
                    )
                );
            }
        }

        // already found any in this directory above
        directoryInfo = directoryInfo.Parent;

        while (directoryInfo is not null)
        {
            var file = directoryInfo
                .EnumerateFiles(".csharpierrc*", SearchOption.TopDirectoryOnly)
                .Where(o => validExtensions.Contains(o.Extension, StringComparer.OrdinalIgnoreCase))
                .MinBy(o => o.Extension);

            if (file != null)
            {
                results.Add(
                    new CSharpierConfigData(
                        file.DirectoryName!,
                        Create(file.FullName, fileSystem, logger)
                    )
                );
            }

            directoryInfo = directoryInfo.Parent;
        }

        return results.OrderByDescending(o => o.DirectoryName.Length).ToList();
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
        return content.TrimStart().StartsWith("{") ? ReadJson(content) : ReadYaml(content);
    }

    private static ConfigurationFileOptions ReadJson(string contents)
    {
        return JsonSerializer.Deserialize<ConfigurationFileOptions>(
                contents,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new();
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
