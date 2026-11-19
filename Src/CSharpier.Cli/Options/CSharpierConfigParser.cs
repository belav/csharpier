using System.Globalization;
using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Core;
using Microsoft.Extensions.Logging;
using YamlDotNet.RepresentationModel;

namespace CSharpier.Cli.Options;

internal static class CSharpierConfigParser
{
    private static readonly string[] validExtensions = [".csharpierrc", ".json", ".yml", ".yaml"];

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
        return JsonSerializer.Deserialize(
                contents,
                ConfigurationJsonContext.Default.ConfigurationFileOptions
            ) ?? new();
    }

    // the reflection based YamlDotNet deserializer does not work with NativeAOT, so the keys are mapped by hand
    private static ConfigurationFileOptions ReadYaml(string contents)
    {
        var yamlStream = new YamlStream();
        using var reader = new StringReader(contents);
        yamlStream.Load(reader);

        if (
            yamlStream.Documents.Count == 0
            || yamlStream.Documents[0].RootNode is not YamlMappingNode root
        )
        {
            return new();
        }

        var overrides = new List<Override>();
        if (FindValue(root, "overrides") is YamlSequenceNode overrideNodes)
        {
            foreach (var overrideNode in overrideNodes.OfType<YamlMappingNode>())
            {
                overrides.Add(
                    new Override
                    {
                        PrintWidth = ReadInt(overrideNode, "printWidth"),
                        IndentSize = ReadInt(overrideNode, "indentSize"),
                        UseTabs = ReadBool(overrideNode, "useTabs"),
                        XmlWhitespaceSensitivity = ReadEnum<XmlWhitespaceSensitivity>(
                            overrideNode,
                            "xmlWhitespaceSensitivity"
                        ),
                        EndOfLine = ReadEnum<EndOfLine>(overrideNode, "endOfLine") ?? default,
                        Files = ReadString(overrideNode, "files") ?? string.Empty,
                        Formatter = ReadString(overrideNode, "formatter") ?? string.Empty,
                    }
                );
            }
        }

        return new ConfigurationFileOptions
        {
            PrintWidth = ReadInt(root, "printWidth"),
            IndentSize = ReadInt(root, "indentSize"),
            UseTabs = ReadBool(root, "useTabs"),
            XmlWhitespaceSensitivity = ReadEnum<XmlWhitespaceSensitivity>(
                root,
                "xmlWhitespaceSensitivity"
            ),
            EndOfLine = ReadEnum<EndOfLine>(root, "endOfLine") ?? default,
            Overrides = [.. overrides],
        };
    }

    private static YamlNode? FindValue(YamlMappingNode mapping, string key)
    {
        foreach (var child in mapping.Children)
        {
            if (child.Key is YamlScalarNode scalar && scalar.Value == key)
            {
                return child.Value;
            }
        }

        return null;
    }

    private static string? ReadString(YamlMappingNode mapping, string key)
    {
        var node = FindValue(mapping, key);
        if (node is null)
        {
            return null;
        }

        if (node is not YamlScalarNode scalar)
        {
            throw new InvalidOperationException(
                $"Expected a single value for '{key}' at line {node.Start.Line}."
            );
        }

        var isPlain = scalar.Style == YamlDotNet.Core.ScalarStyle.Plain;
        if (isPlain && scalar.Value is null or "" or "~" or "null" or "Null" or "NULL")
        {
            return null;
        }

        return scalar.Value;
    }

    private static int? ReadInt(YamlMappingNode mapping, string key)
    {
        var value = ReadString(mapping, key);
        if (value is null)
        {
            return null;
        }

        return int.TryParse(
            value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out var result
        )
            ? result
            : throw new InvalidOperationException(
                $"The value '{value}' for '{key}' is not a valid number."
            );
    }

    private static bool ReadBool(YamlMappingNode mapping, string key)
    {
        var value = ReadString(mapping, key);
        if (value is null)
        {
            return false;
        }

        return bool.TryParse(value, out var result)
            ? result
            : throw new InvalidOperationException(
                $"The value '{value}' for '{key}' is not a valid boolean."
            );
    }

    private static TEnum? ReadEnum<TEnum>(YamlMappingNode mapping, string key)
        where TEnum : struct, Enum
    {
        var value = ReadString(mapping, key);
        if (value is null)
        {
            return null;
        }

        return Enum.TryParse<TEnum>(value, ignoreCase: true, out var result)
            ? result
            : throw new InvalidOperationException(
                $"The value '{value}' for '{key}' is not valid for {typeof(TEnum).Name}."
            );
    }
}
