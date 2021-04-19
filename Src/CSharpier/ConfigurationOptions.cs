using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CSharpier
{
    public class ConfigurationOptions
    {
        public HashSet<string> Exclude { get; set; }

        public ConfigurationOptions(string[]? exclude)
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

        public static ConfigurationOptions Create(string rootPath)
        {
            var directoryInfo = new DirectoryInfo(rootPath);

            var nextPath = Path.Combine(directoryInfo.FullName, ".csharpierrc");
            if (File.Exists(nextPath))
            {
                return JsonConvert.DeserializeObject<ConfigurationOptions>(
                    File.ReadAllText(nextPath)
                );
            }

            return new ConfigurationOptions(null);
        }
    }
}
