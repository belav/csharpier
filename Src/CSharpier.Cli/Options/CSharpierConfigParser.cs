using System.Globalization;
using System.IO.Abstractions;
using System.Text.Json;
using CSharpier.Core;
using Microsoft.Extensions.Logging;
using YamlDotNet.Core;
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
    // the values are read the same way that deserializer read them
    private static ConfigurationFileOptions ReadYaml(string contents)
    {
        var yamlStream = new YamlStream();
        using var reader = new StringReader(contents);
        yamlStream.Load(reader);

        if (yamlStream.Documents.Count == 0 || IsNull(yamlStream.Documents[0].RootNode))
        {
            return new();
        }

        if (yamlStream.Documents[0].RootNode is not YamlMappingNode root)
        {
            throw Invalid(
                yamlStream.Documents[0].RootNode,
                "Expected the configuration file to contain a set of options."
            );
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
            Overrides = ReadOverrides(root),
        };
    }

    private static Override[] ReadOverrides(YamlMappingNode root)
    {
        var node = FindValue(root, "overrides");
        if (node is null || IsNull(node))
        {
            return [];
        }

        if (node is not YamlSequenceNode overrideNodes)
        {
            throw Invalid(node, "Expected 'overrides' to be a list.");
        }

        var overrides = new List<Override>();
        foreach (var child in overrideNodes.Children)
        {
            if (child is not YamlMappingNode overrideNode)
            {
                throw Invalid(child, "Expected each entry in 'overrides' to be a set of options.");
            }

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
                    Files = ReadString(overrideNode, "files")?.Value ?? string.Empty,
                    Formatter = ReadString(overrideNode, "formatter")?.Value ?? string.Empty,
                }
            );
        }

        return [.. overrides];
    }

    private static YamlException Invalid(YamlNode node, string message)
    {
        return new YamlException(node.Start, node.End, message);
    }

    private static bool IsNull(YamlNode node)
    {
        return node is YamlScalarNode { Style: ScalarStyle.Plain } scalar
            && scalar.Value is null or "" or "~" or "null" or "Null" or "NULL";
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

    private static YamlScalarNode? ReadString(YamlMappingNode mapping, string key)
    {
        var node = FindValue(mapping, key);
        if (node is null || IsNull(node))
        {
            return null;
        }

        return node as YamlScalarNode
            ?? throw Invalid(node, $"Expected a single value for '{key}'.");
    }

    private static int? ReadInt(YamlMappingNode mapping, string key)
    {
        var node = ReadString(mapping, key);
        if (node is null)
        {
            return null;
        }

        var value = node.Value!.Replace("_", string.Empty);
        var isNegative = value.StartsWith('-');
        if (isNegative || value.StartsWith('+'))
        {
            value = value[1..];
        }

        try
        {
            if (value.Length == 0 || value[0] is < '0' or > '9')
            {
                throw new FormatException();
            }

            var result = value switch
            {
                _ when value.StartsWith("0x", StringComparison.Ordinal) => Convert.ToInt32(
                    value[2..],
                    16
                ),
                _ when value.StartsWith("0b", StringComparison.Ordinal) => Convert.ToInt32(
                    value[2..],
                    2
                ),
                _ when value.Length > 1 && value[0] == '0' => Convert.ToInt32(value[1..], 8),
                _ => int.Parse(value, NumberStyles.None, CultureInfo.InvariantCulture),
            };

            return isNegative ? -result : result;
        }
        catch (Exception ex) when (ex is FormatException or OverflowException or ArgumentException)
        {
            throw Invalid(node, $"The value '{node.Value}' for '{key}' is not a valid number.");
        }
    }

    private static bool ReadBool(YamlMappingNode mapping, string key)
    {
        var node = ReadString(mapping, key);
        if (node is null)
        {
            return false;
        }

        return node.Value!.ToLowerInvariant() switch
        {
            "true" or "y" or "yes" or "on" => true,
            "false" or "n" or "no" or "off" => false,
            _ => throw Invalid(
                node,
                $"The value '{node.Value}' for '{key}' is not a valid boolean."
            ),
        };
    }

    private static TEnum? ReadEnum<TEnum>(YamlMappingNode mapping, string key)
        where TEnum : struct, Enum
    {
        var node = ReadString(mapping, key);
        if (node is null)
        {
            return null;
        }

        return Enum.TryParse<TEnum>(node.Value, ignoreCase: true, out var result)
            ? result
            : throw Invalid(
                node,
                $"The value '{node.Value}' for '{key}' is not valid for {typeof(TEnum).Name}."
            );
    }
}
