namespace CSharpier.Cli;

using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class ConfigurationFileOptions
{
    public int PrintWidth { get; init; } = 100;
    public int TabWidth { get; init; } = 4;
    public bool UseTabs { get; init; }

    private static readonly string[] validExtensions = { ".csharpierrc", ".json", ".yml", ".yaml" };

    internal static PrinterOptions FindPrinterOptionsForDirectory(
        string baseDirectoryPath,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        DebugLogger.Log("Creating printer options for " + baseDirectoryPath);

        var configurationFileOptions = FindForDirectory(baseDirectoryPath, fileSystem, logger);

        return ConvertToPrinterOptions(configurationFileOptions);
    }

    internal static PrinterOptions CreatePrinterOptionsFromPath(
        string configPath,
        IFileSystem fileSystem,
        ILogger logger
    )
    {
        var configurationFileOptions = Create(configPath, fileSystem, logger);

        return ConvertToPrinterOptions(configurationFileOptions);
    }

    private static PrinterOptions ConvertToPrinterOptions(
        ConfigurationFileOptions configurationFileOptions
    )
    {
        return new PrinterOptions
        {
            TabWidth = configurationFileOptions.TabWidth,
            UseTabs = configurationFileOptions.UseTabs,
            Width = configurationFileOptions.PrintWidth,
            EndOfLine = EndOfLine.Auto
        };
    }

    public static ConfigurationFileOptions FindForDirectory(
        string baseDirectoryPath,
        IFileSystem fileSystem,
        ILogger? logger = null
    )
    {
        var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(baseDirectoryPath);

        while (directoryInfo is not null)
        {
            var file = directoryInfo
                .EnumerateFiles(".csharpierrc*", SearchOption.TopDirectoryOnly)
                .Where(o => validExtensions.Contains(o.Extension, StringComparer.OrdinalIgnoreCase))
                .MinBy(o => o.Extension);

            if (file != null)
            {
                return Create(file.FullName, fileSystem, logger);
            }

            directoryInfo = directoryInfo.Parent;
        }

        return new ConfigurationFileOptions();
    }

    public static ConfigurationFileOptions Create(
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
