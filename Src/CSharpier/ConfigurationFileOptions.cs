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
        public string EndOfLine { get; init; } = "lf";

        public static ConfigurationFileOptions Create(string rootPath, IFileSystem fileSystem)
        {
            ConfigurationFileOptions ReadJson(string path)
            {
                return JsonConvert.DeserializeObject<ConfigurationFileOptions>(
                    fileSystem.File.ReadAllText(path)
                );
            }

            ConfigurationFileOptions ReadYaml(string path)
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(
                        CamelCaseNamingConvention.Instance
                    )
                    .Build();

                return deserializer.Deserialize<ConfigurationFileOptions>(
                    fileSystem.File.ReadAllText(path)
                );
            }

            var potentialPath = fileSystem.Path.Combine(rootPath, ".csharpierrc");

            if (fileSystem.File.Exists(potentialPath))
            {
                var contents = fileSystem.File.ReadAllText(potentialPath);
                if (contents.TrimStart().StartsWith("{"))
                {
                    return ReadJson(potentialPath);
                }

                return ReadYaml(potentialPath);
            }

            var jsonExtensionPath = potentialPath + ".json";
            if (fileSystem.File.Exists(jsonExtensionPath))
            {
                return ReadJson(jsonExtensionPath);
            }

            var yamlExtensionPaths = new[] { potentialPath + ".yaml", potentialPath + ".yml" };
            foreach (var yamlExtensionPath in yamlExtensionPaths)
            {
                if (!fileSystem.File.Exists(yamlExtensionPath))
                {
                    continue;
                }

                return ReadYaml(yamlExtensionPath);
            }

            return new ConfigurationFileOptions();
        }
    }
}
