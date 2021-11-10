using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CSharpier.Cli
{
    public class ConfigurationFileOptions
    {
        public int PrintWidth { get; init; } = 100;
        public List<string>? PreprocessorSymbolSets { get; init; }

        private static readonly string[] validExtensions =
        {
            ".csharpierrc",
            ".json",
            ".yml",
            ".yaml"
        };

        internal static PrinterOptions CreatePrinterOptions(
            string baseDirectoryPath,
            IFileSystem fileSystem,
            ILogger logger
        )
        {
            var configurationFileOptions = Create(baseDirectoryPath, fileSystem, logger);

            List<string[]> preprocessorSymbolSets;
            if (configurationFileOptions.PreprocessorSymbolSets == null)
            {
                preprocessorSymbolSets = new();
            }
            else
            {
                preprocessorSymbolSets = configurationFileOptions.PreprocessorSymbolSets
                    .Select(
                        o =>
                            o.Split(
                                ",",
                                StringSplitOptions.RemoveEmptyEntries
                                    | StringSplitOptions.TrimEntries
                            )
                    )
                    .ToList();
            }

            return new PrinterOptions
            {
                TabWidth = 4,
                UseTabs = false,
                Width = configurationFileOptions.PrintWidth,
                EndOfLine = EndOfLine.Auto,
                PreprocessorSymbolSets = preprocessorSymbolSets,
            };
        }

        public static ConfigurationFileOptions Create(
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
                    .Where(
                        o => validExtensions.Contains(o.Extension, StringComparer.OrdinalIgnoreCase)
                    )
                    .OrderBy(o => o.Extension)
                    .FirstOrDefault();

                if (file == null)
                {
                    directoryInfo = directoryInfo.Parent;
                    continue;
                }

                var contents = fileSystem.File.ReadAllText(file.FullName);

                if (string.IsNullOrWhiteSpace(contents))
                {
                    logger?.LogWarning(
                        "The configuration file at " + file.FullName + " was empty."
                    );

                    return new();
                }

                return contents.TrimStart().StartsWith("{")
                  ? ReadJson(contents)
                  : ReadYaml(contents);
            }

            return new ConfigurationFileOptions();
        }

        private static ConfigurationFileOptions ReadJson(string contents)
        {
            return JsonConvert.DeserializeObject<ConfigurationFileOptions>(contents);
        }

        private static ConfigurationFileOptions ReadYaml(string contents)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<ConfigurationFileOptions>(contents);
        }
    }
}
