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
        public HashSet<string> Exclude { get; }
        public int PrintWidth { get; set; } = 100;
        public int TabWidth { get; set; } = 4;
        public bool UseTabs { get; set; }
        public string EndOfLine { get; set; } = "lf";

        public ConfigurationFileOptions(string[]? exclude)
        {
            if (exclude != null)
            {
                this.Exclude = exclude.Select(o => o.Replace("\\", "/"))
                    .ToHashSet();
            }
            else
            {
                this.Exclude = new HashSet<string>();
            }
        }

        public static ConfigurationFileOptions Create(
            string rootPath,
            IFileSystem fileSystem
        ) {
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

                var tempOptions = deserializer.Deserialize<YamlConfigurationOptions>(
                    fileSystem.File.ReadAllText(path)
                );

                return new ConfigurationFileOptions(
                    tempOptions.Exclude
                )
                {
                    PrintWidth = tempOptions.PrintWidth,
                    TabWidth = tempOptions.TabWidth,
                    UseTabs = tempOptions.UseTabs,
                    EndOfLine = tempOptions.EndOfLine
                };
            }

            var potentialPath = fileSystem.Path.Combine(
                rootPath,
                ".csharpierrc"
            );

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

            var yamlExtensionPaths =
                new[] { potentialPath + ".yaml", potentialPath + ".yml" };
            foreach (var yamlExtensionPath in yamlExtensionPaths)
            {
                if (!fileSystem.File.Exists(yamlExtensionPath))
                {
                    continue;
                }

                return ReadYaml(yamlExtensionPath);
            }

            return new ConfigurationFileOptions(null);
        }

        // we can get rid of this ugly thing when exclude goes away
        // trying to get YamlDotNet to use the constructor for ConfigurationOptions wasn't going well
        private class YamlConfigurationOptions
        {
            public string[]? Exclude { get; set; }
            public int PrintWidth { get; set; } = 100;
            public int TabWidth { get; set; } = 4;
            public bool UseTabs { get; set; }
            public string EndOfLine { get; set; } = "lf";
        }
    }
}
