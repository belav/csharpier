using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Newtonsoft.Json;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CSharpier
{
    public class ConfigurationFileOptions
    {
        public int PrintWidth { get; init; } = 100;
        public int TabWidth { get; init; } = 4;
        public bool UseTabs { get; init; }
        public EndOfLine EndOfLine { get; init; }

        public static ConfigurationFileOptions Create(
            string baseDirectoryPath,
            IFileSystem fileSystem
        ) {
            while (true)
            {
                var potentialPath = fileSystem.Path.Combine(baseDirectoryPath, ".csharpierrc");

                if (fileSystem.File.Exists(potentialPath))
                {
                    var contents = fileSystem.File.ReadAllText(potentialPath);
                    if (contents.TrimStart().StartsWith("{"))
                    {
                        return ReadJson(potentialPath, fileSystem);
                    }

                    return ReadYaml(potentialPath, fileSystem);
                }

                var jsonExtensionPath = potentialPath + ".json";
                if (fileSystem.File.Exists(jsonExtensionPath))
                {
                    return ReadJson(jsonExtensionPath, fileSystem);
                }

                var yamlExtensionPaths = new[] { potentialPath + ".yaml", potentialPath + ".yml" };
                foreach (var yamlExtensionPath in yamlExtensionPaths)
                {
                    if (!fileSystem.File.Exists(yamlExtensionPath))
                    {
                        continue;
                    }

                    return ReadYaml(yamlExtensionPath, fileSystem);
                }

                var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(baseDirectoryPath);
                if (directoryInfo.Parent == null)
                {
                    return new ConfigurationFileOptions();
                }
                baseDirectoryPath = directoryInfo.Parent.FullName;
            }
        }

        private static ConfigurationFileOptions ReadJson(string path, IFileSystem fileSystem)
        {
            return JsonConvert.DeserializeObject<ConfigurationFileOptions>(
                fileSystem.File.ReadAllText(path)
            );
        }

        private static ConfigurationFileOptions ReadYaml(string path, IFileSystem fileSystem)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(
                    CamelCaseNamingConvention.Instance
                )
                .Build();

            return deserializer.Deserialize<ConfigurationFileOptions>(
                fileSystem.File.ReadAllText(path)
            );
        }
    }
}
