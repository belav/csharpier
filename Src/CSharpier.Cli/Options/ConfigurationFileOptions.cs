namespace CSharpier.Cli.Options;

using System.IO.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

internal class ConfigurationFileOptions
{
    public int PrintWidth { get; init; } = 100;
    public int TabWidth { get; init; } = 4;
    public bool UseTabs { get; init; }

    [JsonConverter(typeof(CaseInsensitiveEnumConverter<EndOfLine>))]
    public EndOfLine EndOfLine { get; init; }

    private static readonly string[] validExtensions = { ".csharpierrc", ".json", ".yml", ".yaml" };

    internal static PrinterOptions CreatePrinterOptionsFromPath(
        string configPath,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        var configurationFileOptions = Create(configPath, fileSystem, logger);

        return ConvertToPrinterOptions(configurationFileOptions);
    }

    internal static PrinterOptions ConvertToPrinterOptions(
        ConfigurationFileOptions configurationFileOptions
    )
    {
        return new PrinterOptions
        {
            TabWidth = configurationFileOptions.TabWidth,
            UseTabs = configurationFileOptions.UseTabs,
            Width = configurationFileOptions.PrintWidth,
            EndOfLine = configurationFileOptions.EndOfLine
        };
    }

    /// <summary>Finds all configs above the given directory as well as within the subtree of this directory</summary>
    internal static List<CSharpierConfigData> FindForDirectoryName(
        string directoryName,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        var results = new List<CSharpierConfigData>();
        var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(directoryName);

        var filesByDirectory = directoryInfo
            .EnumerateFiles(".csharpierrc*", SearchOption.AllDirectories)
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
                        firstFile.DirectoryName,
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
                        file.DirectoryName,
                        Create(file.FullName, fileSystem, logger)
                    )
                );
            }

            directoryInfo = directoryInfo.Parent;
        }

        return results.OrderByDescending(o => o.DirectoryName.Length).ToList();
    }

    private static ConfigurationFileOptions Create(
        string configPath,
        IFileSystem fileSystem,
        ILogger? logger = null
    )
    {
        var contents = fileSystem.File.ReadAllText(configPath);

        if (!string.IsNullOrWhiteSpace(contents))
        {
            return contents.TrimStart().StartsWith("{") ? ReadJson(contents) : ReadYaml(contents);
        }

        logger?.LogWarning("The configuration file at " + configPath + " was empty.");

        return new();
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
